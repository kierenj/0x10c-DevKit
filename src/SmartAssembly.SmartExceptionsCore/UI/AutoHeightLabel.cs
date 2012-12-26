using SmartAssembly.SmartExceptionsCore;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SmartAssembly.SmartExceptionsCore.UI
{
	[DesignerCategory("Code")]
	public class AutoHeightLabel : Label
	{
		public AutoHeightLabel()
		{
			base.FlatStyle = FlatStyle.System;
			base.UseMnemonic = false;
		}

		private void DoAutoHeight()
		{
			try
			{
				Graphics graphic = base.CreateGraphics();
				using (graphic)
				{
					int textHeight = Win32.GetTextHeight(graphic, this.Text, this.Font, base.Width);
					if (textHeight > 0)
					{
						base.Height = textHeight;
					}
				}
			}
			catch
			{
			}
		}

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged(e);
			this.DoAutoHeight();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			this.DoAutoHeight();
		}

		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			this.DoAutoHeight();
		}
	}
}