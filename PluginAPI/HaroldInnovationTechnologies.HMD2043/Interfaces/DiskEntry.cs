using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaroldInnovationTechnologies.HMD2043.Interfaces
{
    public abstract class DiskEntry
    {
        public string Name { get; set; }
        public ushort[] Words { get; set; }
    }
}
