using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HaroldInnovationTechnologies.HMD2043.Interfaces;

namespace HaroldInnovationTechnologies.HMD2043.FormatProviders
{
    public class StructurelessFilesystemFormat : IDiskFormatProvider
    {
        public string FormatName
        {
            get { return "Structureless"; }
        }

        public void BuildDisk(Disk blankDisk, IEnumerable<DiskEntry> entries)
        {
            int maxOffset = 0;

            foreach (var ss in entries.OfType<SpecificSectorDiskEntry>())
            {
                AddSectorEntry(blankDisk, ss, ref maxOffset);
            }
            foreach (var so in entries.OfType<SpecificOffsetDiskEntry>())
            {
                AddOffsetEntry(blankDisk, so, ref maxOffset);
            }
            foreach (var fs in entries.OfType<FileSystemDiskEntry>())
            {
                AddFileSystemEntry(blankDisk, fs, ref maxOffset);
            }
        }

        private void AddFileSystemEntry(Disk disk, FileSystemDiskEntry fs, ref int maxOffset)
        {
            int offset = maxOffset;

            var data = disk.GetData();
            Array.Copy(fs.Words, 0, data, offset, fs.Words.Length);
            disk.SetData(data);

            offset += fs.Words.Length;
            maxOffset = Math.Max(offset, maxOffset);
        }

        private void AddOffsetEntry(Disk disk, SpecificOffsetDiskEntry so, ref int maxOffset)
        {
            int offset = so.Offset;

            var data = disk.GetData();
            Array.Copy(so.Words, 0, data, offset, so.Words.Length);
            disk.SetData(data);

            offset += so.Words.Length;
            maxOffset = Math.Max(offset, maxOffset);
        }

        private void AddSectorEntry(Disk disk, SpecificSectorDiskEntry ss, ref int maxOffset)
        {
            int offset = disk.WordsPerSector * ss.Sector;

            var data = disk.GetData();
            Array.Copy(ss.Words, 0, data, offset, ss.Words.Length);
            disk.SetData(data);

            offset += ss.Words.Length;
            maxOffset = Math.Max(offset, maxOffset);
        }
    }
}
