using System;

/// <summary>
/// Holds information about a plugin available to the system
/// </summary>
namespace Devkit.Interfaces
{
	public class PluginInfo
	{
		/// <summary>
		/// Gets/sets the author of the plugin
		/// </summary>
		public string Author
		{
			get;
			set;
		}

		/// <summary>
		/// Gets/sets the class within the filename of the primary plugin assembly
		/// </summary>
		public string ClassName
		{
			get;
			set;
		}

		/// <summary>
		/// Gets/sets the description of the plugin
		/// </summary>
		public string Description
		{
			get;
			set;
		}

		/// <summary>
		/// Gets/sets the filename of the primary plugin assembly
		/// </summary>
		public string Filename
		{
			get;
			set;
		}

		/// <summary>
		/// Gets/sets the Guid of a plugin (will not change with different versions)
		/// </summary>
		public Guid Guid
		{
			get;
			set;
		}

		/// <summary>
		/// Gets/sets the name of the plugin
		/// </summary>
		public string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Gets/sets the URL of the plugin
		/// </summary>
		public string Url
		{
			get;
			set;
		}

		/// <summary>
		/// Gets/sets the version of the plugin
		/// </summary>
		public string Version
		{
			get;
			set;
		}

		public PluginInfo()
		{
		}
	}
}