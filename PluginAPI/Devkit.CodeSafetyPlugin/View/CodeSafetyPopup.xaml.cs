using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Devkit.CodeSafetyPlugin.ViewModel;

namespace Devkit.CodeSafetyPlugin.View
{
    /// <summary>
    /// Interaction logic for CodeSafetyPopup.xaml
    /// </summary>
    public partial class CodeSafetyPopup
    {
        public CodeSafetyUi UiViewModel
        {
            get { return (CodeSafetyUi) DataContext; }
        }

        public CodeSafetyPopup()
        {
            InitializeComponent();
        }

        private void BreakClick(object sender, RoutedEventArgs e)
        {
            this.UiViewModel.Break();
            Close();
        }

        private void ResumeClick(object sender, RoutedEventArgs e)
        {
            this.UiViewModel.Resume();
            Close();
        }
    }
}
