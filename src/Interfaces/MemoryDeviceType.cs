/// <summary>
/// Indicates the type of hooks/overrides a memory device implements
/// </summary>
namespace Devkit.Interfaces
{
	public enum MemoryDeviceType
	{
		/// <summary>
		/// Device handles both read and write requests
		/// </summary>
		ReadWrite,
		/// <summary>
		/// Device handles only read requests
		/// </summary>
		ReadOnly,
		/// <summary>
		/// Device handles only write requests
		/// </summary>
		WriteOnly
	}
}