using System.Windows;

namespace NyaElektriska.LEM1802.View
{
    /// <summary>
    /// Interaction logic for SettingsEditor.xaml
    /// </summary>
    public partial class SettingsEditor
    {
        public ViewModel.Settings UiViewModel
        {
            get { return (ViewModel.Settings)DataContext; }
        }

        public SettingsEditor()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
