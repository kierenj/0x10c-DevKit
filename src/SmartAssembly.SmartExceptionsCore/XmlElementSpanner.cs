using System;
using System.Xml;

namespace SmartAssembly.SmartExceptionsCore
{
	internal class XmlElementSpanner : IDisposable
	{
		private readonly XmlWriter m_XmlWriter;

		public XmlElementSpanner(XmlWriter xmlWriter, string name)
		{
			this.m_XmlWriter = xmlWriter;
			this.m_XmlWriter.WriteStartElement(name);
		}

		public void Dispose()
		{
			this.m_XmlWriter.WriteEndElement();
		}
	}
}