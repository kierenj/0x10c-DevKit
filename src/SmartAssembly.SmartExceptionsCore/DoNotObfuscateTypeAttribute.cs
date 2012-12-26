using System;

namespace SmartAssembly.SmartExceptionsCore
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
	public sealed class DoNotObfuscateTypeAttribute : Attribute
	{
		public DoNotObfuscateTypeAttribute()
		{
		}
	}
}