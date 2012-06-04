using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;

namespace Devkit.DCPUToolchainPlugin
{
    public class FileScope : ISourceFileScope
    {
        private readonly IWorkspace _workspace;
        private readonly string _filename;
        private string _compiledAssembly;
        private List<CompileMessage> _externalErrors;
        private ISourceFileScope _assemblyScope;

        public INode Parent { get; set; }

        public FileScope(string filename, IProjectScope projectScope, IWorkspace workspace)
        {
            this._workspace = workspace;
            this._filename = filename;
            this.Parent = projectScope;

            InvokeCompiler();
        }

        private void InvokeCompiler()
        {
            this._externalErrors = new List<CompileMessage>();

            var cb = Assembly.GetExecutingAssembly().CodeBase.Replace("file://", "").TrimStart('/').Replace("\\\\", "\\");
            var dcpuFolder = Path.Combine(Path.GetDirectoryName(cb), "dcputoolchain");
            var tempPath = Path.GetTempFileName();

            File.Delete(tempPath);
            var psi = new ProcessStartInfo(Path.Combine(dcpuFolder, "dtcc.exe"),
                                           string.Format("-o \"{0}\" \"{1}\"", tempPath, this._filename))
                          {
                              RedirectStandardError = true,
                              RedirectStandardOutput = true,
                              UseShellExecute = false,
                              WindowStyle = ProcessWindowStyle.Hidden,
                              CreateNoWindow = true
                          };
            var process = Process.Start(psi);

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                ProcessErrors(process.StandardError, process.StandardOutput);
            }

            if (File.Exists(tempPath))
            {
                bool bootstrap = false;
                this._compiledAssembly = (bootstrap ? File.ReadAllText(Path.Combine(dcpuFolder, "bootstrap.dasm16")) + Environment.NewLine: "")

                    // dtcc uses a .SECTION INIT for global initialisers; create a segment at prio 250
                    + "#segment init 250" + Environment.NewLine

                    // the generated assembly can start with data..
                    + "#segment data" + Environment.NewLine

                    + File.ReadAllText(tempPath);

                File.Delete(tempPath);
            }
            else
            {
                this._externalErrors.Add(new CompileMessage
                                             {
                                                 Filename = this._filename,
                                                 Line = 0,
                                                 Message = "File was not built: compiled output not found",
                                                 MessageLevel = Level.Error
                                             });
            }
        }

        private void ProcessErrors(StreamReader stdErr, StreamReader stdOut)
        {
            string line;
            while ((line = stdErr.ReadLine()) != null)
            {
                ProcessErrorLine(line);
            }
            while ((line = stdOut.ReadLine()) != null)
            {
                ProcessErrorLine(line);
            }
        }

        private void ProcessErrorLine(string text)
        {
            if (!text.Contains(": ")) return;

            int spacePos = text.IndexOf(':');
            int line = int.Parse(text.Substring(0, spacePos));

            text = text.Substring(spacePos + 1);
            spacePos = text.IndexOf(": ", System.StringComparison.Ordinal);
            string filename = text.Substring(0, spacePos);
            
            text = text.Substring(spacePos + 2);

            this._externalErrors.Add(new CompileMessage
            {
                Filename = filename,
                Line = line,
                Message = text,
                MessageLevel = Level.Error
            });
        }

        public IEnumerable<INode> GetChildren()
        {
            yield break;
        }

        public ICompilerScopeObject Find(string name, out IIdentifierScope scope)
        {
            if (this._assemblyScope == null)
            {
                scope = null;
                return null;
            }

            return this._assemblyScope.Find(name, out scope);
        }

        public ICompilerScopeObject FindInChildren(string name, out IIdentifierScope scope)
        {
            if (this._assemblyScope == null)
            {
                scope = null;
                return null;
            }

            return this._assemblyScope.FindInChildren(name, out scope);
        }

        public void Add(ICompilerScopeObject obj)
        {
            if (this._assemblyScope == null) return;
            this._assemblyScope.Add(obj);
        }

        public IEnumerable<ICompilerScopeObject> Objects
        {
            get
            {
                if (this._assemblyScope == null) return new ICompilerScopeObject[] { };
                return this._assemblyScope.Objects;
            }
        }

        public bool Compile(CompilationPass pass, ICompiler compiler, CompileToolContext context)
        {
            // pass on any outstanding errors
            if (this._externalErrors.Count > 0)
            {
                foreach (var extErr in this._externalErrors)
                {
                    context.AddMessage(extErr);
                }
                this._externalErrors.Clear();
                return false;
            }

            if (this._assemblyScope == null)
            {
                // get the built-in assembler service
                var asmService = this._workspace.GetService<IDevkit10cAssemblyService>();
                var mappedSource = new SourceMappedText();

                var currentText = new StringBuilder();
                var allText = new StringBuilder();
                SourceReference currentRef = null;
                using (var reader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(this._compiledAssembly))))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith(".ULINE"))
                        {
                            var lineInfo = line.Substring(line.IndexOf(' ') + 1);
                            var spacePos = lineInfo.IndexOf(' ');
                            var lineNum = int.Parse(lineInfo.Substring(0, spacePos));
                            var file = lineInfo.Substring(spacePos + 1).TrimStart('\"').TrimEnd('\"');
                            if (currentRef == null || file != currentRef.File || lineNum != currentRef.Line)
                            {
                                if (currentRef != null)
                                {
                                    mappedSource.Add(new Tuple<SourceReference, string>(currentRef, currentText.ToString()));
                                    allText.Append(currentText);
                                    currentText = new StringBuilder();
                                }
                                currentRef = new SourceReference(file, lineNum, 1, 1, lineNum, 1);
                            }
                            continue;
                        }
                        else if (line.StartsWith(".SECTION"))
                        {
                            var sectionInfo = line.Substring(line.IndexOf(' ') + 1);
                            currentText.AppendLine("#segment " + sectionInfo.ToLower());
                            continue;
                        }
                        else if (
                            line.StartsWith(".BOUNDARY")
                            || line.StartsWith(".EXPORT")
                            || line.StartsWith(".IMPORT"))
                        {
                            // ignore
                            continue;
                        }

                        currentText.AppendLine(line);
                    }
                }

                if (currentRef != null)
                {
                    mappedSource.Add(new Tuple<SourceReference, string>(currentRef, currentText.ToString()));
                    allText.Append(currentText);
                }

                var allForDebug = allText.ToString();

                // use built-in assembler
                this._assemblyScope = asmService.CreateCustomAssemblyScope(mappedSource, context);
                this._assemblyScope.Parent = this.Parent;
            }

            return this._assemblyScope.Compile(pass, compiler, context);
        }
    }
}
