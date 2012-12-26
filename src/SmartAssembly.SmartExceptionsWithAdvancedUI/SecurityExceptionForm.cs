using SmartAssembly.SmartExceptionsCore;
using SmartAssembly.SmartExceptionsCore.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SmartAssembly.SmartExceptionsWithAdvancedUI
{
	public class SecurityExceptionForm : Form
	{
		private SecurityExceptionEventArgs securityExceptionEventArgs;

		private Button continueButton;

		private Button quitButton;

		private HeaderControl headerControl1;

		private AutoHeightLabel errorMessage;

		private PoweredBy poweredBy;

		private IContainer components;

		public SecurityExceptionForm()
		{
			this.InitializeComponent();
			base.Icon = Win32.GetApplicationIcon();
			this.Text = this.GetConvertedString(this.Text);
			if (this.Text.Length == 0)
			{
				this.Text = "Security Exception";
			}
			foreach (Control control in base.Controls)
			{
				control.Text = this.GetConvertedString(control.Text);
				foreach (Control convertedString in control.Controls)
				{
					convertedString.Text = this.GetConvertedString(convertedString.Text);
				}
			}
		}

		public SecurityExceptionForm(SecurityExceptionEventArgs securityExceptionEventArgs) : this()
		{
			if (securityExceptionEventArgs != null)
			{
				if (!securityExceptionEventArgs.CanContinue)
				{
					this.continueButton.Visible = false;
				}
				this.securityExceptionEventArgs = securityExceptionEventArgs;
				if (securityExceptionEventArgs.SecurityMessage.Length <= 0)
				{
					StringBuilder stringBuilder = new StringBuilder();
					stringBuilder.Append("%AppName% attempted to perform an operation not allowed by the security policy. To grant this application the required permission, contact your system administrator, or use the Microsoft .NET Framework Configuration tool.\n\n");
					if (securityExceptionEventArgs.CanContinue)
					{
						stringBuilder.Append("If you click Continue, the application will ignore this error and attempt to continue. If you click Quit, the application will close immediately.\n\n");
					}
					stringBuilder.Append(securityExceptionEventArgs.SecurityException.Message);
					this.errorMessage.Text = this.GetConvertedString(stringBuilder.ToString());
				}
				else
				{
					this.errorMessage.Text = securityExceptionEventArgs.SecurityMessage;
				}
				int bottom = this.errorMessage.Bottom + 60;
				Size clientSize = base.ClientSize;
				if (bottom > clientSize.Height)
				{
					Size size = base.ClientSize;
					base.ClientSize = new Size(size.Width, bottom);
				}
				return;
			}
			else
			{
				return;
			}
		}

		private void continueButton_Click(object sender, EventArgs e)
		{
			if (this.securityExceptionEventArgs != null)
			{
				this.securityExceptionEventArgs.TryToContinue = true;
			}
			base.Close();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private string GetConvertedString(string s)
		{
			s = s.Replace("%AppName%", "DevKit.IDE");
			s = s.Replace("%CompanyName%", "Team Chicken");
			return s;
		}

		private void InitializeComponent()
		{
			this.quitButton = new Button();
			this.continueButton = new Button();
			this.headerControl1 = new HeaderControl();
			this.errorMessage = new AutoHeightLabel();
			this.poweredBy = new PoweredBy();
			base.SuspendLayout();
			this.quitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.quitButton.FlatStyle = FlatStyle.System;
			this.quitButton.Location = new Point(308, 188);
			this.quitButton.Name = "quitButton";
			this.quitButton.Size = new Size(100, 24);
			this.quitButton.TabIndex = 0;
			this.quitButton.Text = "&Quit";
			this.quitButton.Click += new EventHandler(this.quitButton_Click);
			this.continueButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.continueButton.FlatStyle = FlatStyle.System;
			this.continueButton.Location = new Point(202, 188);
			this.continueButton.Name = "continueButton";
			this.continueButton.Size = new Size(100, 24);
			this.continueButton.TabIndex = 1;
			this.continueButton.Text = "&Continue";
			this.continueButton.Click += new EventHandler(this.continueButton_Click);
			this.headerControl1.BackColor = Color.FromArgb(36, 96, 179);
			this.headerControl1.Dock = DockStyle.Top;
			this.headerControl1.ForeColor = Color.White;
			this.headerControl1.IconState = IconState.Warning;
			this.headerControl1.Image = null;
			this.headerControl1.Location = new Point(0, 0);
			this.headerControl1.Name = "headerControl1";
			this.headerControl1.Size = new Size(418, 58);
			this.headerControl1.TabIndex = 7;
			this.headerControl1.TabStop = false;
			this.headerControl1.Text = "%AppName% attempted to perform an operation not allowed by the security policy.";
			this.errorMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.errorMessage.FlatStyle = FlatStyle.System;
			this.errorMessage.Location = new Point(20, 72);
			this.errorMessage.Name = "errorMessage";
			this.errorMessage.Size = new Size(382, 13);
			this.errorMessage.TabIndex = 14;
			this.errorMessage.Text = "errorMessage";
			this.errorMessage.UseMnemonic = false;
			this.poweredBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.poweredBy.Cursor = Cursors.Hand;
			this.poweredBy.Location = new Point(6, 186);
			this.poweredBy.Name = "poweredBy";
			this.poweredBy.Size = new Size(120, 32);
			this.poweredBy.TabIndex = 15;
			this.poweredBy.TabStop = false;
			this.poweredBy.Text = "poweredBy1";
			this.AutoScaleBaseSize = new Size(5, 13);
			this.BackColor = SystemColors.Window;
			base.ClientSize = new Size(418, 224);
			base.ControlBox = false;
			base.Controls.Add(this.continueButton);
			base.Controls.Add(this.quitButton);
			base.Controls.Add(this.headerControl1);
			base.Controls.Add(this.errorMessage);
			base.Controls.Add(this.poweredBy);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "SecurityExceptionForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "%AppName%";
			base.ResumeLayout(false);
		}

		private void quitButton_Click(object sender, EventArgs e)
		{
			if (this.securityExceptionEventArgs != null)
			{
				this.securityExceptionEventArgs.TryToContinue = false;
			}
			base.Close();
		}
	}
}