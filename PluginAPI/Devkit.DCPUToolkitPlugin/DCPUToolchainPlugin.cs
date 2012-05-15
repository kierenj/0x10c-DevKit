using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Devkit.Interfaces;

namespace Devkit.DCPUToolchainPlugin
{
    public class DCPUToolchainPlugin : IPlugin
    {
        private FiletypeProvider _provider;

        public Guid Guid
        {
            get { return new Guid("a8d0c909-5eaa-46f6-ba08-09fb4bbd3432"); }
        }

        public string Name
        {
            get { return "DCPU-16 Toolchain Integration for DevKit"; }
        }

        public string Description
        {
            get { return "DCPU-16 Toolchain language provider"; }
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
            get { yield return "Licensing information"; }
        }

        public void Action(string name)
        {
            switch (name)
            {
                case "Licensing information":
                    MessageBox.Show(
@"This plugin uses DCPU-16 Toolchain.  The license follows:

Copyright (C) 2012 DCPU-16 Tools Team

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the ""Software""), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.",
                        "License information",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    break;
            }
        }

        public void Load(IWorkspace workspace)
        {
            this._provider = new FiletypeProvider(workspace);
            workspace.BuildManager.RegisterFileTypeProvider(this._provider);
        }

        public void Unload(IWorkspace workspace)
        {
            workspace.BuildManager.UnregisterFileTypeProvider(this._provider);
        }
    }
}
