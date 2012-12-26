using System;

/// <summary>
/// Provides methods to read/write setting data
/// </summary>
namespace Devkit.Interfaces
{
	public interface ISettingsManager
	{
		/// <summary>
		/// Reads the current value of a given setting
		/// </summary>
		/// <param name="category"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		string ReadSetting(string category, string name);

		/// <summary>
		/// Writes a new value for a given setting
		/// </summary>
		/// <param name="category"></param>
		/// <param name="name"></param>
		/// <param name="value"></param>
		void WriteSetting(string category, string name, string value);
	}
}