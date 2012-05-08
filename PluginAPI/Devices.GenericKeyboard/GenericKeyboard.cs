using System;
using System.Collections.Generic;
using Devices.GenericKeyboard.Resources;
using Devkit.Interfaces;

namespace Devices.GenericKeyboard
{
    public class GenericKeyboard : IPlugin
    {
        private IWorkspace _workspace;
        private Keyboard _keyboard;

        public Guid Guid
        {
            get { return new Guid("71892552-5212-4d50-a450-007453d031b3"); }
        }

        public string Name
        {
            get { return "Generic Keyboard"; }
        }

        public string Description
        {
            get { return "Peripheral: Generic Keyboard"; }
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
                        ResourceHelper.GetContent("Devices.GenericKeyboard.Resources.GenericKeyboard.txt"));
                    break;
            }
        }

        public void Load(IWorkspace workspace)
        {
            this._workspace = workspace;
            this._keyboard = new Keyboard(workspace);
            workspace.RuntimeManager.System.HardwareController.RegisterHardwareDevice(this._keyboard);
        }

        public void Unload(IWorkspace workspace)
        {
            this._keyboard.Unload();
            workspace.RuntimeManager.System.HardwareController.UnregisterHardwareDevice(this._keyboard);
        }
    }
}
