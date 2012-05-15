using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces;

namespace Devkit.ExampleBASICPlugin
{
    public class ExampleBASICPlugin : IPlugin
    {
        private FiletypeProvider _provider;

        public Guid Guid
        {
            get { return new Guid("5d9dcc11-11e7-445d-b92c-be805d4440b1"); }
        }

        public string Name
        {
            get { return "Example BASIC for DevKit"; }
        }

        public string Description
        {
            get { return "Example BASIC language provider"; }
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
            get { yield break; }
        }

        public void Action(string name)
        {
        }

        public void Load(IWorkspace workspace)
        {
            this._provider = new FiletypeProvider();
            workspace.BuildManager.RegisterFileTypeProvider(this._provider);
        }

        public void Unload(IWorkspace workspace)
        {
            workspace.BuildManager.UnregisterFileTypeProvider(this._provider);
        }
    }
}
