using System;

/// <summary>
/// Memory interface methods, used to handle memory access requests
/// </summary>
namespace Devkit.Interfaces
{
	public abstract class MemoryDevice
	{
		protected readonly MemoryDevice[] _innerReadDevices;

		protected readonly MemoryDevice[] _innerWriteDevices;

		protected readonly MemoryDevice[] _outerReadDevices;

		protected readonly MemoryDevice[] _outerWriteDevices;

		protected readonly ushort _firstOffset;

		protected readonly ushort _lastOffset;

		protected readonly MemoryDeviceType _type;

		/// <summary>
		/// The memory controller of the emulated system: set when the device is registered with it
		/// </summary>
		public IMemoryController Controller
		{
			get;
			set;
		}

		/// <summary>
		/// Gets the first memory offset that this device handles IO requests for
		/// </summary>
		public ushort FirstOffset
		{
			get
			{
				return this._firstOffset;
			}
		}

		/// <summary>
		/// Gets the array of devices lower down in the read chain
		/// </summary>
		public MemoryDevice[] InnerReadDevices
		{
			get
			{
				return this._innerReadDevices;
			}
		}

		/// <summary>
		/// Gets the array of devices lower down in the write chain
		/// </summary>
		public MemoryDevice[] InnerWriteDevices
		{
			get
			{
				return this._innerWriteDevices;
			}
		}

		/// <summary>
		/// Gets the last memory offset that this device handles IO requests for
		/// </summary>
		public ushort LastOffset
		{
			get
			{
				return this._lastOffset;
			}
		}

		/// <summary>
		/// Gets the array of devices higher up in the read chain
		/// </summary>
		public MemoryDevice[] OuterReadDevices
		{
			get
			{
				return this._outerReadDevices;
			}
		}

		/// <summary>
		/// Gets the array of devices higher up in the write chain
		/// </summary>
		public MemoryDevice[] OuterWriteDevices
		{
			get
			{
				return this._outerWriteDevices;
			}
		}

		/// <summary>
		/// Gets the type of hooks/overrides the device implements
		/// </summary>
		public MemoryDeviceType Type
		{
			get
			{
				return this._type;
			}
		}

		/// <summary>
		/// Initialises a new instance of a memory device spanning the range of addresses given
		/// </summary>
		/// <param name="firstOffset"></param>
		/// <param name="lastOffset"></param>
		/// <param name="type"></param>
		protected MemoryDevice(ushort firstOffset, ushort lastOffset, MemoryDeviceType type = 0)
		{
			this._type = type;
			this._firstOffset = firstOffset;
			this._lastOffset = lastOffset;
			this._innerReadDevices = new MemoryDevice[lastOffset - firstOffset + 1];
			this._innerWriteDevices = new MemoryDevice[lastOffset - firstOffset + 1];
			this._outerReadDevices = new MemoryDevice[lastOffset - firstOffset + 1];
			this._outerWriteDevices = new MemoryDevice[lastOffset - firstOffset + 1];
		}

		/// <summary>
		/// Translates the given absolute address into an 0-based index, relative to the first address
		/// for this memory device
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public int GetAddressIndex(ushort address)
		{
			return address - this.FirstOffset;
		}

		/// <summary>
		/// Translates the given 0-based index (relative to the first address for this memory device)
		/// into an absolute address
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public ushort GetIndexAddress(int index)
		{
			return (ushort)(index + this.FirstOffset);
		}

		/// <summary>
		/// Reads a single word from the requested address.  The base 
		/// implementation for this method passes the request down the IO
		/// chain.
		/// </summary>
		/// <param name="address"></param>
		/// <returns></returns>
		public virtual ushort Read(ushort address)
		{
			return this.InnerReadDevices[address - this.FirstOffset].Read(address);
		}

		/// <summary>
		/// Called by the memory controller when the system is reset.  The device
		/// will be returned to its initial state.
		/// </summary>
		public virtual void Reset()
		{
		}

		/// <summary>
		/// Writes a single word to the requested address.  The base
		/// implementation for this method passes the request down the IO
		/// chain.
		/// </summary>
		/// <param name="address"></param>
		/// <param name="data"></param>
		public virtual void Write(ushort address, ushort data)
		{
			this.InnerWriteDevices[address - this.FirstOffset].Write(address, data);
		}
	}
}