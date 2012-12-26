using SmartAssembly.SmartExceptionsCore;
using SmartAssembly.SmartExceptionsCore.UI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SmartAssembly.SmartExceptionsWithAdvancedUI
{
	internal class ExceptionReportingForm : Form
	{
		private UnhandledExceptionHandler unhandledExceptionHandler;

		private ReportExceptionEventArgs reportExceptionEventArgs;

		private Thread workingThread;

		private CheckBox continueCheckBox;

		private Label pleaseTellTitle;

		private Button dontSendReport;

		private Button sendReport;

		private Label pleaseTellMessage;

		private Panel panelInformation;

		private Panel panelSending;

		private Button cancelSending;

		private WaitSendingReportControl waitSendingReport;

		private FeedbackControl preparingFeedback;

		private FeedbackControl connectingFeedback;

		private FeedbackControl transferingFeedback;

		private FeedbackControl completedFeedback;

		private Button ok;

		private Button retrySending;

		private HeaderControl headerControl1;

		private HeaderControl headerControl2;

		private Button debug;

		private IContainer components;

		private Panel panelEmail;

		private Label label3;

		private HeaderControl headerControl3;

		private Button continueSendReport;

		private TextBox email;

		private Label labelEmail;

		private CheckBox sendAnonymously;

		private AutoHeightLabel errorMessage;

		private PoweredBy poweredBy;

		private Button saveAsFile;

		private Button saveReport;

		private bool alreadyRetried;

		public ExceptionReportingForm(UnhandledExceptionHandler unhandledExceptionHandler, ReportExceptionEventArgs reportExceptionEventArgs) : this()
		{
			int height = base.Height;
			this.reportExceptionEventArgs = reportExceptionEventArgs;
			this.unhandledExceptionHandler = unhandledExceptionHandler;
			this.errorMessage.Text = reportExceptionEventArgs.Exception.Message;
			height = height + this.errorMessage.Height - base.FontHeight;
			if (!reportExceptionEventArgs.CanContinue)
			{
				this.continueCheckBox.Visible = false;
				height = height - this.continueCheckBox.Height;
			}
			if (height > base.Height)
			{
				base.Height = height;
			}
			if (reportExceptionEventArgs.CanDebug)
			{
				unhandledExceptionHandler.DebuggerLaunched += new EventHandler(this.OnDebuggerLaunched);
				this.debug.Visible = true;
				this.poweredBy.Visible = false;
			}
			if (!reportExceptionEventArgs.CanSendReport)
			{
				this.sendReport.Enabled = false;
				if (this.dontSendReport.CanFocus)
				{
					this.dontSendReport.Focus();
				}
			}
			this.email.Text = RegistryHelper.ReadHKLMRegistryString("Email");
			unhandledExceptionHandler.SendingReportFeedback += new SendingReportFeedbackEventHandler(this.OnFeedback);
		}

		public ExceptionReportingForm()
		{
			this.InitializeComponent();
			base.Size = new Size(419, 264);
			base.MinimizeBox = false;
			base.MaximizeBox = false;
			this.panelInformation.Location = Point.Empty;
			this.panelInformation.Dock = DockStyle.Fill;
			this.retrySending.Location = this.ok.Location;
			this.retrySending.Size = this.ok.Size;
			this.retrySending.BringToFront();
			this.panelSending.Location = Point.Empty;
			this.panelSending.Dock = DockStyle.Fill;
			this.Text = this.GetConvertedString(this.Text);
			this.panelEmail.Location = Point.Empty;
			this.panelEmail.Dock = DockStyle.Fill;
			foreach (Control control in base.Controls)
			{
				control.Text = this.GetConvertedString(control.Text);
				foreach (Control convertedString in control.Controls)
				{
					convertedString.Text = this.GetConvertedString(convertedString.Text);
				}
			}
		}

		private void cancelSending_Click(object sender, EventArgs e)
		{
			try
			{
				if (this.workingThread != null)
				{
					this.workingThread.Abort();
				}
			}
			catch
			{
			}
			base.Close();
		}

		private void continueCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.reportExceptionEventArgs.TryToContinue = this.continueCheckBox.Checked;
		}

		private void continueSendReport_Click(object sender, EventArgs e)
		{
			if (!this.sendAnonymously.Checked && this.reportExceptionEventArgs != null)
			{
				this.reportExceptionEventArgs.AddCustomProperty("Email", this.email.Text);
				RegistryHelper.SaveHKLMRegistryString("Email", this.email.Text);
			}
			this.SendReport();
		}

		private void debug_Click(object sender, EventArgs e)
		{
			if (this.reportExceptionEventArgs != null)
			{
				this.StartWorkingThread(new ThreadStart(this.reportExceptionEventArgs.LaunchDebugger));
			}
		}

		private void DebuggerLaunched(object sender, EventArgs e)
		{
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

		private void dontSendReport_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		private void email_TextChanged(object sender, EventArgs e)
		{
			bool @checked;
			Button button = this.continueSendReport;
			if (this.email.Text.Length > 0)
			{
				@checked = true;
			}
			else
			{
				@checked = this.sendAnonymously.Checked;
			}
			button.Enabled = @checked;
		}

		private void Feedback(object sender, SendingReportFeedbackEventArgs e)
		{
			Button button;
			if (!this.alreadyRetried || Thread.CurrentThread.ApartmentState != ApartmentState.STA)
			{
				button = this.retrySending;
			}
			else
			{
				button = this.saveReport;
			}
			Button button1 = button;
			SendingReportStep step = e.Step;
			switch (step)
			{
				case SendingReportStep.PreparingReport:
				{
					if (!e.Failed)
					{
						this.preparingFeedback.Start();
						return;
					}
					else
					{
						this.preparingFeedback.Stop(e.ErrorMessage);
						button1.Visible = true;
						button1.Focus();
						return;
					}
				}
				case SendingReportStep.ConnectingToServer:
				{
					if (!e.Failed)
					{
						this.preparingFeedback.Stop();
						this.connectingFeedback.Start();
						return;
					}
					else
					{
						this.connectingFeedback.Stop(e.ErrorMessage);
						button1.Visible = true;
						button1.Focus();
						return;
					}
				}
				case SendingReportStep.Transfering:
				{
					if (!e.Failed)
					{
						this.connectingFeedback.Stop();
						this.transferingFeedback.Start();
						this.waitSendingReport.Visible = true;
						return;
					}
					else
					{
						this.waitSendingReport.Visible = false;
						this.transferingFeedback.Stop(e.ErrorMessage);
						button1.Visible = true;
						button1.Focus();
						return;
					}
				}
				case SendingReportStep.Finished:
				{
					this.waitSendingReport.Visible = false;
					this.transferingFeedback.Stop();
					this.completedFeedback.Stop();
					this.ok.Enabled = true;
					this.ok.Focus();
					this.cancelSending.Enabled = false;
					return;
				}
				default:
				{
					return;
				}
			}
		}

		private string GetConvertedString(string s)
		{
			s = s.Replace("%AppName%", "DevKit.IDE");
			s = s.Replace("%CompanyName%", "Team Chicken");
			return s;
		}

		private void InitializeComponent()
		{
			this.panelInformation = new Panel();
			this.debug = new Button();
			this.continueCheckBox = new CheckBox();
			this.pleaseTellTitle = new Label();
			this.dontSendReport = new Button();
			this.sendReport = new Button();
			this.pleaseTellMessage = new Label();
			this.headerControl1 = new HeaderControl();
			this.errorMessage = new AutoHeightLabel();
			this.saveAsFile = new Button();
			this.panelSending = new Panel();
			this.cancelSending = new Button();
			this.ok = new Button();
			this.retrySending = new Button();
			this.waitSendingReport = new WaitSendingReportControl();
			this.headerControl2 = new HeaderControl();
			this.preparingFeedback = new FeedbackControl();
			this.connectingFeedback = new FeedbackControl();
			this.transferingFeedback = new FeedbackControl();
			this.completedFeedback = new FeedbackControl();
			this.panelEmail = new Panel();
			this.labelEmail = new Label();
			this.sendAnonymously = new CheckBox();
			this.email = new TextBox();
			this.headerControl3 = new HeaderControl();
			this.label3 = new Label();
			this.continueSendReport = new Button();
			this.poweredBy = new PoweredBy();
			this.saveReport = new Button();
			this.panelInformation.SuspendLayout();
			this.panelSending.SuspendLayout();
			this.panelEmail.SuspendLayout();
			base.SuspendLayout();
			this.panelInformation.Controls.Add(this.debug);
			this.panelInformation.Controls.Add(this.continueCheckBox);
			this.panelInformation.Controls.Add(this.pleaseTellTitle);
			this.panelInformation.Controls.Add(this.dontSendReport);
			this.panelInformation.Controls.Add(this.sendReport);
			this.panelInformation.Controls.Add(this.pleaseTellMessage);
			this.panelInformation.Controls.Add(this.headerControl1);
			this.panelInformation.Controls.Add(this.errorMessage);
			this.panelInformation.Controls.Add(this.saveAsFile);
			this.panelInformation.Location = new Point(8, 8);
			this.panelInformation.Name = "panelInformation";
			this.panelInformation.Size = new Size(413, 240);
			this.panelInformation.TabIndex = 0;
			this.debug.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.debug.FlatStyle = FlatStyle.System;
			this.debug.Location = new Point(66, 205);
			this.debug.Name = "debug";
			this.debug.Size = new Size(64, 24);
			this.debug.TabIndex = 13;
			this.debug.Text = "Debug";
			this.debug.Visible = false;
			this.debug.Click += new EventHandler(this.debug_Click);
			this.continueCheckBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.continueCheckBox.FlatStyle = FlatStyle.System;
			this.continueCheckBox.Location = new Point(22, 99);
			this.continueCheckBox.Name = "continueCheckBox";
			this.continueCheckBox.Size = new Size(226, 16);
			this.continueCheckBox.TabIndex = 14;
			this.continueCheckBox.Text = "Ignore this error and attempt to &continue.";
			this.continueCheckBox.CheckedChanged += new EventHandler(this.continueCheckBox_CheckedChanged);
			this.pleaseTellTitle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.pleaseTellTitle.FlatStyle = FlatStyle.System;
			this.pleaseTellTitle.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.pleaseTellTitle.Location = new Point(20, 124);
			this.pleaseTellTitle.Name = "pleaseTellTitle";
			this.pleaseTellTitle.Size = new Size(381, 16);
			this.pleaseTellTitle.TabIndex = 11;
			this.pleaseTellTitle.Text = "Please tell %CompanyName% about this problem.";
			this.dontSendReport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.dontSendReport.FlatStyle = FlatStyle.System;
			this.dontSendReport.Location = new Point(325, 205);
			this.dontSendReport.Name = "dontSendReport";
			this.dontSendReport.Size = new Size(75, 24);
			this.dontSendReport.TabIndex = 6;
			this.dontSendReport.Text = "&Don't Send";
			this.dontSendReport.Click += new EventHandler(this.dontSendReport_Click);
			this.sendReport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.sendReport.FlatStyle = FlatStyle.System;
			this.sendReport.Location = new Point(214, 205);
			this.sendReport.Name = "sendReport";
			this.sendReport.Size = new Size(105, 24);
			this.sendReport.TabIndex = 9;
			this.sendReport.Text = "&Send Error Report";
			this.sendReport.Click += new EventHandler(this.sendReport_Click);
			this.pleaseTellMessage.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
			this.pleaseTellMessage.FlatStyle = FlatStyle.System;
			this.pleaseTellMessage.Location = new Point(20, 140);
			this.pleaseTellMessage.Name = "pleaseTellMessage";
			this.pleaseTellMessage.Size = new Size(381, 55);
			this.pleaseTellMessage.TabIndex = 12;
			this.pleaseTellMessage.Text = "To help improve the software you use, %CompanyName% is interested in learning more about this error. We have created a report about the error for you to send to us.";
			this.headerControl1.BackColor = Color.FromArgb(36, 96, 179);
			this.headerControl1.Dock = DockStyle.Top;
			this.headerControl1.ForeColor = Color.White;
			this.headerControl1.IconState = IconState.Error;
			this.headerControl1.Image = null;
			this.headerControl1.Location = new Point(0, 0);
			this.headerControl1.Name = "headerControl1";
			this.headerControl1.Size = new Size(413, 58);
			this.headerControl1.TabIndex = 3;
			this.headerControl1.TabStop = false;
			this.headerControl1.Text = "%AppName% has encountered a problem.\nWe are sorry for the inconvenience.";
			this.errorMessage.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.errorMessage.FlatStyle = FlatStyle.System;
			this.errorMessage.Location = new Point(20, 69);
			this.errorMessage.Name = "errorMessage";
			this.errorMessage.Size = new Size(381, 13);
			this.errorMessage.TabIndex = 10;
			this.errorMessage.Text = "errorMessage";
			this.errorMessage.UseMnemonic = false;
			this.saveAsFile.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.saveAsFile.FlatStyle = FlatStyle.System;
			this.saveAsFile.Location = new Point(136, 205);
			this.saveAsFile.Name = "saveAsFile";
			this.saveAsFile.Size = new Size(72, 24);
			this.saveAsFile.TabIndex = 11;
			this.saveAsFile.Text = "Save as &File";
			this.saveAsFile.Click += new EventHandler(this.saveAsFile_Click);
			this.panelSending.Controls.Add(this.saveReport);
			this.panelSending.Controls.Add(this.cancelSending);
			this.panelSending.Controls.Add(this.ok);
			this.panelSending.Controls.Add(this.retrySending);
			this.panelSending.Controls.Add(this.waitSendingReport);
			this.panelSending.Controls.Add(this.headerControl2);
			this.panelSending.Controls.Add(this.preparingFeedback);
			this.panelSending.Controls.Add(this.connectingFeedback);
			this.panelSending.Controls.Add(this.transferingFeedback);
			this.panelSending.Controls.Add(this.completedFeedback);
			this.panelSending.Location = new Point(8, 264);
			this.panelSending.Name = "panelSending";
			this.panelSending.Size = new Size(413, 232);
			this.panelSending.TabIndex = 2;
			this.panelSending.Visible = false;
			this.cancelSending.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.cancelSending.FlatStyle = FlatStyle.System;
			this.cancelSending.Location = new Point(320, 197);
			this.cancelSending.Name = "cancelSending";
			this.cancelSending.Size = new Size(80, 24);
			this.cancelSending.TabIndex = 10;
			this.cancelSending.Text = "&Cancel";
			this.cancelSending.Click += new EventHandler(this.cancelSending_Click);
			this.ok.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.ok.Enabled = false;
			this.ok.FlatStyle = FlatStyle.System;
			this.ok.Location = new Point(232, 197);
			this.ok.Name = "ok";
			this.ok.Size = new Size(80, 24);
			this.ok.TabIndex = 22;
			this.ok.Text = "&OK";
			this.ok.Click += new EventHandler(this.ok_Click);
			this.retrySending.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.retrySending.FlatStyle = FlatStyle.System;
			this.retrySending.Location = new Point(144, 197);
			this.retrySending.Name = "retrySending";
			this.retrySending.Size = new Size(80, 24);
			this.retrySending.TabIndex = 23;
			this.retrySending.Text = "&Retry";
			this.retrySending.Visible = false;
			this.retrySending.Click += new EventHandler(this.retrySending_Click);
			this.waitSendingReport.Location = new Point(87, 145);
			this.waitSendingReport.Name = "waitSendingReport";
			this.waitSendingReport.Size = new Size(250, 42);
			this.waitSendingReport.TabIndex = 11;
			this.waitSendingReport.TabStop = false;
			this.waitSendingReport.Visible = false;
			this.headerControl2.BackColor = Color.FromArgb(36, 96, 179);
			this.headerControl2.Dock = DockStyle.Top;
			this.headerControl2.ForeColor = Color.White;
			this.headerControl2.IconState = IconState.Error;
			this.headerControl2.Image = null;
			this.headerControl2.Location = new Point(0, 0);
			this.headerControl2.Name = "headerControl2";
			this.headerControl2.Size = new Size(413, 58);
			this.headerControl2.TabIndex = 24;
			this.headerControl2.TabStop = false;
			this.headerControl2.Text = "Please wait while %AppName% is sending the report to %CompanyName% through the Internet.";
			this.preparingFeedback.Location = new Point(24, 72);
			this.preparingFeedback.Name = "preparingFeedback";
			this.preparingFeedback.Size = new Size(368, 16);
			this.preparingFeedback.TabIndex = 18;
			this.preparingFeedback.TabStop = false;
			this.preparingFeedback.Text = "Preparing the error report.";
			this.connectingFeedback.Location = new Point(24, 96);
			this.connectingFeedback.Name = "connectingFeedback";
			this.connectingFeedback.Size = new Size(368, 16);
			this.connectingFeedback.TabIndex = 19;
			this.connectingFeedback.TabStop = false;
			this.connectingFeedback.Text = "Connecting to server.";
			this.transferingFeedback.Location = new Point(24, 120);
			this.transferingFeedback.Name = "transferingFeedback";
			this.transferingFeedback.Size = new Size(368, 16);
			this.transferingFeedback.TabIndex = 20;
			this.transferingFeedback.TabStop = false;
			this.transferingFeedback.Text = "Transferring report.";
			this.completedFeedback.Location = new Point(24, 144);
			this.completedFeedback.Name = "completedFeedback";
			this.completedFeedback.Size = new Size(368, 16);
			this.completedFeedback.TabIndex = 21;
			this.completedFeedback.TabStop = false;
			this.completedFeedback.Text = "Error reporting completed. Thank you.";
			this.panelEmail.Controls.Add(this.labelEmail);
			this.panelEmail.Controls.Add(this.sendAnonymously);
			this.panelEmail.Controls.Add(this.email);
			this.panelEmail.Controls.Add(this.headerControl3);
			this.panelEmail.Controls.Add(this.label3);
			this.panelEmail.Controls.Add(this.continueSendReport);
			this.panelEmail.Location = new Point(11, 512);
			this.panelEmail.Name = "panelEmail";
			this.panelEmail.Size = new Size(413, 232);
			this.panelEmail.TabIndex = 4;
			this.panelEmail.Visible = false;
			this.labelEmail.FlatStyle = FlatStyle.System;
			this.labelEmail.Location = new Point(20, 131);
			this.labelEmail.Name = "labelEmail";
			this.labelEmail.Size = new Size(100, 16);
			this.labelEmail.TabIndex = 9;
			this.labelEmail.Text = "&Email address:";
			this.sendAnonymously.FlatStyle = FlatStyle.System;
			this.sendAnonymously.Location = new Point(120, 160);
			this.sendAnonymously.Name = "sendAnonymously";
			this.sendAnonymously.Size = new Size(232, 16);
			this.sendAnonymously.TabIndex = 11;
			this.sendAnonymously.Text = "I prefer to send this report &anonymously.";
			this.sendAnonymously.CheckedChanged += new EventHandler(this.sendAnonymously_CheckedChanged);
			this.email.Location = new Point(120, 128);
			this.email.Name = "email";
			this.email.Size = new Size(256, 20);
			this.email.TabIndex = 10;
			this.email.TextChanged += new EventHandler(this.email_TextChanged);
			this.headerControl3.BackColor = Color.FromArgb(36, 96, 179);
			this.headerControl3.Dock = DockStyle.Top;
			this.headerControl3.ForeColor = Color.White;
			this.headerControl3.IconState = IconState.Error;
			this.headerControl3.Image = null;
			this.headerControl3.Location = new Point(0, 0);
			this.headerControl3.Name = "headerControl3";
			this.headerControl3.Size = new Size(413, 58);
			this.headerControl3.TabIndex = 3;
			this.headerControl3.TabStop = false;
			this.headerControl3.Text = "Do you want to be contacted by %CompanyName% regarding this problem?";
			this.label3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
			this.label3.FlatStyle = FlatStyle.System;
			this.label3.Location = new Point(20, 69);
			this.label3.Name = "label3";
			this.label3.Size = new Size(381, 43);
			this.label3.TabIndex = 10;
			this.label3.Text = "If you want to be contacted by %CompanyName% regarding this error, please provide your e-mail address. This information will not be used for any other purpose.";
			this.continueSendReport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.continueSendReport.Enabled = false;
			this.continueSendReport.FlatStyle = FlatStyle.System;
			this.continueSendReport.Location = new Point(295, 197);
			this.continueSendReport.Name = "continueSendReport";
			this.continueSendReport.Size = new Size(105, 24);
			this.continueSendReport.TabIndex = 12;
			this.continueSendReport.Text = "&Send Error Report";
			this.continueSendReport.Click += new EventHandler(this.continueSendReport_Click);
			this.poweredBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			this.poweredBy.Cursor = Cursors.Hand;
			this.poweredBy.Location = new Point(6, 730);
			this.poweredBy.Name = "poweredBy";
			this.poweredBy.Size = new Size(112, 32);
			this.poweredBy.TabIndex = 5;
			this.poweredBy.TabStop = false;
			this.poweredBy.Text = "poweredBy1";
			this.saveReport.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			this.saveReport.FlatStyle = FlatStyle.System;
			this.saveReport.Location = new Point(146, 197);
			this.saveReport.Name = "saveReport";
			this.saveReport.Size = new Size(80, 24);
			this.saveReport.TabIndex = 25;
			this.saveReport.Text = "&Save Report";
			this.saveReport.Visible = false;
			this.saveReport.Click += new EventHandler(this.saveReport_Click);
			this.AutoScaleBaseSize = new Size(5, 13);
			this.BackColor = SystemColors.Window;
			base.ClientSize = new Size(434, 768);
			base.ControlBox = false;
			base.Controls.Add(this.poweredBy);
			base.Controls.Add(this.panelEmail);
			base.Controls.Add(this.panelInformation);
			base.Controls.Add(this.panelSending);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.Name = "ExceptionReportingForm";
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "%AppName%";
			base.TopMost = true;
			this.panelInformation.ResumeLayout(false);
			this.panelSending.ResumeLayout(false);
			this.panelEmail.ResumeLayout(false);
			this.panelEmail.PerformLayout();
			base.ResumeLayout(false);
		}

		private void ok_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (this.workingThread != null && this.workingThread.IsAlive)
			{
				this.workingThread.Abort();
			}
			base.OnClosing(e);
		}

		private void OnDebuggerLaunched(object sender, EventArgs e)
		{
			try
			{
				object[] objArray = new object[2];
				objArray[0] = sender;
				objArray[1] = e;
				base.Invoke(new EventHandler(this.DebuggerLaunched), objArray);
			}
			catch (InvalidOperationException invalidOperationException)
			{
			}
		}

		private void OnFeedback(object sender, SendingReportFeedbackEventArgs e)
		{
			try
			{
				object[] objArray = new object[2];
				objArray[0] = sender;
				objArray[1] = e;
				base.Invoke(new SendingReportFeedbackEventHandler(this.Feedback), objArray);
			}
			catch (InvalidOperationException invalidOperationException)
			{
			}
		}

		private void retrySending_Click(object sender, EventArgs e)
		{
			this.alreadyRetried = true;
			this.retrySending.Visible = false;
			this.preparingFeedback.Init();
			this.connectingFeedback.Init();
			this.transferingFeedback.Init();
			if (this.reportExceptionEventArgs != null)
			{
				this.StartWorkingThread(new ThreadStart(this.StartSendReport));
			}
		}

		private void saveAsFile_Click(object sender, EventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.DefaultExt = "saencryptedreport";
			saveFileDialog.Filter = "SmartAssembly Exception Report|*.saencryptedreport|All files|*.*";
			saveFileDialog.Title = "Save an Exception Report";
			if (saveFileDialog.ShowDialog(this) != DialogResult.Cancel)
			{
				if (!this.reportExceptionEventArgs.SaveEncryptedReport(saveFileDialog.FileName))
				{
					MessageBox.Show("Failed to save the report.", "DevKit.IDE", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				else
				{
					MessageBox.Show(string.Format("Please send the Exception Report to {0} Support Team.", "Team Chicken"), "DevKit.IDE", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
					base.Close();
					return;
				}
			}
		}

		private void saveReport_Click(object sender, EventArgs e)
		{
			this.saveAsFile_Click(sender, e);
		}

		private void sendAnonymously_CheckedChanged(object sender, EventArgs e)
		{
			bool @checked;
			this.email.Enabled = !this.sendAnonymously.Checked;
			Button button = this.continueSendReport;
			if (this.email.Text.Length > 0)
			{
				@checked = true;
			}
			else
			{
				@checked = this.sendAnonymously.Checked;
			}
			button.Enabled = @checked;
		}

		public void SendReport()
		{
			try
			{
				this.panelEmail.Visible = false;
				this.panelSending.Visible = true;
				if (this.reportExceptionEventArgs != null)
				{
					this.StartWorkingThread(new ThreadStart(this.StartSendReport));
				}
			}
			catch
			{
			}
		}

		private void sendReport_Click(object sender, EventArgs e)
		{
			this.panelInformation.Visible = false;
			this.panelEmail.Visible = true;
		}

		private void StartSendReport()
		{
			this.reportExceptionEventArgs.SendReport();
		}

		private void StartWorkingThread(ThreadStart start)
		{
			this.workingThread = new Thread(start);
			this.workingThread.Start();
		}
	}
}