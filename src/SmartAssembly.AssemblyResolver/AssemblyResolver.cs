using SmartAssembly.SmartExceptionsCore;
using System;

namespace SmartAssembly.AssemblyResolver
{
	public class AssemblyResolver
	{
		public AssemblyResolver()
		{
		}

		public static void AttachApp()
		{
			try
			{
				try
				{
					AssemblyResolverHelper.Attach();
				}
				catch (Exception exception)
				{
				}
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException0(exception1);
				throw;
			}
		}
	}
}