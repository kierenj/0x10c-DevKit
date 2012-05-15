using System;
using System.Collections.Generic;
using System.Windows;
using Devkit.Interfaces;

namespace Devkit.WebConnectPlugin
{
    public class WebConnectPlugin : IPlugin
    {
        private IEmulatedSystem _system;
        private WebDevice _webDevice;

        public Guid Guid
        {
            get { return new Guid("68e1ec48-3f4a-4c2b-803f-5a4b98d015e7"); }
        }

        public string Name
        {
            get { return "Web Connectivity"; }
        }

        public string Description
        {
            get { return "Provides web connectvity for the emulated system."; }
        }

        public string Author
        {
            get { return "Team Chicken"; }
        }

        public string Version
        {
            get { return "1.7.4"; }
        }

        public string Url
        {
            get { return "http://0x10c-devkit.com/"; }
        }

        public IEnumerable<string> ActionNames
        {
            get
            {
                yield return "Reset";
                yield return "About";
            }
        }

        public void Load(IWorkspace workspace)
        {
            // grab a reference to the system
            this._system = workspace.RuntimeManager.System;

            // create a new WebDevice (default parameters)
            this._webDevice = new WebDevice();

            // register the device with the controllers
            this._system.MemoryController.RegisterMemoryDevice(this._webDevice);
        }

        public void Action(string actionName)
        {
            switch (actionName)
            {
                case "About":
                    MessageBox.Show(string.Format("{0} Version {1}", this.Name, this.Version), this.Name);
                    break;

                case "Reset":
                    if (this._webDevice != null)
                    {
                        this._webDevice.Reset();
                        MessageBox.Show("Web connectivity device reset.", this.Name);
                    }
                    else
                    {
                        MessageBox.Show("Web connectivity device not active.", this.Name);
                    }
                    break;
            }
        }

        public void Unload(IWorkspace workspace)
        {
            if (this._webDevice != null)
            {
                // unregister the device
                this._system.MemoryController.UnregisterMemoryDevice(this._webDevice);
                this._webDevice = null;
            }
        }
    }
}
