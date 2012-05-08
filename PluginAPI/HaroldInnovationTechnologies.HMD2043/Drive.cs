using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Devkit.Interfaces;

namespace HaroldInnovationTechnologies.HMD2043
{
    public class Drive : IHardwareDevice
    {
        private IEmulatedSystem _system;

        private ushort _interruptMessage;
        private InterruptType _lastInterruptType;
        private DeviceFlagBits _deviceFlags;
        private Disk _currentDisk;
        private int _currentSector;
        private int _currentHeadSector;
        private ushort _currentMemoryAddress;
        private int _sectorsRemaining;
        private OperationType _currentOperation;

        #region Hardware interface
        public const int Manufacturer = 0x21544948;
        public const int HardwareType = 0x74fa4cae;
        public const int Revision = 0x07c2;

        public enum InterruptMessage : ushort
        {
            QUERY_MEDIA_PRESENT = 0x0000,
            QUERY_MEDIA_PARAMETERS = 0x0001,
            QUERY_DEVICE_FLAGS = 0x0002,
            UPDATE_DEVICE_FLAGS = 0x0003,
            QUERY_INTERRUPT_TYPE = 0x0004,
            SET_INTERRUPT_MESSAGE = 0x0005,
            READ_SECTORS = 0x0010,
            WRITE_SECTORS = 0x0011,
            QUERY_MEDIA_QUALITY = 0xffff
        }

        [Flags]
        public enum DeviceFlagBits : ushort
        {
            NONE = 0x0,
            NON_BLOCKING = 0x1,
            MEDIA_STATUS_INTERRUPT = 0x2
        }

        public enum InterruptType : ushort
        {
            NONE = 0x0000,
            MEDIA_STATUS = 0x0001,
            READ_COMPLETE = 0x0002,
            WRITE_COMPLETE = 0x0003
        }

        public enum ErrorCode : ushort
        {
            ERROR_NONE = 0x0000,
            ERROR_NO_MEDIA = 0x0001,
            ERROR_INVALID_SECTOR = 0x0002,
            ERROR_PENDING = 0x0003
        }
        #endregion

        private const double RPM = 300.0;
        private const double FullStrokeTimeSec = 0.2;

        private enum OperationType
        {
            None,
            Read,
            Write
        }

        private bool MediaStatusInterruptFlag
        {
            get { return (this._deviceFlags & DeviceFlagBits.MEDIA_STATUS_INTERRUPT) != 0; }
        }

        private bool NonBlockingFlag
        {
            get { return (this._deviceFlags & DeviceFlagBits.NON_BLOCKING) != 0; }
        }

        public void Initialise(IEmulatedSystem system)
        {
            this._system = system;
        }

        public void Reset()
        {
            this._interruptMessage = 0xffff;
            this._deviceFlags = DeviceFlagBits.NONE;
            this._lastInterruptType = InterruptType.NONE;
            this._currentOperation = OperationType.None;
            this._currentSector = 0;
            this._sectorsRemaining = 0;
            this._currentHeadSector = 0;
            this._currentMemoryAddress = 0x0000;
        }

        public void Query(out uint manufacturer, out uint hardwareType, out ushort revision)
        {
            manufacturer = Manufacturer;
            hardwareType = HardwareType;
            revision = Revision;
        }

        public void Eject()
        {
            ChangeMedia(null);
        }

        public void ChangeMedia(Disk disk)
        {
            this._currentDisk = disk;
            this._currentHeadSector = 0;

            if (this.MediaStatusInterruptFlag)
            {
                RaiseInterrupt(InterruptType.MEDIA_STATUS);
            }
        }

        private void RaiseInterrupt(InterruptType type)
        {
            this._lastInterruptType = type;
            this._system.Cpu.Interrupt(this._interruptMessage);
        }

        public void Pulse()
        {
        }

