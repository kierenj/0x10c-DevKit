using System;
using System.Collections.Generic;
using System.Windows.Input;

/// <summary>
/// Provides a means to query and manipulate the UI associated with an emulated system
/// </summary>
namespace Devkit.Interfaces
{
	public interface ISystemUI
	{
		/// <summary>
		/// Adds an object that will be used by DevKit to produce content for the screen
		/// </summary>
		/// <param name="provider"></param>
		void AddDisplayContentProvider(IDisplayContentProvider provider);

		/// <summary>
		/// Gets the object that will be used by DevKit to produce content for the screen
		/// </summary>
		/// <returns></returns>
		IEnumerable<IDisplayContentProvider> GetDisplayContentProviders();

		/// <summary>
		/// Removes anexisting object that is used by DevKit to produce content for the screen
		/// </summary>
		/// <param name="provider"></param>
		void RemoveDisplayContentProvider(IDisplayContentProvider provider);

		/// <summary>
		/// Fired when the visual object used as the content of the display window is changed
		/// </summary>
		event Delegates.DisplayContentProviderChangedHandler DisplayContentProviderChanged;

		/// <summary>
		/// Fired when a key event is fired on the display window
		/// </summary>
		event KeyEventHandler KeyEvent;
	}
}