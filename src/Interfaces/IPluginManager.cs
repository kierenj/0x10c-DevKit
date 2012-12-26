using System;
using System.Collections.ObjectModel;

/// <summary>
/// Allows for querying/manipulation of loaded plugins
/// </summary>
namespace Devkit.Interfaces
{
	public interface IPluginManager
	{
		/// <summary>
		/// Gets the list of currently-loaded plugins
		/// </summary>
		ObservableCollection<IPlugin> LoadedPlugins
		{
			get;
		}

		/// <summary>
		/// Gets the plugin folder
		/// </summary>
		string PluginFolder
		{
			get;
		}

		/// <summary>
		/// Gets the list of unloaded (but available) plugins
		/// </summary>
		ObservableCollection<PluginInfo> UnloadedPlugins
		{
			get;
		}

		/// <summary>
		/// Gets the loaded plugin with the specified guid, or null if the given plugin is not loaded
		/// </summary>
		/// <param name="guid"></param>
		/// <returns></returns>
		IPlugin GetLoadedPlugin(Guid guid);

		/// <summary>
		/// Loads the plugin matching the Guid specified
		/// </summary>
		/// <param name="guid"></param>
		void LoadPlugin(Guid guid);

		/// <summary>
		/// Causes a refresh of unloaded plugins
		/// </summary>
		void RescanPluginFolder();

		/// <summary>
		/// Unloads any plugin matching the Guid specified
		/// </summary>
		/// <param name="guid"></param>
		void UnloadPlugin(Guid guid);
	}
}