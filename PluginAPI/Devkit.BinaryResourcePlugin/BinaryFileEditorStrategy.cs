using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;

namespace Devkit.BinaryResourcePlugin
{
    public class BinaryFileEditorStrategy : CustomEditorControlStrategy
    {
        public override Object CreateEditorControl(IOpenFile file)
        {
            return new View.Editor() { DataContext = file };
        }
    }
}
