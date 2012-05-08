using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaroldInnovationTechnologies.HMD2043.ViewModel
{
    public class LibraryDisk : ViewModelBase
    {
        private readonly Library _library;
        private readonly Disk _disk;

        public string Name
        {
            get { return this._disk.Name; }
        }

        public Disk Disk
        {
            get { return this._disk; }
        }

        public LibraryDisk(Library library, Disk disk)
        {
            this._library = library;
            this._disk = disk;
        }

        public RelayCommand RemoveCommand
        {
            get { return new RelayCommand(Remove); }
        }

        public void Remove(object parameter)
        {
            this._library.Disks.Remove(this);
        }
    }
}
