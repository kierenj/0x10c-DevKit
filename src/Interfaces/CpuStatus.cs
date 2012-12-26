/// <summary>
/// Indicates the status of the CPU state machine
/// </summary>
namespace Devkit.Interfaces
{
	public enum CpuStatus
	{
		/// <summary>
		/// Executing a regular instruction
		/// </summary>
		Normal,
		/// <summary>
		/// Executing a single additional read cycle
		/// </summary>
		SingleExtraReadCycle,
		/// <summary>
		/// Executing the first of two additional read cycles
		/// </summary>
		FirstExtraReadCycle,
		/// <summary>
		/// Executing the second of two additional read cycles
		/// </summary>
		FinalExtraReadCycle,
		/// <summary>
		/// Executing an additional instruction-processing cycle
		/// </summary>
		ExtraProcessingCycle,
		/// <summary>
		/// A fault has occurred
		/// </summary>
		Faulted
	}
}