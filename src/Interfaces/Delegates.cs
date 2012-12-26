using Devkit.Interfaces.Build;
using System;
using System.Collections.Generic;

/// <summary>
/// Delegates for DevKit integration
/// </summary>
namespace Devkit.Interfaces
{
	public static class Delegates
	{
		/// <summary>
		/// Handler for when the build status changes
		/// </summary>
		/// <param name="status"></param>
		public delegate void BuildStatusChangedHandler(BuildStatus status);

		/// <summary>
		/// Handler for when debug information is updated
		/// </summary>
		public delegate void DebugInfoChangedHandler();

		/// <summary>
		/// Handles for when the content used in the UI display is changed
		/// </summary>
		/// <param name="provider"></param>
		public delegate void DisplayContentProviderChangedHandler(IEnumerable<IDisplayContentProvider> provider);

		/// <summary>
		/// Handler for when execution is stopped - stoppedCompletely is true to indicate
		/// execution is stopped completely (false indicates a Break condition)
		/// </summary>
		/// <param name="stoppedCompletely"></param>
		public delegate void ExecutionBreakHandler(bool stoppedCompletely);

		/// <summary>
		/// Handler for when execution begins or resumes
		/// </summary>
		public delegate void ExecutionStartedHandler();

		/// <summary>
		/// Handler for when an open file editor is requested to display a specific line number
		/// </summary>
		/// <param name="lineNumber"></param>
		public delegate void ScrollRequestedHandler(int lineNumber);

		/// <summary>
		/// Handler for when the emulated system is reset
		/// </summary>
		public delegate void SystemResetHandler();
	}
}