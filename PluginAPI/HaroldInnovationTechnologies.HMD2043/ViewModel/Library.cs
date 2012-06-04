using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Devkit.Interfaces;

namespace HaroldInnovationTechnologies.HMD2043.ViewModel
{
    public class Library : ViewModelBase
    {
        private readonly Configuration _configuration;
        private readonly ObservableCollection<LibraryDisk> _disks;

        public ObservableCollection<LibraryDisk> Disks
        {
            get { return this._disks; }
        }

        public Library(Configuration config)
        {
            this._configuration = config;
            this._disks = new ObservableCollection<LibraryDisk>();
        }

        public RelayCommand AddDiskCommand
        {
            get { return new RelayCommand(AddDisk); }
        }

        public void AddDisk(object parameter = null)
        {
            var fn = this._configuration.Workspace.FileDialog("Create disk file",
                                                              "BIEF disks (*.10cdisk)|*.10cdisk|All files (*.*)|*.*",
                                                              true);
            if (fn == null) return;
            this._disks.Add(new LibraryDisk(this, new Disk(this._configuration.DriveSystem, Path.GetFileNameWithoutExtension(fn), fn)));
            this._configuration.SaveConfig();
        }

        public RelayCommand BrowseCommand
        {
            get { return new RelayCommand(Browse); }
        }

        public void Browse(object parameter = null)
        {
            var fn = this._configuration.Workspace.FileDialog("Browse for disk",
                                                              "BIEF disks (*.10cdisk)|*.10cdisk|All files (*.*)|*.*",
                                                              false);
            if (fn == null) return;
            this._disks.Add(new LibraryDisk(this, new Disk(this._configuration.DriveSystem, fn)));
            this._configuration.SaveConfig();
        }

        public void RemoveDisk(LibraryDisk disk)
        {
            this._disks.Remove(disk);
            this._configuration.SaveConfig();
        }

        public LibraryDisk AddSpecificDisk(Disk disk)
        {
            LibraryDisk newDisk;
            this._disks.Add(newDisk = new LibraryDisk(this, disk));
            this._configuration.SaveConfig();
            return newDisk;
        }
    }
}
