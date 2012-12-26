using System;
using System.Windows;

/// <summary>
/// Implemented by objects wishing to provide content for the DevKit system UI display
/// </summary>
namespace Devkit.Interfaces
{
	public interface IDisplayContentProvider
	{
		/// <summary>
		/// Create a visual to be used in the Display window for the DevKit system UI
		/// </summary>
		/// <returns></returns>
		UIElement CreateUIElement();

		/// <summary>
		/// Called when the display window is closed
		/// </summary>
		void DisplayClosed();

		/// <summary>
		/// Called when the display window is opened
		/// </summary>
		void DisplayOpened();
	}
}