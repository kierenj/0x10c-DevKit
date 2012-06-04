using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Devkit.Interfaces;

namespace Devkit.TestingPlugin
{
    public class Debugger : IHardwareDevice
    {
        private IEmulatedSystem _system;

        #region Hardware interface
        public const uint Manufacturer = 0xcafe0666;
        public const uint HardwareType = 0xdeb91111;
        public const ushort Revision = 0x0001;

        public enum InterruptMessage
        {
            TRIGGER_BREAKPOINT = 0,
            OUTPUT_DIAG_WORD = 1,
            OUTPUT_DIAG_WORD_ZSTRING = 2,
            OUTPUT_DIAG_WORD_PSTRING = 3,
            UNIT_TEST_PASS = 4,
            UNIT_TEST_FAIL = 5,
            SET_PROCESSOR_SPEED = 6,
            GET_PROCESSOR_SPEED = 7,
            RESET_CYCLE_COUNTER = 8,
            GET_CYCLE_COUNTER = 9
        }
        #endregion

        public delegate void BreakpointTriggeredHandler();
        public delegate void DiagnosticWordOutputHandler(ushort word);
        public delegate void DiagnosticStringOutputHandler(string text);
        public delegate void UnitTestFinishedHandler(bool pass);

        public event BreakpointTriggeredHandler BreakpointTriggered;
        public event DiagnosticWordOutputHandler DiagnosticWordOutput;
        public event DiagnosticStringOutputHandler DiagnosticStringOutput;
        public event UnitTestFinishedHandler UnitTestFinished;

        private void OnBreakpointTriggered()
        {
            var handler = this.BreakpointTriggered;
            if (handler != null) handler();
        }

        private void OnDiagnosticWordOutput(ushort word)
        {
            var handler = this.DiagnosticWordOutput;
            if (handler != null) handler(word);
        }

        private void OnDiagnosticStringOutput(string text)
        {
            var handler = this.DiagnosticStringOutput;
            if (handler != null) handler(text);
        }

        private void OnUnitTestFinished(bool pass)
        {
            var handler = this.UnitTestFinished;
            if (handler != null) handler(pass);
        }

        public void Initialise(IEmulatedSystem system)
        {
            this._system = system;
        }

        public void Reset()
        {
        }

        public void Interrupt(out int additionalCycles)
        {
            additionalCycles = 0;

            var msg = (InterruptMessage)this._system.Cpu.Registers[0];
            var param = this._system.Cpu.Registers[1];
            switch (msg)
            {
                case InterruptMessage.TRIGGER_BREAKPOINT:
                    OnBreakpointTriggered();
                    break;

                case InterruptMessage.OUTPUT_DIAG_WORD:
                    OnDiagnosticWordOutput(param);
                    break;

                case InterruptMessage.OUTPUT_DIAG_WORD_ZSTRING:
                    OnDiagnosticStringOutput("OUTPUT_DIAG_WORD_ZSTRING not implemented");
                    break;

                case InterruptMessage.OUTPUT_DIAG_WORD_PSTRING:
                    OnDiagnosticStringOutput("OUTPUT_DIAG_WORD_PSTRING not implemented");
                    break;

                case InterruptMessage.UNIT_TEST_PASS:
                    OnUnitTestFinished(true);
                    break;

                case InterruptMessage.UNIT_TEST_FAIL:
                    OnUnitTestFinished(false);
                    break;

                case InterruptMessage.SET_PROCESSOR_SPEED:
                    break;

                case InterruptMessage.GET_PROCESSOR_SPEED:
                    break;

                case InterruptMessage.RESET_CYCLE_COUNTER:
                    break;

                case InterruptMessage.GET_CYCLE_COUNTER:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Query(out uint manufacturer, out uint hardwareType, out ushort revision)
        {
            manufacturer = Manufacturer;
            hardwareType = HardwareType;
            revision = Revision;
        }

        public void Pulse()
        {
        }

        public void CycleTimerCompleted(object state)
        {
        }
    }
}
