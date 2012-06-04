using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HaroldInnovationTechnologies.HMD2043
{
    public interface IDriveSystem
    {
        void RegisterForFlush(Disk disk);
        void SetNumDrives(int numDrives);
        void LoadDisk(int driveIndex, Disk disk);
        void Eject(int driveIndex);
        int NumDrives { get; }
    }
}
