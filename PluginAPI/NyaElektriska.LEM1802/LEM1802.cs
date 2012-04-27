using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Devkit.Interfaces;
using NyaElektriska.LEM1802.Properties;

namespace NyaElektriska.LEM1802
{
    public class LEM1802 : IPlugin
    {
        private GPU _gpu;

        public Guid Guid
        {
            get { return new Guid("bb098861-90c9-4bd8-8973-515b5f509e9e"); }
        }

        public string Name
        {
            get { return "NYA ELEKTRISKA LEM1802"; }
        }

        public string Description
        {
            get { return "Peripheral: LEM1802 - Low Energy Monitor"; }
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
            this._gpu = new GPU(workspace);
            workspace.RuntimeManager.System.HardwareController.RegisterHardwareDevice(this._gpu);
        }

        public void Unload(IWorkspace workspace)
        {
            this._gpu.Unload();
            workspace.RuntimeManager.System.HardwareController.UnregisterHardwareDevice(this._gpu);
        }
    }
}
