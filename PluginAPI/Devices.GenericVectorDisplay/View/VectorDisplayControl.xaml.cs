using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Devices.GenericVectorDisplay.ViewModel;
using SharpGL;
using SharpGL.Enumerations;
using SharpGL.SceneGraph;

namespace Devices.GenericVectorDisplay.View
{
    /// <summary>
    /// Interaction logic for VectorDisplayControl.xaml
    /// </summary>
    public partial class VectorDisplayControl : UserControl
    {
        private readonly Stopwatch watch = new Stopwatch();
        private double lastTimer;

        public VectorDisplayControl()
        {
            InitializeComponent();

            watch.Start();
        }

        private void RenderFrame(object sender, OpenGLEventArgs args)
        {
            if (args == null) return;

            var gl = args.OpenGL;
            var buffer = ((FrameBuffer)DataContext);
            bool trigger = false;

            gl.LoadIdentity();

            gl.Clear(OpenGL.GL_COLOR_BUFFER_BIT | OpenGL.GL_DEPTH_BUFFER_BIT);

            if (IsEnabled)
            {
                gl.LookAt(0.0, -110.0, -140.0, 0.0, 0.0, 0.0, 0.0, -1.0, 0.0);
                buffer.RotatePos += buffer.AutorotateSpeed * ((double)(watch.ElapsedMilliseconds / 1000.0) - lastTimer);
                lastTimer = watch.ElapsedMilliseconds / 1000.0;
                while (buffer.RotatePos > 360.0) buffer.RotatePos -= 360.0;
                while (buffer.RotatePos < 0.0) buffer.RotatePos += 360.0;

                gl.Rotate(buffer.RotatePos, 0, -1.0, 0);
                var states = buffer.GetAndReset();
                gl.Color(0.0f, 1.0f, 0.0f);

                foreach (var state in states)
                {
                    if (!trigger && state.On)
                    {
                        gl.Begin(BeginMode.LineStrip);
                        trigger = true;
                    }
                    if (trigger && !state.On)
                    {
                        gl.End();
                        trigger = false;
                    }
                    if (trigger)
                    {
                        gl.Vertex(state.X, state.Y, state.Z);
                    }
                }
            }
            if (trigger)
            {
                gl.End();
            }
        }

        private void Setup(object sender, OpenGLEventArgs args)
        {
            var gl = args.OpenGL;
        }

        private void glControl_Resized(object sender, OpenGLEventArgs args)
        {
            var gl = args.OpenGL;

            gl.MatrixMode(MatrixMode.Projection);
            gl.LoadIdentity();
            gl.Perspective(60.0, (glControl.ActualWidth / glControl.ActualHeight), 1.0, 1000.0);

            gl.MatrixMode(MatrixMode.Modelview);
        }
    }
}
