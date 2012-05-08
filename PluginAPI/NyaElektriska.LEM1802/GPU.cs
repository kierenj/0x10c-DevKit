using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Devkit.Interfaces;

namespace NyaElektriska.LEM1802
{
    public class GPU : IDisplayContentProvider, IHardwareDevice
    {
        public const double WarmupTimeMs = 1000.0;

        private IWorkspace _workspace;
        private IEmulatedSystem _system;
        private WriteableBitmap _writeableBitmap;
        private Image _currentDisplay;
        private Grid _currentDisplayContainer;
        private ushort[] _fontData;
        private ushort[] _paletteData;
        private int _borderColourIndex;
        private int _lastBorderColourIndex;
        private ushort _displayAddress;
        private ushort _customFontAddress;
        private ushort _customPaletteAddress;
        private BitmapImage _bootScreen;
        private Stopwatch _flashTimer;
        private Stopwatch _warmupTimer;

        #region Hardware interface
        public const int Manufacturer = 0x1c6c8b36;
        public const int HardwareType = 0x7349f615;
        public const int Revision = 0x1802;

        public enum InterruptMessage
        {
            MEM_MAP_SCREEN = 0,
            MEM_MAP_FONT = 1,
            MEM_MAP_PALETTE = 2,
            SET_BORDER_COLOR = 3,
            MEM_DUMP_FONT = 4,
            MEM_DUMP_PALETTE = 5
        }
        #endregion

        public GPU(IWorkspace workspace)
        {
            this._workspace = workspace;
        }

        public void Interrupt(out int additionalCycles)
        {
            // default zero cycles
            additionalCycles = 0;

            var msg = (InterruptMessage)this._system.Cpu.Registers[0];
            var param = this._system.Cpu.Registers[1];
            switch (msg)
            {
                case InterruptMessage.MEM_MAP_SCREEN:
                    if (param == 0)
                    {
                        ShowBoot();
                    }
                    else if (this._displayAddress == 0)
                    {
                        StartWarmup();
                    }
                    this._displayAddress = param;
                    break;

                case InterruptMessage.MEM_MAP_FONT:
                    this._customFontAddress = param;
                    break;

                case InterruptMessage.MEM_MAP_PALETTE:
                    this._customPaletteAddress = param;
                    break;

                case InterruptMessage.SET_BORDER_COLOR:
                    this._borderColourIndex = (param & 0xf);
                    break;

                case InterruptMessage.MEM_DUMP_FONT:
                    for (int i = 0; i < 256; i++)
                    {
                        this._system.MemoryController.Write((ushort)((param + i) & 0xffff), this._fontData[i]);
                    }
                    additionalCycles = 256;
                    break;

                case InterruptMessage.MEM_DUMP_PALETTE:
                    for (int i = 0; i < 16; i++)
                    {
                        this._system.MemoryController.Write((ushort)((param + i) & 0xffff), this._paletteData[i]);
                    }
                    additionalCycles = 16;
                    break;
            }
        }

        public void CycleTimerCompleted(object state)
        {
        }

        public void Pulse()
        {
        }

        public void Query(out uint manufacturer, out uint hardwareType, out ushort revision)
        {
            manufacturer = Manufacturer;
            hardwareType = HardwareType;
            revision = Revision;
        }

        public void Initialise(IEmulatedSystem system)
        {
            this._system = system;

            // load default data
            this._lastBorderColourIndex = -1;
            this._bootScreen = Resources.ResourceHelper.Boot;
            this._fontData = LoadFontFromImage(Resources.ResourceHelper.Font);
            this._paletteData = CreateDefaultPalette().ToArray();

            // hook up rendering stuff
            this._workspace.RuntimeManager.UI.AddDisplayContentProvider(this);
            this._writeableBitmap = new WriteableBitmap(128, 96, 96, 96, PixelFormats.Bgr32, null);
            CompositionTarget.Rendering += RenderCallback;
        }

