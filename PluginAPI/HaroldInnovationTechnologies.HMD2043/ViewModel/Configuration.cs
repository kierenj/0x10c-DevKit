using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Devkit.Interfaces;

namespace HaroldInnovationTechnologies.HMD2043.ViewModel
{
    public class Configuration : ViewModelBase
    {
        private readonly IDriveSystem _driveSystem;
        private readonly IWorkspace _workspace;
        private readonly Library _library;
        private readonly ObservableCollection<Drive> _drives;
        private bool _loadingConfig;

        public Library Library
        {
            get { return this._library; }
        }

        public ObservableCollection<Drive> Drives
        {
            get { return this._drives; }
        }

        internal IWorkspace Workspace
        {
            get { return this._workspace; }
        }

        internal IDriveSystem DriveSystem
        {
            get { return this._driveSystem; }
        }

        public int NumDrives
        {
            get { return this._drives.Count; }
            set
            {
                while (this._drives.Count > value)
                {
                    this._drives.RemoveAt(this._drives.Count - 1);
                }
                while (this._drives.Count < value)
                {
                    this._drives.Add(new Drive(this, this._drives.Count));
                }
                this._driveSystem.SetNumDrives(value);
                OnPropertyChanged("NumDrives");
                SaveConfig();
            }
        }

        public Configuration(IWorkspace workspace, IDriveSystem driveSystem)
        {
            this._workspace = workspace;
            this._driveSystem = driveSystem;
            this._library = new Library(this);
            this._drives = new ObservableCollection<Drive>();

            LoadConfig();
        }

        public void LoadConfig()
        {
            this._loadingConfig = true;

            int drives = 2;
            int.TryParse(this._workspace.SettingsManager.ReadSetting(typeof(HMD2043).Name, "NumDrives") ?? "2", out drives);
            this.NumDrives = drives;

            var files = (this._workspace.SettingsManager.ReadSetting(typeof(HMD2043).Name, "LibraryFiles") ?? "")
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Where(File.Exists);
            foreach (var file in files)
            {
                this._library.Disks.Add(new LibraryDisk(this._library, new Disk(this.DriveSystem, file)));
            }

            this._loadingConfig = false;
        }

        public void SaveConfig()
        {
            if (this._loadingConfig) return;

            this._workspace.SettingsManager.WriteSetting(typeof(HMD2043).Name, "NumDrives", this.NumDrives.ToString(CultureInfo.InvariantCulture));
            this._workspace.SettingsManager.WriteSetting(typeof(HMD2043).Name, "LibraryFiles",
                string.Join(",", this._library.Disks.Select(d => d.Disk.Filename).Concat(this._drives.Where(d => d.HasMedia).Select(d => d.Media.Disk.Filename))));
        }
    }
}
