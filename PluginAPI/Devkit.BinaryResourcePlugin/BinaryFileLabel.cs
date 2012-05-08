using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces.Build;

namespace Devkit.BinaryResourcePlugin
{
    public class BinaryFileLabel : ICompilerScopeObject
    {
        private LabelInstruction _label;
        private string _name;

        public BinaryFileLabel(string name, LabelInstruction label)
        {
            this._name = name;
            this._label = label;
        }

        public string Name
        {
            get { return this._name; }
        }

        public ICompilerScopeObjectData Data
        {
            get { return this._label; }
        }
    }
}
