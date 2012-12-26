using Microsoft.Win32;
using System;

namespace SmartAssembly.Shared
{
	public class AppPathFinder
	{
		public AppPathFinder()
		{
		}

		public static string ReadInstalledSaPath()
		{
			string str;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(AppRegistryKeys.SubkeyApplication);
				if (registryKey == null)
				{
					registryKey = Registry.LocalMachine.OpenSubKey(AppRegistryKeys.WowSubkeyApplication);
				}
				if (registryKey != null)
				{
					string value = (string)registryKey.GetValue("Path", null);
					registryKey.Close();
					str = value;
				}
				else
				{
					str = null;
				}
			}
			catch
			{
				str = null;
			}
			return str;
		}
	}
}