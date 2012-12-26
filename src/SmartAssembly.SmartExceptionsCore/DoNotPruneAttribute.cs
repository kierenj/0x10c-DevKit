using System;

namespace SmartAssembly.SmartExceptionsCore
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Interface | AttributeTargets.Parameter | AttributeTargets.Delegate)]
	public sealed class DoNotPruneAttribute : Attribute
	{
		public DoNotPruneAttribute()
		{
		}
	}
}