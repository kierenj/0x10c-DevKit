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
using HaroldInnovationTechnologies.HMD2043.ViewModel;

namespace HaroldInnovationTechnologies.HMD2043.View
{
    /// <summary>
    /// Interaction logic for Configuration.xaml
    /// </summary>
    public partial class Configuration : Window
    {
        public Configuration()
        {
            InitializeComponent();
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DiskMouseMove(object sender, MouseEventArgs e)
        {
            var fe = sender as FrameworkElement;
            if (fe == null) return;
            var disk = fe.DataContext as LibraryDisk;
            if (disk == null) return;

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(fe, disk, DragDropEffects.Move);
            }
        }

        private void Grid_DragEnter(object sender, DragEventArgs e)
        {
        }

        private void Grid_Drop(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent(typeof(LibraryDisk)))
            {
                var disk = e.Data.GetData(typeof (LibraryDisk)) as LibraryDisk;
                var drive = ((FrameworkElement) sender).DataContext as ViewModel.Drive;
                drive.LoadMedia(disk);
            }
        }

        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.None;
            if (e.Data.GetDataPresent(typeof(LibraryDisk)))
            {
                var disk = e.Data.GetData(typeof(LibraryDisk)) as LibraryDisk;
                e.Effects = DragDropEffects.Move;
            }
            e.Handled = true;
        }
    }
}
