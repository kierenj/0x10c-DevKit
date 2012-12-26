using System;

namespace SmartAssembly.SmartExceptionsCore
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface)]
	public sealed class DoNotPruneTypeAttribute : Attribute
	{
		public DoNotPruneTypeAttribute()
		{
		}
	}
}