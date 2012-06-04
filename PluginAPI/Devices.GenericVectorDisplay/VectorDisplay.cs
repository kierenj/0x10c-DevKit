using System;
using System.Windows;
using System.Windows.Controls;
using Devices.GenericVectorDisplay.View;
using Devices.GenericVectorDisplay.ViewModel;
using Devkit.Interfaces;

namespace Devices.GenericVectorDisplay
{
    public class VectorDisplay : IHardwareDevice, IDisplayContentProvider
    {
        private readonly IWorkspace _workspace;
        private readonly FrameBuffer _buffer;
        private readonly Random _rand;
        private IEmulatedSystem _system;
        private double[] _position;
        private double[] _target;
        private bool _trigger;
        private const double TimerMs = 0.1;
        private Window _window;
        private bool _isOn;
        private bool _testPattern;
        private double _noiseLevelPercent;

        public const double DefaultNoiseLevel = 0.0;

        #region Hardware interface
        public const uint Manufacturer = 0xcafe0666;
        public const uint HardwareType = 0x3c704111;
        public const ushort Revision = 0x0001;

        public enum InterruptMessage
        {
            SET_MODE_OFF = 0x00,
            SET_MODE_TEST = 0x01,
            SET_MODE_USER = 0x02,
            COMMAND_POS_ON = 0x03,
            COMMAND_POS_OFF = 0x04,
            SET_NOISE_LEVEL = 0x05,
            SET_ROTATION_POS = 0x06,
            GET_ROTATION_POS = 0x07,
            SET_AUTOROTATE_SPEED = 0x08
        }
        #endregion

        #region Test Pattern
        private int _patternUpdateCounter = 0;
        private int _currentIndex = 0;
        private readonly int[] _indices = {
                                    //   4    5
                                    // 0   1
                                    //   7    6
                                    // 3   2
                                    0,1,2,3,0,
                                    4,5,6,7,4,
                                    0,1,5,6,2,3,7,4,
                                };

        private readonly double[][] _vertices = new double[][]
                                      {
                                          new double[] { -50.0, -50.0, -50.0 }, 
                                          new double[] { 50.0, -50.0, -50.0 }, 
                                          new double[] { 50.0, 50.0, -50.0 }, 
                                          new double[] { -50.0, 50.0, -50.0 },
                                          new double[] { -50.0, -50.0, 50.0 }, 
                                          new double[] { 50.0, -50.0, 50.0 }, 
                                          new double[] { 50.0, 50.0, 50.0 }, 
                                          new double[] { -50.0, 50.0, 50.0 },
                                          new double[] { 0,0,0 },
                                      };
        #endregion

        public VectorDisplay(IWorkspace workspace, FrameBuffer buffer)
        {
            this._workspace = workspace;
            this._buffer = buffer;

            this._position = new double[3];
            this._target = new double[3];
            this._rand = new Random();

            this._isOn = true;
            this._testPattern = true;
            this._noiseLevelPercent = DefaultNoiseLevel;
        }

        public void Initialise(IEmulatedSystem system)
        {
            this._system = system;
            this._workspace.RuntimeManager.UI.AddDisplayContentProvider(this);
        }

        public void Reset()
        {
            this._position = new double[3];
            this._target = new double[3];
            this._trigger = true;

            this._isOn = true;
            this._testPattern = true;
            this._noiseLevelPercent = DefaultNoiseLevel;

            QueueNextTimer();
        }

        private void QueueNextTimer()
        {
            this._workspace.RuntimeManager.System.StartTimer(TimerMs / 1000.0, this, null);
        }

