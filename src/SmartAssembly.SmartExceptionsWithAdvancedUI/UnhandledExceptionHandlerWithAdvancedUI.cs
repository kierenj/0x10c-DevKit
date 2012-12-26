using SmartAssembly.SmartExceptionsCore;
using System;
using System.Security;
using System.Windows.Forms;

namespace SmartAssembly.SmartExceptionsWithAdvancedUI
{
	public class UnhandledExceptionHandlerWithAdvancedUI : UnhandledExceptionHandler
	{
		public UnhandledExceptionHandlerWithAdvancedUI()
		{
		}

		public static bool AttachApp()
		{
			bool flag;
			try
			{
				UnhandledExceptionHandler.AttachExceptionHandler(new UnhandledExceptionHandlerWithAdvancedUI());
				flag = true;
			}
			catch (SecurityException securityException)
			{
				try
				{
					Application.EnableVisualStyles();
					string str = string.Format("{0} cannot initialize itself because some permissions are not granted.\n\nYou probably try to launch {0} in a partial-trust situation. It's usually the case when the application is hosted on a network share.\n\nYou need to run {0} in full-trust, or at least grant it the UnmanagedCode security permission.\n\nTo grant this application the required permission, contact your system administrator, or use the Microsoft .NET Framework Configuration tool.", "DevKit.IDE");
					SecurityExceptionForm securityExceptionForm = new SecurityExceptionForm(new SecurityExceptionEventArgs(str, false));
					securityExceptionForm.ShowInTaskbar = true;
					securityExceptionForm.ShowDialog();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					MessageBox.Show(exception.ToString(), string.Format("{0} Fatal Error", "DevKit.IDE"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
				}
				flag = false;
			}
			return flag;
		}

		protected override Guid GetUserID()
		{
			Guid guid;
			try
			{
				string str = RegistryHelper.ReadHKLMRegistryString("AnonymousID");
				if (str.Length != 0)
				{
					guid = new Guid(str);
				}
				else
				{
					Guid guid1 = Guid.NewGuid();
					RegistryHelper.SaveHKLMRegistryString("AnonymousID", guid1.ToString("B"));
					if (RegistryHelper.ReadHKLMRegistryString("AnonymousID").Length <= 0)
					{
						guid = Guid.Empty;
					}
					else
					{
						guid = guid1;
					}
				}
			}
			catch
			{
				guid = Guid.Empty;
			}
			return guid;
		}

		protected override void OnFatalException(FatalExceptionEventArgs e)
		{
			MessageBox.Show(e.FatalException.ToString(), string.Format("{0} Fatal Error", "DevKit.IDE"), MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}

		protected override void OnReportException(ReportExceptionEventArgs e)
		{
			ExceptionReportingForm exceptionReportingForm = new ExceptionReportingForm(this, e);
			exceptionReportingForm.ShowDialog();
		}

		protected override void OnSecurityException(SecurityExceptionEventArgs e)
		{
			SecurityExceptionForm securityExceptionForm = new SecurityExceptionForm(e);
			securityExceptionForm.ShowDialog();
		}
	}
}