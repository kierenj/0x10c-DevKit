using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Devkit.Interfaces;

namespace NyaElektriska.LEM1802.ViewModel
{
    public class Settings : INotifyPropertyChanged
    {
        private readonly LEM1802 _plugin;
        private readonly ISettingsManager _manager;
        private readonly string _settingsCategory;

        public enum SettingNames
        {
            NumDevices
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private int _numDevices;
        public int NumDevices
        {
            get { return this._numDevices; }
            set { this._numDevices = value; OnPropertyChanged("NumDevices"); this._plugin.NotifyNumDevicesChanged(value); Save(); }
        }

        public Settings(ISettingsManager manager, LEM1802 plugin)
        {
            this._settingsCategory = typeof(LEM1802).Name;
            this._plugin = plugin;
            this._manager = manager;

            Load();
        }

        private void Save()
        {
            this._manager.WriteSetting(this._settingsCategory, SettingNames.NumDevices.ToString(), this._numDevices.ToString(CultureInfo.InvariantCulture));
        }

        private void Load()
        {
            this._numDevices = 1;
            int.TryParse(this._manager.ReadSetting(this._settingsCategory, SettingNames.NumDevices.ToString()) ?? "1", out this._numDevices);
        }
    }
}
