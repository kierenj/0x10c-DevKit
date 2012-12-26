using Devkit.Workspace.ViewModel;
using ICSharpCode.AvalonEdit.Rendering;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Devkit.IDE.View
{
	public class CodeBackgroundRenderer : IBackgroundRenderer
	{
		private readonly DecorationInfo _info;

		private readonly static Brush CurrentLineBrush;

		private readonly static Pen CurrentLinePen;

		private readonly static Brush BreakpointBrush;

		private readonly static Pen BreakpointPen;

		private readonly static Pen ErrorPen;

		public KnownLayer Layer
		{
			get
			{
				KnownLayer knownLayer;
				try
				{
					knownLayer = 0;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return knownLayer;
			}
		}

		static CodeBackgroundRenderer()
		{
			try
			{
				CodeBackgroundRenderer.CurrentLineBrush = new SolidColorBrush(Color.FromArgb(64, 255, 255, 0));
				CodeBackgroundRenderer.CurrentLineBrush.Freeze();
				CodeBackgroundRenderer.BreakpointBrush = new SolidColorBrush(Color.FromArgb(64, 255, 0, 0));
				CodeBackgroundRenderer.BreakpointBrush.Freeze();
				CodeBackgroundRenderer.CurrentLinePen = new Pen(new SolidColorBrush(Colors.Yellow), 1);
				CodeBackgroundRenderer.CurrentLinePen.Freeze();
				CodeBackgroundRenderer.BreakpointPen = new Pen(new SolidColorBrush(Colors.Red), 1);
				CodeBackgroundRenderer.BreakpointPen.Freeze();
				CodeBackgroundRenderer.ErrorPen = new Pen(new SolidColorBrush(Colors.Red), 1);
				CodeBackgroundRenderer.ErrorPen.Freeze();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException0(exception);
				throw;
			}
		}

		public CodeBackgroundRenderer(DecorationInfo info)
		{
			try
			{
				this._info = info;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, info);
				throw;
			}
		}

		public void Draw(TextView textView, DrawingContext drawingContext)
		{
			VisualLine visualLine = null;
			double visualTop;
			IEnumerator<VisualLine> enumerator = null;
			int? errorLineNumber;
			int? nullable;
			try
			{
				if (textView != null && textView.get_VisualLinesValid())
				{
					foreach (VisualLine visualLine in textView.get_VisualLines())
					{
						if (this._info.get_CurrentLineNumbers() != null && this._info.get_CurrentLineNumbers().Contains<int>(visualLine.get_FirstDocumentLine().get_LineNumber()))
						{
							drawingContext.DrawRectangle(CodeBackgroundRenderer.CurrentLineBrush, CodeBackgroundRenderer.CurrentLinePen, new Rect(0, visualLine.get_VisualTop() - textView.get_VerticalOffset(), 100000, visualLine.get_Height()));
						}
						if (this._info.get_BreakpointLines() != null && this._info.get_BreakpointLines().Contains<int>(visualLine.get_FirstDocumentLine().get_LineNumber()))
						{
							drawingContext.DrawRectangle(CodeBackgroundRenderer.BreakpointBrush, CodeBackgroundRenderer.BreakpointPen, new Rect(0, visualLine.get_VisualTop() - textView.get_VerticalOffset(), 100000, visualLine.get_Height()));
						}
						errorLineNumber = this._info.get_ErrorLineNumber();
						if (!errorLineNumber.HasValue)
						{
							continue;
						}
						nullable = this._info.get_ErrorLineNumber();
						if (nullable.Value != visualLine.get_FirstDocumentLine().get_LineNumber())
						{
							continue;
						}
						visualTop = visualLine.get_VisualTop() - textView.get_VerticalOffset() + visualLine.get_Height();
						drawingContext.DrawLine(CodeBackgroundRenderer.ErrorPen, new Point(0, visualTop), new Point(100000, visualTop));
					}
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException8(exception, visualLine, visualTop, enumerator, errorLineNumber, nullable, this, textView, drawingContext);
				throw;
			}
		}
	}
}