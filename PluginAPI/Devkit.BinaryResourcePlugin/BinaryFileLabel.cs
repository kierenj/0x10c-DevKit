using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;

namespace Devkit.BinaryResourcePlugin
{
    public class BinaryFileLabel : ICompilerScopeObject, IMemoryMapped, ISourceReferenced
    {
        private LabelInstruction _label;
        private SourceReference _sourceRef;
        private string _name;

        public BinaryFileLabel(string name, LabelInstruction label, SourceReference sourceRef)
        {
            this._name = name;
            this._label = label;
            this._sourceRef = sourceRef;
        }

        public string Name
        {
            get { return this._name; }
        }

        public string SymbolTypeName
        {
            get { return "Binary file"; }
        }

        public string SymbolName
        {
            get { return "BinaryFile:" + this._name; }
        }

        public int Offset
        {
            get { return this._label.Offset; }
            set { throw new NotSupportedException("Cannot set the Offset of BinaryFileLabel."); }
        }

        public int GetSize(CompileToolContext context)
        {
            return this._label.GetSize(context);
        }

        public IEnumerable<SourceReference> SourceRefs
        {
            get { yield return this._sourceRef; }
        }
    }
}
