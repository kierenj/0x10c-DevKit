using SmartAssembly.SmartExceptionsCore;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace SmartAssembly.SmartExceptionsCore.UI
{
	[DesignerCategory("Code")]
	public class PoweredBy : Control
	{
		private const string PoweredByText = "Powered by SmartAssembly";

		private const string WebLink = "http://www.red-gate.com/products/dotnet-development/smartassembly/?utm_source=smartassemblyui&utm_medium=supportlink&utm_content=aerdialogbox&utm_campaign=smartassembly";

		private Label label;

		private PictureBox logo;

		private ToolTip toolTip;

		private float dx;

		private float dy;

		public PoweredBy()
		{
			this.label = new Label();
			this.logo = new PictureBox();
			this.toolTip = new ToolTip();
			this.dx = 1f;
			this.dy = 1f;
			base.SuspendLayout();
			this.label.FlatStyle = FlatStyle.System;
			this.label.Location = new Point(0, 10);
			this.label.Size = new Size(62, 24);
			this.label.Text = "Powered by";
			this.logo.Image = Resources.GetBitmap("{logo}");
			this.logo.Location = new Point(72, 0);
			this.logo.Size = new Size(32, 32);
			this.logo.SizeMode = PictureBoxSizeMode.StretchImage;
			this.label.Click += new EventHandler(this.OnClick);
			this.logo.Click += new EventHandler(this.OnClick);
			base.Click += new EventHandler(this.OnClick);
			this.Cursor = Cursors.Hand;
			base.TabStop = false;
			base.Size = new Size(112, 32);
			Control[] controlArray = new Control[2];
			controlArray[0] = this.logo;
			controlArray[1] = this.label;
			base.Controls.AddRange(controlArray);
			this.toolTip.SetToolTip(this, "Powered by SmartAssembly");
			this.toolTip.SetToolTip(this.label, "Powered by SmartAssembly");
			this.toolTip.SetToolTip(this.logo, "Powered by SmartAssembly");
			base.ResumeLayout(true);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.toolTip != null)
				{
					this.toolTip.Dispose();
				}
				if (this.logo != null)
				{
					this.logo.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		private void OnClick(object sender, EventArgs e)
		{
			try
			{
				Process.Start("http://www.red-gate.com/products/dotnet-development/smartassembly/?utm_source=smartassemblyui&utm_medium=supportlink&utm_content=aerdialogbox&utm_campaign=smartassembly");
			}
			catch
			{
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.Size = new Size(Convert.ToInt32(112f * this.dx), Convert.ToInt32(32f * this.dy));
			base.OnResize(e);
		}

		protected override void ScaleCore(float dx, float dy)
		{
			this.dx = dx;
			this.dy = dy;
			base.ScaleCore(dx, dy);
			this.OnResize(EventArgs.Empty);
		}
	}
}