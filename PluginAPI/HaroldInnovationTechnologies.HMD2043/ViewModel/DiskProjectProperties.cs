using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;
using HaroldInnovationTechnologies.HMD2043.Interfaces;

namespace HaroldInnovationTechnologies.HMD2043.ViewModel
{
    public class DiskProjectProperties : INotifyPropertyChanged, IProjectProperties
    {
        private readonly IProjectProperties _project;
        private readonly IWorkspace _workspace;
        private readonly IDriveSystem _driveSystem;
        private Entry _selectedEntry;

        public event PropertyChangedEventHandler PropertyChanged;

        public DiskProjectProperties(IWorkspace workspace, IDriveSystem driveSystem, IProjectProperties project)
        {
            this._workspace = workspace;
            this._project = project;
            this._driveSystem = driveSystem;
        }

        public Entry SelectedEntry
        {
            get { return this._selectedEntry; }
            set { this._selectedEntry = value; OnPropertyChanged("SelectedEntry"); }
        }

        public IEnumerable<Entry> Entries
        {
            get { return GenerateEntries(); }
        }

        public IEnumerable<string> DriveNames
        {
            get { return Enumerable.Range(0, this._driveSystem.NumDrives).Select(n => string.Format("Drive {0}", (char)('A' + n))); }
        }

        public bool AutoLoad
        {
            get { return bool.Parse(GetProperty("autoLoad.enabled") ?? "false"); }
            set { SetProperty("autoLoad.enabled", value.ToString(CultureInfo.InvariantCulture)); }
        }

        public string AutoLoadDriveName
        {
            get
            {
                var idx = int.Parse(GetProperty("autoLoad.drive") ?? "0");
                return string.Format("Drive {0}", (char)('A' + idx));
            }
            set
            {
                SetProperty("autoLoad.drive", (value.Substring(6)[0] - 'A').ToString(CultureInfo.InvariantCulture));
            }
        }

        public IEnumerable<string> FormatNames
        {
            get { return this._workspace.GetServices<IDiskFormatProvider>().Select(dfp => dfp.FormatName); }
        }

        public DiskProjectProperties PropertyIndexer
        {
            get { return this; }
        }

        public string this[string propertyName]
        {
            get { return GetProperty(propertyName); }
            set { SetProperty(propertyName, value); }
        }

        private void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public string Version
        {
            get { return this._project.Version; }
            set { throw new NotSupportedException(); }
        }

        public IList<Reference> References
        {
            get { return this._project.References; }
        }

        public IList<IFile> Files
        {
            get { return this._project.Files; }
        }

        public string TypeCode
        {
            get { return this._project.TypeCode; }
            set { throw new NotSupportedException(); }
        }

        public string Path
        {
            get { return this._project.Path; }
            set { throw new NotSupportedException(); }
        }

        public string Name
        {
            get { return this._project.Name; }
            set { throw new NotSupportedException(); }
        }

        public string AbsolutePath
        {
            get { return this._project.AbsolutePath; }
        }

        private IEnumerable<Entry> GenerateEntries()
        {
            var entriesList = new List<Entry>();
            foreach (var file in this._project.Files)
            {
                var fet = file.GetProperty("disk.entryTypeName");
                Entry newEntry;
                switch (fet)
                {
                    case "sector":
                        newEntry = new Entry(file.AbsolutePath, file.Path)
                        {
                            EntryTypeName = "Sector",
                            EntryIndex = int.Parse(file.GetProperty("disk.index") ?? "0")
                        };
                        break;

                    case "offset":
                        newEntry = new Entry(file.AbsolutePath, file.Path)
                        {
                            EntryTypeName = "Offset",
                            EntryIndex = int.Parse(file.GetProperty("disk.index") ?? "0")
                        };
                        break;

                    case null:
                    case "":
                    case "fileSystem":
                        newEntry = new Entry(file.AbsolutePath, file.Path)
                        {
                            EntryTypeName = "File system"
                        };
                        break;

                    default:
                        continue;
                }

                var file1 = file;
                newEntry.PropertyChanged += (s, e) =>
                {
                    switch (newEntry.EntryTypeName)
                    {
                        case "File system":
                            file1.SetProperty("disk.index", "");
                            file1.SetProperty("disk.entryTypeName", "fileSystem");
                            break;
                        case "Offset":
                            file1.SetProperty("disk.index", newEntry.EntryIndex.ToString(CultureInfo.InvariantCulture));
                            file1.SetProperty("disk.entryTypeName", "offset");
                            break;
                        case "Sector":
                            file1.SetProperty("disk.index", newEntry.EntryIndex.ToString(CultureInfo.InvariantCulture));
                            file1.SetProperty("disk.entryTypeName", "sector");
                            break;
                    }

                    // signal property is dirty
                    var versionIncrement = int.Parse(this._project.GetProperty("versionIncrement") ?? "0") + 1;
                    this._project.SetProperty("versionIncrement", versionIncrement.ToString(CultureInfo.InvariantCulture));
                };

                entriesList.Add(newEntry);
            }

            return entriesList;
        }

        public IEnumerable<string> GetPropertyNames()
        {
            return this._project.GetPropertyNames();
        }

        public string GetProperty(string propertyName)
        {
            return this._project.GetProperty(propertyName);
        }

        public void SetProperty(string propertyName, string propertyValue)
        {
            if (this._project.GetProperty(propertyName) == propertyValue) return;

            this._project.SetProperty(propertyName, propertyValue);
            OnPropertyChanged("");
        }
    }
}
