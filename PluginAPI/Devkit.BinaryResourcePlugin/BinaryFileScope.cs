using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;

namespace Devkit.BinaryResourcePlugin
{
    public class BinaryFileScope : ISourceFileScope
    {
        public INode Parent { get; set; }
        private string _filename;
        private string _labelIdentifier;
        private SourceReference _sourceRef;
        private LabelInstruction _labelInstruction;

        public BinaryFileScope(INode parent, string labelIdentifier, string filename)
        {
            this.Parent = parent;
            this._labelIdentifier = labelIdentifier;
            this._filename = filename;

            this._sourceRef = new SourceReference(filename, 0, 0, 0, 0, 0);
        }

        public IEnumerable<INode> GetChildren()
        {
            yield break;
        }

        public bool Compile(CompilationPass pass, ICompiler compiler, CompileToolContext context)
        {
            var sourceRefs = new SourceReference[] { this._sourceRef };
            var project = (IProjectScope)Parent;

            switch (pass)
            {
                case CompilationPass.Pass1RegisterIdentifiers:
                    IIdentifierScope scope;
                    var existingDef = project.Find(this._labelIdentifier, out scope);
                    if (existingDef != null && scope == project)
                    {
                        context.AddMessage(new BinaryCompileMessage { Filename = this._filename, Line = 0, Message = "Identifier already exists in this scope", MessageLevel = Level.Error });
                        return false;
                    }
                    this._labelInstruction = new LabelInstruction(sourceRefs);
                    project.Add(new BinaryFileLabel(this._labelIdentifier, this._labelInstruction));
                    break;

                case CompilationPass.Pass3GenerateCode:
                    compiler.AddInstruction(this._labelInstruction);
                    compiler.AddInstruction(new BinaryFileDataInstruction(this._filename, sourceRefs));
                    break;
            }

            return true;
        }

        public ICompilerScopeObjectData Find(string name, out IIdentifierScope scope)
        {
            scope = null;
            return null;
        }

        public void Add(ICompilerScopeObject obj)
        {
        }
    }
}
