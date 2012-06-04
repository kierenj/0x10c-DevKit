using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;
using HaroldInnovationTechnologies.HMD2043.Interfaces;
using HaroldInnovationTechnologies.HMD2043.ViewModel;

namespace HaroldInnovationTechnologies.HMD2043
{
    public class DiskProjectProvider : IProjectTypeProvider
    {
        private readonly IWorkspace _workspace;
        private readonly HMD2043 _plugin;

        public string ProjectTypeName
        {
            get { return "Disk"; }
        }

        public string TypeCode
        {
            get { return "disk"; }
        }

        public DiskProjectProvider(HMD2043 plugin, IWorkspace workspace)
        {
            this._plugin = plugin;
            this._workspace = workspace;
        }

        public object CreatePropertiesControl(IProjectProperties project)
        {
            return new View.DiskProjectProperties { DataContext = new DiskProjectProperties(this._workspace, this._plugin, project) };
        }

        public bool CanAddReference(IProject project, IProject otherProject)
        {
            return false;
        }

        private IEnumerable<ushort> ReadBytes(string filename)
        {
            var bytes = File.ReadAllBytes(filename);
            ushort currentWord = 0;
            bool firstByte = true;
            foreach (var fileByte in bytes)
            {
                if (firstByte)
                {
                    currentWord = fileByte;
                }
                else
                {
                    currentWord = (ushort)(currentWord + (fileByte << 8));
                    yield return currentWord;
                }
                firstByte = !firstByte;
            }

            if (!firstByte)
                yield return currentWord;
        }

        public BuildOutput Build(IBuildManager manager, ISolution solution, IProject project, IBuildProgressMonitor monitor)
        {
            var watch = new Stopwatch();
            var entries = new List<DiskEntry>();
            var buildSvc = this._workspace.GetService<ISimpleFileBuildService>();
            var msgs = new List<ICompileMessage>();
            var projPath = Path.GetDirectoryName(project.AbsolutePath);
            var fname = Path.Combine(projPath, project.GetProperty("outputFilename"));

            watch.Start();

            foreach (var file in project.Files)
            {
                DiskEntry newEntry;
                var fet = file.GetProperty("disk.entryTypeName");
                var op = buildSvc.Build(manager, solution, project, file, monitor, false);
                ushort[] words = op.Words;

                msgs.AddRange(op.Messages);

                if (!op.Success || words == null)
                {
                    monitor.StatusUpdate("Build failed for " + file.AbsolutePath);
                    return new BuildOutput { Success = false, Time = watch.Elapsed, Messages = msgs };
                }

                switch (fet)
                {
                    case "sector":
                        newEntry = new SpecificSectorDiskEntry { Name = file.Path, Sector = int.Parse(file.GetProperty("disk.index") ?? "0"), Words = words };
                        break;

                    case "offset":
                        newEntry = new SpecificOffsetDiskEntry { Name = file.Path, Offset = int.Parse(file.GetProperty("disk.index") ?? "0"), Words = words };
                        break;

                    case null:
                    case "":
                    case "fileSystem":
                        newEntry = new FileSystemDiskEntry { Name = file.Path, Path = file.Path, Words = words };
                        break;

                    default:
                        continue;
                }

                entries.Add(newEntry);
            }

            var disk = new Disk(this._plugin, project.Name, fname);
            disk.Compress = bool.Parse(project.GetProperty("compress") ?? "true");

            var provider = this._workspace.GetServices<IDiskFormatProvider>().Single(df => df.FormatName == project.GetProperty("filesystem.type"));
            provider.BuildDisk(disk, entries);

            disk.Save();

            watch.Stop();

            if (bool.Parse(project.GetProperty("autoLoad.enabled") ?? "false"))
            {
                int driveIndex = int.Parse(project.GetProperty("autoLoad.drive") ?? "0");
                this._plugin.LoadDisk(driveIndex, disk);
            }

            return new BuildOutput
                       {
                           CodeModel = null,
                           DebugInfo = null,
                           Messages = msgs,
                           Success = true,
                           Time = watch.Elapsed,
                           Words = disk.GetData()
                       };
        }

        public void InitialiseProject(IProject proj)
        {
            proj.SetProperty("outputFilename", proj.Name + ".bief");
            proj.SetProperty("compress", "true");
            proj.SetProperty("filesystem.enabled", "true");
            proj.SetProperty("filesystem.type", "Structureless");
        }

        public IEnumerable<string> GetOutputAbsolutePaths(IProject proj)
        {
            var projPath = Path.GetDirectoryName(proj.AbsolutePath);
            yield return Path.Combine(projPath, proj.GetProperty("outputFilename"));
        }
    }
}
