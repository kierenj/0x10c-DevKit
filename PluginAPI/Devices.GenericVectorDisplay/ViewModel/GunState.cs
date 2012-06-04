using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Devices.GenericVectorDisplay.ViewModel
{
    public class GunState
    {
        public double X;
        public double Y;
        public double Z;
        public bool On;

        public override string ToString()
        {
            return string.Format("{0}: {1:0.0},{2:0.0},{3:0.0}", On, X, Y, Z);
        }
    }
}
