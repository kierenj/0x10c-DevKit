using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devices.GenericVectorDisplay.ViewModel;
using Devkit.Interfaces;

namespace Devices.GenericVectorDisplay
{
    public class GenericVectorDisplayPlugin : IPlugin
    {
        private IWorkspace _workspace;
        private VectorDisplay _display;

        public Guid Guid
        {
            get { return new Guid("f3a9b3e5-5d56-4d38-b5f8-62daf668c515"); }
        }

        public string Name
        {
            get { return "Generic Vector Display"; }
        }

        public string Description
        {
            get { return "Generic Vector Display"; }
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
                    this._workspace.ShowDocumentationWindow("Generic Vector Display device", new Uri("https://raw.github.com/gist/2864586/f6866274ba62d1d0db400804e0e1a987227d8ac6/gistfile1.txt"));
                    break;
            }
        }

        public void Load(IWorkspace workspace)
        {
            this._workspace = workspace;
            this._display = new VectorDisplay(this._workspace, new FrameBuffer());
            workspace.RuntimeManager.System.HardwareController.RegisterHardwareDevice(this._display);
        }

        public void Unload(IWorkspace workspace)
        {
            workspace.RuntimeManager.System.HardwareController.UnregisterHardwareDevice(this._display);
        }
    }
}
