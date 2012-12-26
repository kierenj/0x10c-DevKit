using Devkit.Interfaces.Build;
using System;
using System.Collections.Generic;

/// <summary>
/// Allows for retrieval and manipulation of debug information
/// </summary>
namespace Devkit.Interfaces
{
	public interface IDebugInfo
	{
		/// <summary>
		/// Gets the root code model
		/// </summary>
		INode CodeModel
		{
			get;
		}

		/// <summary>
		/// Gets all symbols
		/// </summary>
		IEnumerable<Symbol> Symbols
		{
			get;
		}

		/// <summary>
		/// Adds additional source referencing information for the given memory location
		/// </summary>
		/// <param name="location"></param>
		/// <param name="info"></param>
		/// <param name="type"></param>
		void AddSourceInfo(long location, IEnumerable<SourceReference> info, MemoryRangeType type);

		/// <summary>
		/// Finds a symbol given an offset
		/// </summary>
		/// <param name="offset"></param>
		/// <returns></returns>
		IEnumerable<Symbol> FindSymbol(int offset);

		/// <summary>
		/// Gets all memory locations with any associated debug information
		/// </summary>
		/// <returns></returns>
		IEnumerable<ushort> GetDebugInfoOffsets();

		/// <summary>
		/// Gets all memory offsets associated with the given source file/line number
		/// </summary>
		/// <param name="path"></param>
		/// <param name="lineNumber"></param>
		/// <returns></returns>
		IEnumerable<ushort> GetOffsets(string path, int lineNumber);

		/// <summary>
		/// Retrieves all source referencing information for the given memory location
		/// </summary>
		/// <param name="location"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		IEnumerable<SourceReference> GetSourceInfo(long location, out MemoryRangeType type);

		/// <summary>
		/// Removes all debug information associated with the given address
		/// </summary>
		/// <param name="address"></param>
		void RemoveDebugInformation(ushort address);
	}
}