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
using NyaElektriska.LEM1802.Resources;
using NyaElektriska.LEM1802.View;
using NyaElektriska.LEM1802.ViewModel;
using Settings = NyaElektriska.LEM1802.ViewModel.Settings;

namespace NyaElektriska.LEM1802
{
    public class LEM1802 : IPlugin
    {
        private readonly List<GPU> _gpus;
        private ViewModel.Settings _settings;
        private IWorkspace _workspace;

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
                yield return "Configure settings";
                yield return "Show documentation";
            }
        }

        public void Action(string name)
        {
            switch (name)
            {
                case "Configure settings":
                    new SettingsEditor { DataContext = this._settings }.ShowDialog();
                    break;

                case "Show documentation":
                    this._workspace.ShowDocumentationWindow(
                        "LEM1802 Documentation",
                        new Uri("http://dcpu.com/highnerd/rc_1/lem1802.txt"));
                    break;
            }
        }

        public LEM1802()
        {
            this._gpus = new List<GPU>();
        }

        public void Load(IWorkspace workspace)
        {
            this._workspace = workspace;
            this._settings = new Settings(this._workspace.SettingsManager, this);

            NotifyNumDevicesChanged(this._settings.NumDevices);
        }

        public void Unload(IWorkspace workspace)
        {
            foreach (var gpu in this._gpus)
            {
                gpu.Unload();
                workspace.RuntimeManager.System.HardwareController.UnregisterHardwareDevice(gpu);
            }
            this._gpus.Clear();
        }

        public void NotifyNumDevicesChanged(int numDevices)
        {
            while (this._gpus.Count > numDevices)
            {
                var removeGpu = this._gpus.Last();
                removeGpu.Unload();
                this._workspace.RuntimeManager.System.HardwareController.UnregisterHardwareDevice(removeGpu);
                this._gpus.Remove(removeGpu);
            }
            while (this._gpus.Count < numDevices)
            {
                var gpu = new GPU(this._workspace);
                this._workspace.RuntimeManager.System.HardwareController.RegisterHardwareDevice(gpu);

                this._gpus.Add(gpu);
            }
        }
    }
}
