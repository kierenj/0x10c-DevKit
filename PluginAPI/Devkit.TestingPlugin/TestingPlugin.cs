using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces;

namespace Devkit.TestingPlugin
{
    public class TestingPlugin : IPlugin
    {
        private IWorkspace _workspace;
        private AsmUnitTestProvider _asmTestProvider;
        private TestProjectProvider _testProjectProvider;
        private Debugger _debugger;

        public Guid Guid
        {
            get { return new Guid("90341c62-ada4-4c25-acdf-ebd53c6272e4"); }
        }

        public string Name
        {
            get { return "Testing (debugger hardware device, unit tests)"; }
        }

        public string Description
        {
            get { return ""; }
        }

        public string Author
        {
            get { return "Team Chicken"; }
        }

        public string Version
        {
            get { return "1.0.0"; }
        }

        public string Url
        {
            get { return "http://0x10c-devkit.com/"; }
        }

        public IEnumerable<string> ActionNames
        {
            get { yield return "Documentation"; }
        }

        public void Action(string name)
        {
            switch (name)
            {
                case "Documentation":
                    this._workspace.ShowDocumentationWindow("Debugger device", new Uri("https://raw.github.com/gist/2718395/50c71de262ed1090b8b53478b56609a4e3a6b117/DCPU16%20debugger%20device"));
                    break;
            }
        }

        public void Load(IWorkspace workspace)
        {
            this._workspace = workspace;
            this._testProjectProvider = new TestProjectProvider(workspace);
            this._asmTestProvider = new AsmUnitTestProvider(workspace);
            this._debugger = new Debugger();

            workspace.BuildManager.RegisterProjectTypeProvider(this._testProjectProvider);
            workspace.BuildManager.RegisterFileTypeProvider(this._asmTestProvider);
            workspace.RuntimeManager.System.HardwareController.RegisterHardwareDevice(this._debugger);
        }

        public void Unload(IWorkspace workspace)
        {
            this._workspace.RuntimeManager.System.HardwareController.UnregisterHardwareDevice(this._debugger);
            this._workspace.BuildManager.UnregisterFileTypeProvider(this._asmTestProvider);
            this._workspace.BuildManager.UnregisterProjectTypeProvider(this._testProjectProvider);
        }
    }
}
