using SmartAssembly.Zip;
using System;
using System.Net;
using System.Text;

namespace SmartAssembly.SmartExceptionsCore
{
	public class ReportSender
	{
		protected const string AssemblyID = "{100fd8cd-4fe2-410e-8c33-ae1af08ef31d}";

		private const string LicenseID = "{be78a0c5-c47c-4127-a428-52bdc580a02f}";

		private const string CryptoPublicKey = "{bf13b64c-b3d2-4165-b3f5-7f852d4744cf}";

		private IWebProxy m_Proxy;

		public ReportSender()
		{
		}

		protected void InvokeSendingReportFeedback(SendingReportStep step, string errorMessage, string reportId)
		{
			SendingReportFeedbackEventHandler sendingReportFeedbackEventHandler = this.SendingReportFeedback;
			if (sendingReportFeedbackEventHandler != null)
			{
				sendingReportFeedbackEventHandler(this, new SendingReportFeedbackEventArgs(step, errorMessage, reportId));
			}
		}

		protected void InvokeSendingReportFeedback(SendingReportStep step, string errorMessage)
		{
			this.InvokeSendingReportFeedback(step, errorMessage, string.Empty);
		}

		protected void InvokeSendingReportFeedback(SendingReportStep step)
		{
			this.InvokeSendingReportFeedback(step, string.Empty);
		}

		internal bool SendReport(byte[] reportData, ReportSender.NotificationEmailSettings notificationEmailSettings)
		{
			byte[] numArray;
			bool flag;
			try
			{
				numArray = SimpleZip.Zip(reportData);
				goto Label0;
			}
			catch (Exception exception)
			{
				this.InvokeSendingReportFeedback(SendingReportStep.PreparingReport, SimpleZip.ExceptionMessage);
				flag = false;
			}
			return flag;
		Label0:
			byte[] numArray1 = Encryption.Encrypt(numArray, "<RSAKeyValue><Modulus>s3i8v1TIvLPXY9D2QXApSYXgdpiFbD5n3PGcGKNDDrbc1rSAkgu0So/uBn6kUoGcSP9zlHOlyWKpCHz+pMuRQd7kg2lgu7h3pN0levcjuMfqqCYW710dnaniMevPoC9MgoYz9M0QmWg9Sug1VvuCwLrki9nF+/3WY5R0JE9nOOU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
			if (numArray1 != null)
			{
				this.InvokeSendingReportFeedback(SendingReportStep.ConnectingToServer);
				WebServicesClient webServicesClient = new WebServicesClient("e7cf250c-986a-dc34-b349-d75555e66510");
				if (this.m_Proxy != null)
				{
					webServicesClient.SetProxy(this.m_Proxy);
				}
				ReportSender.AfterLoginClosure afterLoginClosure = new ReportSender.AfterLoginClosure(this, numArray1, webServicesClient, notificationEmailSettings);
				webServicesClient.LoginToServer(new StringCallback(afterLoginClosure.AfterLogin));
				return afterLoginClosure.Succeeded;
			}
			else
			{
				this.InvokeSendingReportFeedback(SendingReportStep.PreparingReport, Encryption.ExceptionMessage);
				return false;
			}
		}

		public void SetProxy(IWebProxy proxy)
		{
			this.m_Proxy = proxy;
		}

		public event SendingReportFeedbackEventHandler SendingReportFeedback;

		private class AfterLoginClosure
		{
			private readonly ReportSender m_ReportSender;

			private readonly byte[] m_EncryptedData;

			private readonly WebServicesClient m_Services;

			private readonly ReportSender.NotificationEmailSettings m_NotificationEmailSettings;

			public bool Succeeded;

			public AfterLoginClosure(ReportSender reportSender, byte[] encryptedData, WebServicesClient services, ReportSender.NotificationEmailSettings notificationEmailSettings)
			{
				this.Succeeded = true;
				this.m_ReportSender = reportSender;
				this.m_NotificationEmailSettings = notificationEmailSettings;
				this.m_Services = services;
				this.m_EncryptedData = encryptedData;
			}

			public void AfterLogin(string loginResult)
			{
				if (loginResult != "OK")
				{
					string str = loginResult;
					if (this.m_ReportSender.SendingReportFeedback != null)
					{
						this.m_ReportSender.SendingReportFeedback(this, new SendingReportFeedbackEventArgs(SendingReportStep.ConnectingToServer, str));
					}
					this.Succeeded = false;
					return;
				}
				else
				{
					this.m_ReportSender.InvokeSendingReportFeedback(SendingReportStep.Transfering);
					byte[] bytes = Encoding.UTF8.GetBytes("{727AFF2A-2466-4D47-9015-42C37D73C405}");
					byte[] numArray = new byte[(int)bytes.Length + (int)this.m_EncryptedData.Length];
					Array.Copy(bytes, numArray, (int)bytes.Length);
					Array.Copy(this.m_EncryptedData, 0, numArray, (int)bytes.Length, (int)this.m_EncryptedData.Length);
					ReportSender.AfterUploadClosure afterUploadClosure = new ReportSender.AfterUploadClosure(this.m_ReportSender);
					this.m_Services.Upload(numArray, this.m_NotificationEmailSettings.EmailAddress, this.m_NotificationEmailSettings.AppFriendlyName, this.m_NotificationEmailSettings.BuildFriendlyNumber, new StringCallback(afterUploadClosure.AfterUpload));
					this.Succeeded = afterUploadClosure.Succeeded;
					return;
				}
			}
		}

		private class AfterUploadClosure
		{
			private readonly ReportSender m_ReportSender;

			public bool Succeeded;

			public AfterUploadClosure(ReportSender reportSender)
			{
				this.m_ReportSender = reportSender;
			}

			public void AfterUpload(string transferingResult)
			{
				if (!transferingResult.StartsWith("ERR"))
				{
					this.m_ReportSender.InvokeSendingReportFeedback(SendingReportStep.Finished, string.Empty, transferingResult);
					this.Succeeded = true;
					return;
				}
				else
				{
					this.m_ReportSender.InvokeSendingReportFeedback(SendingReportStep.Transfering, transferingResult);
					this.Succeeded = false;
					return;
				}
			}
		}

		internal class NotificationEmailSettings
		{
			public static ReportSender.NotificationEmailSettings NullEmailSettings;

			private readonly string m_EmailAddress;

			private readonly string m_AppFriendlyName;

			private readonly string m_BuildFriendlyNumber;

			public string AppFriendlyName
			{
				get
				{
					return this.m_AppFriendlyName;
				}
			}

			public string BuildFriendlyNumber
			{
				get
				{
					return this.m_BuildFriendlyNumber;
				}
			}

			public string EmailAddress
			{
				get
				{
					return this.m_EmailAddress;
				}
			}

			static NotificationEmailSettings()
			{
				ReportSender.NotificationEmailSettings.NullEmailSettings = new ReportSender.NotificationEmailSettings(null, null, null);
			}

			public NotificationEmailSettings(string emailAddress, string appFriendlyName, string buildFriendlyNumber)
			{
				this.m_EmailAddress = emailAddress;
				this.m_BuildFriendlyNumber = buildFriendlyNumber;
				this.m_AppFriendlyName = appFriendlyName;
			}
		}
	}
}