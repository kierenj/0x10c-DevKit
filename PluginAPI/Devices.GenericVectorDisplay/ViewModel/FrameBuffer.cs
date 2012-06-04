using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MS.Internal.Xml.XPath;

namespace Devices.GenericVectorDisplay.ViewModel
{
    public class FrameBuffer
    {
        public List<GunState> States;
        public double RotatePos;
        public double AutorotateSpeed;

        public FrameBuffer()
        {
            States = new List<GunState>();
        }

        public void Add(GunState state)
        {
            lock (this)
            {
                States.Add(state);
            }
        }

        public void SetLastTrigger(bool state)
        {
            lock (this)
            {
                if (States.Count == 0) return;
                States[States.Count - 1].On = state;
            }
        }

        public IEnumerable<GunState> GetAndReset()
        {
            lock (this)
            {
                var copy = States.ToList();
                States.Clear();
                return copy;
            }
        }
    }
}
