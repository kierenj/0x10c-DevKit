using System;

namespace SmartAssembly.SmartExceptionsCore
{
	[AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Method)]
	public class ReportUsageAttribute : Attribute
	{
		public ReportUsageAttribute()
		{
		}

		public ReportUsageAttribute(string featureName)
		{
		}
	}
}