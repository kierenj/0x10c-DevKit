using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Rendering;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.Windows.Media;

namespace Devkit.IDE.View
{
	public sealed class StandardBrushFactory : HighlightingBrush
	{
		private readonly Brush _brush;

		public StandardBrushFactory(Brush brush)
		{
			try
			{
				brush.Freeze();
				this._brush = brush;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, brush);
				throw;
			}
		}

		public override Brush GetBrush(ITextRunConstructionContext context)
		{
			Brush brush;
			try
			{
				brush = this._brush;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, context);
				throw;
			}
			return brush;
		}

		public override string ToString()
		{
			string str;
			try
			{
				str = this._brush.ToString();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
			return str;
		}
	}
}