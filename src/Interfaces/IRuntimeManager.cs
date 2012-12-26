using System;

/// <summary>
/// Represents the runtime manager - controls the emulated system, holds debug information
/// </summary>
namespace Devkit.Interfaces
{
	public interface IRuntimeManager
	{
		/// <summary>
		/// Gets the current debug information
		/// </summary>
		IDebugInfo CurrentDebugInfo
		{
			get;
		}

		/// <summary>
		/// Retrieves a representation of the emulated system and its components
		/// </summary>
		IEmulatedSystem System
		{
			get;
		}

		/// <summary>
		/// Retrieves the user interface for the emulated system
		/// </summary>
		ISystemUI UI
		{
			get;
		}

		/// <summary>
		/// Causes the emulation to break
		/// </summary>
		void Break();

		/// <summary>
		/// Creates a new discrete system for a custom use
		/// </summary>
		/// <returns></returns>
		IEmulatedSystem CreateSystem();

		/// <summary>
		/// Fired when the current debug information changes
		/// </summary>
		event Delegates.DebugInfoChangedHandler DebugInfoChanged;

		/// <summary>
		/// Fired when execution is broken or stopped
		/// </summary>
		event Delegates.ExecutionBreakHandler ExecutionBreak;

		/// <summary>
		/// Fired when execution begins or resumes
		/// </summary>
		event Delegates.ExecutionStartedHandler ExecutionStarted;
	}
}