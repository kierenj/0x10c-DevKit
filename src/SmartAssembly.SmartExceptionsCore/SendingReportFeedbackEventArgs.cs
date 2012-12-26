using System;

namespace SmartAssembly.SmartExceptionsCore
{
	public class SendingReportFeedbackEventArgs : EventArgs
	{
		private SendingReportStep step;

		private readonly bool failed;

		private readonly string errorMessage;

		private readonly string reportID;

		public string ErrorMessage
		{
			get
			{
				return this.errorMessage;
			}
		}

		public bool Failed
		{
			get
			{
				return this.failed;
			}
		}

		public string ReportID
		{
			get
			{
				return this.reportID;
			}
		}

		public SendingReportStep Step
		{
			get
			{
				return this.step;
			}
		}

		internal SendingReportFeedbackEventArgs(SendingReportStep step) : this(step, string.Empty)
		{
		}

		internal SendingReportFeedbackEventArgs(SendingReportStep step, string errorMessage) : this(step, errorMessage, string.Empty)
		{
		}

		internal SendingReportFeedbackEventArgs(SendingReportStep step, string errorMessage, string reportId)
		{
			bool length;
			this.errorMessage = string.Empty;
			this.reportID = string.Empty;
			this.step = step;
			SendingReportFeedbackEventArgs sendingReportFeedbackEventArg = this;
			if (errorMessage == null)
			{
				length = false;
			}
			else
			{
				length = errorMessage.Length > 0;
			}
			sendingReportFeedbackEventArg.failed = length;
			this.errorMessage = errorMessage;
			this.reportID = reportId;
		}
	}
}