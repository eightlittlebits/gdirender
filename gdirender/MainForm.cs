using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using gdirender.Interop.Gdi32;

using Gdi32 = gdirender.Interop.Gdi32.NativeMethods;
using User32 = gdirender.Interop.User32.NativeMethods;

namespace gdirender
{
    public partial class MainForm : Form
    {
        private static readonly long StopwatchFrequency = Stopwatch.Frequency;

        private const int BitmapWidth = 1024;
        private const int BitmapHeight = 512;

        private GdiBitmap _backBuffer;

        private static bool ApplicationStillIdle => !User32.PeekMessage(out _, IntPtr.Zero, 0, 0, 0);

        public MainForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e) => Initialise();

        protected void Initialise()
        {
            PrepareUserInterface();
        }

        private void PrepareUserInterface()
        {
            ClientSize = new Size(BitmapWidth, BitmapHeight);

            _backBuffer = new GdiBitmap(BitmapWidth, BitmapHeight);

            RenderWeirdGradient(0, 0);

            Application.Idle += (s, ev) => { while (ApplicationStillIdle) { Update(); } };
        }

        private double _averageDuration = 0.0;

        protected new void Update()
        {
            long gradientStart = Stopwatch.GetTimestamp();

            //RenderWeirdGradient(_xOffset++, _yOffset);

            long gradientEnd = Stopwatch.GetTimestamp();

            UpdateWindow();

            long updateEnd = Stopwatch.GetTimestamp();

            double gradientDuration = (gradientEnd - gradientStart) * 1000.0 / StopwatchFrequency;

            double updateDuration = (updateEnd - gradientEnd) * 1000.0 / StopwatchFrequency;

            // https://answers.unity.com/questions/326621/how-to-calculate-an-average-fps.html
            _averageDuration += (((updateEnd - gradientStart) / (double)StopwatchFrequency) - _averageDuration) * 0.005;

            double fps = 1.0 / _averageDuration;

            base.Text = string.Format("{0:0.0000}ms {1:0.0000}ms {2:0000}fps", gradientDuration, updateDuration, fps);
        }

        private unsafe void RenderWeirdGradient(int xOffset, int yOffset)
        {
            int pitch = BitmapWidth * _backBuffer.BytesPerPixel;

            byte* row = (byte*)_backBuffer.BitmapData;

            for (int y = 0; y < BitmapHeight; y++)
            {
                uint* pixel = (uint*)row;

                uint green = (uint)((y + yOffset) << 8);

                for (int x = 0; x < BitmapWidth; x++)
                {
                    byte blue = (byte)(x + xOffset);

                    // xx RR GG BB
                    *pixel++ = green | blue;
                }

                row += pitch;
            }
        }

        private void UpdateWindow()
        {
            using var deviceContext = new GdiDeviceContext(_renderPanel.Handle);

            Gdi32.StretchBlt(deviceContext, 0, 0, _renderPanel.Width, _renderPanel.Height,
                     _backBuffer.DeviceContext, 0, 0, BitmapWidth, BitmapHeight,
                     RasterOp.SRCCOPY);
        }

        protected override void Dispose(bool disposing)
        {
            // free managed resources
            if (disposing)
            {
                if (components != null) { components.Dispose(); }
                _backBuffer.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