        public void Interrupt(out int additionalCycles)
        {
            additionalCycles = 0;
            var msg = (InterruptMessage)this._system.Cpu.Registers[0];
            var param = this._system.Cpu.Registers[1];

            switch (msg)
            {
                case InterruptMessage.SET_MODE_OFF:
                    this._isOn = false;
                    break;

                case InterruptMessage.SET_MODE_TEST:
                    this._isOn = true;
                    this._testPattern = true;
                    break;

                case InterruptMessage.SET_MODE_USER:
                    this._isOn = true;
                    this._testPattern = false;
                    break;

                case InterruptMessage.COMMAND_POS_ON:
                    if (this._testPattern) break;
                    this._target[0] = ((double)((short)this._system.Cpu.Registers[3]) / 32768.0) * 50.0;
                    this._target[1] = ((double)((short)this._system.Cpu.Registers[4]) / 32768.0) * 50.0;
                    this._target[2] = ((double)((short)this._system.Cpu.Registers[5]) / 32768.0) * 50.0;
                    this._trigger = true;
                    break;

                case InterruptMessage.COMMAND_POS_OFF:
                    if (this._testPattern) break;
                    this._target[0] = ((double)((short)this._system.Cpu.Registers[3]) / 32768.0) * 50.0;
                    this._target[1] = ((double)((short)this._system.Cpu.Registers[4]) / 32768.0) * 50.0;
                    this._target[2] = ((double)((short)this._system.Cpu.Registers[5]) / 32768.0) * 50.0;
                    this._trigger = false;
                    break;

                case InterruptMessage.SET_NOISE_LEVEL:
                    this._noiseLevelPercent = ((double)param / 65536.0) * 100.0;
                    break;

                case InterruptMessage.SET_ROTATION_POS:
                    this._buffer.RotatePos = (double)param * 360.0 / 65535.0;
                    break;

                case InterruptMessage.GET_ROTATION_POS:
                    this._system.Cpu.Registers[1] = (ushort)(this._buffer.RotatePos / 360.0 * 65535.0);
                    break;

                case InterruptMessage.SET_AUTOROTATE_SPEED:
                    this._buffer.AutorotateSpeed = (double)((short)param);
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

        private void NextTestPatternVertex()
        {
            this._target[0] = this._vertices[this._indices[this._currentIndex]][0];
            this._target[1] = this._vertices[this._indices[this._currentIndex]][1];
            this._target[2] = this._vertices[this._indices[this._currentIndex]][2];
            this._currentIndex++;
            if (this._currentIndex == this._indices.Length) this._currentIndex = 0;
            this._trigger = true;
        }

        public void CycleTimerCompleted(object state)
        {
            if (this._isOn)
            {
                if (this._testPattern)
                {
                    this._patternUpdateCounter++;
                    if (this._patternUpdateCounter == 2)
                    {
                        NextTestPatternVertex();
                        this._patternUpdateCounter = 0;
                    }
                }

                double[] direction = {
                                         this._target[0] - this._position[0],
                                         this._target[1] - this._position[1],
                                         this._target[2] - this._position[2]
                                     };
                double dirMag = Math.Sqrt(direction[0]*direction[0] + direction[1]*direction[1] + direction[2]*direction[2]);
                const double maxSpd = 8.0/TimerMs;
                double rand = 0.5;
                if (dirMag > maxSpd)
                {
                    direction[0] *= maxSpd/dirMag;
                    direction[1] *= maxSpd/dirMag;
                    direction[2] *= maxSpd/dirMag;
                    rand += (maxSpd - dirMag)*0.1;
                }
                this._position[0] += direction[0];
                this._position[1] += direction[1];
                this._position[2] += direction[2];

                rand += this._noiseLevelPercent/5.0;

                this._position[0] += (this._rand.NextDouble() - 0.5)*rand;
                this._position[1] += (this._rand.NextDouble() - 0.5)*rand;
                this._position[2] += (this._rand.NextDouble() - 0.5)*rand;

                if (this._trigger) this._buffer.SetLastTrigger(this._trigger);

                this._buffer.Add(new GunState {X = this._position[0], Y = this._position[1], Z = this._position[2], On = this._trigger});
            }

            QueueNextTimer();
        }

        public UIElement CreateUIElement()
        {
            var grid = new Grid();
            var ctl = new VectorDisplayControl
            {
                DataContext = this._buffer,
                Width = 128 + 20,
                Height = 96 + 20
            };
            grid.Children.Add(ctl);
            var btn = new Button
                          {
                              HorizontalAlignment = HorizontalAlignment.Right,
                              VerticalAlignment = VerticalAlignment.Bottom,
                              Content = "Open",
                              FontSize = 8.0,
                              Margin = new Thickness(8.0)
                          };
            btn.Click += (s, e) =>
                             {
                                 if (this._window != null)
                                 {
                                     this._window.Close();
                                     this._window = null;
                                 }
                                 ctl.IsEnabled = false;
                                 this._window = new Window();
                                 this._window.WindowStyle = WindowStyle.ToolWindow;
                                 this._window.Title = "Vector display";
                                 this._window.Content = new VectorDisplayControl { DataContext = this._buffer };
                                 this._window.Closed += (s2, e2) =>
                                                            {
                                                                ctl.IsEnabled = true;
                                                                this._window = null;
                                                            };
                                 this._window.Show();
                             };
            grid.Children.Add(btn);
            return grid;
        }

        public void DisplayOpened()
        {
        }

        public void DisplayClosed()
        {
            if (this._window == null) return;
            this._window.Close();
        }
    }
}