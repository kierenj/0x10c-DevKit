using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;

namespace Devkit.ExampleBASICPlugin
{
    public class FiletypeProvider : IFileTypeProvider
    {
        private readonly CodeEditorStrategy _editorStrategy;

        public string FileTypeName
        {
            get { return "Example BASIC"; }
        }

        public IEnumerable<string> DefaultFileExtensions
        {
            get
            {
                yield return ".10cbas";
                yield return ".10cbasic";
                yield return ".bas";
            }
        }

        public bool CanCreateNew
        {
            get { return true; }
        }

        public IEditorControlStrategy EditorControlStrategy
        {
            get { return this._editorStrategy; }
        }

        public bool IsTextual
        {
            get { return true; }
        }

        public FiletypeProvider()
        {
            this._editorStrategy = new CodeEditorStrategy();

            this._editorStrategy.HighlightingRules.Add(new Tuple<Regex, Brush>(
                new Regex(@"REM .*"),
                Brushes.Green));
            this._editorStrategy.HighlightingRules.Add(new Tuple<Regex, Brush>(
                 new Regex(@"(?i:\b(((0x)((\d)|a|b|c|d|e|f)+)|(\d+))\b)"),
                 Brushes.Fuchsia));
            this._editorStrategy.HighlightingRules.Add(new Tuple<Regex, Brush>(
                 new Regex(@"(?i:\b(print|input|let|if|else|while|for|goto|gosub|return)\b)"),
                 Brushes.Blue));
            this._editorStrategy.HighlightingRules.Add(new Tuple<Regex, Brush>(
                new Regex("(?s:\".*?\")"),
                Brushes.Orange));
        }

        public string GetDefaultFileContent(IOpenFile openFile)
        {
            return "10 REM file " + Path.GetFileName(openFile.AbsolutePath) + " created " + DateTime.Now.ToLongDateString() + " by " + Environment.UserName + Environment.NewLine;
        }

        public ISourceFileScope CreateFileScope(IFile file, IProjectScope projectScope, CompileToolContext context)
        {
            return new FileScope(file.AbsolutePath, projectScope);
        }
    }
}
