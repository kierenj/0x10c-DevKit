using System;
using System.Security;

namespace SmartAssembly.SmartExceptionsCore
{
	public class SecurityExceptionEventArgs : EventArgs
	{
		private SecurityException securityException;

		private string securityMessage;

		private bool tryToContinue;

		private bool reportException;

		private bool canContinue;

		public bool CanContinue
		{
			get
			{
				return this.canContinue;
			}
		}

		public bool ReportException
		{
			get
			{
				return this.reportException;
			}
			set
			{
				this.reportException = value;
			}
		}

		public SecurityException SecurityException
		{
			get
			{
				return this.securityException;
			}
		}

		public string SecurityMessage
		{
			get
			{
				return this.securityMessage;
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

		public SecurityExceptionEventArgs(SecurityException securityException)
		{
			this.securityMessage = string.Empty;
			this.canContinue = true;
			this.securityException = securityException;
		}

		public SecurityExceptionEventArgs(SecurityException securityException, bool canContinue) : this(securityException)
		{
			this.canContinue = canContinue;
		}

		public SecurityExceptionEventArgs(string securityMessage, bool canContinue) : this(new SecurityException(securityMessage), canContinue)
		{
			this.securityMessage = securityMessage;
		}
	}
}