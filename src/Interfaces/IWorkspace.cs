using Devkit.Interfaces.Build;
using Devkit.Interfaces.FileDecorators;
using System;
using System.Collections.Generic;
using System.Windows.Threading;

/// <summary>
/// Represents the entire application session
/// </summary>
namespace Devkit.Interfaces
{
	public interface IWorkspace
	{
		/// <summary>
		/// Gets the build manager, which controls and monitors the build process
		/// </summary>
		IBuildManager BuildManager
		{
			get;
		}

		/// <summary>
		/// Gets the debugger - performs debug functionality and expression evaluation
		/// </summary>
		IDebugger Debugger
		{
			get;
		}

		/// <summary>
		/// Gets the plugin manager, which controls and monitors loaded and unloaded plugins
		/// </summary>
		IPluginManager PluginManager
		{
			get;
		}

		/// <summary>
		/// Gets the runtime manager, which controls and monitors the emulated system
		/// </summary>
		IRuntimeManager RuntimeManager
		{
			get;
		}

		/// <summary>
		/// Gets the settings manager, allows for read/write of setting data
		/// </summary>
		ISettingsManager SettingsManager
		{
			get;
		}

		/// <summary>
		/// Gets the dispatcher associated with the UI thread
		/// </summary>
		Dispatcher UiDispatcher
		{
			get;
		}

		/// <summary>
		/// Adds a new decorator to a given file in the workspace
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="file"></param>
		void AddFileDecorator<T>(IFile file)
		where T : FileDecorator, new();

		/// <summary>
		/// Gets a filename to save/load a file from the user
		/// </summary>
		/// <param name="title"></param>
		/// <param name="filetypeFilter"></param>
		/// <param name="defaultExtension"> </param>
		/// <param name="saveDialog"></param>
		/// <returns></returns>
		string FileDialog(string title, string filetypeFilter, bool saveDialog);

		/// <summary>
		/// Retrieves a service of the given type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T GetService<T>()
		where T : class;

		/// <summary>
		/// Retrieves all services of the given type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		IEnumerable<T> GetServices<T>()
		where T : class;

		/// <summary>
		/// Registers a service of the given type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serviceImplementation"></param>
		void RegisterService<T>(T serviceImplementation)
		where T : class;

		/// <summary>
		/// Removes a decorator from a file in the workspace
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="file"></param>
		void RemoveFileDecorator<T>(IFile file)
		where T : FileDecorator, new();

		/// <summary>
		/// Shows a new documentation window with the given content
		/// </summary>
		/// <param name="title"></param>
		/// <param name="data"></param>
		void ShowDocumentationWindow(string title, string data);

		/// <summary>
		/// Shows a new documentation window with the content downloaded from the given Uri
		/// </summary>
		/// <param name="title"></param>
		/// <param name="uri"></param>
		void ShowDocumentationWindow(string title, Uri uri);

		/// <summary>
		/// Unregisters a service of the given type
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="serviceImplementation"></param>
		void UnregisterService<T>(T serviceImplementation)
		where T : class;
	}
}