using SmartAssembly.Shared;
using System;
using System.Net;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Web.Services.Protocols;
using System.Windows.Forms;
using System.Windows.Threading;

namespace SmartAssembly.SmartExceptionsCore
{
	public abstract class UnhandledExceptionHandler
	{
		public const string ApplicationName = "{1fe9e38e-05cc-46a3-ae48-6cda8fb62056}";

		public const string CompanyName = "{395edd3b-130e-4160-bb08-6931086cea46}";

		private readonly static bool AlwaysContinueOnError;

		private readonly static string SecurityExceptionsHaveSpecialTreatment;

		private static UnhandledExceptionHandler s_Handler;

		private IWebProxy m_Proxy;

		private static UnhandledExceptionHandler Handler
		{
			get
			{
				if (UnhandledExceptionHandler.s_Handler == null)
				{
					Type[] types = Assembly.GetExecutingAssembly().GetTypes();
					for (int i = 0; i < (int)types.Length; i++)
					{
						Type type = types[i];
						if (type != null && type.BaseType != null && type.BaseType == typeof(UnhandledExceptionHandler))
						{
							try
							{
								UnhandledExceptionHandler.s_Handler = (UnhandledExceptionHandler)Activator.CreateInstance(type, true);
								if (UnhandledExceptionHandler.s_Handler != null)
								{
									break;
								}
							}
							catch
							{
							}
						}
					}
				}
				return UnhandledExceptionHandler.s_Handler;
			}
		}

		static UnhandledExceptionHandler()
		{
			UnhandledExceptionHandler.AlwaysContinueOnError = Convert.ToBoolean("False");
			UnhandledExceptionHandler.SecurityExceptionsHaveSpecialTreatment = "1";
		}

		protected UnhandledExceptionHandler()
		{
		}

		[SecurityPermission(SecurityAction.Demand, UnmanagedCode=true)]
		public static void AttachExceptionHandler(UnhandledExceptionHandler unhandledExceptionHandler)
		{
			if (unhandledExceptionHandler != null)
			{
				UnhandledExceptionHandler.s_Handler = unhandledExceptionHandler;
				Dispatcher.CurrentDispatcher.UnhandledException += new DispatcherUnhandledExceptionEventHandler(unhandledExceptionHandler.CurrentDispatcherOnUnhandledException);
				AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(unhandledExceptionHandler.OnUnhandledException);
				Application.ThreadException += new ThreadExceptionEventHandler(unhandledExceptionHandler.OnThreadException);
			}
		}

		private void CurrentDispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			try
			{
				if (e.Exception as SecurityException == null || !(UnhandledExceptionHandler.SecurityExceptionsHaveSpecialTreatment == "1") || !this.InvokeSecurityException(e.Exception as SecurityException))
				{
					this.ReportException(e.Exception, true, false);
					e.Handled = true;
				}
			}
			catch
			{
			}
		}

		public static void EntryPointException(Exception exception, object[] objects)
		{
			if (exception == null || exception as SecurityException == null || !(UnhandledExceptionHandler.SecurityExceptionsHaveSpecialTreatment == "1") || !UnhandledExceptionHandler.Handler.InvokeSecurityException((SecurityException)exception))
			{
				StackFrameHelper.CreateExceptionN(exception, objects);
				UnhandledExceptionHandler.Handler.ReportException(exception, false, false);
				return;
			}
			else
			{
				return;
			}
		}

		protected virtual Guid GetUserID()
		{
			return Guid.Empty;
		}

		private void InvokeDebuggerLaunched(object sender, EventArgs eventArgs)
		{
			EventHandler eventHandler = this.DebuggerLaunched;
			if (eventHandler != null)
			{
				eventHandler(sender, eventArgs);
			}
		}

		private void InvokeOnFatalException(object sender, FatalExceptionEventArgs fatalExceptionEventArgs)
		{
			this.OnFatalException(fatalExceptionEventArgs);
		}

		private bool InvokeSecurityException(SecurityException exception)
		{
			SecurityExceptionEventArgs securityExceptionEventArg = new SecurityExceptionEventArgs(exception);
			this.OnSecurityException(securityExceptionEventArg);
			if (!securityExceptionEventArg.ReportException)
			{
				if (!securityExceptionEventArg.TryToContinue)
				{
					Application.Exit();
				}
				return true;
			}
			else
			{
				return false;
			}
		}

		private void InvokeSendingReportFeedback(object sender, SendingReportFeedbackEventArgs sendingReportFeedbackEventArgs)
		{
			SendingReportFeedbackEventHandler sendingReportFeedbackEventHandler = this.SendingReportFeedback;
			if (sendingReportFeedbackEventHandler != null)
			{
				sendingReportFeedbackEventHandler(sender, sendingReportFeedbackEventArgs);
			}
		}

		protected abstract void OnFatalException(FatalExceptionEventArgs e);

		protected abstract void OnReportException(ReportExceptionEventArgs e);

		protected abstract void OnSecurityException(SecurityExceptionEventArgs e);

