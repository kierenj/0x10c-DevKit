using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Dk.x10c;

namespace HaroldInnovationTechnologies.HMD2043
{
    public class BackgroundFlusher
    {
        private readonly object _lockObject = new object();
        private Thread _thread;
        private ManualResetEvent _stopSignal;
        private List<Disk> _flushQueue;

        public BackgroundFlusher()
        {
            this._flushQueue = new List<Disk>();
            this._stopSignal = new ManualResetEvent(false);
            this._thread = new Thread(new ThreadStart(ThreadFunc)) { IsBackground = true };
            this._thread.Start();
        }

        public void Stop()
        {
            // signal a stop
            this._stopSignal.Set();

            // wait up to 1 sec, then abort
            if (!this._thread.Join(1000))
            {
                this._thread.Abort();
            }

            this._thread = null;
        }

        public void QueueToFlush(Disk disk)
        {
            lock(this._lockObject)
            {
                if (this._flushQueue.Contains(disk)) return;
                this._flushQueue.Add(disk);
            }
        }

        private void ThreadFunc()
        {
            do
            {
                Disk diskToWrite = null;
                lock(this._lockObject)
                {
                    if (this._flushQueue.Count > 0)
                    {
                        diskToWrite = this._flushQueue[0];
                        this._flushQueue.RemoveAt(0);
                    }
                }
                if (diskToWrite != null)
                {
                    string filename;
                    Dictionary<string, string> headers;
                    var allData = diskToWrite.GetSaveData(out filename, out headers);
                    BinaryImage.WriteImage(filename, allData.ToArray(), headers);
                }
            } while (!this._stopSignal.WaitOne(5000));
        }
    }
}
