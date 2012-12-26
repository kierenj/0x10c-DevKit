/// <summary>
/// Indicates a type of emulation fault
/// </summary>
namespace Devkit.Interfaces
{
	public enum EmulationFault
	{
		/// <summary>
		/// No fault
		/// </summary>
		NoFault,
		/// <summary>
		/// The CPU attempted to write to an literal operand
		/// </summary>
		AttemptToWriteToOpcodeLiteral,
		/// <summary>
		/// The CPU attempted to write to a literal word
		/// </summary>
		AttemptToWriteToLiteralWord,
		/// <summary>
		/// The CPU attempted to execute an invalid or advanced opcodse
		/// </summary>
		ReservedAdvancedOpcode
	}
}