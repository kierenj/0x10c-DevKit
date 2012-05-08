using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaroldInnovationTechnologies.HMD2043.ViewModel
{
    public class Drive : ViewModelBase
    {
        private readonly Configuration _configuration;
        private readonly int _index;
        private LibraryDisk _media;

        public string Name
        {
            get { return string.Format("Drive {0}", (char)('A' + this._index)); }
        }

        public string MediaDescription
        {
            get { return this._media == null ? "No media loaded" : this._media.Name; }
        }

        public LibraryDisk Media
        {
            get { return this._media; }
        }

        public bool HasMedia
        {
            get { return this._media != null; }
        }

        public Drive(Configuration config, int index)
        {
            this._configuration = config;
            this._index = index;
        }

        public RelayCommand EjectCommand
        {
            get { return new RelayCommand(Eject); }
        }

        public void Eject(object parameter = null)
        {
            if (this._media == null) return;
            this._configuration.Library.Disks.Add(this._media);
            this._configuration.DriveSystem.Eject(this._index);
            this._media = null;
            OnPropertyChanged("MediaDescription");
            OnPropertyChanged("HasMedia");
        }

        public void LoadMedia(LibraryDisk media)
        {
            if (this._media != null)
            {
                Eject();
            }

            this._configuration.DriveSystem.LoadDisk(this._index, media.Disk);
            this._configuration.Library.Disks.Remove(media);
            this._media = media;
            OnPropertyChanged("MediaDescription");
            OnPropertyChanged("HasMedia");
        }
    }
}