        public void Unload()
        {
            CompositionTarget.Rendering -= RenderCallback;
            this._workspace.RuntimeManager.UI.RemoveDisplayContentProvider(this);
        }

        private void RenderCallback(object sender, EventArgs e)
        {
            UpdateDisplay();
        }

        public void Reset()
        {
            ShowBoot();
            this._customFontAddress = 0;
            this._customPaletteAddress = 0;
            this._displayAddress = 0;
            this._borderColourIndex = 0;
        }

        public UIElement CreateUIElement()
        {
            // create any resources that might be needed during render
            this._flashTimer = new Stopwatch();
            this._flashTimer.Start();

            this._currentDisplayContainer = new Grid();
            this._currentDisplayContainer.Children.Add(CreateImageHost(this._writeableBitmap));

            return this._currentDisplayContainer;
        }

        public void DisplayOpened()
        {
        }

        public void DisplayClosed()
        {
        }

        private void UpdateDisplay()
        {
            if (this._currentDisplay == null) return;

            // ensure we run on UI thread
            if (!this._currentDisplay.CheckAccess())
            {
                this._currentDisplay.Dispatcher.Invoke(new Action(UpdateDisplay));
                return;
            }

            if (this._warmupTimer != null)
            {
                // still warming up?
                if (this._warmupTimer.ElapsedMilliseconds < (long) WarmupTimeMs) return;

                // just warmed up.
                this._warmupTimer = null;
                this._currentDisplay.Source = this._writeableBitmap;
            }

            // read state of vram etc
            bool flashOn = ((int)(this._flashTimer.ElapsedMilliseconds / 320.0) % 2) == 0;
            ushort[] fontData = this._customFontAddress == 0 ? this._fontData : ReadFontData();
            ushort[] paletteData = this._customPaletteAddress == 0 ? this._paletteData : ReadPaletteData();
            int[] fullColours = paletteData.Select(GetFullColour).ToArray();

            // update border/background colour
            if (this._lastBorderColourIndex != this._borderColourIndex)
            {
                int fullColour = fullColours[this._borderColourIndex];
                byte r, g, b;
                r = (byte)((fullColour >> 16) & 0xff);
                g = (byte)((fullColour >> 8) & 0xff);
                b = (byte)(fullColour & 0xff);
                this._currentDisplayContainer.Background = new SolidColorBrush(Color.FromRgb(r, g, b));
                this._lastBorderColourIndex = this._borderColourIndex;
            }

            // update pixels here
            var mem = this._system.MemoryController;
            this._writeableBitmap.Lock();
            unsafe
            {
                int pBackBuffer = (int)_writeableBitmap.BackBuffer;
                int stride = _writeableBitmap.BackBufferStride / 4;

                for (int y = 0; y < 12; y++)
                {
                    for (int x = 0; x < 32; x++)
                    {
                        ushort locData = mem.Read((ushort)(this._displayAddress + (x + (y << 5))));
                        char chr = (char)(locData & 0x7f);
                        int colours = locData >> 8;
                        int charDataOffset = (int)chr * 2;

                        int colour = fullColours[colours & 0xf];
                        int colourAdd = fullColours[(colours >> 4) & 0xf] - colour;
                        if (!flashOn && ((locData & 0x80) > 0)) colourAdd = 0;

                        int pixelOffs = x * 4 + y * 8 * stride;

                        for (int xx = 0; xx < 4; xx++)
                        {
                            int bits = fontData[charDataOffset + (xx >> 1)] >> (xx + 1 & 0x1) * 8 & 0xff;
                            for (int yy = 0; yy < 8; yy++)
                            {
                                int col = colour + colourAdd * (bits >> yy & 0x1);
                                ((int*)pBackBuffer)[pixelOffs + xx + yy * stride] = col;
                            }
                        }
                    }
                }
            }

            this._writeableBitmap.AddDirtyRect(new Int32Rect(0, 0, 128, 96));
            this._writeableBitmap.Unlock();
        }

