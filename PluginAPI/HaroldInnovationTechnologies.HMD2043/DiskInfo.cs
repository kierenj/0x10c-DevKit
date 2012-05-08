using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaroldInnovationTechnologies.HMD2043
{
    public abstract class DiskInfo
    {
        public abstract string Name { get; }
        public abstract string Filename { get; }
        public abstract MediaType MediaType { get; }
        public abstract int WordsPerSector { get; }
        public abstract int SectorsPerTrack { get; }
        public abstract int NumTracks { get; }
        public abstract bool WriteLocked { get; }

        public int NumSectors
        {
            get { return this.NumTracks * this.SectorsPerTrack; }
        }
    }
}
