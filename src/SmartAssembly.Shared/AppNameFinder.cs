using System;

namespace SmartAssembly.Shared
{
	public sealed class AppNameFinder
	{
		public static string AppName
		{
			get
			{
				return string.Concat(AppNameFinder.AppNameMinusVersion, " ", AppNameFinder.MajorVersion);
			}
		}

		public static string AppNameMinusVersion
		{
			get
			{
				return "SmartAssembly";
			}
		}

		public static int MajorVersion
		{
			get
			{
				Version version = new Version("6.6.4.95");
				return version.Major;
			}
		}

		private AppNameFinder()
		{
		}
	}
}