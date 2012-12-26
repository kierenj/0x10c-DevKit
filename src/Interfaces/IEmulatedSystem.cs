using System;

/// <summary>
/// Represents an emulated system and its components
/// </summary>
namespace Devkit.Interfaces
{
	public interface IEmulatedSystem
	{
		/// <summary>
		/// Retrieves the CPU interface for the system
		/// </summary>
		ICpu Cpu
		{
			get;
		}

		/// <summary>
		/// Retrieves the hardware controller for the system
		/// </summary>
		IHardwareController HardwareController
		{
			get;
		}

		/// <summary>
		/// Retrieves the memory controller for the system
		/// </summary>
		IMemoryController MemoryController
		{
			get;
		}

		/// <summary>
		/// Causes instruction execution to be blocked (or unblocked)
		/// </summary>
		/// <param name="block"></param>
		void BlockExecution(bool block);

		/// <summary>
		/// Causes emulation to be delayed by the given number of cycles, before notifying
		/// the hardware device specified (via the CycleDelayCompleted method)
		/// </summary>
		/// <param name="cycles"></param>
		/// <param name="device"></param>
		/// <param name="state"></param>
		void StartCycleTimer(long cycles, IHardwareDevice device, object state);

		/// <summary>
		/// Causes emulation to be delayed by the given number of seconds, before notifying
		/// the hardware device specified (via the CycleDelayCompleted method).  Cycle-accurate.
		/// </summary>
		/// <param name="cycles"></param>
		/// <param name="device"></param>
		/// <param name="state"></param>
		void StartTimer(double timeSeconds, IHardwareDevice device, object state);

		/// <summary>
		/// Fired when the system is reset completely
		/// </summary>
		event Delegates.SystemResetHandler SystemReset;
	}
}