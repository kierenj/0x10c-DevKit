using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces;

namespace Devices.GenericClock
{
    public class Clock : IHardwareDevice
    {
        private IEmulatedSystem _system;
        private ushort _tickCounter;
        private int _interruptNum;
        private int _interval;
        private int _interruptCountdown;

        #region Hardware interface
        public const uint Manufacturer = 0xcafe0666;
        public const uint HardwareType = 0x12d0b402;
        public const ushort Revision = 0x0001;

        public enum InterruptMessage
        {
            SET_INTERVAL = 0,
            GET_TICK_COUNT = 1,
            INTERRUPT_CONTROL = 2
        }
        #endregion

        public void Initialise(IEmulatedSystem system)
        {
            this._system = system;
        }

        public void Reset()
        {
            this._interruptNum = 0;
            this._interval = 1;
            this._tickCounter = 0;
        }

        public void Interrupt(out int additionalCycles)
        {
            // default zero cycles
            additionalCycles = 0;

            var msg = (InterruptMessage)this._system.Cpu.Registers[0];
            var param = this._system.Cpu.Registers[1];
            switch (msg)
            {
                case InterruptMessage.SET_INTERVAL:
                    this._interruptCountdown = this._interval = param;
                    this._tickCounter = 0;
                    break;

                case InterruptMessage.GET_TICK_COUNT:
                    this._system.Cpu.Registers[2] = this._tickCounter;
                    break;

                case InterruptMessage.INTERRUPT_CONTROL:
                    this._interruptNum = param;
                    break;

                default:
                    break;
            }
        }

        public void CycleTimerCompleted(object state)
        {
        }

        public void Pulse()
        {
            // detect 'turned off' condition
            if (this._interval == 0) return;

            this._tickCounter++;
            this._interruptCountdown--;
            if (this._interruptCountdown == 0 && this._interruptNum != 0)
            {
                this._system.Cpu.Interrupt((ushort)this._interruptNum);
                this._interruptCountdown = this._interval;
            }
        }

        public void Query(out uint manufacturer, out uint hardwareType, out ushort revision)
        {
            manufacturer = Manufacturer;
            hardwareType = HardwareType;
            revision = Revision;
        }
    }
}
