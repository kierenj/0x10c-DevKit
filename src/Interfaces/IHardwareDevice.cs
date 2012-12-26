using System;

/// <summary>
/// Represents a hardware device which may be attached to an emulated system
/// </summary>
namespace Devkit.Interfaces
{
	public interface IHardwareDevice
	{
		/// <summary>
		/// Called by the runtime manager when a requested cycle timer has
		/// been completed/elapsed
		/// </summary>
		/// <param name="state"></param>
		void CycleTimerCompleted(object state);

		/// <summary>
		/// Called by the hardware controller, to initialise the device
		/// </summary>
		/// <param name="system"></param>
		void Initialise(IEmulatedSystem system);

		/// <summary>
		/// Handles processing of a hardware interrupt.  The additionalCycles output
		/// parameter must be set to the number of additional cycles that passed during
		/// the handling of the interrupt
		/// </summary>
		/// <param name="additionalCycles"></param>
		void Interrupt(out int additionalCycles);

		/// <summary>
		/// Called at a rate of 60Hz from the host system
		/// </summary>
		void Pulse();

		/// <summary>
		/// Responds to a hardware device information query.
		/// </summary>
		/// <param name="manufacturer"></param>
		/// <param name="hardwareType"></param>
		/// <param name="revision"></param>
		void Query(out uint manufacturer, out uint hardwareType, out ushort revision);

		/// <summary>
		/// Called when the emulated system is reset
		/// </summary>
		void Reset();
	}
}