using SmartAssembly.SmartExceptionsCore;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SmartAssembly.SmartExceptionsCore.UI
{
	[DesignerCategory("Code")]
	public class FeedbackControl : Control
	{
		private readonly Label m_Label;

		private Image m_Image;

		private bool m_ShowText;

		private readonly Timer m_Timer;

		private bool m_ShowImage;

		private string m_ErrorMessage;

		private float m_Dx;

		private float m_Dy;

		[Browsable(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.Refresh();
			}
		}

		public FeedbackControl()
		{
			this.m_Label = new Label();
			this.m_Timer = new Timer();
			this.m_ShowImage = true;
			this.m_ErrorMessage = string.Empty;
			this.m_Dx = 1f;
			this.m_Dy = 1f;
			this.m_Timer.Interval = 250;
			this.m_Timer.Tick += new EventHandler(this.OnTimerTick);
			this.m_Label.FlatStyle = FlatStyle.System;
			base.Controls.Add(this.m_Label);
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
			base.TabStop = false;
		}

		public FeedbackControl(string text) : this()
		{
			base.Text = string.Concat(" ", text);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.m_Image != null)
				{
					this.m_Image.Dispose();
				}
				this.m_Timer.Dispose();
			}
			base.Dispose(disposing);
		}

		public void Init()
		{
			this.m_Timer.Enabled = false;
			this.m_Image = null;
			this.m_ShowText = false;
			this.m_ErrorMessage = string.Empty;
			this.Refresh();
			base.Height = 16;
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			string text;
			base.OnPaint(e);
			if (base.DesignMode)
			{
				this.m_Image = Resources.GetBitmap("current");
				this.m_ShowText = true;
			}
			if (this.m_Image != null && this.m_ShowImage)
			{
				e.Graphics.DrawImage(this.m_Image, new Rectangle(0, 0, Convert.ToInt32(16f * this.m_Dx), Convert.ToInt32(16f * this.m_Dy)), new Rectangle(0, 0, 16, 16), GraphicsUnit.Pixel);
			}
			if (!this.m_ShowText)
			{
				this.m_Label.Text = string.Empty;
				return;
			}
			else
			{
				Label mLabel = this.m_Label;
				if (this.m_ErrorMessage.Length > 0)
				{
					text = string.Concat(base.Text, " (", this.m_ErrorMessage, ")");
				}
				else
				{
					text = base.Text;
				}
				mLabel.Text = text;
				return;
			}
		}

		protected override void OnResize(EventArgs e)
		{
			this.m_Label.SetBounds(Convert.ToInt32(22f * this.m_Dx), Convert.ToInt32(this.m_Dy), base.Width - Convert.ToInt32(22f * this.m_Dx), base.Height - Convert.ToInt32(this.m_Dy));
			base.OnResize(e);
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			this.m_ShowImage = !this.m_ShowImage;
			this.Refresh();
		}

		protected override void ScaleCore(float dx, float dy)
		{
			this.m_Dx = dx;
			this.m_Dy = dy;
			base.ScaleCore(dx, dy);
			this.OnResize(EventArgs.Empty);
		}

		public void Start()
		{
			this.m_Timer.Enabled = true;
			this.m_Image = Resources.GetBitmap("current");
			this.m_ShowText = true;
			this.Refresh();
		}

		public void Stop()
		{
			this.Stop(string.Empty);
		}

		public void Stop(string errorMessage)
		{
			string str;
			this.m_ErrorMessage = errorMessage;
			this.m_Timer.Enabled = false;
			FeedbackControl bitmap = this;
			if (errorMessage.Length > 0)
			{
				str = "error";
			}
			else
			{
				str = "ok";
			}
			bitmap.m_Image = Resources.GetBitmap(str);
			this.m_ShowImage = true;
			this.m_ShowText = true;
			if (errorMessage.Length > 0)
			{
				base.Height = 100;
			}
			this.Refresh();
		}
	}
}