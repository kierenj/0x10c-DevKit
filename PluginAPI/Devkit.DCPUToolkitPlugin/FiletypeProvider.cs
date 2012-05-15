using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;

namespace Devkit.DCPUToolchainPlugin
{
    public class FiletypeProvider : IFileTypeProvider
    {
        private readonly IWorkspace _workspace;

        public FiletypeProvider(IWorkspace workspace)
        {
            this._workspace = workspace;
        }

        public string FileTypeName
        {
            get { return "DCPU-16 Toolchain C"; }
        }

        public IEnumerable<string> DefaultFileExtensions
        {
            get { yield return ".c"; }
        }

        public bool CanCreateNew
        {
            get { return true; }
        }

        public bool IsTextual
        {
            get { return true; }
        }

        public string GetDefaultFileContent(IOpenFile openFile)
        {
            return @"// File:    " + Path.GetFileName(openFile.AbsolutePath) + @"
// Version: 1.0.0
// Author:  " + Environment.UserName + @"
//
";
        }

        public IEditorControlStrategy EditorControlStrategy
        {
            get
            {
                var strat = new CodeEditorStrategy();
                Highlighting.Apply(strat);
                return strat;
            }
        }

        public ISourceFileScope CreateFileScope(IFile file, IProjectScope projectScope, CompileToolContext context)
        {
            return new FileScope(file.AbsolutePath, projectScope, this._workspace);
        }
    }
}