		private void OnThreadException(object sender, ThreadExceptionEventArgs e)
		{
			try
			{
				Exception exception = e.Exception;
				Type type = exception.GetType();
				if (type.Name == "UnhandledException" && type.Namespace == "SmartAssembly.SmartExceptionsCore")
				{
					exception = (Exception)type.GetField("PreviousException").GetValue(exception);
				}
				if (exception as SecurityException == null || !(UnhandledExceptionHandler.SecurityExceptionsHaveSpecialTreatment == "1") || !this.InvokeSecurityException(exception as SecurityException))
				{
					this.ReportException(exception, true, false);
				}
			}
			catch
			{
			}
		}

		private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			try
			{
				if (e.ExceptionObject as SecurityException == null || !(UnhandledExceptionHandler.SecurityExceptionsHaveSpecialTreatment == "1") || !this.InvokeSecurityException(e.ExceptionObject as SecurityException))
				{
					if (e.ExceptionObject is Exception)
					{
						this.ReportException((Exception)e.ExceptionObject, !e.IsTerminating, false);
					}
				}
			}
			catch
			{
			}
		}

		public static void ReportException(Exception exception, object[] objects)
		{
			try
			{
				if (exception.GetType() != typeof(Exception) || !(exception.Message == "{report}"))
				{
					StackFrameHelper.CreateExceptionN(exception, objects);
				}
				else
				{
					exception = exception.InnerException;
				}
				UnhandledExceptionHandler.Handler.ReportException(exception, true, true);
			}
			catch
			{
			}
		}

		[ReportUsage("Unhandled Exception Encountered")]
		private void ReportException(Exception exception, bool canContinue, bool manuallyReported)
		{
			Type type = exception.GetType();
			if (type.Name == "UnhandledException" && type.Namespace == "SmartAssembly.SmartExceptionsCore")
			{
				exception = (Exception)type.GetField("PreviousException").GetValue(exception);
			}
			bool tryToContinue = true;
			if (exception == null || exception is ThreadAbortException)
			{
				return;
			}
			else
			{
				try
				{
					ErrorReportSender errorReportSender = new ErrorReportSender(this.GetUserID(), exception, this.m_Proxy);
					errorReportSender.SendingReportFeedback += new SendingReportFeedbackEventHandler(this.InvokeSendingReportFeedback);
					errorReportSender.DebuggerLaunched += new EventHandler(this.InvokeDebuggerLaunched);
					errorReportSender.FatalException += new FatalExceptionEventHandler(this.InvokeOnFatalException);
					ReportExceptionEventArgs reportExceptionEventArg = new ReportExceptionEventArgs(errorReportSender, exception);
					if (AppPathFinder.ReadInstalledSaPath() != null)
					{
						reportExceptionEventArg.EnableDebug();
					}
					if (canContinue)
					{
						if (manuallyReported || UnhandledExceptionHandler.AlwaysContinueOnError)
						{
							reportExceptionEventArg.SetShowContinueCheckBox(false);
							reportExceptionEventArg.TryToContinue = true;
						}
					}
					else
					{
						reportExceptionEventArg.SetShowContinueCheckBox(false);
						reportExceptionEventArg.TryToContinue = false;
					}
					this.OnReportException(reportExceptionEventArg);
					tryToContinue = !reportExceptionEventArg.TryToContinue;
				}
				catch (ThreadAbortException threadAbortException)
				{
				}
				catch (Exception exception2)
				{
					Exception exception1 = exception2;
					this.OnFatalException(new FatalExceptionEventArgs(exception1));
				}
				if (tryToContinue)
				{
					Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
					for (int i = 0; i < (int)assemblies.Length; i++)
					{
						Assembly assembly = assemblies[i];
						try
						{
							string fullName = assembly.FullName;
							if (fullName.EndsWith("31bf3856ad364e35") && fullName.StartsWith("PresentationFramework,"))
							{
								object obj = assembly.GetType("System.Windows.Application").GetProperty("Current").GetGetMethod().Invoke(null, null);
								obj.GetType().GetMethod("Shutdown", new Type[0]).Invoke(obj, null);
							}
						}
						catch
						{
						}
					}
					try
					{
						Application.Exit();
					}
					catch
					{
						try
						{
							Environment.Exit(0);
						}
						catch
						{
						}
					}
				}
				return;
			}
		}

		public static Exception ReportWebMethodException(Exception exception, object[] objects)
		{
			try
			{
				if (exception.GetType() != typeof(Exception) || !(exception.Message == "{report}"))
				{
					StackFrameHelper.CreateExceptionN(exception, objects);
				}
				else
				{
					exception = exception.InnerException;
				}
				UnhandledExceptionHandler.Handler.ReportException(exception, true, false);
			}
			catch
			{
			}
			return new SoapException(exception.Message, SoapException.ServerFaultCode);
		}

		public void SetProxy(IWebProxy proxy)
		{
			this.m_Proxy = proxy;
		}

		public event EventHandler DebuggerLaunched;

		public event SendingReportFeedbackEventHandler SendingReportFeedback;
	}
}