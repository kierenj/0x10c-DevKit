using System;

namespace SmartAssembly.Zip
{
	[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method)]
	public sealed class DoNotEncodeStringsAttribute : Attribute
	{
		public DoNotEncodeStringsAttribute()
		{
		}
	}
}