using System;

/// <summary>
/// Allows querying and manipulation of CPU state
/// </summary>
namespace Devkit.Interfaces
{
	public interface ICpu
	{
		/// <summary>
		/// Gets or sets the value of the Extra register
		/// </summary>
		ushort Extra
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value of the interrupt address
		/// </summary>
		ushort IA
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value of the 'on fire' flag
		/// </summary>
		bool IsOnFire
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value of the program counter
		/// </summary>
		ushort PC
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value of the 'queue interrupts' flag
		/// </summary>
		bool QueueInterrupts
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the array of general-purpose register values
		/// </summary>
		ushort[] Registers
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the value of the skip flag
		/// </summary>
		bool SkipFlag
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or set the value of the stack pointer
		/// </summary>
		ushort SP
		{
			get;
			set;
		}

		/// <summary>
		/// Flags an emulation fault
		/// </summary>
		/// <param name="fault"></param>
		void EmulationFault(EmulationFault fault);

		/// <summary>
		/// Runs a single instruction, returning the number of cycles passed.
		/// Only to be used when controlling a system created with RuntimeManager.CreateSystem
		/// </summary>
		/// <returns></returns>
		int InstructionStep();

		/// <summary>
		/// Interrupts CPU execution
		/// </summary>
		/// <param name="interrupt"></param>
		void Interrupt(ushort interrupt);
	}
}