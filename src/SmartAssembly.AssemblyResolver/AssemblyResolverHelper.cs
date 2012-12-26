using SmartAssembly.SmartExceptionsCore;
using SmartAssembly.Zip;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace SmartAssembly.AssemblyResolver
{
	internal class AssemblyResolverHelper
	{
		internal const string BindList = "{71461f04-2faa-4bb9-a0dd-28a79101b599}";

		private const int MOVEFILE_DELAY_UNTIL_REBOOT = 4;

		private static Hashtable hashtable;

		internal static bool IsWebApplication
		{
			get
			{
				string lower;
				bool flag;
				bool flag1;
				try
				{
					try
					{
						lower = Process.GetCurrentProcess().MainModule.ModuleName.ToLower();
						if (lower != "w3wp.exe")
						{
							if (lower != "aspnet_wp.exe")
							{
								flag1 = false;
								return flag1;
							}
							else
							{
								flag = true;
							}
						}
						else
						{
							flag = true;
						}
					}
					catch
					{
						flag1 = false;
						return flag1;
					}
					flag1 = flag;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, lower, flag);
					throw;
				}
				return flag1;
			}
		}

		static AssemblyResolverHelper()
		{
			try
			{
				AssemblyResolverHelper.hashtable = new Hashtable();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException0(exception);
				throw;
			}
		}

		public AssemblyResolverHelper()
		{
		}

		internal static void Attach()
		{
			try
			{
				try
				{
					AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolverHelper.ResolveAssembly);
				}
				catch
				{
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException0(exception);
				throw;
			}
		}

		[DllImport("kernel32", CharSet=CharSet.None)]
		private static extern bool MoveFileEx(string existingFileName, string newFileName, int flags);

		internal static Assembly ResolveAssembly(object sender, ResolveEventArgs e)
		{
			AssemblyResolverHelper.AssemblyInfo assemblyInfo;
			string assemblyFullName;
			string base64String;
			string[] strArrays;
			string empty;
			bool flag;
			bool flag1;
			int num;
			int num1;
			int num2;
			string str;
			Stream manifestResourceStream;
			int length;
			byte[] numArray;
			Assembly assembly;
			string str1;
			string str2;
			FileStream fileStream;
			Assembly item;
			char[] chrArray;
			Hashtable hashtables = null;
			Assembly assembly1;
			try
			{
				assemblyInfo = new AssemblyResolverHelper.AssemblyInfo(e.Name);
				assemblyFullName = assemblyInfo.GetAssemblyFullName(false);
				base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(assemblyFullName));
				chrArray = new char[1];
				chrArray[0] = ',';
				strArrays = "QXZhbG9uRG9jaywgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj04NWExZTBhZGE3ZWMxM2U0,[z]{64355e1d-e1c9-4b52-9dfd-8eca2f4fc5f7},QXZhbG9uRG9jaw==,[z]{64355e1d-e1c9-4b52-9dfd-8eca2f4fc5f7},RGV2a2l0LkFzc2VtYmxlciwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxs,[z]{05a01177-3cae-4e4e-8d96-124a61febcec},RGV2a2l0LkJ1aWxkLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPW51bGw=,[z]{ef27fa85-8691-49f2-a275-db09171c068c},RGV2a2l0LkNvcmUsIEN1bHR1cmU9bmV1dHJhbCwgUHVibGljS2V5VG9rZW49bnVsbA==,[z]{dae1045a-31c4-4a06-b0e1-9a4d2516b6b2},RGV2a2l0LldvcmtzcGFjZSwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxs,[z]{5362a84b-7e19-4e6d-b4af-6eaf1ba8fcb2},SUNTaGFycENvZGUuQXZhbG9uRWRpdCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1udWxs,[z]{afbb3f7b-1edb-4405-bf4d-0bc28bf872e1},TmluamVjdCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1jNzE5MmRjNTM4MDk0NWU3,[z]{3de09a99-5ded-48ea-8722-68ef60587e0e},TmluamVjdA==,[z]{3de09a99-5ded-48ea-8722-68ef60587e0e}".Split(chrArray);
				empty = string.Empty;
				flag = false;
				flag1 = false;
				num = 0;
				while (num < (int)strArrays.Length - 1)
				{
					if (strArrays[num] != base64String)
					{
						num = num + 2;
					}
					else
					{
						empty = strArrays[num + 1];
						break;
					}
				}
				if (empty.Length == 0 && assemblyInfo.PublicKeyToken.Length == 0)
				{
					base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(assemblyInfo.Name));
					num1 = 0;
					while (num1 < (int)strArrays.Length - 1)
					{
						if (strArrays[num1] != base64String)
						{
							num1 = num1 + 2;
						}
						else
						{
							empty = strArrays[num1 + 1];
							break;
						}
					}
				}
				if (empty.Length > 0)
				{
					if (empty[0] == '[')
					{
						num2 = empty.IndexOf(']');
						str = empty.Substring(1, num2 - 1);
						flag = str.IndexOf('z') >= 0;
						flag1 = str.IndexOf('t') >= 0;
						empty = empty.Substring(num2 + 1);
					}
					lock (AssemblyResolverHelper.hashtable)
					{
						if (!AssemblyResolverHelper.hashtable.ContainsKey(empty))
						{
							manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(empty);
							if (manifestResourceStream == null)
							{
								goto Label1;
							}
							else
							{
								length = (int)manifestResourceStream.Length;
								numArray = new byte[length];
								manifestResourceStream.Read(numArray, 0, length);
								if (flag)
								{
									numArray = SimpleZip.Unzip(numArray);
								}
								assembly = null;
								if (!flag1)
								{
									try
									{
										assembly = Assembly.Load(numArray);
									}
									catch (FileLoadException fileLoadException)
									{
										flag1 = true;
									}
									catch (BadImageFormatException badImageFormatException)
									{
										flag1 = true;
									}
								}
								if (flag1)
								{
									try
									{
										str1 = string.Format("{0}{1}\\", Path.GetTempPath(), empty);
										Directory.CreateDirectory(str1);
										str2 = string.Concat(str1, assemblyInfo.Name, ".dll");
										if (!File.Exists(str2))
										{
											fileStream = File.OpenWrite(str2);
											fileStream.Write(numArray, 0, (int)numArray.Length);
											fileStream.Close();
											AssemblyResolverHelper.MoveFileEx(str2, null, 4);
											AssemblyResolverHelper.MoveFileEx(str1, null, 4);
										}
										assembly = Assembly.LoadFile(str2);
									}
									catch
									{
									}
								}
								AssemblyResolverHelper.hashtable[empty] = assembly;
								item = assembly;
							}
						}
						else
						{
							item = (Assembly)AssemblyResolverHelper.hashtable[empty];
						}
					}
					assembly1 = item;
					return assembly1;
				}
			Label1:
				assembly1 = null;
			}
			catch (Exception exception)
			{
				object[] objArray = new object[23];
				objArray[0] = assemblyInfo;
				objArray[1] = assemblyFullName;
				objArray[2] = base64String;
				objArray[3] = strArrays;
				objArray[4] = empty;
				objArray[5] = flag;
				objArray[6] = flag1;
				objArray[7] = num;
				objArray[8] = num1;
				objArray[9] = num2;
				objArray[10] = str;
				objArray[11] = manifestResourceStream;
				objArray[12] = length;
				objArray[13] = numArray;
				objArray[14] = assembly;
				objArray[15] = str1;
				objArray[16] = str2;
				objArray[17] = fileStream;
				objArray[18] = item;
				objArray[19] = chrArray;
				objArray[20] = hashtables;
				objArray[21] = sender;
				objArray[22] = e;
				StackFrameHelper.CreateExceptionN(exception, objArray);
				throw;
			}
			return assembly1;
		}

		internal struct AssemblyInfo
		{
			public string Name;

			public Version Version;

			public string Culture;

			public string PublicKeyToken;

			public AssemblyInfo(string assemblyFullName)
			{
				string str;
				string str1;
				char[] chrArray;
				string[] strArrays;
				int i;
				try
				{
					this.Version = null;
					this.Culture = string.Empty;
					this.PublicKeyToken = string.Empty;
					this.Name = string.Empty;
					chrArray = new char[1];
					chrArray[0] = ',';
					strArrays = assemblyFullName.Split(chrArray);
					for (i = 0; i < (int)strArrays.Length; i++)
					{
						str = strArrays[i];
						str1 = str.Trim();
						if (!str1.StartsWith("Version="))
						{
							if (!str1.StartsWith("Culture="))
							{
								if (!str1.StartsWith("PublicKeyToken="))
								{
									this.Name = str1;
								}
								else
								{
									this.PublicKeyToken = str1.Substring(15);
									if (this.PublicKeyToken == "null")
									{
										this.PublicKeyToken = string.Empty;
									}
								}
							}
							else
							{
								this.Culture = str1.Substring(8);
								if (this.Culture == "neutral")
								{
									this.Culture = string.Empty;
								}
							}
						}
						else
						{
							this.Version = new Version(str1.Substring(8));
						}
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException7(exception, str, str1, chrArray, strArrays, i, this, assemblyFullName);
					throw;
				}
			}

			public string GetAssemblyFullName(bool includeVersion)
			{
				StringBuilder stringBuilder;
				string str;
				string culture;
				string publicKeyToken;
				try
				{
					stringBuilder = new StringBuilder();
					stringBuilder.Append(this.Name);
					if (includeVersion && this.Version != null)
					{
						stringBuilder.Append(", Version=");
						stringBuilder.Append(this.Version);
					}
					stringBuilder.Append(", Culture=");
					StringBuilder stringBuilder1 = stringBuilder;
					if (this.Culture.Length == 0)
					{
						culture = "neutral";
					}
					else
					{
						culture = this.Culture;
					}
					stringBuilder1.Append(culture);
					stringBuilder.Append(", PublicKeyToken=");
					StringBuilder stringBuilder2 = stringBuilder;
					if (this.PublicKeyToken.Length == 0)
					{
						publicKeyToken = "null";
					}
					else
					{
						publicKeyToken = this.PublicKeyToken;
					}
					stringBuilder2.Append(publicKeyToken);
					str = stringBuilder.ToString();
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException3(exception, stringBuilder, this, includeVersion);
					throw;
				}
				return str;
			}
		}
	}
}