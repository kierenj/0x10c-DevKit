using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SmartAssembly.SmartExceptionsCore
{
	public class StackFrameHelper
	{
		public const string DataEntryName = "SmartStackFrames";

		public StackFrameHelper()
		{
		}

		public static void CreateException0(Exception exception)
		{
			StackFrameHelper.CreateExceptionN(exception, new object[0]);
		}

		public static void CreateException1(Exception exception, object o1)
		{
			object[] objArray = new object[1];
			objArray[0] = o1;
			StackFrameHelper.CreateExceptionN(exception, objArray);
		}

		public static void CreateException10(Exception exception, object o1, object o2, object o3, object o4, object o5, object o6, object o7, object o8, object o9, object o10)
		{
			object[] objArray = new object[10];
			objArray[0] = o1;
			objArray[1] = o2;
			objArray[2] = o3;
			objArray[3] = o4;
			objArray[4] = o5;
			objArray[5] = o6;
			objArray[6] = o7;
			objArray[7] = o8;
			objArray[8] = o9;
			objArray[9] = o10;
			StackFrameHelper.CreateExceptionN(exception, objArray);
		}

		public static void CreateException2(Exception exception, object o1, object o2)
		{
			object[] objArray = new object[2];
			objArray[0] = o1;
			objArray[1] = o2;
			StackFrameHelper.CreateExceptionN(exception, objArray);
		}

		public static void CreateException3(Exception exception, object o1, object o2, object o3)
		{
			object[] objArray = new object[3];
			objArray[0] = o1;
			objArray[1] = o2;
			objArray[2] = o3;
			StackFrameHelper.CreateExceptionN(exception, objArray);
		}

		public static void CreateException4(Exception exception, object o1, object o2, object o3, object o4)
		{
			object[] objArray = new object[4];
			objArray[0] = o1;
			objArray[1] = o2;
			objArray[2] = o3;
			objArray[3] = o4;
			StackFrameHelper.CreateExceptionN(exception, objArray);
		}

		public static void CreateException5(Exception exception, object o1, object o2, object o3, object o4, object o5)
		{
			object[] objArray = new object[5];
			objArray[0] = o1;
			objArray[1] = o2;
			objArray[2] = o3;
			objArray[3] = o4;
			objArray[4] = o5;
			StackFrameHelper.CreateExceptionN(exception, objArray);
		}

		public static void CreateException6(Exception exception, object o1, object o2, object o3, object o4, object o5, object o6)
		{
			object[] objArray = new object[6];
			objArray[0] = o1;
			objArray[1] = o2;
			objArray[2] = o3;
			objArray[3] = o4;
			objArray[4] = o5;
			objArray[5] = o6;
			StackFrameHelper.CreateExceptionN(exception, objArray);
		}

		public static void CreateException7(Exception exception, object o1, object o2, object o3, object o4, object o5, object o6, object o7)
		{
			object[] objArray = new object[7];
			objArray[0] = o1;
			objArray[1] = o2;
			objArray[2] = o3;
			objArray[3] = o4;
			objArray[4] = o5;
			objArray[5] = o6;
			objArray[6] = o7;
			StackFrameHelper.CreateExceptionN(exception, objArray);
		}

		public static void CreateException8(Exception exception, object o1, object o2, object o3, object o4, object o5, object o6, object o7, object o8)
		{
			object[] objArray = new object[8];
			objArray[0] = o1;
			objArray[1] = o2;
			objArray[2] = o3;
			objArray[3] = o4;
			objArray[4] = o5;
			objArray[5] = o6;
			objArray[6] = o7;
			objArray[7] = o8;
			StackFrameHelper.CreateExceptionN(exception, objArray);
		}

		public static void CreateException9(Exception exception, object o1, object o2, object o3, object o4, object o5, object o6, object o7, object o8, object o9)
		{
			object[] objArray = new object[9];
			objArray[0] = o1;
			objArray[1] = o2;
			objArray[2] = o3;
			objArray[3] = o4;
			objArray[4] = o5;
			objArray[5] = o6;
			objArray[6] = o7;
			objArray[7] = o8;
			objArray[8] = o9;
			StackFrameHelper.CreateExceptionN(exception, objArray);
		}

		public static void CreateExceptionN(Exception caughtException, object[] objects)
		{
			LinkedList<object> item;
			int metadataToken = -1;
			int lOffset = -1;
			int num = 0;
			StackTrace stackTrace = new StackTrace(caughtException);
			try
			{
				if (caughtException.StackTrace != null)
				{
					char[] chrArray = new char[2];
					chrArray[0] = '\r';
					chrArray[1] = '\n';
					string[] strArrays = caughtException.StackTrace.Split(chrArray);
					string[] strArrays1 = strArrays;
					for (int i = 0; i < (int)strArrays1.Length; i++)
					{
						string str = strArrays1[i];
						if (str.Length > 0)
						{
							num++;
						}
					}
				}
			}
			catch
			{
				num = -1;
			}
			try
			{
				if (stackTrace.FrameCount > 0)
				{
					StackFrame frame = stackTrace.GetFrame(stackTrace.FrameCount - 1);
					metadataToken = (frame.GetMethod().MetadataToken & 16777215) - 1;
					lOffset = frame.GetILOffset();
				}
			}
			catch
			{
			}
			try
			{
				SmartStackFrame smartStackFrame = new SmartStackFrame(metadataToken, objects, lOffset, num);
				if (caughtException.Data.Contains("SmartStackFrames"))
				{
					item = (LinkedList<object>)caughtException.Data["SmartStackFrames"];
				}
				else
				{
					item = new LinkedList<object>();
					caughtException.Data["SmartStackFrames"] = item;
				}
				item.AddLast(smartStackFrame);
			}
			catch
			{
			}
		}
	}
}