        public void Interrupt(out int additionalCycles)
        {
            // default to 0 cycles
            additionalCycles = 0;

            var msg = (InterruptMessage)this._system.Cpu.Registers[0];
            var param = this._system.Cpu.Registers[1];
            switch (msg)
            {
                case InterruptMessage.QUERY_MEDIA_PRESENT:
                    this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NONE;
                    this._system.Cpu.Registers[1] = this._currentDisk != null ? (ushort)1 : (ushort)0;
                    break;

                case InterruptMessage.QUERY_MEDIA_PARAMETERS:
                    if (this._currentDisk == null)
                    {
                        this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NO_MEDIA;
                        break;
                    }
                    this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NONE;
                    this._system.Cpu.Registers[1] = (ushort)this._currentDisk.WordsPerSector;
                    this._system.Cpu.Registers[2] = (ushort)this._currentDisk.NumSectors;
                    this._system.Cpu.Registers[3] = this._currentDisk.WriteLocked ? (ushort)1 : (ushort)0;
                    break;

                case InterruptMessage.QUERY_DEVICE_FLAGS:
                    this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NONE;
                    this._system.Cpu.Registers[1] = (ushort)this._deviceFlags;
                    break;

                case InterruptMessage.UPDATE_DEVICE_FLAGS:
                    this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NONE;
                    this._deviceFlags = (DeviceFlagBits)this._system.Cpu.Registers[1];
                    break;

                case InterruptMessage.QUERY_INTERRUPT_TYPE:
                    this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NONE;
                    this._system.Cpu.Registers[1] = (ushort)this._lastInterruptType;
                    break;

                case InterruptMessage.SET_INTERRUPT_MESSAGE:
                    this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NONE;
                    this._interruptMessage = this._system.Cpu.Registers[1];
                    break;

                case InterruptMessage.READ_SECTORS:
                    if (this._currentDisk == null)
                    {
                        this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NO_MEDIA;
                        break;
                    }
                    if (this._currentOperation != OperationType.None)
                    {
                        this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_PENDING;
                        break;
                    }
                    if (param >= this._currentDisk.NumSectors || param + this._system.Cpu.Registers[2] > this._currentDisk.NumSectors)
                    {
                        this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_INVALID_SECTOR;
                        break;
                    }

                    this._currentOperation = OperationType.Read;
                    this._currentSector = param;
                    this._sectorsRemaining = this._system.Cpu.Registers[2];
                    this._currentMemoryAddress = this._system.Cpu.Registers[3];
                    if (!this.NonBlockingFlag) this._system.BlockExecution(true);
                    QueueNextStep();
                    this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NONE;
                    break;

                case InterruptMessage.WRITE_SECTORS:
                    if (this._currentDisk == null)
                    {
                        this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NO_MEDIA;
                        break;
                    }
                    if (this._currentOperation != OperationType.None)
                    {
                        this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_PENDING;
                        break;
                    }
                    if (param >= this._currentDisk.NumSectors || param + this._system.Cpu.Registers[2] > this._currentDisk.NumSectors)
                    {
                        this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_INVALID_SECTOR;
                        break;
                    }

                    this._currentOperation = OperationType.Write;
                    this._currentSector = param;
                    this._sectorsRemaining = this._system.Cpu.Registers[2];
                    this._currentMemoryAddress = this._system.Cpu.Registers[3];
                    if (!this.NonBlockingFlag) this._system.BlockExecution(true);
                    QueueNextStep();
                    this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NONE;
                    break;

                case InterruptMessage.QUERY_MEDIA_QUALITY:
                    if (this._currentDisk == null)
                    {
                        this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NO_MEDIA;
                        break;
                    }
                    this._system.Cpu.Registers[0] = (ushort)ErrorCode.ERROR_NONE;
                    this._system.Cpu.Registers[1] = (ushort)this._currentDisk.MediaType;
                    break;
            }
        }

        private void QueueNextStep()
        {
            if (this._sectorsRemaining-- <= 0)
            {
                if (!this.NonBlockingFlag)
                {
                    // we were blocking: now unblock
                    this._system.BlockExecution(false);
                }
                else
                {
                    // we weren't blocking, fire interrupt
                    switch (this._currentOperation)
                    {
                        case OperationType.Read:
                            RaiseInterrupt(InterruptType.READ_COMPLETE);
                            break;

                        case OperationType.Write:
                            RaiseInterrupt(InterruptType.WRITE_COMPLETE);
                            break;
                    }
                }

                this._currentOperation = OperationType.None;
                return;
            }

            double seekTimeSec =
                Math.Floor(Math.Abs(this._currentSector - this._currentHeadSector) / (double)this._currentDisk.SectorsPerTrack)
                * FullStrokeTimeSec / ((double)this._currentDisk.NumTracks - 1.0);
            double ioTimeSec = 1.0 / ((RPM / 60.0) * this._currentDisk.SectorsPerTrack);
            double delayTime = seekTimeSec + ioTimeSec;

            this._system.StartTimer(delayTime, this, null);
        }

        public void CycleTimerCompleted(object state)
        {
            if (this._currentOperation == OperationType.None)
            {
                // this shouldn't happen, perhaps the drive was reset while an operation was pending,
                // but the whole system should be reset in that case.. break if debugging
                if (Debugger.IsAttached) Debugger.Break();
                return;
            }

            // move head to the targetted sector
            this._currentHeadSector = this._currentSector;

            // do the operation for the current sector
            switch (this._currentOperation)
            {
                case OperationType.Read:
                    var dataRead = this._currentDisk.ReadSector(this._currentHeadSector);
                    foreach (var datum in dataRead)
                    {
                        this._system.MemoryController.Write(this._currentMemoryAddress, datum);
                        this._currentMemoryAddress = (ushort)((this._currentMemoryAddress + 1) & 0xffff);
                    }
                    break;

                case OperationType.Write:
                    int wordCount = this._currentDisk.WordsPerSector;
                    var dataToWrite = new ushort[wordCount];
                    for (int i = 0; i < wordCount; i++)
                    {
                        dataToWrite[i] = this._system.MemoryController.Read(this._currentMemoryAddress);
                        this._currentMemoryAddress = (ushort)((this._currentMemoryAddress + 1) & 0xffff);
                    }
                    this._currentDisk.WriteSector(this._currentHeadSector, dataToWrite);
                    break;
            }

            if (this._sectorsRemaining > 0)
            {
                // set target sector to the next one
                this._currentSector++;
            }

            QueueNextStep();
        }
    }
}
