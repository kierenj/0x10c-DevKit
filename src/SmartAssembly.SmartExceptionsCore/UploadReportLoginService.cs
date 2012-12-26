using System;
using System.Net;
using System.Web.Services;
using System.Web.Services.Protocols;

namespace SmartAssembly.SmartExceptionsCore
{
	[WebServiceBinding(Name="LoginServiceSoap", Namespace="http://www.smartassembly.com/webservices/UploadReportLogin/")]
	internal class UploadReportLoginService : SoapHttpClientProtocol
	{
		public UploadReportLoginService()
		{
			base.set_Url(string.Concat(WebServicesClient.UploadReportServer, "UploadReportLogin.asmx"));
			base.set_Timeout(30000);
		}

		[SoapDocumentMethod("http://www.smartassembly.com/webservices/UploadReportLogin/GetServerURL")]
		public string GetServerURL(string licenseID)
		{
			object[] objArray = new object[1];
			objArray[0] = licenseID;
			return (string)base.Invoke("GetServerURL", objArray)[0];
		}

		protected override WebRequest GetWebRequest(Uri uri)
		{
			WebRequest webRequest = base.GetWebRequest(uri);
			HttpWebRequest httpWebRequest = webRequest as HttpWebRequest;
			if (httpWebRequest != null)
			{
				httpWebRequest.ServicePoint.Expect100Continue = false;
			}
			return webRequest;
		}
	}
}