using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces.Build;

namespace Devkit.ExampleBASICPlugin
{
    public class FileScope : ISourceFileScope
    {
        private readonly string _filename;

        public INode Parent { get; set; }

        public FileScope(string filename, IProjectScope projectScope)
        {
            this._filename = filename;
            this.Parent = projectScope;
        }

        public IEnumerable<INode> GetChildren()
        {
            yield break;
        }

        public ICompilerScopeObject Find(string name, out IIdentifierScope scope)
        {
            scope = null;
            return null;
        }

        public ICompilerScopeObject FindInChildren(string name, out IIdentifierScope scope)
        {
            scope = null;
            return null;
        }

        public void Add(ICompilerScopeObject obj)
        {
        }

        public IEnumerable<ICompilerScopeObject> Objects
        {
            get { yield break; }
        }

        public bool Compile(CompilationPass pass, ICompiler compiler, CompileToolContext context)
        {
            switch (pass)
            {
                case CompilationPass.Pass1RegisterIdentifiers:
                    // any identifiers should be registered in this scope or
                    // the parent (project) scope here
                    break;

                case CompilationPass.Pass2ExpandMacros:
                    // if applicable, this is a handy place to expand any
                    // macros or otherwise transform the code model
                    break;

                case CompilationPass.Pass3GenerateCode:
                    // here's where instructions (code/data) should be created
                    // and added to the compiler (compiler.AddInstruction)
                    context.AddMessage(new CompileMessage
                    {
                        Filename = this._filename,
                        Line = 0,
                        Message = "Example BASIC does not currently compile code",
                        MessageLevel = Level.Warning
                    });
                    break;

                case CompilationPass.Pass4FillInOffsets:
                    // at this point, all instructions in the compiler should
                    // have a known size - and so, offset.  this is where
                    // identifiers that represent labels can be resolved, with
                    // parameters or properties of instructions in the output
                    // being updated [although not in such a way that would modify
                    // their size]
                    break;
            }
            
            return true;
        }
    }
}
