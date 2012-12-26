using SmartAssembly.SmartExceptionsCore;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace SmartAssembly.SmartExceptionsCore.UI
{
	[DesignerCategory("Code")]
	public class WaitSendingReportControl : Control
	{
		private int m_Step;

		private readonly Bitmap m_DataImage;

		private readonly Bitmap m_NetworkImage;

		private readonly Timer m_Timer;

		private float m_Dx;

		private float m_Dy;

		public WaitSendingReportControl()
		{
			this.m_Step = 99;
			this.m_DataImage = Resources.GetBitmap("data");
			this.m_NetworkImage = Resources.GetBitmap("network");
			this.m_Timer = new Timer();
			this.m_Dx = 1f;
			this.m_Dy = 1f;
			this.m_Timer.Interval = 85;
			this.m_Timer.Tick += new EventHandler(this.OnTimerTick);
			base.Size = new Size(250, 42);
			base.TabStop = false;
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.m_DataImage != null)
				{
					this.m_DataImage.Dispose();
				}
				this.m_Timer.Dispose();
			}
			base.Dispose(disposing);
		}

		private void Init(bool run)
		{
			this.m_Timer.Enabled = run;
			this.m_Step = 0;
			this.Refresh();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (this.m_NetworkImage != null)
			{
				e.Graphics.DrawImage(this.m_NetworkImage, new Rectangle(0, 0, Convert.ToInt32(250f * this.m_Dx), Convert.ToInt32(42f * this.m_Dy)), new Rectangle(0, 0, 250, 42), GraphicsUnit.Pixel);
			}
			if (this.m_DataImage != null && this.m_Step > 0)
			{
				e.Graphics.SetClip(new Rectangle(Convert.ToInt32(46f * this.m_Dx), 0, Convert.ToInt32(165f * this.m_Dx), Convert.ToInt32(34f * this.m_Dy)));
				e.Graphics.DrawImage(this.m_DataImage, new Rectangle(Convert.ToInt32((float)(this.m_Step - 6) * this.m_Dx), Convert.ToInt32(16f * this.m_Dy), Convert.ToInt32(40f * this.m_Dx), Convert.ToInt32(12f * this.m_Dy)), 0, 0, 40, 12, GraphicsUnit.Pixel);
			}
		}

		protected override void OnResize(EventArgs e)
		{
			base.Size = new Size(Convert.ToInt32(250f * this.m_Dx), Convert.ToInt32(42f * this.m_Dy));
			base.OnResize(e);
		}

		private void OnTimerTick(object sender, EventArgs e)
		{
			WaitSendingReportControl mStep = this;
			mStep.m_Step = mStep.m_Step + 11;
			if (this.m_Step > 198)
			{
				this.m_Step = 0;
			}
			this.Refresh();
		}

		protected override void OnVisibleChanged(EventArgs e)
		{
			base.OnVisibleChanged(e);
			if (!base.DesignMode)
			{
				this.Init(base.Visible);
			}
		}

		protected override void ScaleCore(float dx, float dy)
		{
			this.m_Dx = dx;
			this.m_Dy = dy;
			base.ScaleCore(dx, dy);
			this.OnResize(EventArgs.Empty);
		}
	}
}