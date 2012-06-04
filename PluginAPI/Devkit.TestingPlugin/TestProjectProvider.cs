using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;
using Devkit.Interfaces.FileDecorators;
using Devkit.TestingPlugin.View;

namespace Devkit.TestingPlugin
{
    public class TestProjectProvider : IProjectTypeProvider
    {
        private readonly IWorkspace _workspace;
        
        public string ProjectTypeName
        {
            get { return "Test"; }
        }

        public string TypeCode
        {
            get { return "test"; }
        }

        public TestProjectProvider(IWorkspace workspace)
        {
            this._workspace = workspace;
        }

        public object CreatePropertiesControl(IProjectProperties project)
        {
            return new TestProjectProperties { DataContext = project };
        }

        public bool CanAddReference(IProject project, IProject otherProject)
        {
            return (otherProject.TypeCode == "code");
        }

        public BuildOutput Build(IBuildManager manager, ISolution solution, IProject project, IBuildProgressMonitor monitor)
        {
            var watch = new Stopwatch();
            var projPath = Path.GetDirectoryName(project.AbsolutePath);
            var buildSvc = this._workspace.GetService<ISimpleFileBuildService>();
            var msgs = new List<ICompileMessage>();

            watch.Start();

            foreach (var file in project.Files)
            {
                var op = buildSvc.Build(manager, solution, project, file, monitor, true);
                msgs.AddRange(op.Messages);

                if (!op.Success)
                {
                    monitor.StatusUpdate("Build failed for " + file.AbsolutePath);
                    return new BuildOutput { Success = false, Time = watch.Elapsed, Messages = msgs };
                }

                // do stuff with output
                bool pass = RunUnitTest(op.Words);

                if (pass)
                {
                    monitor.StatusUpdate("Unit test PASSED: " + file.Path);
                    this._workspace.RemoveFileDecorator<UnitTestFailDecorator>(file);
                    this._workspace.AddFileDecorator<UnitTestPassDecorator>(file);
                }
                else
                {
                    monitor.StatusUpdate("Unit test FAILED: " + file.Path);
                    this._workspace.RemoveFileDecorator<UnitTestPassDecorator>(file);
                    this._workspace.AddFileDecorator<UnitTestFailDecorator>(file);

                    return op;
                }
            }

            watch.Stop();

            return new BuildOutput
            {
                CodeModel = null,
                DebugInfo = null,
                Messages = msgs,
                Success = true,
                Time = watch.Elapsed,
                Words = null
            };
        }

        private bool RunUnitTest(ushort[] words)
        {
            var sys = this._workspace.RuntimeManager.CreateSystem();
            for (int i = 0; i < words.Length; i++)
            {
                sys.MemoryController.Write((ushort)i, words[i]);
            }

            var dbgr = new Debugger();
            sys.HardwareController.RegisterHardwareDevice(dbgr);
            bool passed = false, failed = false;
            int cyclesLeft = 1000000;
            dbgr.UnitTestFinished += pass => { if (pass) passed = true; else failed = true; };

            do
            {
                cyclesLeft -= sys.Cpu.InstructionStep();
            } while (cyclesLeft > 0 && !passed && !failed);
            if (!passed) failed = true;

            return passed;
        }

        public void InitialiseProject(IProject proj)
        {
        }

        public IEnumerable<string> GetOutputAbsolutePaths(IProject proj)
        {
            return Enumerable.Empty<string>();
        }
    }
}
