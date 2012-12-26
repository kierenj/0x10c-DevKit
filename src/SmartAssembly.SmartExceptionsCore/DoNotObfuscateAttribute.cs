using System;

namespace SmartAssembly.SmartExceptionsCore
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Interface | AttributeTargets.Delegate)]
	public sealed class DoNotObfuscateAttribute : Attribute
	{
		public DoNotObfuscateAttribute()
		{
		}
	}
}