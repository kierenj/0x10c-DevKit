using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devices.GenericClock.Resources;
using Devkit.Interfaces;

namespace Devices.GenericClock
{
    public class GenericClock : IPlugin
    {
        private IWorkspace _workspace;
        private Clock _clock;

        public Guid Guid
        {
            get { return new Guid("b4bc5440-2eca-490c-bb26-e7e1d9563667"); }
        }

        public string Name
        {
            get { return "Generic Clock"; }
        }

        public string Description
        {
            get { return "Peripheral: Generic Clock"; }
        }

        public string Author
        {
            get { return "Team Chicken"; }
        }

        public string Version
        {
            get { return "1.7.3"; }
        }

        public string Url
        {
            get { return "http://0x10c-devkit.com/"; }
        }

        public IEnumerable<string> ActionNames
        {
            get
            {
                yield return "Show documentation";
            }
        }

        public void Action(string name)
        {
            switch (name)
            {
                case "Show documentation":
                    this._workspace.ShowDocumentationWindow(
                        "Generic Clock Documentation",
                        ResourceHelper.GetContent("Devices.GenericClock.Resources.GenericClock.txt"));
                    break;
            }
        }

        public void Load(IWorkspace workspace)
        {
            this._workspace = workspace;
            this._clock = new Clock();
            workspace.RuntimeManager.System.HardwareController.RegisterHardwareDevice(this._clock);
        }

        public void Unload(IWorkspace workspace)
        {
            workspace.RuntimeManager.System.HardwareController.UnregisterHardwareDevice(this._clock);
        }
    }
}
