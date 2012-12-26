using SmartAssembly.SmartExceptionsCore;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace SmartAssembly.Zip
{
	public class SimpleZip
	{
		public static string ExceptionMessage;

		public SimpleZip()
		{
		}

		private static bool PublicKeysMatch(Assembly executingAssembly, Assembly callingAssembly)
		{
			byte[] publicKey;
			byte[] numArray;
			int num;
			bool flag;
			try
			{
				publicKey = executingAssembly.GetName().GetPublicKey();
				numArray = callingAssembly.GetName().GetPublicKey();
				if (numArray == null == publicKey == null)
				{
					if (numArray != null)
					{
						num = 0;
						while (num < (int)numArray.Length)
						{
							if (numArray[num] == publicKey[num])
							{
								num++;
							}
							else
							{
								flag = false;
								return flag;
							}
						}
					}
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException5(exception, publicKey, numArray, num, executingAssembly, callingAssembly);
				throw;
			}
			return flag;
		}

		public static byte[] Unzip(byte[] buffer)
		{
			Assembly callingAssembly;
			Assembly executingAssembly;
			SimpleZip.ZipStream zipStream;
			byte[] numArray;
			int num;
			short num1;
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			byte[] numArray1;
			byte[] numArray2;
			byte[] numArray3;
			SimpleZip.Inflater inflater;
			int num7;
			int num8;
			int i;
			int num9;
			int num10 = 0;
			SimpleZip.Inflater inflater1;
			byte[] numArray4;
			byte[] numArray5;
			byte[] numArray6;
			DESCryptoIndirector dESCryptoIndirector;
			ICryptoTransform dESCryptoTransform;
			byte[] numArray7;
			byte[] numArray8;
			byte[] numArray9;
			AESCryptoIndirector aESCryptoIndirector;
			ICryptoTransform aESCryptoTransform;
			byte[] numArray10;
			byte[] numArray11;
			try
			{
				callingAssembly = Assembly.GetCallingAssembly();
				executingAssembly = Assembly.GetExecutingAssembly();
				if (callingAssembly == executingAssembly || SimpleZip.PublicKeysMatch(executingAssembly, callingAssembly))
				{
					zipStream = new SimpleZip.ZipStream(buffer);
					numArray = new byte[0];
					num = zipStream.ReadInt();
					if (num != 67324752)
					{
						num7 = num >> 24;
						num = num - (num7 << 24);
						if (num != 8223355)
						{
							throw new FormatException("Unknown Header");
						}
						else
						{
							if (num7 == 1)
							{
								num8 = zipStream.ReadInt();
								numArray = new byte[num8];
								for (i = 0; i < num8; i = i + num10)
								{
									num9 = zipStream.ReadInt();
									num10 = zipStream.ReadInt();
									numArray4 = new byte[num9];
									zipStream.Read(numArray4, 0, (int)numArray4.Length);
									inflater1 = new SimpleZip.Inflater(numArray4);
									inflater1.Inflate(numArray, i, num10);
								}
							}
							if (num7 == 2)
							{
								byte[] numArray12 = new byte[] { 106, 84, 222, 233, 232, 187, 6, 154 };
								numArray5 = numArray12;
								byte[] numArray13 = new byte[] { 4, 64, 166, 103, 181, 232, 254, 101 };
								numArray6 = numArray13;
								using (dESCryptoIndirector = new DESCryptoIndirector())
								{
									dESCryptoTransform = dESCryptoIndirector.GetDESCryptoTransform(numArray5, numArray6, true);
									using (dESCryptoTransform)
									{
										numArray7 = dESCryptoTransform.TransformFinalBlock(buffer, 4, (int)buffer.Length - 4);
										numArray = SimpleZip.Unzip(numArray7);
									}
								}
							}
							if (num7 == 3)
							{
								byte[] numArray14 = new byte[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
								numArray8 = numArray14;
								byte[] numArray15 = new byte[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };
								numArray9 = numArray15;
								using (aESCryptoIndirector = new AESCryptoIndirector())
								{
									aESCryptoTransform = aESCryptoIndirector.GetAESCryptoTransform(numArray8, numArray9, true);
									using (aESCryptoTransform)
									{
										numArray10 = aESCryptoTransform.TransformFinalBlock(buffer, 4, (int)buffer.Length - 4);
										numArray = SimpleZip.Unzip(numArray10);
									}
								}
							}
						}
					}
					else
					{
						num1 = (short)zipStream.ReadShort();
						num2 = zipStream.ReadShort();
						num3 = zipStream.ReadShort();
						if (num != 67324752 || num1 != 20 || num2 != 0 || num3 != 8)
						{
							throw new FormatException("Wrong Header Signature");
						}
						else
						{
							zipStream.ReadInt();
							zipStream.ReadInt();
							zipStream.ReadInt();
							num4 = zipStream.ReadInt();
							num5 = zipStream.ReadShort();
							num6 = zipStream.ReadShort();
							if (num5 > 0)
							{
								numArray1 = new byte[num5];
								zipStream.Read(numArray1, 0, num5);
							}
							if (num6 > 0)
							{
								numArray2 = new byte[num6];
								zipStream.Read(numArray2, 0, num6);
							}
							numArray3 = new byte[(IntPtr)(zipStream.Length - zipStream.Position)];
							zipStream.Read(numArray3, 0, (int)numArray3.Length);
							inflater = new SimpleZip.Inflater(numArray3);
							numArray = new byte[num4];
							inflater.Inflate(numArray, 0, (int)numArray.Length);
							numArray3 = null;
						}
					}
					zipStream.Close();
					zipStream = null;
					numArray11 = numArray;
				}
				else
				{
					numArray11 = null;
				}
			}
			catch (Exception exception)
			{
				object[] objArray = new object[33];
				objArray[0] = callingAssembly;
				objArray[1] = executingAssembly;
				objArray[2] = zipStream;
				objArray[3] = numArray;
				objArray[4] = num;
				objArray[5] = num1;
				objArray[6] = num2;
				objArray[7] = num3;
				objArray[8] = num4;
				objArray[9] = num5;
				objArray[10] = num6;
				objArray[11] = numArray1;
				objArray[12] = numArray2;
				objArray[13] = numArray3;
				objArray[14] = inflater;
				objArray[15] = num7;
				objArray[16] = num8;
				objArray[17] = i;
				objArray[18] = num9;
				objArray[19] = num10;
				objArray[20] = inflater1;
				objArray[21] = numArray4;
				objArray[22] = numArray5;
				objArray[23] = numArray6;
				objArray[24] = dESCryptoIndirector;
				objArray[25] = dESCryptoTransform;
				objArray[26] = numArray7;
				objArray[27] = numArray8;
				objArray[28] = numArray9;
				objArray[29] = aESCryptoIndirector;
				objArray[30] = aESCryptoTransform;
				objArray[31] = numArray10;
				objArray[32] = buffer;
				StackFrameHelper.CreateExceptionN(exception, objArray);
				throw;
			}
			return numArray11;
		}

		public static byte[] Zip(byte[] buffer)
		{
			byte[] numArray;
			try
			{
				numArray = SimpleZip.Zip(buffer, 1, null, null);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, buffer);
				throw;
			}
			return numArray;
		}

		private static byte[] Zip(byte[] buffer, int version, byte[] key, byte[] iv)
		{
			unsafe
			{
				SimpleZip.ZipStream zipStream;
				SimpleZip.Deflater deflater;
				DateTime now;
				long year;
				uint[] numArray;
				uint num;
				uint num1;
				int num2;
				int length;
				long position;
				byte[] bytes;
				byte[] numArray1;
				int num3;
				byte[] numArray2;
				int num4;
				long totalOut;
				int i;
				byte[] numArray3 = null;
				long position1;
				SimpleZip.Deflater deflater1;
				byte[] numArray4;
				int num5;
				byte[] numArray5;
				int num6;
				long position2;
				byte[] numArray6;
				DESCryptoIndirector dESCryptoIndirector;
				ICryptoTransform dESCryptoTransform;
				byte[] numArray7;
				byte[] numArray8;
				AESCryptoIndirector aESCryptoIndirector;
				ICryptoTransform aESCryptoTransform;
				byte[] numArray9;
				Exception exception;
				byte[] array;
				byte[] numArray10;
				try
				{
					try
					{
						zipStream = new SimpleZip.ZipStream();
						if (version != 0)
						{
							if (version != 1)
							{
								if (version != 2)
								{
									if (version == 3)
									{
										zipStream.WriteInt(58555003);
										numArray8 = SimpleZip.Zip(buffer, 1, null, null);
										using (aESCryptoIndirector = new AESCryptoIndirector())
										{
											aESCryptoTransform = aESCryptoIndirector.GetAESCryptoTransform(key, iv, false);
											using (aESCryptoTransform)
											{
												numArray9 = aESCryptoTransform.TransformFinalBlock(numArray8, 0, (int)numArray8.Length);
												zipStream.Write(numArray9, 0, (int)numArray9.Length);
											}
										}
									}
								}
								else
								{
									zipStream.WriteInt(41777787);
									numArray6 = SimpleZip.Zip(buffer, 1, null, null);
									using (dESCryptoIndirector = new DESCryptoIndirector())
									{
										dESCryptoTransform = dESCryptoIndirector.GetDESCryptoTransform(key, iv, false);
										using (dESCryptoTransform)
										{
											numArray7 = dESCryptoTransform.TransformFinalBlock(numArray6, 0, (int)numArray6.Length);
											zipStream.Write(numArray7, 0, (int)numArray7.Length);
										}
									}
								}
							}
							else
							{
								zipStream.WriteInt(25000571);
								zipStream.WriteInt((int)buffer.Length);
								for (i = 0; i < (int)buffer.Length; i = i + (int)numArray3.Length)
								{
									numArray3 = new byte[Math.Min(2097151, (int)buffer.Length - i)];
									Buffer.BlockCopy(buffer, i, numArray3, 0, (int)numArray3.Length);
									position1 = zipStream.Position;
									zipStream.WriteInt(0);
									zipStream.WriteInt((int)numArray3.Length);
									deflater1 = new SimpleZip.Deflater();
									deflater1.SetInput(numArray3);
									while (!deflater1.IsNeedingInput)
									{
										numArray4 = new byte[512];
										num5 = deflater1.Deflate(numArray4);
										if (num5 <= 0)
										{
											break;
										}
										zipStream.Write(numArray4, 0, num5);
									}
									deflater1.Finish();
									while (!deflater1.IsFinished)
									{
										numArray5 = new byte[512];
										num6 = deflater1.Deflate(numArray5);
										if (num6 <= 0)
										{
											break;
										}
										zipStream.Write(numArray5, 0, num6);
									}
									position2 = zipStream.Position;
									zipStream.Position = position1;
									zipStream.WriteInt((int)deflater1.TotalOut);
									zipStream.Position = position2;
								}
							}
						}
						else
						{
							deflater = new SimpleZip.Deflater();
							now = DateTime.Now;
							year = (ulong)((now.Year - 1980 & 127) << 25 | now.Month << 21 | now.Day << 16 | now.Hour << 11 | now.Minute << 5 | now.Second >> 1);
							uint[] numArray11 = new uint[] { 0, 1996959894, 3993919788, 2567524794, 124634137, 1886057615, 3915621685, 2657392035, 249268274, 2044508324, 3772115230, 2547177864, 162941995, 2125561021, 3887607047, 2428444049, 498536548, 1789927666, 4089016648, 2227061214, 450548861, 1843258603, 4107580753, 2211677639, 325883990, 1684777152, 4251122042, 2321926636, 335633487, 1661365465, 4195302755, 2366115317, 997073096, 1281953886, 3579855332, 2724688242, 1006888145, 1258607687, 3524101629, 2768942443, 901097722, 1119000684, 3686517206, 2898065728, 853044451, 1172266101, 3705015759, 2882616665, 651767980, 1373503546, 3369554304, 3218104598, 565507253, 1454621731, 3485111705, 3099436303, 671266974, 1594198024, 3322730930, 2970347812, 795835527, 1483230225, 3244367275, 3060149565, 1994146192, 31158534, 2563907772, 4023717930, 1907459465, 112637215, 2680153253, 3904427059, 2013776290, 251722036, 2517215374, 3775830040, 2137656763, 141376813, 2439277719, 3865271297, 1802195444, 476864866, 2238001368, 4066508878, 1812370925, 453092731, 2181625025, 4111451223, 1706088902, 314042704, 2344532202, 4240017532, 1658658271, 366619977, 2362670323, 4224994405, 1303535960, 984961486, 2747007092, 3569037538, 1256170817, 1037604311, 2765210733, 3554079995, 1131014506, 879679996, 2909243462, 3663771856, 1141124467, 855842277, 2852801631, 3708648649, 1342533948, 654459306, 3188396048, 3373015174, 1466479909, 544179635, 3110523913, 3462522015, 1591671054, 702138776, 2966460450, 3352799412, 1504918807, 783551873, 3082640443, 3233442989, 3988292384, 2596254646, 62317068, 1957810842, 3939845945, 2647816111, 81470997, 1943803523, 3814918930, 2489596804, 225274430, 2053790376, 3826175755, 2466906013, 167816743, 2097651377, 4027552580, 2265490386, 503444072, 1762050814, 4150417245, 2154129355, 426522225, 1852507879, 4275313526, 2312317920, 282753626, 1742555852, 4189708143, 2394877945, 397917763, 1622183637, 3604390888, 2714866558, 953729732, 1340076626, 3518719985, 2797360999, 1068828381, 1219638859, 3624741850, 2936675148, 906185462, 1090812512, 3747672003, 2825379669, 829329135, 1181335161, 3412177804, 3160834842, 628085408, 1382605366, 3423369109, 3138078467, 570562233, 1426400815, 3317316542, 2998733608, 733239954, 1555261956, 3268935591, 3050360625, 752459403, 1541320221, 2607071920, 3965973030, 1969922972, 40735498, 2617837225, 3943577151, 1913087877, 83908371, 2512341634, 3803740692, 2075208622, 213261112, 2463272603, 3855990285, 2094854071, 198958881, 2262029012, 4057260610, 1759359992, 534414190, 2176718541, 4139329115, 1873836001, 414664567, 2282248934, 4279200368, 1711684554, 285281116, 2405801727, 4167216745, 1634467795, 376229701, 2685067896, 3608007406, 1308918612, 956543938, 2808555105, 3495958263, 1231636301, 1047427035, 2932959818, 3654703836, 1088359270, 936918000, 2847714899, 3736837829, 1202900863, 817233897, 3183342108, 3401237130, 1404277552, 615818150, 3134207493, 3453421203, 1423857449, 601450431, 3009837614, 3294710456, 1567103746, 711928724, 3020668471, 3272380065, 1510334235, 755167117 };
							numArray = numArray11;
							num = -1;
							num1 = num;
							num2 = 0;
							length = (int)buffer.Length;
							while (true)
							{
								int num7 = length - 1;
								length = num7;
								if (num7 < 0)
								{
									break;
								}
								int num8 = num2;
								num2 = num8 + 1;
								num1 = numArray[(num1 ^ buffer[num8]) & 255] ^ num1 >> 8;
							}
							num1 = num1 ^ num;
							zipStream.WriteInt(67324752);
							zipStream.WriteShort(20);
							zipStream.WriteShort(0);
							zipStream.WriteShort(8);
							zipStream.WriteInt((int)year);
							zipStream.WriteInt(num1);
							position = zipStream.Position;
							zipStream.WriteInt(0);
							zipStream.WriteInt((int)buffer.Length);
							bytes = Encoding.UTF8.GetBytes("{data}");
							zipStream.WriteShort((int)bytes.Length);
							zipStream.WriteShort(0);
							zipStream.Write(bytes, 0, (int)bytes.Length);
							deflater.SetInput(buffer);
							while (!deflater.IsNeedingInput)
							{
								numArray1 = new byte[512];
								num3 = deflater.Deflate(numArray1);
								if (num3 <= 0)
								{
									break;
								}
								zipStream.Write(numArray1, 0, num3);
							}
							deflater.Finish();
							while (!deflater.IsFinished)
							{
								numArray2 = new byte[512];
								num4 = deflater.Deflate(numArray2);
								if (num4 <= 0)
								{
									break;
								}
								zipStream.Write(numArray2, 0, num4);
							}
							totalOut = deflater.TotalOut;
							zipStream.WriteInt(33639248);
							zipStream.WriteShort(20);
							zipStream.WriteShort(20);
							zipStream.WriteShort(0);
							zipStream.WriteShort(8);
							zipStream.WriteInt((int)year);
							zipStream.WriteInt(num1);
							zipStream.WriteInt((int)totalOut);
							zipStream.WriteInt((int)buffer.Length);
							zipStream.WriteShort((int)bytes.Length);
							zipStream.WriteShort(0);
							zipStream.WriteShort(0);
							zipStream.WriteShort(0);
							zipStream.WriteShort(0);
							zipStream.WriteInt(0);
							zipStream.WriteInt(0);
							zipStream.Write(bytes, 0, (int)bytes.Length);
							zipStream.WriteInt(101010256);
							zipStream.WriteShort(0);
							zipStream.WriteShort(0);
							zipStream.WriteShort(1);
							zipStream.WriteShort(1);
							zipStream.WriteInt(46 + (int)bytes.Length);
							zipStream.WriteInt((int)((long)(30 + (int)bytes.Length) + totalOut));
							zipStream.WriteShort(0);
							zipStream.Seek(position, SeekOrigin.Begin);
							zipStream.WriteInt((int)totalOut);
						}
						zipStream.Flush();
						zipStream.Close();
						array = zipStream.ToArray();
					}
					catch (Exception exception1)
					{
						exception = exception1;
						SimpleZip.ExceptionMessage = string.Concat("ERR 2003: ", exception.Message);
						throw;
					}
					numArray10 = array;
				}
				catch (Exception exception2)
				{
					object[] objArray = new object[39];
					objArray[0] = zipStream;
					objArray[1] = deflater;
					objArray[2] = now;
					objArray[3] = year;
					objArray[4] = numArray;
					objArray[5] = num;
					objArray[6] = num1;
					objArray[7] = num2;
					objArray[8] = length;
					objArray[9] = position;
					objArray[10] = bytes;
					objArray[11] = numArray1;
					objArray[12] = num3;
					objArray[13] = numArray2;
					objArray[14] = num4;
					objArray[15] = totalOut;
					objArray[16] = i;
					objArray[17] = numArray3;
					objArray[18] = position1;
					objArray[19] = deflater1;
					objArray[20] = numArray4;
					objArray[21] = num5;
					objArray[22] = numArray5;
					objArray[23] = num6;
					objArray[24] = position2;
					objArray[25] = numArray6;
					objArray[26] = dESCryptoIndirector;
					objArray[27] = dESCryptoTransform;
					objArray[28] = numArray7;
					objArray[29] = numArray8;
					objArray[30] = aESCryptoIndirector;
					objArray[31] = aESCryptoTransform;
					objArray[32] = numArray9;
					objArray[33] = exception;
					objArray[34] = array;
					objArray[35] = buffer;
					objArray[36] = version;
					objArray[37] = key;
					objArray[38] = iv;
					StackFrameHelper.CreateExceptionN(exception2, objArray);
					throw;
				}
				return numArray10;
			}
		}

		public static byte[] ZipAndAES(byte[] buffer, byte[] key, byte[] iv)
		{
			byte[] numArray;
			try
			{
				numArray = SimpleZip.Zip(buffer, 3, key, iv);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, buffer, key, iv);
				throw;
			}
			return numArray;
		}

		public static byte[] ZipAndEncrypt(byte[] buffer, byte[] key, byte[] iv)
		{
			byte[] numArray;
			try
			{
				numArray = SimpleZip.Zip(buffer, 2, key, iv);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, buffer, key, iv);
				throw;
			}
			return numArray;
		}

		internal sealed class Deflater
		{
			private const int IS_FLUSHING = 4;

			private const int IS_FINISHING = 8;

			private const int BUSY_STATE = 16;

			private const int FLUSHING_STATE = 20;

			private const int FINISHING_STATE = 28;

			private const int FINISHED_STATE = 30;

			private int state;

			private long totalOut;

			private SimpleZip.DeflaterPending pending;

			private SimpleZip.DeflaterEngine engine;

			public bool IsFinished
			{
				get
				{
					bool isFlushed;
					try
					{
						if (this.state != 30)
						{
							isFlushed = false;
						}
						else
						{
							isFlushed = this.pending.IsFlushed;
						}
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return isFlushed;
				}
			}

			public bool IsNeedingInput
			{
				get
				{
					bool flag;
					try
					{
						flag = this.engine.NeedsInput();
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return flag;
				}
			}

			public long TotalOut
			{
				get
				{
					long num;
					try
					{
						num = this.totalOut;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return num;
				}
			}

			public Deflater()
			{
				this.state = 16;
				this.totalOut = (long)0;
				try
				{
					this.pending = new SimpleZip.DeflaterPending();
					this.engine = new SimpleZip.DeflaterEngine(this.pending);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
			}

			public int Deflate(byte[] output)
			{
				int num;
				int length;
				int num1;
				int num2;
				int i;
				int num3;
				try
				{
					num = 0;
					length = (int)output.Length;
					num1 = length;
					while (true)
					{
						num2 = this.pending.Flush(output, num, length);
						num = num + num2;
						SimpleZip.Deflater deflater = this;
						deflater.totalOut = deflater.totalOut + (long)num2;
						length = length - num2;
						if (length == 0 || this.state == 30)
						{
							break;
						}
						if (!this.engine.Deflate(((this.state & 4) != 0), ((this.state & 8) != 0)))
						{
							if (this.state == 16)
							{
								num3 = num1 - length;
								return num3;
							}
							if (this.state != 20)
							{
								if (this.state == 28)
								{
									this.pending.AlignToByte();
									this.state = 30;
								}
							}
							else
							{
								for (i = 8 + (-this.pending.BitCount & 7); i > 0; i = i - 10)
								{
									this.pending.WriteBits(2, 10);
								}
								this.state = 16;
							}
						}
					}
					num3 = num1 - length;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException7(exception, num, length, num1, num2, i, this, output);
					throw;
				}
				return num3;
			}

			public void Finish()
			{
				try
				{
					SimpleZip.Deflater deflater = this;
					deflater.state = deflater.state | 12;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
			}

			public void SetInput(byte[] buffer)
			{
				try
				{
					this.engine.SetInput(buffer);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, buffer);
					throw;
				}
			}
		}

		internal sealed class DeflaterEngine
		{
			private const int MAX_MATCH = 258;

			private const int MIN_MATCH = 3;

			private const int WSIZE = 32768;

			private const int WMASK = 32767;

			private const int HASH_SIZE = 32768;

			private const int HASH_MASK = 32767;

			private const int HASH_SHIFT = 5;

			private const int MIN_LOOKAHEAD = 262;

			private const int MAX_DIST = 32506;

			private const int TOO_FAR = 4096;

			private int ins_h;

			private short[] head;

			private short[] prev;

			private int matchStart;

			private int matchLen;

			private bool prevAvailable;

			private int blockStart;

			private int strstart;

			private int lookahead;

			private byte[] window;

			private byte[] inputBuf;

			private int totalIn;

			private int inputOff;

			private int inputEnd;

			private SimpleZip.DeflaterPending pending;

			private SimpleZip.DeflaterHuffman huffman;

			public DeflaterEngine(SimpleZip.DeflaterPending pending)
			{
				int num;
				try
				{
					this.pending = pending;
					this.huffman = new SimpleZip.DeflaterHuffman(pending);
					this.window = new byte[65536];
					this.head = new short[32768];
					this.prev = new short[32768];
					bool flag = true;
					num = (int)flag;
					this.strstart = (int)flag;
					this.blockStart = num;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException3(exception, num, this, pending);
					throw;
				}
			}

			public bool Deflate(bool flush, bool finish)
			{
				bool flag;
				bool flag1;
				bool flag2;
				bool flag3;
				try
				{
					do
					{
						this.FillWindow();
						if (!flush)
						{
							flag3 = false;
						}
						else
						{
							flag3 = this.inputOff == this.inputEnd;
						}
						flag1 = flag3;
						flag = this.DeflateSlow(flag1, finish);
					}
					while (this.pending.IsFlushed && flag);
					flag2 = flag;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, flag, flag1, this, flush, finish);
					throw;
				}
				return flag2;
			}

			private bool DeflateSlow(bool flush, bool finish)
			{
				int num;
				int num1;
				int num2;
				int num3;
				bool flag;
				bool flag1;
				bool flag2;
				int num4;
				try
				{
					if (this.lookahead >= 262 || flush)
					{
						while (this.lookahead >= 262 || flush)
						{
							if (this.lookahead != 0)
							{
								if (this.strstart >= 65274)
								{
									this.SlideWindow();
								}
								num = this.matchStart;
								num1 = this.matchLen;
								if (this.lookahead >= 3)
								{
									num2 = this.InsertString();
									if (num2 != 0 && this.strstart - num2 <= 32506 && this.FindLongestMatch(num2) && this.matchLen <= 5 && this.matchLen == 3 && this.strstart - this.matchStart > 4096)
									{
										this.matchLen = 2;
									}
								}
								if (num1 < 3 || this.matchLen > num1)
								{
									if (this.prevAvailable)
									{
										this.huffman.TallyLit(this.window[this.strstart - 1] & 255);
									}
									this.prevAvailable = true;
									SimpleZip.DeflaterEngine deflaterEngine = this;
									deflaterEngine.strstart = deflaterEngine.strstart + 1;
									SimpleZip.DeflaterEngine deflaterEngine1 = this;
									deflaterEngine1.lookahead = deflaterEngine1.lookahead - 1;
								}
								else
								{
									this.huffman.TallyDist(this.strstart - 1 - num, num1);
									num1 = num1 - 2;
									do
									{
										SimpleZip.DeflaterEngine deflaterEngine2 = this;
										deflaterEngine2.strstart = deflaterEngine2.strstart + 1;
										SimpleZip.DeflaterEngine deflaterEngine3 = this;
										deflaterEngine3.lookahead = deflaterEngine3.lookahead - 1;
										if (this.lookahead >= 3)
										{
											this.InsertString();
										}
										num4 = num1 - 1;
										num1 = num4;
									}
									while (num4 > 0);
									SimpleZip.DeflaterEngine deflaterEngine4 = this;
									deflaterEngine4.strstart = deflaterEngine4.strstart + 1;
									SimpleZip.DeflaterEngine deflaterEngine5 = this;
									deflaterEngine5.lookahead = deflaterEngine5.lookahead - 1;
									this.prevAvailable = false;
									this.matchLen = 2;
								}
								if (!this.huffman.IsFull())
								{
									continue;
								}
								num3 = this.strstart - this.blockStart;
								if (this.prevAvailable)
								{
									num3--;
								}
								if (!finish || this.lookahead != 0)
								{
									flag2 = false;
								}
								else
								{
									flag2 = !this.prevAvailable;
								}
								flag = flag2;
								this.huffman.FlushBlock(this.window, this.blockStart, num3, flag);
								SimpleZip.DeflaterEngine deflaterEngine6 = this;
								deflaterEngine6.blockStart = deflaterEngine6.blockStart + num3;
								flag1 = !flag;
								return flag1;
							}
							else
							{
								if (this.prevAvailable)
								{
									this.huffman.TallyLit(this.window[this.strstart - 1] & 255);
								}
								this.prevAvailable = false;
								this.huffman.FlushBlock(this.window, this.blockStart, this.strstart - this.blockStart, finish);
								this.blockStart = this.strstart;
								flag1 = false;
								return flag1;
							}
						}
						flag1 = true;
					}
					else
					{
						flag1 = false;
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException8(exception, num, num1, num2, num3, flag, this, flush, finish);
					throw;
				}
				return flag1;
			}

			public void FillWindow()
			{
				int num;
				try
				{
					if (this.strstart >= 65274)
					{
						this.SlideWindow();
					}
					while (this.lookahead < 262 && this.inputOff < this.inputEnd)
					{
						num = 65536 - this.lookahead - this.strstart;
						if (num > this.inputEnd - this.inputOff)
						{
							num = this.inputEnd - this.inputOff;
						}
						Array.Copy(this.inputBuf, this.inputOff, this.window, this.strstart + this.lookahead, num);
						SimpleZip.DeflaterEngine deflaterEngine = this;
						deflaterEngine.inputOff = deflaterEngine.inputOff + num;
						SimpleZip.DeflaterEngine deflaterEngine1 = this;
						deflaterEngine1.totalIn = deflaterEngine1.totalIn + num;
						SimpleZip.DeflaterEngine deflaterEngine2 = this;
						deflaterEngine2.lookahead = deflaterEngine2.lookahead + num;
					}
					if (this.lookahead >= 3)
					{
						this.UpdateHash();
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, num, this);
					throw;
				}
			}

			private bool FindLongestMatch(int curMatch)
			{
				int num;
				int num1;
				short[] numArray;
				int num2;
				int num3;
				int num4;
				int num5;
				int num6;
				int num7;
				byte num8;
				byte num9;
				bool flag;
				int num10;
				int num11;
				int num12;
				try
				{
					num = 128;
					num1 = 128;
					numArray = this.prev;
					num2 = this.strstart;
					num4 = this.strstart + this.matchLen;
					num5 = Math.Max(this.matchLen, 2);
					num6 = Math.Max(this.strstart - 32506, 0);
					num7 = this.strstart + 258 - 1;
					num8 = this.window[num4 - 1];
					num9 = this.window[num4];
					if (num5 >= 8)
					{
						num = num >> 2;
					}
					if (num1 > this.lookahead)
					{
						num1 = this.lookahead;
					}
					do
					{
						if (this.window[curMatch + num5] == num9 && this.window[curMatch + num5 - 1] == num8 && this.window[curMatch] == this.window[num2] && this.window[curMatch + 1] == this.window[num2 + 1])
						{
							num3 = curMatch + 2;
							num2 = num2 + 2;
							do
							{
								int num13 = num2 + 1;
								num2 = num13;
								int num14 = num3 + 1;
								num3 = num14;
								if (this.window[num13] != this.window[num14])
								{
									break;
								}
								int num15 = num2 + 1;
								num2 = num15;
								int num16 = num3 + 1;
								num3 = num16;
								if (this.window[num15] != this.window[num16])
								{
									break;
								}
								int num17 = num2 + 1;
								num2 = num17;
								int num18 = num3 + 1;
								num3 = num18;
								if (this.window[num17] != this.window[num18])
								{
									break;
								}
								int num19 = num2 + 1;
								num2 = num19;
								int num20 = num3 + 1;
								num3 = num20;
								if (this.window[num19] != this.window[num20])
								{
									break;
								}
								int num21 = num2 + 1;
								num2 = num21;
								int num22 = num3 + 1;
								num3 = num22;
								if (this.window[num21] != this.window[num22])
								{
									break;
								}
								int num23 = num2 + 1;
								num2 = num23;
								int num24 = num3 + 1;
								num3 = num24;
								if (this.window[num23] != this.window[num24])
								{
									break;
								}
								int num25 = num2 + 1;
								num2 = num25;
								int num26 = num3 + 1;
								num3 = num26;
								if (this.window[num25] != this.window[num26])
								{
									break;
								}
								num11 = num2 + 1;
								num2 = num11;
								num12 = num3 + 1;
								num3 = num12;
							}
							while (this.window[num11] == this.window[num12] && num2 < num7);
							if (num2 > num4)
							{
								this.matchStart = curMatch;
								num4 = num2;
								num5 = num2 - this.strstart;
								if (num5 >= num1)
								{
									break;
								}
								num8 = this.window[num4 - 1];
								num9 = this.window[num4];
							}
							num2 = this.strstart;
						}
						int num27 = numArray[curMatch & 32767] & 65535;
						curMatch = num27;
						if (num27 <= num6)
						{
							break;
						}
						num10 = num - 1;
						num = num10;
					}
					while (num10 != 0);
					this.matchLen = Math.Min(num5, this.lookahead);
					flag = this.matchLen >= 3;
				}
				catch (Exception exception)
				{
					object[] objArray = new object[13];
					objArray[0] = num;
					objArray[1] = num1;
					objArray[2] = numArray;
					objArray[3] = num2;
					objArray[4] = num3;
					objArray[5] = num4;
					objArray[6] = num5;
					objArray[7] = num6;
					objArray[8] = num7;
					objArray[9] = num8;
					objArray[10] = num9;
					objArray[11] = this;
					objArray[12] = curMatch;
					StackFrameHelper.CreateExceptionN(exception, objArray);
					throw;
				}
				return flag;
			}

			private int InsertString()
			{
				short num;
				int insH;
				int num1;
				try
				{
					insH = (this.ins_h << 5 ^ this.window[this.strstart + 2]) & 32767;
					short num2 = this.head[insH];
					num = num2;
					this.prev[this.strstart & 32767] = num2;
					this.head[insH] = (short)this.strstart;
					this.ins_h = insH;
					num1 = num & 65535;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException3(exception, num, insH, this);
					throw;
				}
				return num1;
			}

			public bool NeedsInput()
			{
				bool flag;
				try
				{
					flag = this.inputEnd == this.inputOff;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return flag;
			}

			public void SetInput(byte[] buffer)
			{
				try
				{
					this.inputBuf = buffer;
					this.inputOff = 0;
					this.inputEnd = (int)buffer.Length;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, buffer);
					throw;
				}
			}

			private void SlideWindow()
			{
				int i;
				int num;
				int j;
				int num1;
				object obj;
				object obj1;
				try
				{
					Array.Copy(this.window, 32768, this.window, 0, 32768);
					SimpleZip.DeflaterEngine deflaterEngine = this;
					deflaterEngine.matchStart = deflaterEngine.matchStart - 32768;
					SimpleZip.DeflaterEngine deflaterEngine1 = this;
					deflaterEngine1.strstart = deflaterEngine1.strstart - 32768;
					SimpleZip.DeflaterEngine deflaterEngine2 = this;
					deflaterEngine2.blockStart = deflaterEngine2.blockStart - 32768;
					for (i = 0; i < 32768; i++)
					{
						num = this.head[i] & 65535;
						short[] numArray = this.head;
						int num2 = i;
						if (num >= 32768)
						{
							obj = num - 32768;
						}
						else
						{
							obj = null;
						}
						numArray[num2] = (short)obj;
					}
					for (j = 0; j < 32768; j++)
					{
						num1 = this.prev[j] & 65535;
						short[] numArray1 = this.prev;
						int num3 = j;
						if (num1 >= 32768)
						{
							obj1 = num1 - 32768;
						}
						else
						{
							obj1 = null;
						}
						numArray1[num3] = (short)obj1;
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, i, num, j, num1, this);
					throw;
				}
			}

			private void UpdateHash()
			{
				try
				{
					this.ins_h = this.window[this.strstart] << 5 ^ this.window[this.strstart + 1];
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
			}
		}

		internal sealed class DeflaterHuffman
		{
			private const int BUFSIZE = 16384;

			private const int LITERAL_NUM = 286;

			private const int DIST_NUM = 30;

			private const int BITLEN_NUM = 19;

			private const int REP_3_6 = 16;

			private const int REP_3_10 = 17;

			private const int REP_11_138 = 18;

			private const int EOF_SYMBOL = 256;

			private readonly static int[] BL_ORDER;

			private readonly static byte[] bit4Reverse;

			private SimpleZip.DeflaterPending pending;

			private SimpleZip.DeflaterHuffman.Tree literalTree;

			private SimpleZip.DeflaterHuffman.Tree distTree;

			private SimpleZip.DeflaterHuffman.Tree blTree;

			private short[] d_buf;

			private byte[] l_buf;

			private int last_lit;

			private int extra_bits;

			private readonly static short[] staticLCodes;

			private readonly static byte[] staticLLength;

			private readonly static short[] staticDCodes;

			private readonly static byte[] staticDLength;

			static DeflaterHuffman()
			{
				int num;
				try
				{
					int[] numArray = new int[] { 16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15 };
					SimpleZip.DeflaterHuffman.BL_ORDER = numArray;
					byte[] numArray1 = new byte[] { 0, 8, 4, 12, 2, 10, 6, 14, 1, 9, 5, 13, 3, 11, 7, 15 };
					SimpleZip.DeflaterHuffman.bit4Reverse = numArray1;
					SimpleZip.DeflaterHuffman.staticLCodes = new short[286];
					SimpleZip.DeflaterHuffman.staticLLength = new byte[286];
					num = 0;
					while (num < 144)
					{
						SimpleZip.DeflaterHuffman.staticLCodes[num] = SimpleZip.DeflaterHuffman.BitReverse(48 + num << 8);
						int num1 = num;
						num = num1 + 1;
						SimpleZip.DeflaterHuffman.staticLLength[num1] = 8;
					}
					while (num < 256)
					{
						SimpleZip.DeflaterHuffman.staticLCodes[num] = SimpleZip.DeflaterHuffman.BitReverse(256 + num << 7);
						int num2 = num;
						num = num2 + 1;
						SimpleZip.DeflaterHuffman.staticLLength[num2] = 9;
					}
					while (num < 280)
					{
						SimpleZip.DeflaterHuffman.staticLCodes[num] = SimpleZip.DeflaterHuffman.BitReverse(-256 + num << 9);
						int num3 = num;
						num = num3 + 1;
						SimpleZip.DeflaterHuffman.staticLLength[num3] = 7;
					}
					while (num < 286)
					{
						SimpleZip.DeflaterHuffman.staticLCodes[num] = SimpleZip.DeflaterHuffman.BitReverse(-88 + num << 8);
						int num4 = num;
						num = num4 + 1;
						SimpleZip.DeflaterHuffman.staticLLength[num4] = 8;
					}
					SimpleZip.DeflaterHuffman.staticDCodes = new short[30];
					SimpleZip.DeflaterHuffman.staticDLength = new byte[30];
					num = 0;
					while (num < 30)
					{
						SimpleZip.DeflaterHuffman.staticDCodes[num] = SimpleZip.DeflaterHuffman.BitReverse(num << 11);
						SimpleZip.DeflaterHuffman.staticDLength[num] = 5;
						num++;
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, num);
					throw;
				}
			}

			public DeflaterHuffman(SimpleZip.DeflaterPending pending)
			{
				try
				{
					this.pending = pending;
					this.literalTree = new SimpleZip.DeflaterHuffman.Tree(this, 286, 257, 15);
					this.distTree = new SimpleZip.DeflaterHuffman.Tree(this, 30, 1, 15);
					this.blTree = new SimpleZip.DeflaterHuffman.Tree(this, 19, 4, 7);
					this.d_buf = new short[16384];
					this.l_buf = new byte[16384];
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, pending);
					throw;
				}
			}

			public static short BitReverse(int toReverse)
			{
				short num;
				try
				{
					num = (short)(SimpleZip.DeflaterHuffman.bit4Reverse[toReverse & 15] << 12 | SimpleZip.DeflaterHuffman.bit4Reverse[toReverse >> 4 & 15] << 8 | SimpleZip.DeflaterHuffman.bit4Reverse[toReverse >> 8 & 15] << 4 | SimpleZip.DeflaterHuffman.bit4Reverse[toReverse >> 12]);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, toReverse);
					throw;
				}
				return num;
			}

			public void CompressBlock()
			{
				int i;
				int lBuf;
				int dBuf;
				int num;
				int num1;
				int num2;
				try
				{
					for (i = 0; i < this.last_lit; i++)
					{
						lBuf = this.l_buf[i] & 255;
						dBuf = this.d_buf[i];
						int num3 = dBuf;
						dBuf = num3 - 1;
						if (num3 == 0)
						{
							this.literalTree.WriteSymbol(lBuf);
						}
						else
						{
							num = this.Lcode(lBuf);
							this.literalTree.WriteSymbol(num);
							num1 = (num - 261) / 4;
							if (num1 > 0 && num1 <= 5)
							{
								this.pending.WriteBits(lBuf & (1 << (num1 & 31)) - 1, num1);
							}
							num2 = this.Dcode(dBuf);
							this.distTree.WriteSymbol(num2);
							num1 = num2 / 2 - 1;
							if (num1 > 0)
							{
								this.pending.WriteBits(dBuf & (1 << (num1 & 31)) - 1, num1);
							}
						}
					}
					this.literalTree.WriteSymbol(256);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException7(exception, i, lBuf, dBuf, num, num1, num2, this);
					throw;
				}
			}

			private int Dcode(int distance)
			{
				int num;
				int num1;
				try
				{
					num = 0;
					while (distance >= 4)
					{
						num = num + 2;
						distance = distance >> 1;
					}
					num1 = num + distance;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException3(exception, num, this, distance);
					throw;
				}
				return num1;
			}

			public void FlushBlock(byte[] stored, int storedOffset, int storedLength, bool lastBlock)
			{
				int num;
				int i;
				int encodedLength;
				int extraBits;
				int j;
				int k;
				short[] numArray;
				int num1;
				int num2;
				try
				{
					short[] numArray1 = this.literalTree.freqs;
					numArray = numArray1;
					numArray1[256] = (short)(numArray[256] + 1);
					this.literalTree.BuildTree();
					this.distTree.BuildTree();
					this.literalTree.CalcBLFreq(this.blTree);
					this.distTree.CalcBLFreq(this.blTree);
					this.blTree.BuildTree();
					num = 4;
					for (i = 18; i > num; i--)
					{
						if (this.blTree.length[SimpleZip.DeflaterHuffman.BL_ORDER[i]] > 0)
						{
							num = i + 1;
						}
					}
					encodedLength = 14 + num * 3 + this.blTree.GetEncodedLength() + this.literalTree.GetEncodedLength() + this.distTree.GetEncodedLength() + this.extra_bits;
					extraBits = this.extra_bits;
					for (j = 0; j < 286; j++)
					{
						extraBits = extraBits + this.literalTree.freqs[j] * SimpleZip.DeflaterHuffman.staticLLength[j];
					}
					for (k = 0; k < 30; k++)
					{
						extraBits = extraBits + this.distTree.freqs[k] * SimpleZip.DeflaterHuffman.staticDLength[k];
					}
					if (encodedLength >= extraBits)
					{
						encodedLength = extraBits;
					}
					if (storedOffset < 0 || storedLength + 4 >= encodedLength >> 3)
					{
						if (encodedLength != extraBits)
						{
							SimpleZip.DeflaterPending deflaterPending = this.pending;
							int num3 = 4;
							if (lastBlock)
							{
								num1 = 1;
							}
							else
							{
								num1 = 0;
							}
							deflaterPending.WriteBits(num3 + num1, 3);
							this.SendAllTrees(num);
							this.CompressBlock();
							this.Init();
						}
						else
						{
							SimpleZip.DeflaterPending deflaterPending1 = this.pending;
							int num4 = 2;
							if (lastBlock)
							{
								num2 = 1;
							}
							else
							{
								num2 = 0;
							}
							deflaterPending1.WriteBits(num4 + num2, 3);
							this.literalTree.SetStaticCodes(SimpleZip.DeflaterHuffman.staticLCodes, SimpleZip.DeflaterHuffman.staticLLength);
							this.distTree.SetStaticCodes(SimpleZip.DeflaterHuffman.staticDCodes, SimpleZip.DeflaterHuffman.staticDLength);
							this.CompressBlock();
							this.Init();
						}
					}
					else
					{
						this.FlushStoredBlock(stored, storedOffset, storedLength, lastBlock);
					}
				}
				catch (Exception exception)
				{
					object[] objArray = new object[12];
					objArray[0] = num;
					objArray[1] = i;
					objArray[2] = encodedLength;
					objArray[3] = extraBits;
					objArray[4] = j;
					objArray[5] = k;
					objArray[6] = numArray;
					objArray[7] = this;
					objArray[8] = stored;
					objArray[9] = storedOffset;
					objArray[10] = storedLength;
					objArray[11] = lastBlock;
					StackFrameHelper.CreateExceptionN(exception, objArray);
					throw;
				}
			}

			public void FlushStoredBlock(byte[] stored, int storedOffset, int storedLength, bool lastBlock)
			{
				int num;
				try
				{
					SimpleZip.DeflaterPending deflaterPending = this.pending;
					if (lastBlock)
					{
						num = 1;
					}
					else
					{
						num = 0;
					}
					deflaterPending.WriteBits(num, 3);
					this.pending.AlignToByte();
					this.pending.WriteShort(storedLength);
					this.pending.WriteShort(~storedLength);
					this.pending.WriteBlock(stored, storedOffset, storedLength);
					this.Init();
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, this, stored, storedOffset, storedLength, lastBlock);
					throw;
				}
			}

			public void Init()
			{
				try
				{
					this.last_lit = 0;
					this.extra_bits = 0;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
			}

			public bool IsFull()
			{
				bool lastLit;
				try
				{
					lastLit = this.last_lit >= 16384;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return lastLit;
			}

			private int Lcode(int len)
			{
				int num;
				int num1;
				try
				{
					if (len != 255)
					{
						num = 257;
						while (len >= 8)
						{
							num = num + 4;
							len = len >> 1;
						}
						num1 = num + len;
					}
					else
					{
						num1 = 285;
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException3(exception, num, this, len);
					throw;
				}
				return num1;
			}

			public void SendAllTrees(int blTreeCodes)
			{
				int i;
				try
				{
					this.blTree.BuildCodes();
					this.literalTree.BuildCodes();
					this.distTree.BuildCodes();
					this.pending.WriteBits(this.literalTree.numCodes - 257, 5);
					this.pending.WriteBits(this.distTree.numCodes - 1, 5);
					this.pending.WriteBits(blTreeCodes - 4, 4);
					for (i = 0; i < blTreeCodes; i++)
					{
						this.pending.WriteBits(this.blTree.length[SimpleZip.DeflaterHuffman.BL_ORDER[i]], 3);
					}
					this.literalTree.WriteTree(this.blTree);
					this.distTree.WriteTree(this.blTree);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException3(exception, i, this, blTreeCodes);
					throw;
				}
			}

			public bool TallyDist(int dist, int len)
			{
				int num;
				int num1;
				int num2;
				short[] numArray;
				IntPtr intPtr;
				bool flag;
				try
				{
					this.d_buf[this.last_lit] = (short)dist;
					SimpleZip.DeflaterHuffman deflaterHuffman = this;
					int lastLit = deflaterHuffman.last_lit;
					num2 = lastLit;
					deflaterHuffman.last_lit = lastLit + 1;
					this.l_buf[num2] = (byte)(len - 3);
					num = this.Lcode(len - 3);
					short[] numArray1 = this.literalTree.freqs;
					numArray = numArray1;
					int num3 = num;
					intPtr = (IntPtr)num3;
					numArray1[num3] = (short)(numArray[intPtr] + 1);
					if (num >= 265 && num < 285)
					{
						SimpleZip.DeflaterHuffman extraBits = this;
						extraBits.extra_bits = extraBits.extra_bits + (num - 261) / 4;
					}
					num1 = this.Dcode(dist - 1);
					short[] numArray2 = this.distTree.freqs;
					numArray = numArray2;
					int num4 = num1;
					intPtr = (IntPtr)num4;
					numArray2[num4] = (short)(numArray[intPtr] + 1);
					if (num1 >= 4)
					{
						SimpleZip.DeflaterHuffman extraBits1 = this;
						extraBits1.extra_bits = extraBits1.extra_bits + num1 / 2 - 1;
					}
					flag = this.IsFull();
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException8(exception, num, num1, num2, numArray, intPtr, this, dist, len);
					throw;
				}
				return flag;
			}

			public bool TallyLit(int lit)
			{
				int num;
				short[] numArray;
				IntPtr intPtr;
				bool flag;
				try
				{
					this.d_buf[this.last_lit] = 0;
					SimpleZip.DeflaterHuffman deflaterHuffman = this;
					int lastLit = deflaterHuffman.last_lit;
					num = lastLit;
					deflaterHuffman.last_lit = lastLit + 1;
					this.l_buf[num] = (byte)lit;
					short[] numArray1 = this.literalTree.freqs;
					numArray = numArray1;
					int num1 = lit;
					intPtr = (IntPtr)num1;
					numArray1[num1] = (short)(numArray[intPtr] + 1);
					flag = this.IsFull();
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, num, numArray, intPtr, this, lit);
					throw;
				}
				return flag;
			}

			public sealed class Tree
			{
				public short[] freqs;

				public byte[] length;

				public int minNumCodes;

				public int numCodes;

				private short[] codes;

				private int[] bl_counts;

				private int maxLength;

				private SimpleZip.DeflaterHuffman dh;

				public Tree(SimpleZip.DeflaterHuffman dh, int elems, int minCodes, int maxLength)
				{
					try
					{
						this.dh = dh;
						this.minNumCodes = minCodes;
						this.maxLength = maxLength;
						this.freqs = new short[elems];
						this.bl_counts = new int[maxLength];
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException5(exception, this, dh, elems, minCodes, maxLength);
						throw;
					}
				}

				public void BuildCodes()
				{
					int[] numArray;
					int blCounts;
					int i;
					int j;
					int num;
					int[] numArray1;
					IntPtr intPtr;
					try
					{
						numArray = new int[this.maxLength];
						blCounts = 0;
						this.codes = new short[(int)this.freqs.Length];
						for (i = 0; i < this.maxLength; i++)
						{
							numArray[i] = blCounts;
							blCounts = blCounts + (this.bl_counts[i] << (15 - i & 31));
						}
						for (j = 0; j < this.numCodes; j++)
						{
							num = this.length[j];
							if (num > 0)
							{
								this.codes[j] = SimpleZip.DeflaterHuffman.BitReverse(numArray[num - 1]);
								int[] numArray2 = numArray;
								numArray1 = numArray2;
								int num1 = num - 1;
								intPtr = (IntPtr)num1;
								numArray2[num1] = numArray1[intPtr] + (1 << (16 - num & 31));
							}
						}
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException8(exception, numArray, blCounts, i, j, num, numArray1, intPtr, this);
						throw;
					}
				}

				private void BuildLength(int[] childs)
				{
					int length;
					int num;
					int num1;
					int i;
					int[] numArray;
					int j;
					int num2;
					int num3;
					int num4;
					int num5;
					int k;
					int l;
					int num6;
					int num7;
					int[] numArray1;
					IntPtr intPtr;
					try
					{
						this.length = new byte[(int)this.freqs.Length];
						length = (int)childs.Length / 2;
						num = (length + 1) / 2;
						num1 = 0;
						for (i = 0; i < this.maxLength; i++)
						{
							this.bl_counts[i] = 0;
						}
						numArray = new int[length];
						numArray[length - 1] = 0;
						for (j = length - 1; j >= 0; j--)
						{
							if (childs[2 * j + 1] == -1)
							{
								num3 = numArray[j];
								int[] blCounts = this.bl_counts;
								numArray1 = blCounts;
								int num8 = num3 - 1;
								intPtr = (IntPtr)num8;
								blCounts[num8] = numArray1[intPtr] + 1;
								this.length[childs[2 * j]] = (byte)numArray[j];
							}
							else
							{
								num2 = numArray[j] + 1;
								if (num2 > this.maxLength)
								{
									num2 = this.maxLength;
									num1++;
								}
								int num9 = num2;
								num7 = num9;
								numArray[childs[2 * j + 1]] = num9;
								numArray[childs[2 * j]] = num7;
							}
						}
						if (num1 != 0)
						{
							num4 = this.maxLength - 1;
							do
							{
							Label0:
								int num10 = num4 - 1;
								num4 = num10;
								if (this.bl_counts[num10] != 0)
								{
									do
									{
										int[] blCounts1 = this.bl_counts;
										numArray1 = blCounts1;
										int num11 = num4;
										intPtr = (IntPtr)num11;
										blCounts1[num11] = numArray1[intPtr] - 1;
										int[] blCounts2 = this.bl_counts;
										numArray1 = blCounts2;
										int num12 = num4 + 1;
										num4 = num12;
										intPtr = (IntPtr)num12;
										blCounts2[num12] = numArray1[intPtr] + 1;
										num1 = num1 - (1 << (this.maxLength - 1 - num4 & 31));
									}
									while (num1 > 0 && num4 < this.maxLength - 1);
								}
								else
								{
									goto Label0;
								}
							}
							while (num1 > 0);
							int[] numArray2 = this.bl_counts;
							numArray1 = numArray2;
							int num13 = this.maxLength - 1;
							intPtr = (IntPtr)num13;
							numArray2[num13] = numArray1[intPtr] + num1;
							int[] blCounts3 = this.bl_counts;
							numArray1 = blCounts3;
							int num14 = this.maxLength - 2;
							intPtr = (IntPtr)num14;
							blCounts3[num14] = numArray1[intPtr] - num1;
							num5 = 2 * num;
							for (k = this.maxLength; k != 0; k--)
							{
								for (l = this.bl_counts[k - 1]; l > 0; l--)
								{
									int num15 = num5;
									num5 = num15 + 1;
									num6 = 2 * childs[num15];
									if (childs[num6 + 1] != -1)
									{
										continue;
									}
									this.length[childs[num6]] = (byte)k;
								}
							}
						}
					}
					catch (Exception exception)
					{
						object[] objArray = new object[18];
						objArray[0] = length;
						objArray[1] = num;
						objArray[2] = num1;
						objArray[3] = i;
						objArray[4] = numArray;
						objArray[5] = j;
						objArray[6] = num2;
						objArray[7] = num3;
						objArray[8] = num4;
						objArray[9] = num5;
						objArray[10] = k;
						objArray[11] = l;
						objArray[12] = num6;
						objArray[13] = num7;
						objArray[14] = numArray1;
						objArray[15] = intPtr;
						objArray[16] = this;
						objArray[17] = childs;
						StackFrameHelper.CreateExceptionN(exception, objArray);
						throw;
					}
				}

				public void BuildTree()
				{
					int length;
					int[] numArray;
					int num;
					int num1;
					int i;
					int num2;
					int j;
					int num3 = 0;
					int num4;
					int num5;
					int[] numArray1;
					int[] numArray2;
					int num6;
					int num7;
					int num8;
					int num9;
					int num10;
					int num11;
					int k;
					int num12;
					int num13;
					int num14;
					int num15;
					try
					{
						length = (int)this.freqs.Length;
						numArray = new int[length];
						num = 0;
						num1 = 0;
						for (i = 0; i < length; i++)
						{
							num2 = this.freqs[i];
							if (num2 != 0)
							{
								int num16 = num;
								num = num16 + 1;
								for (j = num16; j > 0; j = num3)
								{
									int num17 = (j - 1) / 2;
									num3 = num17;
									if (this.freqs[numArray[num17]] <= num2)
									{
										break;
									}
									numArray[j] = numArray[num3];
								}
								numArray[j] = i;
								num1 = i;
							}
						}
						while (num < 2)
						{
							if (num1 < 2)
							{
								int num18 = num1 + 1;
								num15 = num18;
								num1 = num18;
							}
							else
							{
								num15 = 0;
							}
							num4 = num15;
							int num19 = num;
							num = num19 + 1;
							numArray[num19] = num4;
						}
						this.numCodes = Math.Max(num1 + 1, this.minNumCodes);
						num5 = num;
						numArray1 = new int[4 * num - 2];
						numArray2 = new int[2 * num - 1];
						num6 = num5;
						num7 = 0;
						while (num7 < num)
						{
							num8 = numArray[num7];
							numArray1[2 * num7] = num8;
							numArray1[2 * num7 + 1] = -1;
							numArray2[num7] = this.freqs[num8] << 8;
							numArray[num7] = num7;
							num7++;
						}
						do
						{
							num9 = numArray[0];
							int num20 = num - 1;
							num = num20;
							num10 = numArray[num20];
							num11 = 0;
							for (k = 1; k < num; k = k * 2 + 1)
							{
								if (k + 1 < num && numArray2[numArray[k]] > numArray2[numArray[k + 1]])
								{
									k++;
								}
								numArray[num11] = numArray[k];
								num11 = k;
							}
							num12 = numArray2[num10];
							while (true)
							{
								int num21 = num11;
								k = num21;
								if (num21 <= 0)
								{
									break;
								}
								int num22 = (k - 1) / 2;
								num11 = num22;
								if (numArray2[numArray[num22]] <= num12)
								{
									break;
								}
								numArray[k] = numArray[num11];
							}
							numArray[k] = num10;
							num13 = numArray[0];
							int num23 = num6;
							num6 = num23 + 1;
							num10 = num23;
							numArray1[2 * num10] = num9;
							numArray1[2 * num10 + 1] = num13;
							num14 = Math.Min(numArray2[num9] & 255, numArray2[num13] & 255);
							int num24 = numArray2[num9] + numArray2[num13] - num14 + 1;
							num12 = num24;
							numArray2[num10] = num24;
							num11 = 0;
							k = 1;
							while (k < num)
							{
								if (k + 1 < num && numArray2[numArray[k]] > numArray2[numArray[k + 1]])
								{
									k++;
								}
								numArray[num11] = numArray[k];
								num11 = k;
								k = num11 * 2 + 1;
							}
							while (true)
							{
								int num25 = num11;
								k = num25;
								if (num25 <= 0)
								{
									break;
								}
								int num26 = (k - 1) / 2;
								num11 = num26;
								if (numArray2[numArray[num26]] <= num12)
								{
									break;
								}
								numArray[k] = numArray[num11];
							}
							numArray[k] = num10;
						}
						while (num > 1);
						this.BuildLength(numArray1);
					}
					catch (Exception exception)
					{
						object[] objArray = new object[23];
						objArray[0] = length;
						objArray[1] = numArray;
						objArray[2] = num;
						objArray[3] = num1;
						objArray[4] = i;
						objArray[5] = num2;
						objArray[6] = j;
						objArray[7] = num3;
						objArray[8] = num4;
						objArray[9] = num5;
						objArray[10] = numArray1;
						objArray[11] = numArray2;
						objArray[12] = num6;
						objArray[13] = num7;
						objArray[14] = num8;
						objArray[15] = num9;
						objArray[16] = num10;
						objArray[17] = num11;
						objArray[18] = k;
						objArray[19] = num12;
						objArray[20] = num13;
						objArray[21] = num14;
						objArray[22] = this;
						StackFrameHelper.CreateExceptionN(exception, objArray);
						throw;
					}
				}

				public void CalcBLFreq(SimpleZip.DeflaterHuffman.Tree blTree)
				{
					int num;
					int num1;
					int num2;
					int num3;
					int num4;
					int num5;
					short[] numArray;
					IntPtr intPtr;
					int num6;
					try
					{
						num3 = -1;
						num4 = 0;
						while (num4 < this.numCodes)
						{
							num2 = 1;
							num5 = this.length[num4];
							if (num5 != 0)
							{
								num = 6;
								num1 = 3;
								if (num3 != num5)
								{
									short[] numArray1 = blTree.freqs;
									numArray = numArray1;
									int num7 = num5;
									intPtr = (IntPtr)num7;
									numArray1[num7] = (short)(numArray[intPtr] + 1);
									num2 = 0;
								}
							}
							else
							{
								num = 138;
								num1 = 3;
							}
							num3 = num5;
							num4++;
							do
							{
								if (num4 >= this.numCodes || num3 != this.length[num4])
								{
									break;
								}
								num4++;
								num6 = num2 + 1;
								num2 = num6;
							}
							while (num6 < num);
							if (num2 >= num1)
							{
								if (num3 == 0)
								{
									if (num2 > 10)
									{
										short[] numArray2 = blTree.freqs;
										numArray = numArray2;
										numArray2[18] = (short)(numArray[18] + 1);
									}
									else
									{
										short[] numArray3 = blTree.freqs;
										numArray = numArray3;
										numArray3[17] = (short)(numArray[17] + 1);
									}
								}
								else
								{
									short[] numArray4 = blTree.freqs;
									numArray = numArray4;
									numArray4[16] = (short)(numArray[16] + 1);
								}
							}
							else
							{
								short[] numArray5 = blTree.freqs;
								numArray = numArray5;
								int num8 = num3;
								intPtr = (IntPtr)num8;
								numArray5[num8] = (short)(numArray[intPtr] + (short)num2);
							}
						}
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException10(exception, num, num1, num2, num3, num4, num5, numArray, intPtr, this, blTree);
						throw;
					}
				}

				public int GetEncodedLength()
				{
					int num;
					int i;
					int num1;
					try
					{
						num = 0;
						for (i = 0; i < (int)this.freqs.Length; i++)
						{
							num = num + this.freqs[i] * this.length[i];
						}
						num1 = num;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException3(exception, num, i, this);
						throw;
					}
					return num1;
				}

				public void SetStaticCodes(short[] stCodes, byte[] stLength)
				{
					try
					{
						this.codes = stCodes;
						this.length = stLength;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException3(exception, this, stCodes, stLength);
						throw;
					}
				}

				public void WriteSymbol(int code)
				{
					try
					{
						this.dh.pending.WriteBits(this.codes[code] & 65535, this.length[code]);
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException2(exception, this, code);
						throw;
					}
				}

				public void WriteTree(SimpleZip.DeflaterHuffman.Tree blTree)
				{
					int num;
					int num1;
					int num2;
					int num3;
					int num4;
					int num5;
					int num6;
					try
					{
						num3 = -1;
						num4 = 0;
						while (num4 < this.numCodes)
						{
							num2 = 1;
							num5 = this.length[num4];
							if (num5 != 0)
							{
								num = 6;
								num1 = 3;
								if (num3 != num5)
								{
									blTree.WriteSymbol(num5);
									num2 = 0;
								}
							}
							else
							{
								num = 138;
								num1 = 3;
							}
							num3 = num5;
							num4++;
							do
							{
								if (num4 >= this.numCodes || num3 != this.length[num4])
								{
									break;
								}
								num4++;
								num6 = num2 + 1;
								num2 = num6;
							}
							while (num6 < num);
							if (num2 >= num1)
							{
								if (num3 == 0)
								{
									if (num2 > 10)
									{
										blTree.WriteSymbol(18);
										this.dh.pending.WriteBits(num2 - 11, 7);
									}
									else
									{
										blTree.WriteSymbol(17);
										this.dh.pending.WriteBits(num2 - 3, 3);
									}
								}
								else
								{
									blTree.WriteSymbol(16);
									this.dh.pending.WriteBits(num2 - 3, 2);
								}
							}
							else
							{
								while (true)
								{
									int num7 = num2;
									num2 = num7 - 1;
									if (num7 <= 0)
									{
										break;
									}
									blTree.WriteSymbol(num3);
								}
							}
						}
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException8(exception, num, num1, num2, num3, num4, num5, this, blTree);
						throw;
					}
				}
			}
		}

		internal sealed class DeflaterPending
		{
			protected byte[] buf;

			private int start;

			private int end;

			private uint bits;

			private int bitCount;

			public int BitCount
			{
				get
				{
					int num;
					try
					{
						num = this.bitCount;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return num;
				}
			}

			public bool IsFlushed
			{
				get
				{
					bool flag;
					try
					{
						flag = this.end == 0;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return flag;
				}
			}

			public DeflaterPending()
			{
				this.buf = new byte[65536];
				this.start = 0;
				this.end = 0;
				this.bits = 0;
				this.bitCount = 0;
			}

			public void AlignToByte()
			{
				int num;
				try
				{
					if (this.bitCount > 0)
					{
						SimpleZip.DeflaterPending deflaterPending = this;
						int num1 = deflaterPending.end;
						num = num1;
						deflaterPending.end = num1 + 1;
						this.buf[num] = (byte)this.bits;
						if (this.bitCount > 8)
						{
							SimpleZip.DeflaterPending deflaterPending1 = this;
							int num2 = deflaterPending1.end;
							num = num2;
							deflaterPending1.end = num2 + 1;
							this.buf[num] = (byte)(this.bits >> 8);
						}
					}
					this.bits = 0;
					this.bitCount = 0;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, num, this);
					throw;
				}
			}

			public int Flush(byte[] output, int offset, int length)
			{
				int num;
				int num1;
				try
				{
					if (this.bitCount >= 8)
					{
						SimpleZip.DeflaterPending deflaterPending = this;
						int num2 = deflaterPending.end;
						num = num2;
						deflaterPending.end = num2 + 1;
						this.buf[num] = (byte)this.bits;
						SimpleZip.DeflaterPending deflaterPending1 = this;
						deflaterPending1.bits = deflaterPending1.bits >> 8;
						SimpleZip.DeflaterPending deflaterPending2 = this;
						deflaterPending2.bitCount = deflaterPending2.bitCount - 8;
					}
					if (length <= this.end - this.start)
					{
						Array.Copy(this.buf, this.start, output, offset, length);
						SimpleZip.DeflaterPending deflaterPending3 = this;
						deflaterPending3.start = deflaterPending3.start + length;
					}
					else
					{
						length = this.end - this.start;
						Array.Copy(this.buf, this.start, output, offset, length);
						this.start = 0;
						this.end = 0;
					}
					num1 = length;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, num, this, output, offset, length);
					throw;
				}
				return num1;
			}

			public void WriteBits(int b, int count)
			{
				int num;
				try
				{
					SimpleZip.DeflaterPending deflaterPending = this;
					deflaterPending.bits = deflaterPending.bits | b << (this.bitCount & 31);
					SimpleZip.DeflaterPending deflaterPending1 = this;
					deflaterPending1.bitCount = deflaterPending1.bitCount + count;
					if (this.bitCount >= 16)
					{
						SimpleZip.DeflaterPending deflaterPending2 = this;
						int num1 = deflaterPending2.end;
						num = num1;
						deflaterPending2.end = num1 + 1;
						this.buf[num] = (byte)this.bits;
						SimpleZip.DeflaterPending deflaterPending3 = this;
						int num2 = deflaterPending3.end;
						num = num2;
						deflaterPending3.end = num2 + 1;
						this.buf[num] = (byte)(this.bits >> 8);
						SimpleZip.DeflaterPending deflaterPending4 = this;
						deflaterPending4.bits = deflaterPending4.bits >> 16;
						SimpleZip.DeflaterPending deflaterPending5 = this;
						deflaterPending5.bitCount = deflaterPending5.bitCount - 16;
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException4(exception, num, this, b, count);
					throw;
				}
			}

			public void WriteBlock(byte[] block, int offset, int len)
			{
				try
				{
					Array.Copy(block, offset, this.buf, this.end, len);
					SimpleZip.DeflaterPending deflaterPending = this;
					deflaterPending.end = deflaterPending.end + len;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException4(exception, this, block, offset, len);
					throw;
				}
			}

			public void WriteShort(int s)
			{
				int num;
				try
				{
					SimpleZip.DeflaterPending deflaterPending = this;
					int num1 = deflaterPending.end;
					num = num1;
					deflaterPending.end = num1 + 1;
					this.buf[num] = (byte)s;
					SimpleZip.DeflaterPending deflaterPending1 = this;
					int num2 = deflaterPending1.end;
					num = num2;
					deflaterPending1.end = num2 + 1;
					this.buf[num] = (byte)(s >> 8);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException3(exception, num, this, s);
					throw;
				}
			}
		}

		internal sealed class Inflater
		{
			private const int DECODE_HEADER = 0;

			private const int DECODE_DICT = 1;

			private const int DECODE_BLOCKS = 2;

			private const int DECODE_STORED_LEN1 = 3;

			private const int DECODE_STORED_LEN2 = 4;

			private const int DECODE_STORED = 5;

			private const int DECODE_DYN_HEADER = 6;

			private const int DECODE_HUFFMAN = 7;

			private const int DECODE_HUFFMAN_LENBITS = 8;

			private const int DECODE_HUFFMAN_DIST = 9;

			private const int DECODE_HUFFMAN_DISTBITS = 10;

			private const int DECODE_CHKSUM = 11;

			private const int FINISHED = 12;

			private readonly static int[] CPLENS;

			private readonly static int[] CPLEXT;

			private readonly static int[] CPDIST;

			private readonly static int[] CPDEXT;

			private int mode;

			private int neededBits;

			private int repLength;

			private int repDist;

			private int uncomprLen;

			private bool isLastBlock;

			private SimpleZip.StreamManipulator input;

			private SimpleZip.OutputWindow outputWindow;

			private SimpleZip.InflaterDynHeader dynHeader;

			private SimpleZip.InflaterHuffmanTree litlenTree;

			private SimpleZip.InflaterHuffmanTree distTree;

			static Inflater()
			{
				try
				{
					int[] numArray = new int[] { 3, 4, 5, 6, 7, 8, 9, 10, 11, 13, 15, 17, 19, 23, 27, 31, 35, 43, 51, 59, 67, 83, 99, 115, 131, 163, 195, 227, 258 };
					SimpleZip.Inflater.CPLENS = numArray;
					int[] numArray1 = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 0 };
					SimpleZip.Inflater.CPLEXT = numArray1;
					int[] numArray2 = new int[] { 1, 2, 3, 4, 5, 7, 9, 13, 17, 25, 33, 49, 65, 97, 129, 193, 257, 385, 513, 769, 1025, 1537, 2049, 3073, 4097, 6145, 8193, 12289, 16385, 24577 };
					SimpleZip.Inflater.CPDIST = numArray2;
					int[] numArray3 = new int[] { 0, 0, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13 };
					SimpleZip.Inflater.CPDEXT = numArray3;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
			}

			public Inflater(byte[] bytes)
			{
				try
				{
					this.input = new SimpleZip.StreamManipulator();
					this.outputWindow = new SimpleZip.OutputWindow();
					this.mode = 2;
					this.input.SetInput(bytes, 0, (int)bytes.Length);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, bytes);
					throw;
				}
			}

			private bool Decode()
			{
				int num;
				int num1;
				int num2;
				int num3;
				bool isNeedingInput;
				try
				{
					num3 = this.mode;
					switch (num3)
					{
						case 2:
						{
							if (!this.isLastBlock)
							{
								num = this.input.PeekBits(3);
								if (num >= 0)
								{
									this.input.DropBits(3);
									if ((num & 1) != 0)
									{
										this.isLastBlock = true;
									}
									num3 = num >> 1;
									switch (num3)
									{
										case 0:
										{
											this.input.SkipToByteBoundary();
											this.mode = 3;
											break;
										}
										case 1:
										{
											this.litlenTree = SimpleZip.InflaterHuffmanTree.defLitLenTree;
											this.distTree = SimpleZip.InflaterHuffmanTree.defDistTree;
											this.mode = 7;
											break;
										}
										case 2:
										{
											this.dynHeader = new SimpleZip.InflaterDynHeader();
											this.mode = 6;
											break;
										}
									}
									isNeedingInput = true;
									break;
								}
								else
								{
									isNeedingInput = false;
									break;
								}
							}
							else
							{
								this.mode = 12;
								isNeedingInput = false;
								break;
							}
						}
						case 3:
						{
							int num4 = this.input.PeekBits(16);
							num3 = num4;
							this.uncomprLen = num4;
							if (num3 >= 0)
							{
								this.input.DropBits(16);
								this.mode = 4;
								goto Label0;
							}
							else
							{
								isNeedingInput = false;
								break;
							}
						}
						case 4:
						{
						Label0:
							num1 = this.input.PeekBits(16);
							if (num1 >= 0)
							{
								this.input.DropBits(16);
								this.mode = 5;
								goto Label1;
							}
							else
							{
								isNeedingInput = false;
								break;
							}
						}
						case 5:
						{
						Label1:
							num2 = this.outputWindow.CopyStored(this.input, this.uncomprLen);
							SimpleZip.Inflater inflater = this;
							inflater.uncomprLen = inflater.uncomprLen - num2;
							if (this.uncomprLen != 0)
							{
								isNeedingInput = !this.input.IsNeedingInput;
								break;
							}
							else
							{
								this.mode = 2;
								isNeedingInput = true;
								break;
							}
						}
						case 6:
						{
							if (this.dynHeader.Decode(this.input))
							{
								this.litlenTree = this.dynHeader.BuildLitLenTree();
								this.distTree = this.dynHeader.BuildDistTree();
								this.mode = 7;
								goto Label2;
							}
							else
							{
								isNeedingInput = false;
								break;
							}
						}
						case 7:
						case 8:
						case 9:
						case 10:
						{
						Label2:
							isNeedingInput = this.DecodeHuffman();
							break;
						}
						case 11:
						{
						Label3:
							isNeedingInput = false;
							break;
						}
						case 12:
						{
							isNeedingInput = false;
							break;
						}
						default:
						{
							goto Label3;
						}
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, num, num1, num2, num3, this);
					throw;
				}
				return isNeedingInput;
			}

			private bool DecodeHuffman()
			{
				int freeSpace;
				int symbol;
				int num;
				int num1;
				int num2;
				bool flag;
				int num3;
				try
				{
					freeSpace = this.outputWindow.GetFreeSpace();
					while (freeSpace >= 258)
					{
						num2 = this.mode;
						switch (num2)
						{
							case 7:
							{
								do
								{
									int symbol1 = this.litlenTree.GetSymbol(this.input);
									symbol = symbol1;
									if ((symbol1 & -256) == 0)
									{
										this.outputWindow.Write(symbol);
										num3 = freeSpace - 1;
										freeSpace = num3;
									}
									else
									{
										if (symbol >= 257)
										{
											this.repLength = SimpleZip.Inflater.CPLENS[symbol - 257];
											this.neededBits = SimpleZip.Inflater.CPLEXT[symbol - 257];
											goto Label1;
										}
										else
										{
											if (symbol >= 0)
											{
												this.distTree = null;
												this.litlenTree = null;
												this.mode = 2;
												flag = true;
												return flag;
											}
											else
											{
												flag = false;
												return flag;
											}
										}
									}
								}
								while (num3 >= 258);
								flag = true;
								return flag;
							}
							case 8:
							{
							Label1:
								if (this.neededBits > 0)
								{
									this.mode = 8;
									num = this.input.PeekBits(this.neededBits);
									if (num >= 0)
									{
										this.input.DropBits(this.neededBits);
										SimpleZip.Inflater inflater = this;
										inflater.repLength = inflater.repLength + num;
									}
									else
									{
										flag = false;
										return flag;
									}
								}
								this.mode = 9;
								goto Label2;
							}
							case 9:
							{
							Label2:
								symbol = this.distTree.GetSymbol(this.input);
								if (symbol >= 0)
								{
									this.repDist = SimpleZip.Inflater.CPDIST[symbol];
									this.neededBits = SimpleZip.Inflater.CPDEXT[symbol];
									goto Label3;
								}
								else
								{
									flag = false;
									return flag;
								}
							}
							case 10:
							{
							Label3:
								if (this.neededBits > 0)
								{
									this.mode = 10;
									num1 = this.input.PeekBits(this.neededBits);
									if (num1 >= 0)
									{
										this.input.DropBits(this.neededBits);
										SimpleZip.Inflater inflater1 = this;
										inflater1.repDist = inflater1.repDist + num1;
									}
									else
									{
										flag = false;
										return flag;
									}
								}
								this.outputWindow.Repeat(this.repLength, this.repDist);
								freeSpace = freeSpace - this.repLength;
								this.mode = 7;
							}
						}
					}
					flag = true;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException6(exception, freeSpace, symbol, num, num1, num2, this);
					throw;
				}
				return flag;
			}

			public int Inflate(byte[] buf, int offset, int len)
			{
				int num;
				int num1;
				int num2;
				try
				{
					num = 0;
					do
					{
						if (this.mode == 11)
						{
							continue;
						}
						num1 = this.outputWindow.CopyOutput(buf, offset, len);
						offset = offset + num1;
						num = num + num1;
						len = len - num1;
						if (len != 0)
						{
							continue;
						}
						num2 = num;
						return num2;
					}
					while (this.Decode() || this.outputWindow.GetAvailable() > 0 && this.mode != 11);
					num2 = num;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException6(exception, num, num1, this, buf, offset, len);
					throw;
				}
				return num2;
			}
		}

		internal sealed class InflaterDynHeader
		{
			private const int LNUM = 0;

			private const int DNUM = 1;

			private const int BLNUM = 2;

			private const int BLLENS = 3;

			private const int LENS = 4;

			private const int REPS = 5;

			private readonly static int[] repMin;

			private readonly static int[] repBits;

			private byte[] blLens;

			private byte[] litdistLens;

			private SimpleZip.InflaterHuffmanTree blTree;

			private int mode;

			private int lnum;

			private int dnum;

			private int blnum;

			private int num;

			private int repSymbol;

			private byte lastLen;

			private int ptr;

			private readonly static int[] BL_ORDER;

			static InflaterDynHeader()
			{
				try
				{
					int[] numArray = new int[] { 3, 3, 11 };
					SimpleZip.InflaterDynHeader.repMin = numArray;
					int[] numArray1 = new int[] { 2, 3, 7 };
					SimpleZip.InflaterDynHeader.repBits = numArray1;
					int[] numArray2 = new int[] { 16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15 };
					SimpleZip.InflaterDynHeader.BL_ORDER = numArray2;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException0(exception);
					throw;
				}
			}

			public InflaterDynHeader()
			{
			}

			public SimpleZip.InflaterHuffmanTree BuildDistTree()
			{
				byte[] numArray;
				SimpleZip.InflaterHuffmanTree inflaterHuffmanTree;
				try
				{
					numArray = new byte[this.dnum];
					Array.Copy(this.litdistLens, this.lnum, numArray, 0, this.dnum);
					inflaterHuffmanTree = new SimpleZip.InflaterHuffmanTree(numArray);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, numArray, this);
					throw;
				}
				return inflaterHuffmanTree;
			}

			public SimpleZip.InflaterHuffmanTree BuildLitLenTree()
			{
				byte[] numArray;
				SimpleZip.InflaterHuffmanTree inflaterHuffmanTree;
				try
				{
					numArray = new byte[this.lnum];
					Array.Copy(this.litdistLens, 0, numArray, 0, this.lnum);
					inflaterHuffmanTree = new SimpleZip.InflaterHuffmanTree(numArray);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, numArray, this);
					throw;
				}
				return inflaterHuffmanTree;
			}

			public bool Decode(SimpleZip.StreamManipulator input)
			{
				int num;
				int num1;
				int num2;
				int num3;
				int num4;
				byte num5;
				bool flag;
				try
				{
					while (true)
					{
						num4 = this.mode;
						switch (num4)
						{
							case 0:
							{
								this.lnum = input.PeekBits(5);
								if (this.lnum < 0)
								{
									flag = false;
									return flag;
								}
								SimpleZip.InflaterDynHeader inflaterDynHeader = this;
								inflaterDynHeader.lnum = inflaterDynHeader.lnum + 257;
								input.DropBits(5);
								this.mode = 1;
								goto Label6;
							}
							case 1:
							{
							Label6:
								this.dnum = input.PeekBits(5);
								if (this.dnum < 0)
								{
									flag = false;
									return flag;
								}
								SimpleZip.InflaterDynHeader inflaterDynHeader1 = this;
								inflaterDynHeader1.dnum = inflaterDynHeader1.dnum + 1;
								input.DropBits(5);
								this.num = this.lnum + this.dnum;
								this.litdistLens = new byte[this.num];
								this.mode = 2;
								goto Label7;
							}
							case 2:
							{
							Label7:
								this.blnum = input.PeekBits(4);
								if (this.blnum < 0)
								{
									flag = false;
									return flag;
								}
								SimpleZip.InflaterDynHeader inflaterDynHeader2 = this;
								inflaterDynHeader2.blnum = inflaterDynHeader2.blnum + 4;
								input.DropBits(4);
								this.blLens = new byte[19];
								this.ptr = 0;
								this.mode = 3;
								goto Label8;
							}
							case 3:
							{
							Label8:
								while (this.ptr < this.blnum)
								{
									num = input.PeekBits(3);
									if (num >= 0)
									{
										input.DropBits(3);
										this.blLens[SimpleZip.InflaterDynHeader.BL_ORDER[this.ptr]] = (byte)num;
										SimpleZip.InflaterDynHeader inflaterDynHeader3 = this;
										inflaterDynHeader3.ptr = inflaterDynHeader3.ptr + 1;
									}
									else
									{
										flag = false;
										return flag;
									}
								}
								this.blTree = new SimpleZip.InflaterHuffmanTree(this.blLens);
								this.blLens = null;
								this.ptr = 0;
								this.mode = 4;
								goto Label9;
							}
							case 4:
							{
							Label9:
								do
								{
									int symbol = this.blTree.GetSymbol(input);
									num1 = symbol;
									if ((symbol & -16) == 0)
									{
										SimpleZip.InflaterDynHeader inflaterDynHeader4 = this;
										int num6 = inflaterDynHeader4.ptr;
										num4 = num6;
										inflaterDynHeader4.ptr = num6 + 1;
										byte num7 = (byte)num1;
										num5 = num7;
										this.lastLen = num7;
										this.litdistLens[num4] = num5;
									}
									else
									{
										if (num1 >= 0)
										{
											if (num1 >= 17)
											{
												this.lastLen = 0;
											}
											this.repSymbol = num1 - 16;
											this.mode = 5;
											goto Label10;
										}
										else
										{
											flag = false;
											return flag;
										}
									}
								}
								while (this.ptr != this.num);
								flag = true;
								return flag;
							}
							case 5:
							{
							Label10:
								num2 = SimpleZip.InflaterDynHeader.repBits[this.repSymbol];
								num3 = input.PeekBits(num2);
								if (num3 < 0)
								{
									flag = false;
									return flag;
								}
								input.DropBits(num2);
								num3 = num3 + SimpleZip.InflaterDynHeader.repMin[this.repSymbol];
								while (true)
								{
									int num8 = num3;
									num3 = num8 - 1;
									if (num8 <= 0)
									{
										break;
									}
									SimpleZip.InflaterDynHeader inflaterDynHeader5 = this;
									int num9 = inflaterDynHeader5.ptr;
									num4 = num9;
									inflaterDynHeader5.ptr = num9 + 1;
									this.litdistLens[num4] = this.lastLen;
								}
								if (this.ptr == this.num)
								{
									goto Label11;
								}
								this.mode = 4;
							}
						}
					}
				Label11:
					flag = true;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException8(exception, num, num1, num2, num3, num4, num5, this, input);
					throw;
				}
				return flag;
			}
		}

		internal sealed class InflaterHuffmanTree
		{
			private const int MAX_BITLEN = 15;

			private short[] tree;

			public readonly static SimpleZip.InflaterHuffmanTree defLitLenTree;

			public readonly static SimpleZip.InflaterHuffmanTree defDistTree;

			static InflaterHuffmanTree()
			{
				byte[] numArray;
				int num;
				try
				{
					numArray = new byte[288];
					num = 0;
					while (num < 144)
					{
						int num1 = num;
						num = num1 + 1;
						numArray[num1] = 8;
					}
					while (num < 256)
					{
						int num2 = num;
						num = num2 + 1;
						numArray[num2] = 9;
					}
					while (num < 280)
					{
						int num3 = num;
						num = num3 + 1;
						numArray[num3] = 7;
					}
					while (num < 288)
					{
						int num4 = num;
						num = num4 + 1;
						numArray[num4] = 8;
					}
					SimpleZip.InflaterHuffmanTree.defLitLenTree = new SimpleZip.InflaterHuffmanTree(numArray);
					numArray = new byte[32];
					num = 0;
					while (num < 32)
					{
						int num5 = num;
						num = num5 + 1;
						numArray[num5] = 5;
					}
					SimpleZip.InflaterHuffmanTree.defDistTree = new SimpleZip.InflaterHuffmanTree(numArray);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, numArray, num);
					throw;
				}
			}

			public InflaterHuffmanTree(byte[] codeLengths)
			{
				try
				{
					this.BuildTree(codeLengths);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, codeLengths);
					throw;
				}
			}

			private void BuildTree(byte[] codeLengths)
			{
				int[] numArray;
				int[] numArray1;
				int i;
				int num;
				int num1;
				int num2;
				int j;
				int num3;
				int num4;
				int num5;
				int k;
				int num6;
				int num7;
				int l;
				int num8;
				int num9;
				int num10;
				int num11;
				int num12;
				int[] numArray2;
				IntPtr intPtr;
				try
				{
					numArray = new int[16];
					numArray1 = new int[16];
					for (i = 0; i < (int)codeLengths.Length; i++)
					{
						num = codeLengths[i];
						if (num > 0)
						{
							int[] numArray3 = numArray;
							numArray2 = numArray3;
							int num13 = num;
							intPtr = (IntPtr)num13;
							numArray3[num13] = numArray2[intPtr] + 1;
						}
					}
					num1 = 0;
					num2 = 512;
					for (j = 1; j <= 15; j++)
					{
						numArray1[j] = num1;
						num1 = num1 + (numArray[j] << (16 - j & 31));
						if (j >= 10)
						{
							num3 = numArray1[j] & 130944;
							num4 = num1 & 130944;
							num2 = num2 + (num4 - num3 >> (16 - j & 31));
						}
					}
					this.tree = new short[num2];
					num5 = 512;
					for (k = 15; k >= 10; k--)
					{
						num6 = num1 & 130944;
						num1 = num1 - (numArray[k] << (16 - k & 31));
						num7 = num1 & 130944;
						for (l = num7; l < num6; l = l + 128)
						{
							this.tree[SimpleZip.DeflaterHuffman.BitReverse(l)] = (short)(-num5 << 4 | k);
							num5 = num5 + (1 << (k - 9 & 31));
						}
					}
					num8 = 0;
					while (num8 < (int)codeLengths.Length)
					{
						num9 = codeLengths[num8];
						if (num9 != 0)
						{
							num1 = numArray1[num9];
							num10 = SimpleZip.DeflaterHuffman.BitReverse(num1);
							if (num9 > 9)
							{
								num11 = this.tree[num10 & 511];
								num12 = 1 << (num11 & 15 & 31);
								num11 = -(num11 >> 4);
								do
								{
									this.tree[num11 | num10 >> 9] = (short)(num8 << 4 | num9);
									num10 = num10 + (1 << (num9 & 31));
								}
								while (num10 < num12);
							}
							else
							{
								do
								{
									this.tree[num10] = (short)(num8 << 4 | num9);
									num10 = num10 + (1 << (num9 & 31));
								}
								while (num10 < 512);
							}
							numArray1[num9] = num1 + (1 << (16 - num9 & 31));
						}
						num8++;
					}
				}
				catch (Exception exception)
				{
					object[] objArray = new object[23];
					objArray[0] = numArray;
					objArray[1] = numArray1;
					objArray[2] = i;
					objArray[3] = num;
					objArray[4] = num1;
					objArray[5] = num2;
					objArray[6] = j;
					objArray[7] = num3;
					objArray[8] = num4;
					objArray[9] = num5;
					objArray[10] = k;
					objArray[11] = num6;
					objArray[12] = num7;
					objArray[13] = l;
					objArray[14] = num8;
					objArray[15] = num9;
					objArray[16] = num10;
					objArray[17] = num11;
					objArray[18] = num12;
					objArray[19] = numArray2;
					objArray[20] = intPtr;
					objArray[21] = this;
					objArray[22] = codeLengths;
					StackFrameHelper.CreateExceptionN(exception, objArray);
					throw;
				}
			}

			public int GetSymbol(SimpleZip.StreamManipulator input)
			{
				int num;
				int num1;
				int num2;
				int num3;
				int availableBits;
				int availableBits1;
				int num4;
				try
				{
					int num5 = input.PeekBits(9);
					num = num5;
					if (num5 < 0)
					{
						availableBits1 = input.AvailableBits;
						num = input.PeekBits(availableBits1);
						num1 = this.tree[num];
						if (num1 < 0 || (num1 & 15) > availableBits1)
						{
							num4 = -1;
						}
						else
						{
							input.DropBits(num1 & 15);
							num4 = num1 >> 4;
						}
					}
					else
					{
						short num6 = this.tree[num];
						num1 = (int)num6;
						if (num6 < 0)
						{
							num2 = -(num1 >> 4);
							num3 = num1 & 15;
							int num7 = input.PeekBits(num3);
							num = num7;
							if (num7 < 0)
							{
								availableBits = input.AvailableBits;
								num = input.PeekBits(availableBits);
								num1 = this.tree[num2 | num >> 9];
								if ((num1 & 15) > availableBits)
								{
									num4 = -1;
								}
								else
								{
									input.DropBits(num1 & 15);
									num4 = num1 >> 4;
								}
							}
							else
							{
								num1 = this.tree[num2 | num >> 9];
								input.DropBits(num1 & 15);
								num4 = num1 >> 4;
							}
						}
						else
						{
							input.DropBits(num1 & 15);
							num4 = num1 >> 4;
						}
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException8(exception, num, num1, num2, num3, availableBits, availableBits1, this, input);
					throw;
				}
				return num4;
			}
		}

		internal sealed class OutputWindow
		{
			private const int WINDOW_SIZE = 32768;

			private const int WINDOW_MASK = 32767;

			private byte[] window;

			private int windowEnd;

			private int windowFilled;

			public OutputWindow()
			{
				this.window = new byte[32768];
				this.windowEnd = 0;
				this.windowFilled = 0;
			}

			public void CopyDict(byte[] dict, int offset, int len)
			{
				try
				{
					if (this.windowFilled <= 0)
					{
						if (len > 32768)
						{
							offset = offset + len - 32768;
							len = 32768;
						}
						Array.Copy(dict, offset, this.window, 0, len);
						this.windowEnd = len & 32767;
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException4(exception, this, dict, offset, len);
					throw;
				}
			}

			public int CopyOutput(byte[] output, int offset, int len)
			{
				int num;
				int num1;
				int num2;
				int num3;
				try
				{
					num = this.windowEnd;
					if (len <= this.windowFilled)
					{
						num = this.windowEnd - this.windowFilled + len & 32767;
					}
					else
					{
						len = this.windowFilled;
					}
					num1 = len;
					num2 = len - num;
					if (num2 > 0)
					{
						Array.Copy(this.window, 32768 - num2, output, offset, num2);
						offset = offset + num2;
						len = num;
					}
					Array.Copy(this.window, num - len, output, offset, len);
					SimpleZip.OutputWindow outputWindow = this;
					outputWindow.windowFilled = outputWindow.windowFilled - num1;
					if (this.windowFilled >= 0)
					{
						num3 = num1;
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException7(exception, num, num1, num2, this, output, offset, len);
					throw;
				}
				return num3;
			}

			public int CopyStored(SimpleZip.StreamManipulator input, int len)
			{
				int num;
				int num1;
				int num2;
				try
				{
					len = Math.Min(Math.Min(len, 32768 - this.windowFilled), input.AvailableBytes);
					num1 = 32768 - this.windowEnd;
					if (len <= num1)
					{
						num = input.CopyBytes(this.window, this.windowEnd, len);
					}
					else
					{
						num = input.CopyBytes(this.window, this.windowEnd, num1);
						if (num == num1)
						{
							num = num + input.CopyBytes(this.window, 0, len - num1);
						}
					}
					this.windowEnd = this.windowEnd + num & 32767;
					SimpleZip.OutputWindow outputWindow = this;
					outputWindow.windowFilled = outputWindow.windowFilled + num;
					num2 = num;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, num, num1, this, input, len);
					throw;
				}
				return num2;
			}

			public int GetAvailable()
			{
				int num;
				try
				{
					num = this.windowFilled;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return num;
			}

			public int GetFreeSpace()
			{
				int num;
				try
				{
					num = 32768 - this.windowFilled;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return num;
			}

			public void Repeat(int len, int dist)
			{
				int num;
				int num1;
				int num2;
				try
				{
					SimpleZip.OutputWindow outputWindow = this;
					int num3 = outputWindow.windowFilled + len;
					num2 = num3;
					outputWindow.windowFilled = num3;
					if (num2 <= 32768)
					{
						num = this.windowEnd - dist & 32767;
						num1 = 32768 - len;
						if (num > num1 || this.windowEnd >= num1)
						{
							this.SlowRepeat(num, len, dist);
						}
						else
						{
							if (len > dist)
							{
								while (true)
								{
									int num4 = len;
									len = num4 - 1;
									if (num4 <= 0)
									{
										break;
									}
									SimpleZip.OutputWindow outputWindow1 = this;
									int num5 = outputWindow1.windowEnd;
									num2 = num5;
									outputWindow1.windowEnd = num5 + 1;
									int num6 = num;
									num = num6 + 1;
									this.window[num2] = this.window[num6];
								}
							}
							else
							{
								Array.Copy(this.window, num, this.window, this.windowEnd, len);
								SimpleZip.OutputWindow outputWindow2 = this;
								outputWindow2.windowEnd = outputWindow2.windowEnd + len;
							}
						}
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException6(exception, num, num1, num2, this, len, dist);
					throw;
				}
			}

			public void Reset()
			{
				int num;
				try
				{
					bool flag = false;
					num = (int)flag;
					this.windowEnd = (int)flag;
					this.windowFilled = num;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, num, this);
					throw;
				}
			}

			private void SlowRepeat(int repStart, int len, int dist)
			{
				int num;
				try
				{
					while (true)
					{
						int num1 = len;
						len = num1 - 1;
						if (num1 <= 0)
						{
							break;
						}
						SimpleZip.OutputWindow outputWindow = this;
						int num2 = outputWindow.windowEnd;
						num = num2;
						outputWindow.windowEnd = num2 + 1;
						int num3 = repStart;
						repStart = num3 + 1;
						this.window[num] = this.window[num3];
						SimpleZip.OutputWindow outputWindow1 = this;
						outputWindow1.windowEnd = outputWindow1.windowEnd & 32767;
						repStart = repStart & 32767;
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, num, this, repStart, len, dist);
					throw;
				}
			}

			public void Write(int abyte)
			{
				int num;
				try
				{
					SimpleZip.OutputWindow outputWindow = this;
					int num1 = outputWindow.windowFilled;
					num = num1;
					outputWindow.windowFilled = num1 + 1;
					if (num != 32768)
					{
						SimpleZip.OutputWindow outputWindow1 = this;
						int num2 = outputWindow1.windowEnd;
						num = num2;
						outputWindow1.windowEnd = num2 + 1;
						this.window[num] = (byte)abyte;
						SimpleZip.OutputWindow outputWindow2 = this;
						outputWindow2.windowEnd = outputWindow2.windowEnd & 32767;
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException3(exception, num, this, abyte);
					throw;
				}
			}
		}

		internal sealed class StreamManipulator
		{
			private byte[] window;

			private int window_start;

			private int window_end;

			private uint buffer;

			private int bits_in_buffer;

			public int AvailableBits
			{
				get
				{
					int bitsInBuffer;
					try
					{
						bitsInBuffer = this.bits_in_buffer;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return bitsInBuffer;
				}
			}

			public int AvailableBytes
			{
				get
				{
					int windowEnd;
					try
					{
						windowEnd = this.window_end - this.window_start + (this.bits_in_buffer >> 3);
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return windowEnd;
				}
			}

			public bool IsNeedingInput
			{
				get
				{
					bool windowStart;
					try
					{
						windowStart = this.window_start == this.window_end;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return windowStart;
				}
			}

			public StreamManipulator()
			{
				this.window_start = 0;
				this.window_end = 0;
				this.buffer = 0;
				this.bits_in_buffer = 0;
			}

			public int CopyBytes(byte[] output, int offset, int length)
			{
				int num;
				int windowEnd;
				int num1;
				int num2;
				try
				{
					num = 0;
					while (this.bits_in_buffer > 0 && length > 0)
					{
						int num3 = offset;
						offset = num3 + 1;
						output[num3] = (byte)this.buffer;
						SimpleZip.StreamManipulator streamManipulator = this;
						streamManipulator.buffer = streamManipulator.buffer >> 8;
						SimpleZip.StreamManipulator bitsInBuffer = this;
						bitsInBuffer.bits_in_buffer = bitsInBuffer.bits_in_buffer - 8;
						length--;
						num++;
					}
					if (length != 0)
					{
						windowEnd = this.window_end - this.window_start;
						if (length > windowEnd)
						{
							length = windowEnd;
						}
						Array.Copy(this.window, this.window_start, output, offset, length);
						SimpleZip.StreamManipulator windowStart = this;
						windowStart.window_start = windowStart.window_start + length;
						if ((this.window_start - this.window_end & 1) != 0)
						{
							SimpleZip.StreamManipulator streamManipulator1 = this;
							int windowStart1 = streamManipulator1.window_start;
							num1 = windowStart1;
							streamManipulator1.window_start = windowStart1 + 1;
							this.buffer = this.window[num1] & 255;
							this.bits_in_buffer = 8;
						}
						num2 = num + length;
					}
					else
					{
						num2 = num;
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException7(exception, num, windowEnd, num1, this, output, offset, length);
					throw;
				}
				return num2;
			}

			public void DropBits(int n)
			{
				try
				{
					SimpleZip.StreamManipulator streamManipulator = this;
					streamManipulator.buffer = streamManipulator.buffer >> (n & 31);
					SimpleZip.StreamManipulator bitsInBuffer = this;
					bitsInBuffer.bits_in_buffer = bitsInBuffer.bits_in_buffer - n;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, n);
					throw;
				}
			}

			public int PeekBits(int n)
			{
				int num;
				int num1;
				try
				{
					if (this.bits_in_buffer < n)
					{
						if (this.window_start != this.window_end)
						{
							SimpleZip.StreamManipulator bitsInBuffer = this;
							SimpleZip.StreamManipulator streamManipulator = this;
							int windowStart = streamManipulator.window_start;
							num = windowStart;
							streamManipulator.window_start = windowStart + 1;
							SimpleZip.StreamManipulator streamManipulator1 = this;
							int windowStart1 = streamManipulator1.window_start;
							num = windowStart1;
							streamManipulator1.window_start = windowStart1 + 1;
							bitsInBuffer.buffer = bitsInBuffer.buffer | (this.window[num] & 255 | (this.window[num] & 255) << 8) << (this.bits_in_buffer & 31);
							SimpleZip.StreamManipulator bitsInBuffer1 = this;
							bitsInBuffer1.bits_in_buffer = bitsInBuffer1.bits_in_buffer + 16;
						}
						else
						{
							num1 = -1;
							return num1;
						}
					}
					num1 = (int)((ulong)this.buffer & (long)((1 << (n & 31)) - 1));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException3(exception, num, this, n);
					throw;
				}
				return num1;
			}

			public void Reset()
			{
				int num;
				try
				{
					bool flag = false;
					num = (int)flag;
					this.bits_in_buffer = (int)flag;
					int num1 = num;
					num = num1;
					this.window_end = num1;
					int num2 = num;
					num = num2;
					this.window_start = num2;
					this.buffer = (uint)num;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, num, this);
					throw;
				}
			}

			public void SetInput(byte[] buf, int off, int len)
			{
				int num;
				try
				{
					if (this.window_start >= this.window_end)
					{
						num = off + len;
						if (0 > off || off > num || num > (int)buf.Length)
						{
							throw new ArgumentOutOfRangeException();
						}
						else
						{
							if ((len & 1) != 0)
							{
								SimpleZip.StreamManipulator streamManipulator = this;
								int num1 = off;
								off = num1 + 1;
								streamManipulator.buffer = streamManipulator.buffer | (buf[num1] & 255) << (this.bits_in_buffer & 31);
								SimpleZip.StreamManipulator bitsInBuffer = this;
								bitsInBuffer.bits_in_buffer = bitsInBuffer.bits_in_buffer + 8;
							}
							this.window = buf;
							this.window_start = off;
							this.window_end = num;
						}
					}
					else
					{
						throw new InvalidOperationException();
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, num, this, buf, off, len);
					throw;
				}
			}

			public void SkipToByteBoundary()
			{
				try
				{
					SimpleZip.StreamManipulator bitsInBuffer = this;
					bitsInBuffer.buffer = bitsInBuffer.buffer >> (this.bits_in_buffer & 7 & 31);
					SimpleZip.StreamManipulator streamManipulator = this;
					streamManipulator.bits_in_buffer = streamManipulator.bits_in_buffer & -8;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
			}
		}

		internal sealed class ZipStream : MemoryStream
		{
			public ZipStream()
			{
			}

			public ZipStream(byte[] buffer) : base(buffer, false)
			{
			}

			public int ReadInt()
			{
				int num;
				try
				{
					num = this.ReadShort() | this.ReadShort() << 16;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return num;
			}

			public int ReadShort()
			{
				int num;
				try
				{
					num = this.ReadByte() | this.ReadByte() << 8;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return num;
			}

			public void WriteInt(int value)
			{
				try
				{
					this.WriteShort(value);
					this.WriteShort(value >> 16);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}

			public void WriteShort(int value)
			{
				try
				{
					this.WriteByte((byte)(value & 255));
					this.WriteByte((byte)(value >> 8 & 255));
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}
	}
}