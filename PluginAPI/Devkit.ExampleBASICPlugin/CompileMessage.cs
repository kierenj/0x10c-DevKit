using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces.Build;

namespace Devkit.ExampleBASICPlugin
{
    public class CompileMessage : ICompileMessage
    {
        public Level MessageLevel { get; set; }

        public string Message { get; set; }

        public string Filename { get; set; }

        public int Line { get; set; }

        public override string ToString()
        {
            return string.Format("BASIC compilation {0}: {1} ({2}, line {3})", MessageLevel, Message, Filename, Line);
        }
    }
}
