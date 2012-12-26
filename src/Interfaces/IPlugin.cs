using System;
using System.Collections.Generic;

/// <summary>
/// Interface that plugins must implement to interface with DevKit
/// </summary>
namespace Devkit.Interfaces
{
	public interface IPlugin
	{
		/// <summary>
		/// Gets a list of named actions which can be invoked on the plugin - typically
		/// will appear on a menu in the host application
		/// </summary>
		IEnumerable<string> ActionNames
		{
			get;
		}

		/// <summary>
		/// Gets the author of the plugin
		/// </summary>
		string Author
		{
			get;
		}

		/// <summary>
		/// Gets a description of the plugin
		/// </summary>
		string Description
		{
			get;
		}

		/// <summary>
		/// Gets the unique guid for the plugin (should not change with a new version)
		/// </summary>
		Guid Guid
		{
			get;
		}

		/// <summary>
		/// Gets the name of the plugin
		/// </summary>
		string Name
		{
			get;
		}

		/// <summary>
		/// Gets the URL associated with the plugin
		/// </summary>
		string Url
		{
			get;
		}

		/// <summary>
		/// Gets the current version of the plugin
		/// </summary>
		string Version
		{
			get;
		}

		/// <summary>
		/// Invokes the action with the specified name
		/// </summary>
		/// <param name="name"></param>
		void Action(string name);

		/// <summary>
		/// Initialises the plugin, loads it into the workspace - hooks any required events or functions
		/// </summary>
		/// <param name="workspace"></param>
		void Load(IWorkspace workspace);

		/// <summary>
		/// Uninitialises the plugin, unloads it from the workspace - unhooks and required events or functions
		/// </summary>
		/// <param name="workspace"></param>
		void Unload(IWorkspace workspace);
	}
}