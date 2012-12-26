using System;
using System.Net;

namespace SmartAssembly.SmartExceptionsCore
{
	internal class WebServicesClient
	{
		internal readonly static string UploadReportServer;

		private string licenseID;

		private string m_ServerURL;

		private IWebProxy proxy;

		static WebServicesClient()
		{
			WebServicesClient.UploadReportServer = "http://sawebservice.red-gate.com/";
		}

		public WebServicesClient(string licenseID)
		{
			this.licenseID = licenseID;
		}

		public void LoginToServer(StringCallback whenDone)
		{
			string mServerURL;
			if (this.m_ServerURL == null)
			{
				try
				{
					UploadReportLoginService uploadReportLoginService = new UploadReportLoginService();
					if (this.proxy != null)
					{
						uploadReportLoginService.set_Proxy(this.proxy);
					}
					this.m_ServerURL = uploadReportLoginService.GetServerURL(this.licenseID);
					if (this.m_ServerURL.Length != 0)
					{
						if (this.m_ServerURL == "ditto")
						{
							this.m_ServerURL = WebServicesClient.UploadReportServer;
						}
					}
					else
					{
						throw new ApplicationException("Cannot connect to webservice");
					}
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					whenDone(string.Concat("ERR 2001: ", exception.Message));
					return;
				}
			}
			StringCallback stringCallback = whenDone;
			if (this.m_ServerURL.StartsWith("ERR"))
			{
				mServerURL = this.m_ServerURL;
			}
			else
			{
				mServerURL = "OK";
			}
			stringCallback(mServerURL);
		}

		public void SetProxy(IWebProxy proxy)
		{
			this.proxy = proxy;
		}

		public void Upload(byte[] data, string email, string appFriendlyName, string buildFriendlyNumber, StringCallback whenDone)
		{
			try
			{
				ReportingService reportingService = new ReportingService(this.m_ServerURL);
				if (this.proxy != null)
				{
					reportingService.set_Proxy(this.proxy);
				}
				whenDone(reportingService.UploadReport2(this.licenseID, data, email, appFriendlyName, buildFriendlyNumber));
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				whenDone(string.Concat("ERR 2002: ", exception.Message));
			}
		}
	}
}