using System;
using System.Runtime.Serialization;

namespace SmartAssembly.SmartExceptionsCore
{
	[DoNotObfuscateType]
	[Serializable]
	public class SmartStackFrame : ISerializable
	{
		public readonly int MethodID;

		public readonly object[] Objects;

		public readonly int ILOffset;

		public readonly int ExceptionStackDepth;

		internal SmartStackFrame(SerializationInfo info, StreamingContext context)
		{
			this.MethodID = info.GetInt32("UnhandledException.MethodID");
			this.ILOffset = info.GetInt32("UnhandledException.ILOffset");
			this.ExceptionStackDepth = info.GetInt32("UnhandledException.ExceptionStackDepth");
			int num = info.GetInt32("UnhandledException.Objects.Length");
			this.Objects = new object[num];
			for (int i = 0; i < num; i++)
			{
				try
				{
					this.Objects[i] = info.GetValue(string.Format("UnhandledException.Objects[{0}]", i), typeof(string));
				}
				catch (Exception exception)
				{
					this.Objects[i] = "Could not deserialize the obect";
				}
			}
		}

		internal SmartStackFrame(int methodID, object[] objects, int ilOffset, int exceptionStackDepth)
		{
			this.MethodID = methodID;
			this.ExceptionStackDepth = exceptionStackDepth;
			this.ILOffset = ilOffset;
			this.Objects = objects;
		}

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			int length;
			info.AddValue("UnhandledException.MethodID", this.MethodID, typeof(int));
			info.AddValue("UnhandledException.ILOffset", this.ILOffset, typeof(int));
			info.AddValue("UnhandledException.ExceptionStackDepth", this.ExceptionStackDepth, typeof(int));
			if (this.Objects == null)
			{
				length = 0;
			}
			else
			{
				length = (int)this.Objects.Length;
			}
			int num = length;
			info.AddValue("UnhandledException.Objects.Length", num, typeof(int));
			for (int i = 0; i < num; i++)
			{
				string str = string.Format("UnhandledException.Objects[{0}]", i);
				try
				{
					if (this.Objects[i] != null)
					{
						info.AddValue(str, string.Concat(this.Objects[i].GetType(), " - ", this.Objects[i]), typeof(string));
					}
					else
					{
						info.AddValue(str, null, typeof(object));
					}
				}
				catch (Exception exception)
				{
				}
			}
		}
	}
}