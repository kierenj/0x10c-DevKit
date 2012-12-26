using Microsoft.Win32;
using System;

namespace SmartAssembly.SmartExceptionsWithAdvancedUI
{
	internal class RegistryHelper
	{
		private const string REGISTRY_ROOT = "SOFTWARE\\RedGate\\SmartAssembly";

		public RegistryHelper()
		{
		}

		public static string ReadHKLMRegistryString(string name)
		{
			string empty;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\RedGate\\SmartAssembly");
				if (registryKey != null)
				{
					string value = (string)registryKey.GetValue(name, string.Empty);
					registryKey.Close();
					empty = value;
				}
				else
				{
					empty = string.Empty;
				}
			}
			catch
			{
				empty = string.Empty;
			}
			return empty;
		}

		public static void SaveHKLMRegistryString(string name, string value)
		{
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\RedGate\\SmartAssembly", true);
				if (registryKey == null)
				{
					registryKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\RedGate\\SmartAssembly");
				}
				registryKey.SetValue(name, value);
				registryKey.Close();
			}
			catch
			{
			}
		}
	}
}