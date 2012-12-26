using System;

/// <summary>
/// Represents the controller for hardware attached to an emulated system
/// </summary>
namespace Devkit.Interfaces
{
	public interface IHardwareController
	{
		/// <summary>
		/// Retrieves the number of hardware devices associated with the system
		/// </summary>
		int NumDevices
		{
			get;
		}

		/// <summary>
		/// Registers a new device to handle hardware interrupts, queries
		/// </summary>
		/// <param name="device"></param>
		void RegisterHardwareDevice(IHardwareDevice device);

		/// <summary>
		/// Unregisters an already-registered hardware device
		/// </summary>
		/// <param name="device"></param>
		void UnregisterHardwareDevice(IHardwareDevice device);
	}
}