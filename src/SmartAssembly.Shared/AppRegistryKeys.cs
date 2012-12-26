using Microsoft.Win32;
using System;

namespace SmartAssembly.Shared
{
	public sealed class AppRegistryKeys
	{
		public static string SubkeyApplication
		{
			get
			{
				return string.Concat("Software\\Red Gate\\", AppNameFinder.AppName);
			}
		}

		public static string WowSubkeyApplication
		{
			get
			{
				return string.Concat("Software\\Wow6432Node\\Red Gate\\", AppNameFinder.AppName);
			}
		}

		private AppRegistryKeys()
		{
		}

		public static object GetRegistryEntry(string valueName, object defaultValue)
		{
			object obj;
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(AppRegistryKeys.SubkeyApplication);
			using (registryKey)
			{
				if (registryKey != null)
				{
					object value = registryKey.GetValue(valueName, defaultValue);
					return value;
				}
				else
				{
					obj = defaultValue;
				}
			}
			return obj;
		}
	}
}