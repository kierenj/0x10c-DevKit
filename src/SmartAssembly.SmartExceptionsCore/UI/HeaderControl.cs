using SmartAssembly.SmartExceptionsCore;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SmartAssembly.SmartExceptionsCore.UI
{
	[DesignerCategory("Code")]
	public class HeaderControl : Control
	{
		private Label label;

		private Image image;

		private Icon applicationIcon;

		private Bitmap stateImage;

		private IconState iconState;

		private float dx;

		private float dy;

		public IconState IconState
		{
			get
			{
				return this.iconState;
			}
			set
			{
				if (this.iconState != value)
				{
					this.iconState = value;
					IconState iconState = this.iconState;
					switch (iconState)
					{
						case IconState.Error:
						{
							this.stateImage = Resources.GetBitmap("error16");
							break;
						}
						case IconState.Warning:
						{
							this.stateImage = Resources.GetBitmap("warning16");
							break;
						}
						default:
						{
							this.stateImage = null;
							break;
						}
					}
					this.Refresh();
				}
			}
		}

		public Image Image
		{
			get
			{
				return this.image;
			}
			set
			{
				this.image = value;
				this.Refresh();
			}
		}

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get
			{
				return this.label.Text;
			}
			set
			{
				this.label.Text = value;
			}
		}

		public HeaderControl()
		{
			this.label = new Label();
			this.dx = 1f;
			this.dy = 1f;
			try
			{
				this.label.FlatStyle = FlatStyle.System;
				this.label.Font = new Font(this.Font, FontStyle.Bold);
			}
			catch
			{
			}
			base.Controls.Add(this.label);
			this.BackColor = SystemColors.Window;
			base.TabStop = false;
			this.Dock = DockStyle.Top;
			base.Height = 58;
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
			this.applicationIcon = Win32.GetApplicationIcon();
			this.OnResize(EventArgs.Empty);
		}

		public HeaderControl(string text) : this()
		{
			this.label.Text = text;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.applicationIcon != null)
				{
					this.applicationIcon.Dispose();
					this.applicationIcon = null;
				}
				if (this.image != null)
				{
					this.image.Dispose();
					this.image = null;
				}
				if (this.stateImage != null)
				{
					this.stateImage.Dispose();
					this.stateImage = null;
				}
			}
			base.Dispose(disposing);
		}

		protected override void OnFontChanged(EventArgs e)
		{
			try
			{
				this.label.Font = new Font(this.Font, FontStyle.Bold);
				base.OnFontChanged(e);
			}
			catch
			{
			}
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			Size clientSize = base.ClientSize;
			Size size = base.ClientSize;
			Size clientSize1 = base.ClientSize;
			e.Graphics.DrawLine(SystemPens.ControlDark, 0, clientSize.Height - 2, size.Width, clientSize1.Height - 2);
			Size size1 = base.ClientSize;
			Size clientSize2 = base.ClientSize;
			Size size2 = base.ClientSize;
			e.Graphics.DrawLine(SystemPens.ControlLightLight, 0, size1.Height - 1, clientSize2.Width, size2.Height - 1);
			Size clientSize3 = base.ClientSize;
			Rectangle rectangle = new Rectangle(clientSize3.Width - Convert.ToInt32(48f * this.dx), Convert.ToInt32(11f * this.dy), Convert.ToInt32(32f * this.dx), Convert.ToInt32(32f * this.dy));
			if (this.image == null)
			{
				if (this.applicationIcon != null)
				{
					e.Graphics.DrawIcon(this.applicationIcon, rectangle);
					if (this.stateImage != null)
					{
						e.Graphics.DrawImage(this.stateImage, new Rectangle(rectangle.Right - Convert.ToInt32(12f * this.dx), rectangle.Bottom - Convert.ToInt32(12f * this.dy), Convert.ToInt32(16f * this.dx), Convert.ToInt32(16f * this.dy)), new Rectangle(0, 0, 16, 16), GraphicsUnit.Pixel);
					}
				}
				return;
			}
			else
			{
				e.Graphics.DrawImage(this.image, rectangle, new Rectangle(0, 0, 32, 32), GraphicsUnit.Pixel);
				return;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			this.label.SetBounds(Convert.ToInt32(13f * this.dx), Convert.ToInt32(15f * this.dy), base.Width - Convert.ToInt32(69f * this.dx), base.Height - Convert.ToInt32(18f * this.dy));
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