        private void ShowBoot()
        {
            if (this._currentDisplay == null) return;

            // ensure we run on UI thread
            if (!this._currentDisplay.CheckAccess())
            {
                this._currentDisplay.Dispatcher.Invoke(new Action(ShowBoot));
                return;
            }

            this._warmupTimer = new Stopwatch();
            this._currentDisplay.Source = this._bootScreen;
            this._lastBorderColourIndex = -1;
            this._currentDisplayContainer.Background = Brushes.Black;
        }

        private void StartWarmup()
        {
            this._warmupTimer = new Stopwatch();
            this._warmupTimer.Start();
        }

        #region Utility methods
        private Image CreateImageHost(WriteableBitmap bitmap)
        {
            var imageHost = new Image { Margin = new Thickness(10.0) };
            RenderOptions.SetBitmapScalingMode(imageHost, BitmapScalingMode.NearestNeighbor);
            RenderOptions.SetEdgeMode(imageHost, EdgeMode.Aliased);
            imageHost.Source = bitmap;
            this._currentDisplay = imageHost;

            return imageHost;
        }

        private ushort[] LoadFontFromImage(BitmapImage img)
        {
            var fontPixels = new int[4096];
            var data = new ushort[256];

            int stride = (img.PixelWidth * img.Format.BitsPerPixel + 7) / 8;
            img.CopyPixels(fontPixels, stride, 0);

            for (int charIdx = 0; charIdx < 128; charIdx++)
            {
                int charDataOfs = charIdx * 2;
                int sourceXPos = charIdx % 32 * 4;
                int sourceYPos = charIdx / 32 * 8;

                // clear out words
                data[(ushort)(charDataOfs + 0)] = 0x0000;
                data[(ushort)(charDataOfs + 1)] = 0x0000;

                for (int xPixel = 0; xPixel < 4; xPixel++)
                {
                    int bitmap = 0;
                    for (int yPixel = 0; yPixel < 8; yPixel++)
                    {
                        if ((fontPixels[(sourceXPos + xPixel + (sourceYPos + yPixel) * 128)] & 0xff) > 128)
                        {
                            bitmap |= 1 << yPixel;
                        }

                        // shift, OR data into the correct byte of appropriate word
                        ushort finalOffset = (ushort)(charDataOfs + xPixel / 2);
                        data[finalOffset] = (ushort)(data[finalOffset] | bitmap << (xPixel + 1 & 0x1) * 8);
                    }
                }
            }

            return data;
        }

        private int GetFullColour(ushort colour)
        {
            var fromMem = colour;
            int r = (fromMem >> 8) & 0xf;
            int g = (fromMem >> 4) & 0xf;
            int b = fromMem & 0xf;
            r *= (255 / 15);
            g *= (255 / 15);
            b *= (255 / 15);
            return (r << 16) + (g << 8) + b;
        }

        private IEnumerable<ushort> CreateDefaultPalette()
        {
            for (int idx = 0; idx < 16; idx++)
            {
                int b = (idx >> 0 & 0x1) * 10;
                int g = (idx >> 1 & 0x1) * 10;
                int r = (idx >> 2 & 0x1) * 10;
                if (idx == 6)
                {
                    g -= 5;
                }
                else if (idx >= 8)
                {
                    r += 5;
                    g += 5;
                    b += 5;
                }
                yield return (ushort)((r << 8) + (g << 4) + b);
            }
        }

        private ushort[] ReadFontData()
        {
            var data = new ushort[256];
            for (int i = 0; i < 256; i++)
            {
                data[i] = this._system.MemoryController.Read((ushort)((this._customFontAddress + i) & 0xffff));
            }
            return data;
        }

        private ushort[] ReadPaletteData()
        {
            var data = new ushort[16];
            for (int i = 0; i < 16; i++)
            {
                data[i] = this._system.MemoryController.Read((ushort)((this._customPaletteAddress + i) & 0xffff));
            }
            return data;
        }
        #endregion
    }
}
