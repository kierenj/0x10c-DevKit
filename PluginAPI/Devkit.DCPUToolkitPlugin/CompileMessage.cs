using Devkit.Interfaces.Build;

namespace Devkit.DCPUToolchainPlugin
{
    public class CompileMessage : ICompileMessage
    {
        public Level MessageLevel { get; set; }

        public string Message { get; set; }

        public string Filename { get; set; }

        public int Line { get; set; }

        public override string ToString()
        {
            return string.Format("DCPUToolchain C compilation {0}: {1} ({2}, line {3})", MessageLevel, Message, Filename, Line);
        }
    }
}
