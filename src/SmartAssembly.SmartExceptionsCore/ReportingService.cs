using System;
using System.Net;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Serialization;

namespace SmartAssembly.SmartExceptionsCore
{
	[WebServiceBinding(Name="ReportingServiceSoap", Namespace="http://www.smartassembly.com/webservices/Reporting/")]
	internal class ReportingService : SoapHttpClientProtocol
	{
		public ReportingService(string serverUrl)
		{
			base.set_Url(string.Concat(serverUrl, "Reporting.asmx"));
			base.set_Timeout(180000);
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

		[SoapDocumentMethod("http://www.smartassembly.com/webservices/Reporting/UploadReport2")]
		public string UploadReport2(string licenseID, [XmlElement(DataType="base64Binary")] byte[] data, string email, string appFriendlyName, string buildFriendlyNumber)
		{
			object[] objArray = new object[5];
			objArray[0] = licenseID;
			objArray[1] = data;
			objArray[2] = email;
			objArray[3] = appFriendlyName;
			objArray[4] = buildFriendlyNumber;
			return (string)base.Invoke("UploadReport2", objArray)[0];
		}
	}
}