using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaroldInnovationTechnologies.HMD2043.Interfaces
{
    public interface IDiskFormatProvider
    {
        string FormatName { get; }
        void BuildDisk(Disk blankDisk, IEnumerable<DiskEntry> entries);
    }
}
