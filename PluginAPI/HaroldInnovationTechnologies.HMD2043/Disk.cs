using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Dk.x10c;

namespace HaroldInnovationTechnologies.HMD2043
{
    public class Disk : DiskInfo
    {
        private readonly object _lockObject = new object();
        private readonly string _filename;
        private readonly IDriveSystem _system;
        private ushort[][] _data;
        private string _name;
        private MediaType _mediaType;
        private int _wordsPerSector;
        private int _sectorsPerTrack;
        private int _numTracks;
        private bool _writeLocked;

        public const int DefaultWordsPerSector = 512;
        public const int DefaultSectorsPerTrack = 18;
        public const int DefaultNumTracks = 80;
        public const MediaType DefaultMediaType = MediaType.AuthenticHIT;

        public override string Name { get { return this._name; } }
        public override string Filename { get { return this._filename; } }
        public override MediaType MediaType { get { return this._mediaType; } }
        public override int WordsPerSector { get { return this._wordsPerSector; } }
        public override int SectorsPerTrack { get { return this._sectorsPerTrack; } }
        public override int NumTracks { get { return this._numTracks; } }
        public override bool WriteLocked { get { return this._writeLocked; } }

        public Disk(IDriveSystem system, string filename)
        {
            this._system = system;
            this._filename = filename;

            Load();
        }

        public Disk(IDriveSystem system, string name, string filename, MediaType mediaType = DefaultMediaType, int wordsPerSector = DefaultWordsPerSector,
            int sectorsPerTrack = DefaultSectorsPerTrack, int numTracks = DefaultNumTracks, bool writeLocked = false)
        {
            this._system = system;
            this._name = name;
            this._filename = filename;
            this._mediaType = mediaType;
            this._wordsPerSector = wordsPerSector;
            this._sectorsPerTrack = sectorsPerTrack;
            this._numTracks = numTracks;
            this._writeLocked = writeLocked;

            this._data = new ushort[this.NumSectors][];
            for (int i = 0; i < this.NumSectors; i++)
            {
                this._data[i] = new ushort[this._wordsPerSector];
            }

            Save();
        }

        private void Load()
        {
            lock (this._lockObject)
            {
                var headers = new Dictionary<string, string>();
                var allData = BinaryImage.ReadImage(this._filename, headers);
                this._mediaType = (MediaType) Enum.Parse(typeof (MediaType), headers["media-type"]);
                this._wordsPerSector = int.Parse(headers["words-per-sector"]);
                this._sectorsPerTrack = int.Parse(headers["sectors-per-track"]);
                this._numTracks = int.Parse(headers["tracks"]);
                this._name = headers["disk-name"];
                this._writeLocked = headers["access"] == "Read-Only";

                this._data = new ushort[this.NumSectors][];
                int srcIndex = 0;
                for (int i = 0; i < this.NumSectors; i++)
                {
                    this._data[i] = new ushort[this.WordsPerSector];
                    for (int j = 0; j < this.WordsPerSector; j++)
                    {
                        this._data[i][j] = allData[srcIndex++];
                    }
                }
            }
        }

        public void Save()
        {
            lock (this._lockObject)
            {
                string filename;
                Dictionary<string, string> headers;
                var allData = GetSaveData(out filename, out headers);
                BinaryImage.WriteImage(filename, allData.ToArray(), headers);
            }
        }

        public void SetData(ushort[] words)
        {
            var dataLeft = words.ToList();
            int secNum = 0;
            while (dataLeft.Count > 0)
            {
                int sectorLength = this._data[secNum].Length;
                if (dataLeft.Count >= sectorLength)
                {
                    this._data[secNum] = dataLeft.Take(sectorLength).ToArray();
                    dataLeft.RemoveRange(0, sectorLength);
                }
                else
                {
                    Array.Copy(dataLeft.ToArray(), this._data[secNum], dataLeft.Count);
                    dataLeft.Clear();
                }
            }
        }

        internal ushort[] GetSaveData(out string filename, out Dictionary<string,string> headers)
        {
            lock (this._lockObject)
            {
                headers = new Dictionary<string, string>();
                headers["media-type"] = this._mediaType.ToString();
                headers["words-per-sector"] = this._wordsPerSector.ToString(CultureInfo.InvariantCulture);
                headers["sectors-per-track"] = this._sectorsPerTrack.ToString(CultureInfo.InvariantCulture);
                headers["tracks"] = this._numTracks.ToString(CultureInfo.InvariantCulture);
                headers["access"] = this._writeLocked ? "Read-Only" : "Read-Write";
                headers["type"] = "Floppy";
                headers["disk-name"] = this._name;

                var allData = new List<ushort>();
                for (int i = 0; i < this.NumSectors; i++)
                {
                    allData.AddRange(this._data[i]);
                }

                filename = this._filename;

                return allData.ToArray();
            }
        }

        public virtual void WriteSector(int sector, ushort[] data)
        {
            lock (this._lockObject)
            {
                this._data[sector] = data;
            }
            this._system.RegisterForFlush(this);
        }

        public virtual ushort[] ReadSector(int sector)
        {
            lock (this._lockObject)
            {
                return this._data[sector];
            }
        }
    }
}
