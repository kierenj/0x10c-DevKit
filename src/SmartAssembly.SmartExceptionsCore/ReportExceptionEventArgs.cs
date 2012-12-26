using System;
using System.IO;

namespace SmartAssembly.SmartExceptionsCore
{
	public class ReportExceptionEventArgs : EventArgs
	{
		private ErrorReportSender m_ReportSender;

		private Exception exception;

		private bool canDebug;

		private bool canSendReport;

		private bool showContinueCheckBox;

		private bool tryToContinue;

		[Obsolete("Use ShowContinueCheckbox instead, as this is now also false when the builder has chosen not to show the checkbox.")]
		public bool CanContinue
		{
			get
			{
				return this.showContinueCheckBox;
			}
		}

		public bool CanDebug
		{
			get
			{
				return this.canDebug;
			}
		}

		public bool CanSendReport
		{
			get
			{
				return this.canSendReport;
			}
		}

		public Exception Exception
		{
			get
			{
				return this.exception;
			}
		}

		public bool ShowContinueCheckbox
		{
			get
			{
				return this.showContinueCheckBox;
			}
		}

		public bool TryToContinue
		{
			get
			{
				return this.tryToContinue;
			}
			set
			{
				this.tryToContinue = value;
			}
		}

		internal ReportExceptionEventArgs(ErrorReportSender reportSender, Exception exception)
		{
			this.canSendReport = true;
			this.showContinueCheckBox = true;
			this.m_ReportSender = reportSender;
			this.exception = exception;
		}

		public void AddCustomProperty(string name, string value)
		{
			this.m_ReportSender.AddCustomProperty(name, value);
		}

		public void AttachFile(string name, string fileName)
		{
			this.m_ReportSender.AttachFile(name, fileName);
		}

		internal void DisableSendReport()
		{
			this.canSendReport = false;
		}

		internal void EnableDebug()
		{
			this.canDebug = true;
		}

		public byte[] GetReportRawData()
		{
			return this.m_ReportSender.GetReportRawData();
		}

		public void LaunchDebugger()
		{
			if (this.canDebug)
			{
				this.m_ReportSender.LaunchDebugger();
			}
		}

		public bool SaveEncryptedReport(string fileName)
		{
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}
			return this.m_ReportSender.SaveEncryptedReport(fileName);
		}

		public bool SendReport()
		{
			if (this.canSendReport)
			{
				return this.m_ReportSender.SendReport();
			}
			else
			{
				return false;
			}
		}

		internal void SetShowContinueCheckBox(bool value)
		{
			this.showContinueCheckBox = value;
		}
	}
}