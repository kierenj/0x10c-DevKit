using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using Devkit.Interfaces;
using HaroldInnovationTechnologies.HMD2043.FormatProviders;
using HaroldInnovationTechnologies.HMD2043.Interfaces;
using HaroldInnovationTechnologies.HMD2043.Resources;
using HaroldInnovationTechnologies.HMD2043.View;

namespace HaroldInnovationTechnologies.HMD2043
{
    public class HMD2043 : IPlugin, IDriveSystem
    {
        private Dispatcher _uiDispatcher;
        private IWorkspace _workspace;
        private DiskProjectProvider _diskProjectProvider;
        private BackgroundFlusher _flusher;
        private ViewModel.Configuration _configuration;
        private List<Drive> _drives;
        private Disk _disk;
        private StructurelessFilesystemFormat _structurelessFormat;

        public Guid Guid
        {
            get { return new Guid("a9b61d18-dae3-41ba-b80f-afd7620b7ff4"); }
        }

        public string Name
        {
            get { return "Harold Innovation Technologies HMD2043"; }
        }

        public string Description
        {
            get { return "Peripheral: HMD2043 - Harold Media Drive"; }
        }

        public string Author
        {
            get { return "Team Chicken"; }
        }

        public string Version
        {
            get { return "1.7.4"; }
        }

        public string Url
        {
            get { return "http://0x10c-devkit.com/"; }
        }

        public IEnumerable<string> ActionNames
        {
            get
            {
                yield return "Configure disks";
                yield return "Show documentation";
            }
        }

        public void Action(string name)
        {
            switch (name)
            {
                case "Configure disks":
                    var config = new Configuration { Owner = Application.Current.MainWindow, DataContext = this._configuration };
                    config.Show();
                    break;

                case "Show documentation":
                    this._workspace.ShowDocumentationWindow(
                        "HMD2043 Documentation",
                        new Uri("https://raw.github.com/gist/2495578/8a002b8095b91178a08ac14539780255dd23a154/HIT_HMD2043.txt"));
                    break;
            }
        }

        public IEnumerable<IDiskFormatProvider> DiskFormats
        {
            get { return this._workspace.GetServices<IDiskFormatProvider>(); }
        }

        public void Load(IWorkspace workspace)
        {
            this._uiDispatcher = Dispatcher.CurrentDispatcher;
            this._workspace = workspace;
            this._flusher = new BackgroundFlusher();
            this._drives = new List<Drive>();
            this._configuration = new ViewModel.Configuration(this._workspace, this);
            this._diskProjectProvider = new DiskProjectProvider(this, this._workspace);
            this._structurelessFormat = new StructurelessFilesystemFormat();

            workspace.RegisterService<IDiskFormatProvider>(this._structurelessFormat);
            workspace.BuildManager.RegisterProjectTypeProvider(this._diskProjectProvider);
        }

        public void Unload(IWorkspace workspace)
        {
            this._flusher.Stop();
            SetNumDrives(0);

            workspace.BuildManager.UnregisterProjectTypeProvider(this._diskProjectProvider);
            workspace.UnregisterService(this._structurelessFormat);
        }

        public int NumDrives
        {
            get { return this._drives.Count; }
        }

        public void SetNumDrives(int numDrives)
        {
            while (this._drives.Count > numDrives)
            {
                var drive = this._drives[this._drives.Count - 1];
                this._workspace.RuntimeManager.System.HardwareController.UnregisterHardwareDevice(drive);
                this._drives.Remove(drive);
            }
            while (this._drives.Count < numDrives)
            {
                var drive = new Drive();
                this._workspace.RuntimeManager.System.HardwareController.RegisterHardwareDevice(drive);
                this._drives.Add(drive);
            }
        }

        public void LoadDisk(int driveIndex, Disk disk)
        {
            if (!this._uiDispatcher.CheckAccess())
            {
                this._uiDispatcher.Invoke(new Action(() => LoadDisk(driveIndex, disk)));
                return;
            }

            var loadedDrive = this._configuration.Drives.SingleOrDefault(dr => dr.HasMedia && dr.Media.Disk.Filename == disk.Filename);
            if (loadedDrive != null)
            {
                loadedDrive.Eject();
            }
            var libraryDisk = this._configuration.Library.Disks.SingleOrDefault(d => d.Disk.Filename == disk.Filename);
            if (libraryDisk != null)
            {
                this._configuration.Library.RemoveDisk(libraryDisk);
            }
            var newDisk = this._configuration.Library.AddSpecificDisk(disk);
            this._configuration.Drives[driveIndex].LoadMedia(newDisk);
        }

        void IDriveSystem.LoadDisk(int driveIndex, Disk disk)
        {
            this._drives[driveIndex].ChangeMedia(disk);
        }

        public void Eject(int driveIndex)
        {
            this._drives[driveIndex].Eject();
        }

        public void RegisterForFlush(Disk disk)
        {
            this._flusher.QueueToFlush(disk);
        }
    }
}
