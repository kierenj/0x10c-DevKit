using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces;

namespace Devices.GenericClock
{
    public class GenericClock : IPlugin
    {
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
            get { return "1.0.0"; }
        }

        public string Url
        {
            get { return "http://0x10c-devkit.com/"; }
        }

        public IEnumerable<string> ActionNames
        {
            get { yield break; }
        }

        public void Action(string name)
        {
        }

        public void Load(IWorkspace workspace)
        {
            this._clock = new Clock();
            workspace.RuntimeManager.System.HardwareController.RegisterHardwareDevice(this._clock);
        }

        public void Unload(IWorkspace workspace)
        {
            workspace.RuntimeManager.System.HardwareController.UnregisterHardwareDevice(this._clock);
        }
    }
}
