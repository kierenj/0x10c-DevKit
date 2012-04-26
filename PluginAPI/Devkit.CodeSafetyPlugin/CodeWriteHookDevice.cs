using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces;

namespace Devkit.CodeSafetyPlugin
{
    public class CodeWriteHookDevice : MemoryDevice
    {
        private readonly CodeSafetyPlugin _plugin;

        public CodeWriteHookDevice(CodeSafetyPlugin plugin, ushort firstOffset, ushort lastOffset) : base(firstOffset, lastOffset, MemoryDeviceType.WriteOnly)
        {
            this._plugin = plugin;
        }

        public override void Write(ushort address, ushort data)
        {
            base.Write(address, data);

            this._plugin.OnCodeWritten(address, data);
        }
    }
}
