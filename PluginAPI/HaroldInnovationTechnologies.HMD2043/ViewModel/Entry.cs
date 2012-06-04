using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HaroldInnovationTechnologies.HMD2043.ViewModel
{
    public class Entry : ViewModelBase
    {
        private readonly string _filename;
        private readonly string _absolutePath;
        private string _entryTypeName;
        private int _entryIndex;

        public string Filename
        {
            get { return this._filename; }
        }

        public double SizeKb
        {
            get
            {
                if (!File.Exists(this._absolutePath)) return 0;
                return (double)(new FileInfo(this._absolutePath).Length);
            }
        }

        public string EntryTypeName
        {
            get { return this._entryTypeName; }
            set { this._entryTypeName = value; OnPropertyChanged("EntryTypeName"); OnPropertyChanged("Location"); }
        }

        public int EntryIndex
        {
            get { return this._entryIndex; }
            set { this._entryIndex = value; OnPropertyChanged("EntryIndex"); OnPropertyChanged("Location"); }
        }

        public string Location
        {
            get
            {
                switch (this.EntryTypeName)
                {
                    case "File system":
                        return "(auto)";

                    case "Offset":
                        return string.Format("Offset {0}", this.EntryIndex);

                    case "Sector":
                        return string.Format("Sector {0}", this.EntryIndex);
                }

                return "???";
            }
        }

        public Entry(string absolutePath, string filename)
        {
            this._absolutePath = absolutePath;
            this._filename = filename;
        }

        public override int GetHashCode()
        {
            return this._absolutePath.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var that = obj as Entry;
            if (that == null) return false;
            return this._absolutePath.Equals(that._absolutePath);
        }
    }
}
