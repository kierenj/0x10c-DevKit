using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using Devkit.Interfaces;

namespace Devices.GenericKeyboard
{
    public class Keyboard : IHardwareDevice
    {
        private readonly IWorkspace _workspace;
        private Dictionary<int, int> _virtualKeyMap;
        private IEmulatedSystem _system;
        private ushort _interruptNum;
        private Queue<ushort> _queue;
        private bool[] _keyStates;

        #region Hardware interface
        public const uint Manufacturer = 0xcafe0666;
        public const uint HardwareType = 0x30cf7406;
        public const ushort Revision = 0x0001;

        public enum InterruptMessage
        {
            CLEAR_BUFFER = 0,
            GET_NEXT_KEY = 1,
            CHECK_KEY_PRESSED = 2,
            INTERRUPT_CONTROL = 3
        }

        public enum KeyCodes
        {
            Backspace = 0x10,
            Return = 0x11,
            Insert = 0x12,
            Delete = 0x13,
            UpArrow = 0x80,
            DownArrow = 0x81,
            LeftArrow = 0x82,
            RightArrow = 0x83,
            Shift = 0x90,
            Control = 0x91
        }
        #endregion

        public Keyboard(IWorkspace workspace)
        {
            this._workspace = workspace;
            InitDictionary();
        }

        public void Initialise(IEmulatedSystem system)
        {
            this._system = system;
            this._workspace.RuntimeManager.UI.KeyEvent += UiOnKeyEvent;
        }

        public void Unload()
        {
            this._workspace.RuntimeManager.UI.KeyEvent -= UiOnKeyEvent;
        }

        public void Interrupt(out int additionalCycles)
        {
            // default zero cycles
            additionalCycles = 0;

            var msg = (InterruptMessage)this._system.Cpu.Registers[0];
            var param = this._system.Cpu.Registers[1];
            switch (msg)
            {
                case InterruptMessage.CLEAR_BUFFER:
                    this._queue = new Queue<ushort>();
                    break;

                case InterruptMessage.GET_NEXT_KEY:
                    this._system.Cpu.Registers[2] = (this._queue.Count == 0) ? (ushort)0 : this._queue.Dequeue();
                    break;

                case InterruptMessage.CHECK_KEY_PRESSED:
                    this._system.Cpu.Registers[2] = (ushort)(this._keyStates[param & 0xff] ? 1 : 0);
                    break;

                case InterruptMessage.INTERRUPT_CONTROL:
                    this._interruptNum = param;
                    break;

                default:
                    break;
            }
        }

        public void Query(out uint manufacturer, out uint hardwareType, out ushort revision)
        {
            manufacturer = Manufacturer;
            hardwareType = HardwareType;
            revision = Revision;
        }

        public void Reset()
        {
            this._queue = new Queue<ushort>();
            this._keyStates = new bool[256];
            this._interruptNum = 0;
        }

        public void Pulse()
        {
        }

        // VK codes from http://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx
        private enum VirtualKeys
        {
            VK_UP = 0x26,
            VK_DOWN = 0x28,
            VK_LEFT = 0x25,
            VK_RIGHT = 0x27,
            VK_BACK = 0x08,
            VK_RETURN = 0x0D,
            VK_INSERT = 0x2D,
            VK_DELETE = 0x2E,
            VK_SHIFT = 0x10,
            VK_LSHIFT = 0xA0,
            VK_RSHIFT = 0xA1,
            VK_CONTROL = 0x11,
            VK_LCONTROL = 0xA2,
            VK_RCONTROL = 0xA3
        }

        private void InitDictionary()
        {
            this._virtualKeyMap = new Dictionary<int, int>();

            this._virtualKeyMap[(int)VirtualKeys.VK_BACK] = (int)KeyCodes.Backspace;
            this._virtualKeyMap[(int)VirtualKeys.VK_RETURN] = (int)KeyCodes.Return;
            this._virtualKeyMap[(int)VirtualKeys.VK_INSERT] = (int)KeyCodes.Insert;
            this._virtualKeyMap[(int)VirtualKeys.VK_DELETE] = (int)KeyCodes.Delete;

            this._virtualKeyMap[(int)VirtualKeys.VK_UP] = (int)KeyCodes.UpArrow;
            this._virtualKeyMap[(int)VirtualKeys.VK_DOWN] = (int)KeyCodes.DownArrow;
            this._virtualKeyMap[(int)VirtualKeys.VK_LEFT] = (int)KeyCodes.LeftArrow;
            this._virtualKeyMap[(int)VirtualKeys.VK_RIGHT] = (int)KeyCodes.RightArrow;

            this._virtualKeyMap[(int)VirtualKeys.VK_SHIFT] = (int)KeyCodes.Shift;
            this._virtualKeyMap[(int)VirtualKeys.VK_LSHIFT] = (int)KeyCodes.Shift;
            this._virtualKeyMap[(int)VirtualKeys.VK_RSHIFT] = (int)KeyCodes.Shift;
            this._virtualKeyMap[(int)VirtualKeys.VK_CONTROL] = (int)KeyCodes.Control;
            this._virtualKeyMap[(int)VirtualKeys.VK_LCONTROL] = (int)KeyCodes.Control;
            this._virtualKeyMap[(int)VirtualKeys.VK_RCONTROL] = (int)KeyCodes.Control;
        }

        private void UiOnKeyEvent(object sender, KeyEventArgs keyEventArgs)
        {
            bool queued = false;
            char key;

            // build a variety of info about the keypress..
            var virtualKey = GetKeyInfo(keyEventArgs, out key);

            // use LUT
            virtualKey = this._virtualKeyMap.ContainsKey(virtualKey) ? this._virtualKeyMap[virtualKey] : -1;

            // act on event
            if (keyEventArgs.IsUp)
            {
                // released
                if (virtualKey >= 0)
                {
                    this._keyStates[virtualKey] = false;
                    queued = true;
                }
            }
            else
            {
                // pressed
                if (virtualKey >= 0 && !keyEventArgs.IsRepeat)
                {
                    this._keyStates[virtualKey] = true;
                    if (virtualKey <= 0x13)
                    {
                        this._queue.Enqueue((ushort)virtualKey);
                    }
                    queued = true;
                }

                // typed
                if ((int)key > 20 && (int)key <= 127)
                {
                    this._queue.Enqueue((ushort)key);
                    queued = true;
                }
            }

            if (queued && this._interruptNum != 0)
            {
                this._system.Cpu.Interrupt(this._interruptNum);
            }
        }

        #region Key mapping
        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)] 
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        private static int GetKeyInfo(KeyEventArgs keyEventArgs, out char key)
        {
            key = '\0';
            int virtualKey = KeyInterop.VirtualKeyFromKey(keyEventArgs.Key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);
            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            var stringBuilder = new StringBuilder(2);
            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            if (result > 0)
            {
                key = stringBuilder[0];
            }
            return virtualKey;
        }
        #endregion

    }
}
