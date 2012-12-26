using System;

/// <summary>
/// Represents the emulated system memory controller
/// </summary>
namespace Devkit.Interfaces
{
	public interface IMemoryController
	{
		/// <summary>
		/// Reads a single word from the requested address.
		/// Passes the request onto the configured memory device
		/// for the address.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		ushort Read(ushort address);

		/// <summary>
		/// Registers a new memory device to handle IO requests
		/// </summary>
		/// <param name="device"></param>
		void RegisterMemoryDevice(MemoryDevice device);

		/// <summary>
		/// Unregisters an already-registered memory device
		/// </summary>
		/// <param name="device"></param>
		void UnregisterMemoryDevice(MemoryDevice device);

		/// <summary>
		/// Writes a single word to the requested address.
		/// Passes the request onto the configured memory device
		/// for the address.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="data"></param>
		void Write(ushort address, ushort data);
	}
}