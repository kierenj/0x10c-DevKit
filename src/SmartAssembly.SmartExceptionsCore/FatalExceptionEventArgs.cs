using System;

namespace SmartAssembly.SmartExceptionsCore
{
	public class FatalExceptionEventArgs : EventArgs
	{
		private Exception fatalException;

		public Exception FatalException
		{
			get
			{
				return this.fatalException;
			}
		}

		internal FatalExceptionEventArgs(Exception fatalException)
		{
			this.fatalException = fatalException;
		}
	}
}