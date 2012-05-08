using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Devkit.BinaryResourcePlugin.View;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;

namespace Devkit.BinaryResourcePlugin
{
    public class BinaryResourcePlugin : IPlugin, IFileTypeProvider
    {
        private IEditorControlStrategy _editorStrategy;

        public Guid Guid
        {
            get { return new Guid("dda0681c-5b9b-4582-aee6-1b58cd9f25fe"); }
        }

        public string Name
        {
            get { return "Binary Resource support"; }
        }

        public string Description
        {
            get { return "File type provider plugin for binary resources (.bin)"; }
        }

        public string Author
        {
            get { return "Team Chicken"; }
        }

        public string Version
        {
            get { return "1.7.3"; }
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
            this._editorStrategy = new BinaryFileEditorStrategy();
            workspace.BuildManager.RegisterFileTypeProvider(this);
        }

        public void Unload(IWorkspace workspace)
        {
            workspace.BuildManager.UnregisterFileTypeProvider(this);
        }

        public string FileTypeName
        {
            get { return "Binary Resource file"; }
        }

        public IEnumerable<string> DefaultFileExtensions
        {
            get
            {
                yield return ".bin";
                yield return ".dat";
            }
        }

        public ISourceFileScope CreateFileScope(IFile file, IProjectScope projectScope, CompileToolContext context)
        {
            var identifier = file.GetProperty("Identifier");
            if (string.IsNullOrEmpty(identifier))
            {
                context.AddMessage(new BinaryCompileMessage { Filename = file.AbsolutePath, Line = 0, Message = "No identifier was specified for the file", MessageLevel = Level.Error});
                return null;
            }

            return new BinaryFileScope(projectScope, identifier, file.AbsolutePath);
        }

        public IEditorControlStrategy EditorControlStrategy
        {
            get { return this._editorStrategy; }
        }

        public bool CanCreateNew
        {
            get { return false; }
        }

        public string GetDefaultFileContent(IOpenFile openFile)
        {
            return null;
        }
    }
}
