using System;

/// <summary>
/// Used to retrieve information about the state of an emulated CPU
/// </summary>
namespace Devkit.Interfaces
{
	public interface ICpuState
	{
		/// <summary>
		/// Retrieves the value of the extra register
		/// </summary>
		ushort Extra
		{
			get;
		}

		/// <summary>
		/// Retrieves the last emulation fault (relevant if Status is set to Faulted)
		/// </summary>
		EmulationFault Fault
		{
			get;
		}

		/// <summary>
		/// Retrieves the value of the interrupt address register
		/// </summary>
		ushort IA
		{
			get;
		}

		/// <summary>
		/// Indicates whether the CPU is on fire
		/// </summary>
		bool IsOnFire
		{
			get;
		}

		/// <summary>
		/// Retrieves the value of the program counter register
		/// </summary>
		ushort PC
		{
			get;
		}

		/// <summary>
		/// Indicates whether the CPU will currently queue interrupts
		/// </summary>
		bool QueueInterrupts
		{
			get;
		}

		/// <summary>
		/// Gets an array containing general-purpose register values
		/// </summary>
		ushort[] Registers
		{
			get;
		}

		/// <summary>
		/// Retrieves the status of the skip flag
		/// </summary>
		bool SkipFlag
		{
			get;
		}

		/// <summary>
		/// Retrieves the value of the stack pointer register
		/// </summary>
		ushort SP
		{
			get;
		}

		/// <summary>
		/// Retrieves the CPU status flag
		/// </summary>
		CpuStatus Status
		{
			get;
		}

	}
}