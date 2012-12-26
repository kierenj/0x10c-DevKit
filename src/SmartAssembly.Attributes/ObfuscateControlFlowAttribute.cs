using System;

namespace SmartAssembly.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Constructor | AttributeTargets.Method)]
	internal class ObfuscateControlFlowAttribute : Attribute
	{
		public ObfuscateControlFlowAttribute()
		{
		}
	}
}