using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Devkit.CodeSafetyPlugin.View;
using Devkit.CodeSafetyPlugin.ViewModel;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;

namespace Devkit.CodeSafetyPlugin
{
    public class CodeSafetyPlugin : IPlugin
    {
        private IWorkspace _workspace;
        private IMemoryController _memController;
        private List<CodeWriteHookDevice> _hooks;
        private CodeSafetyUi _ui;

        public Guid Guid
        {
            get { return new Guid("b642f7d2-ef0b-4cb3-9107-1ef635a64e52"); }
        }

        public string Name
        {
            get { return "Code Safety"; }
        }

        public string Description
        {
            get { return "Monitors and warns when user code is changed."; }
        }

        public string Author
        {
            get { return "Team Chicken"; }
        }

        public string Version
        {
            get { return "1.7.3"; }
        }

        public string Url
        {
            get { return "http://0x10c-devkit.com/"; }
        }

        public IEnumerable<string> ActionNames
        {
            get
            {
                yield return "Reset";
                yield return "About";
            }
        }

        public void Load(IWorkspace workspace)
        {
            this._hooks = new List<CodeWriteHookDevice>();

            // grab a reference to some useful interfaces
            this._workspace = workspace;
            this._memController = workspace.RuntimeManager.System.MemoryController;
            this._ui = new CodeSafetyUi(this);

            workspace.BuildManager.BuildStatusChanged += new Delegates.BuildStatusChangedHandler(BuildManagerBuildStatusChanged);
            workspace.RuntimeManager.DebugInfoChanged += new Delegates.DebugInfoChangedHandler(RuntimeManagerDebugInfoChanged);
            workspace.RuntimeManager.ExecutionBreak += new Delegates.ExecutionBreakHandler(RuntimeManagerExecutionBreak);
        }

        public void Action(string actionName)
        {
            switch (actionName)
            {
                case "About":
                    MessageBox.Show(string.Format("{0} Version {1}", this.Name, this.Version), this.Name);
                    break;

                case "Reset":
                    RecreateHooks();
                    MessageBox.Show("Code Safety plugin reset.", this.Name);
                    break;
            }
        }

        private void BuildManagerBuildStatusChanged(BuildStatus status)
        {
            // remove hooks when building or failed, so for example we don't detect when 
            // a solution is rebuilt and data loaded over existing app
            if (status == BuildStatus.Building || status == BuildStatus.BuildFailed)
            {
                ClearHooks();
            }
        }

        public void Unload(IWorkspace workspace)
        {
            ClearHooks();
        }

        private void RuntimeManagerDebugInfoChanged()
        {
            RecreateHooks();
        }

        private void RuntimeManagerExecutionBreak(bool stoppedCompletely)
        {
            // if messages only surpressed this session, and session ends.. then reset
            if (stoppedCompletely && this._ui.DontInformThisSession)
            {
                this._ui.AlwaysInform = true;
            }
        }

        internal void OnCodeWritten(ushort address, ushort data)
        {
            var dbgInfo = this._workspace.RuntimeManager.CurrentDebugInfo;
            dbgInfo.RemoveDebugInformation(address);

            RecreateHooks();

            if (this._ui.AlwaysInform)
            {
                this._ui.SafetyMessage = string.Format("The contents of address {0:X4} were overwritten with value {1:X4}.", address, data);
                this._workspace.UiDispatcher.Invoke(new Action(() => new CodeSafetyPopup {DataContext = this._ui}.ShowDialog()));
            }
        }

        public void UserBreak()
        {
            this._workspace.RuntimeManager.Break();
        }

        private void RecreateHooks()
        {
            ClearHooks();
            AddHooks();
        }

        private void ClearHooks()
        {
            foreach (var hook in this._hooks)
            {
                this._memController.UnregisterMemoryDevice(hook);
            }
            this._hooks.Clear();
        }

        private void AddHooks()
        {
            var dbgInfo = this._workspace.RuntimeManager.CurrentDebugInfo;
            var offsets = dbgInfo.GetDebugInfoOffsets();
            long rangeStart = -1;
            long rangeLength = 0;

            var ranges = new List<Tuple<long, long>>();

            foreach (var offset in offsets)
            {
                // only look at code, not data..
                MemoryRangeType type;
                dbgInfo.GetSourceInfo(offset, out type);
                if (type != MemoryRangeType.Code) continue;

                if (rangeStart == -1)
                {
                    rangeStart = offset;
                    rangeLength = 1;
                }
                else if (offset > rangeStart + rangeLength)
                {
                    ranges.Add(new Tuple<long, long>(rangeStart, rangeStart + rangeLength - 1));
                    rangeStart = offset;
                }
                else
                {
                    rangeLength = offset - rangeStart + 1;
                }
            }
            if (rangeStart != -1)
            {
                ranges.Add(new Tuple<long, long>(rangeStart, rangeStart + rangeLength - 1));
            }

            foreach (var range in ranges)
            {
                var hook = new CodeWriteHookDevice(this, (ushort) range.Item1, (ushort) range.Item2);
                this._memController.RegisterMemoryDevice(hook);
                this._hooks.Add(hook);
            }
        }
    }
}
