using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace Devkit.CodeSafetyPlugin.ViewModel
{
    public class CodeSafetyUi : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            var handler = PropertyChanged;
            if (handler == null) return;
            handler(this, new PropertyChangedEventArgs(name));
        }
        #endregion

        private readonly CodeSafetyPlugin _plugin;

        public string Title
        {
            get { return this._plugin.Name; }
        }

        private string _safetyMessage;
        public string SafetyMessage
        {
            get { return this._safetyMessage; }
            set { this._safetyMessage = value; OnPropertyChanged("SafetyMessage"); }
        }

        private bool _alwaysInform;
        public bool AlwaysInform
        {
            get { return this._alwaysInform; }
            set
            { 
                this._alwaysInform = value; OnPropertyChanged("AlwaysInform");
                if (value)
                {
                    this.DontInform = false;
                    this.DontInformThisSession = false;
                }
            }
        }

        private bool _dontInformThisSession;
        public bool DontInformThisSession
        {
            get { return this._dontInformThisSession; }
            set
            {
                this._dontInformThisSession = value; OnPropertyChanged("DontInformThisSession");
                if (value)
                {
                    this.AlwaysInform = false;
                    this.DontInform = false;
                }
            }
        }

        private bool _dontInform;
        public bool DontInform
        {
            get { return this._dontInform; }
            set
            {
                this._dontInform = value; OnPropertyChanged("DontInform");
                if (value)
                {
                    this.AlwaysInform = false;
                    this.DontInformThisSession = false;
                }
            }
        }

        public CodeSafetyUi(CodeSafetyPlugin plugin)
        {
            this._plugin = plugin;

            this.AlwaysInform = true;
        }

        public void Break()
        {
            this._plugin.UserBreak();
        }

        public void Resume()
        {
            // nothing to do..
        }
    }
}
