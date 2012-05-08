using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces.Build;

namespace Devkit.BinaryResourcePlugin
{
    public class BinaryCompileMessage : ICompileMessage
    {
        public Level MessageLevel { get; set; }

        public string Message { get; set; }

        public string Filename { get; set; }

        public int Line { get; set; }

        public override string ToString()
        {
            return string.Format("Binary compilation {0}: {1} ({2}, line {3})", MessageLevel, Message, Filename, Line);
        }
    }
}
