/// <summary>
/// Indicates the type of content of a memory location
/// </summary>
namespace Devkit.Interfaces
{
	public enum MemoryRangeType
	{
		/// <summary>
		/// Content type unknown
		/// </summary>
		Unknown,
		/// <summary>
		/// The memory location/range contains code
		/// </summary>
		Code,
		/// <summary>
		/// The memory location/range contains data
		/// </summary>
		Data
	}
}