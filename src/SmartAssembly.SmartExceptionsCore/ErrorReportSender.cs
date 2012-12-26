using SmartAssembly.Shared;
using SmartAssembly.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace SmartAssembly.SmartExceptionsCore
{
	internal class ErrorReportSender : ReportSender
	{
		private const string CryptoPublicKey = "{bf13b64c-b3d2-4165-b3f5-7f852d4744cf}";

		private const string ExcludeFieldCaptureNamespaces = "{07572d6f-5375-47d5-8a8c-b5f0cbe5bad0}";

		private const string RenamingScheme = "{6d3806d4-1193-4601-a7df-2249c7f0014b}";

		private const string EmailToBeNotified = "{d316c294-ed40-4778-8b7b-29800a2dcbc3}";

		private const string ProductFriendlyName = "{a9035fc5-7ed1-4e0c-8962-dfcb1d508afc}";

		private const string BuildFriendlyNumber = "{73fbfb9b-41e7-4744-bf74-74b7c6c117c1}";

		private const string SmartAssemblyExe = "SmartAssembly.exe";

		private readonly Exception m_CurrentException;

		private readonly Guid m_UserId;

		private readonly char[] m_ObfuscationChars;

		private readonly Dictionary<string, object> m_CustomProperties;

		private readonly Dictionary<string, ErrorReportSender.AttachedFile> m_AttachedFiles;

		private readonly XmlWriter m_XmlWriter;

		private readonly List<ObjectAndType> m_ExceptionObjects;

		private readonly List<string> m_TypeNames;

		private readonly Dictionary<string, int> m_TypeNamesCache;

		private readonly List<ErrorReportSender.AssemblyInformation> m_AssemblyIDs;

		private readonly Dictionary<string, int> m_AssemblyIDsCache;

		private readonly MemoryStream m_MemoryStream;

		private byte[] m_ReportData;

		public ErrorReportSender(Guid userId, Exception currentException, IWebProxy proxy)
		{
			this.m_ObfuscationChars = new char[0];
			this.m_CustomProperties = new Dictionary<string, object>();
			this.m_AttachedFiles = new Dictionary<string, ErrorReportSender.AttachedFile>();
			this.m_ExceptionObjects = new List<ObjectAndType>();
			this.m_TypeNames = new List<string>();
			this.m_TypeNamesCache = new Dictionary<string, int>();
			this.m_AssemblyIDs = new List<ErrorReportSender.AssemblyInformation>();
			this.m_AssemblyIDsCache = new Dictionary<string, int>();
			this.m_UserId = userId;
			this.m_CurrentException = currentException;
			this.m_MemoryStream = new MemoryStream();
			this.m_XmlWriter = new XmlTextWriter(this.m_MemoryStream, new UTF8Encoding(false));
			base.SetProxy(proxy);
			string upper = "UNICODE".ToUpper();
			string str = upper;
			if (upper != null)
			{
				if (str == "ASCII")
				{
					char[] chrArray = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
					this.m_ObfuscationChars = chrArray;
					return;
				}
				else
				{
					if (str == "UNICODE")
					{
						char[] chrArray1 = new char[] { '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006', '\a', '\b', '\u000E', '\u000F', '\u0010', '\u0011', '\u0012', '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018', '\u0019', '\u001A', '\u001B', '\u001C', '\u001D', '\u001E', '\u001F', '\u007F', '\u0080', '\u0081', '\u0082', '\u0083', '\u0084', '\u0086', '\u0087', '\u0088', '\u0089', '\u008A', '\u008B', '\u008C', '\u008D', '\u008E', '\u008F', '\u0090', '\u0091', '\u0092', '\u0093', '\u0094', '\u0095', '\u0096', '\u0097', '\u0098', '\u0099', '\u009A', '\u009B', '\u009C', '\u009D', '\u009E', '\u009F' };
						this.m_ObfuscationChars = chrArray1;
					}
					else
					{
						return;
					}
				}
			}
		}

		internal void AddCustomProperty(string name, object value)
		{
			this.m_CustomProperties.Add(name, value);
		}

		private void AddNameAttribute(string name)
		{
			int dFromObfuscatedName = this.GetIDFromObfuscatedName(name);
			if (dFromObfuscatedName == -1)
			{
				this.m_XmlWriter.WriteAttributeString("Name", name);
				return;
			}
			else
			{
				this.m_XmlWriter.WriteAttributeString("NameID", dFromObfuscatedName.ToString());
				return;
			}
		}

		private void AddObjectTypeAttribute(Type objectType)
		{
			int count;
			if (objectType != null)
			{
				try
				{
					ErrorReportSender.TypeInformation typeInformation = ErrorReportSender.GetTypeInformation(objectType);
					if (typeInformation.IsEmpty)
					{
						string fullName = objectType.FullName;
						if (!this.m_TypeNamesCache.ContainsKey(fullName))
						{
							StringBuilder stringBuilder = new StringBuilder();
							string name = objectType.Assembly.GetName().Name;
							if (name.Length > 0 && name != "mscorlib")
							{
								stringBuilder.Append('[');
								stringBuilder.Append(name);
								stringBuilder.Append(']');
							}
							string @namespace = objectType.Namespace;
							if (@namespace.Length > 0)
							{
								stringBuilder.Append(@namespace);
								stringBuilder.Append('.');
							}
							if (objectType.HasElementType)
							{
								objectType = objectType.GetElementType();
							}
							int num = fullName.LastIndexOf("+");
							if (num > 0)
							{
								string str = fullName.Substring(@namespace.Length + 1, num - @namespace.Length).Replace("+", "/");
								stringBuilder.Append(str);
							}
							stringBuilder.Append(objectType.Name);
							count = this.m_TypeNames.Count;
							this.m_TypeNames.Add(stringBuilder.ToString());
							this.m_TypeNamesCache.Add(fullName, count);
						}
						else
						{
							count = this.m_TypeNamesCache[fullName];
						}
						this.m_XmlWriter.WriteAttributeString("TypeName", count.ToString());
					}
					else
					{
						this.m_XmlWriter.WriteAttributeString("TypeDefID", typeInformation.ID);
						int indexForAssemblyID = this.GetIndexForAssemblyID(typeInformation);
						if (indexForAssemblyID > 0)
						{
							this.m_XmlWriter.WriteAttributeString("Assembly", indexForAssemblyID.ToString());
						}
					}
				}
				catch
				{
				}
				return;
			}
			else
			{
				return;
			}
		}

		internal void AttachFile(string name, string fileName)
		{
			if (File.Exists(fileName))
			{
				ErrorReportSender.AttachedFile attachedFile = new ErrorReportSender.AttachedFile(fileName);
				this.m_AttachedFiles.Add(name, attachedFile);
				return;
			}
			else
			{
				return;
			}
		}

		private Assembly[] GetAssemblies()
		{
			Assembly[] assemblies;
			try
			{
				assemblies = AppDomain.CurrentDomain.GetAssemblies();
			}
			catch
			{
				Assembly[] currentAssembly = new Assembly[1];
				currentAssembly[0] = ErrorReportSender.GetCurrentAssembly();
				assemblies = currentAssembly;
			}
			return assemblies;
		}

		private static Assembly GetCurrentAssembly()
		{
			Assembly executingAssembly;
			try
			{
				executingAssembly = Assembly.GetExecutingAssembly();
			}
			catch
			{
				executingAssembly = null;
			}
			return executingAssembly;
		}

		private static string GetExecutablePath()
		{
			string executablePath;
			try
			{
				executablePath = Application.ExecutablePath;
			}
			catch
			{
				executablePath = "N/A";
			}
			return executablePath;
		}

		private static string GetHexValue(object o)
		{
			string str;
			try
			{
				if (o != null)
				{
					if (o as int == 0)
					{
						if (o as long == 0)
						{
							if (o as short == 0)
							{
								if (o as uint == 0)
								{
									if (o as ulong == 0)
									{
										if (o as ushort == 0)
										{
											if (o as byte == 0)
											{
												if (o as sbyte == 0)
												{
													if (o as IntPtr == 0)
													{
														if (o as UIntPtr == 0)
														{
															return string.Empty;
														}
														else
														{
															UIntPtr uIntPtr = (UIntPtr)o;
															ulong num = uIntPtr.ToUInt64();
															str = num.ToString("x");
														}
													}
													else
													{
														IntPtr intPtr = (IntPtr)o;
														long num1 = intPtr.ToInt64();
														str = num1.ToString("x");
													}
												}
												else
												{
													sbyte num2 = (sbyte)o;
													str = num2.ToString("x");
												}
											}
											else
											{
												byte num3 = (byte)o;
												str = num3.ToString("x");
											}
										}
										else
										{
											ushort num4 = (ushort)o;
											str = num4.ToString("x");
										}
									}
									else
									{
										ulong num5 = (ulong)o;
										str = num5.ToString("x");
									}
								}
								else
								{
									uint num6 = (uint)o;
									str = num6.ToString("x");
								}
							}
							else
							{
								short num7 = (short)o;
								str = num7.ToString("x");
							}
						}
						else
						{
							long num8 = (long)o;
							str = num8.ToString("x");
						}
					}
					else
					{
						int num9 = (int)o;
						str = num9.ToString("x");
					}
				}
				else
				{
					str = string.Empty;
				}
			}
			catch
			{
				return string.Empty;
			}
			return str;
		}

		private int GetIDFromObfuscatedName(string obfuscatedName)
		{
			int num;
			try
			{
				bool mObfuscationChars = this.m_ObfuscationChars[0] == '\u0001';
				if (obfuscatedName == null || obfuscatedName.Length == 0 || mObfuscationChars && obfuscatedName.Length > 4 || !mObfuscationChars && obfuscatedName[0] != '#')
				{
					num = -1;
				}
				else
				{
					int length = 0;
					int length1 = obfuscatedName.Length - 1;
					while (length1 >= 0 && (mObfuscationChars || length1 != 0))
					{
						char chr = obfuscatedName[length1];
						bool flag = false;
						int num1 = 0;
						while (num1 < (int)this.m_ObfuscationChars.Length)
						{
							if (this.m_ObfuscationChars[num1] != chr)
							{
								num1++;
							}
							else
							{
								length = length * (int)this.m_ObfuscationChars.Length + num1;
								flag = true;
								break;
							}
						}
						if (flag)
						{
							length1--;
						}
						else
						{
							num = -1;
							return num;
						}
					}
					num = length;
				}
			}
			catch
			{
				num = -1;
			}
			return num;
		}

		private int GetIndexForAssemblyID(ErrorReportSender.TypeInformation typeInformation)
		{
			string upper = typeInformation.AssemblyInformation.AssemblyId.ToUpper();
			if (!this.m_AssemblyIDsCache.ContainsKey(upper))
			{
				int count = this.m_AssemblyIDs.Count;
				this.m_AssemblyIDs.Add(typeInformation.AssemblyInformation);
				this.m_AssemblyIDsCache.Add(upper, count);
				return count;
			}
			else
			{
				return this.m_AssemblyIDsCache[upper];
			}
		}

		private byte[] GetReportData()
		{
			string str;
			string str1;
			string str2;
			string str3;
			if (this.m_ReportData == null)
			{
				this.m_XmlWriter.WriteStartDocument();
				using (XmlElementSpanner xmlElementSpanner = new XmlElementSpanner(this.m_XmlWriter, "UnhandledExceptionReport"))
				{
					this.m_XmlWriter.WriteAttributeString("AssemblyID", "{727AFF2A-2466-4D47-9015-42C37D73C405}".ToUpper());
					DateTime now = DateTime.Now;
					this.m_XmlWriter.WriteAttributeString("DateTime", now.ToString("s"));
					this.m_XmlWriter.WriteAttributeString("Path", ErrorReportSender.GetExecutablePath());
					if (this.m_UserId != Guid.Empty)
					{
						Guid mUserId = this.m_UserId;
						this.m_XmlWriter.WriteAttributeString("UserID", mUserId.ToString("B"));
					}
					Guid guid = Guid.NewGuid();
					this.m_XmlWriter.WriteAttributeString("ReportID", guid.ToString("B"));
					if (this.m_AssemblyIDs.Count > 0)
					{
						this.m_AssemblyIDs.Clear();
					}
					this.m_AssemblyIDs.Add(new ErrorReportSender.AssemblyInformation("{727AFF2A-2466-4D47-9015-42C37D73C405}", string.Empty));
					if (this.m_AssemblyIDsCache.Count > 0)
					{
						this.m_AssemblyIDsCache.Clear();
					}
					this.m_AssemblyIDsCache.Add("{727AFF2A-2466-4D47-9015-42C37D73C405}", 0);
					using (XmlElementSpanner xmlElementSpanner1 = new XmlElementSpanner(this.m_XmlWriter, "Assemblies"))
					{
						Assembly currentAssembly = ErrorReportSender.GetCurrentAssembly();
						Assembly[] assemblies = this.GetAssemblies();
						for (int i = 0; i < (int)assemblies.Length; i++)
						{
							Assembly assembly = assemblies[i];
							if (assembly != null)
							{
								using (XmlElementSpanner xmlElementSpanner2 = new XmlElementSpanner(this.m_XmlWriter, "Assembly"))
								{
									try
									{
										this.m_XmlWriter.WriteAttributeString("Name", assembly.FullName);
										this.m_XmlWriter.WriteAttributeString("CodeBase", assembly.CodeBase);
										if (assembly == currentAssembly)
										{
											this.m_XmlWriter.WriteAttributeString("This", "1");
										}
									}
									catch
									{
									}
								}
							}
						}
					}
					using (XmlElementSpanner xmlElementSpanner3 = new XmlElementSpanner(this.m_XmlWriter, "CustomProperties"))
					{
						if (this.m_CustomProperties != null && this.m_CustomProperties.Count > 0)
						{
							foreach (string key in this.m_CustomProperties.Keys)
							{
								using (XmlElementSpanner xmlElementSpanner4 = new XmlElementSpanner(this.m_XmlWriter, "CustomProperty"))
								{
									this.m_XmlWriter.WriteAttributeString("Name", key);
									string item = (string)this.m_CustomProperties[key];
									if (item != null)
									{
										this.m_XmlWriter.WriteAttributeString("Value", string.Concat("\"", item, "\""));
									}
									else
									{
										this.m_XmlWriter.WriteAttributeString("Null", "1");
									}
								}
							}
						}
					}
					if (this.m_AttachedFiles != null && this.m_AttachedFiles.Count > 0)
					{
						using (XmlElementSpanner xmlElementSpanner5 = new XmlElementSpanner(this.m_XmlWriter, "AttachedFiles"))
						{
							foreach (string key1 in this.m_AttachedFiles.Keys)
							{
								using (XmlElementSpanner xmlElementSpanner6 = new XmlElementSpanner(this.m_XmlWriter, "AttachedFile"))
								{
									this.m_XmlWriter.WriteAttributeString("Key", key1);
									ErrorReportSender.AttachedFile attachedFile = this.m_AttachedFiles[key1];
									this.m_XmlWriter.WriteAttributeString("FileName", attachedFile.FileName);
									int length = attachedFile.Length;
									this.m_XmlWriter.WriteAttributeString("Length", length.ToString());
									if (attachedFile.Error.Length <= 0)
									{
										this.m_XmlWriter.WriteAttributeString("Data", attachedFile.Data);
									}
									else
									{
										this.m_XmlWriter.WriteAttributeString("Error", attachedFile.Error);
									}
								}
							}
						}
					}
					using (XmlElementSpanner xmlElementSpanner7 = new XmlElementSpanner(this.m_XmlWriter, "SystemInformation"))
					{
						try
						{
							this.m_XmlWriter.WriteElementString("NETVersion", Environment.Version.ToString());
							this.m_XmlWriter.WriteElementString("OSVersion", Environment.OSVersion.Version.ToString());
							this.m_XmlWriter.WriteElementString("OSPlatformID", Environment.OSVersion.Platform.ToString());
							this.m_XmlWriter.WriteElementString("ServicePack", Win32.ServicePack);
							XmlWriter mXmlWriter = this.m_XmlWriter;
							string str4 = "ServerR2";
							if (Win32.IsServerR2)
							{
								str1 = "1";
							}
							else
							{
								str1 = "0";
							}
							mXmlWriter.WriteElementString(str4, str1);
							XmlWriter xmlWriter = this.m_XmlWriter;
							string str5 = "X64";
							if (Win32.IsX64)
							{
								str2 = "1";
							}
							else
							{
								str2 = "0";
							}
							xmlWriter.WriteElementString(str5, str2);
							XmlWriter mXmlWriter1 = this.m_XmlWriter;
							string str6 = "Workstation";
							if (Win32.IsWorkstation)
							{
								str3 = "1";
							}
							else
							{
								str3 = "0";
							}
							mXmlWriter1.WriteElementString(str6, str3);
						}
						catch
						{
						}
					}
					List<Exception> exceptions = new List<Exception>();
					Exception mCurrentException = this.m_CurrentException;
					while (mCurrentException != null)
					{
						exceptions.Add(mCurrentException);
						mCurrentException = mCurrentException.InnerException;
					}
					exceptions.Reverse();
					using (XmlElementSpanner xmlElementSpanner8 = new XmlElementSpanner(this.m_XmlWriter, "StackTrace"))
					{
						foreach (Exception exception in exceptions)
						{
							try
							{
								this.WriteException(exception);
								if (exception.Data.Contains("SmartStackFrames"))
								{
									ICollection collections = (ICollection)exception.Data["SmartStackFrames"];
									int count = collections.Count;
									int num = 0;
									foreach (object obj2 in collections)
									{
										try
										{
											Type type = obj2.GetType();
											num++;
											if (num <= 100 || num != count - 100)
											{
												if (num <= 100 || num > count - 100)
												{
													int value = (int)type.GetField("MethodID").GetValue(obj2);
													int value1 = (int)type.GetField("ILOffset").GetValue(obj2);
													int num1 = (int)type.GetField("ExceptionStackDepth").GetValue(obj2);
													object[] objArray = (object[])type.GetField("Objects").GetValue(obj2);
													ErrorReportSender.TypeInformation typeInformation = ErrorReportSender.GetTypeInformation(type);
													if (!typeInformation.IsEmpty)
													{
														using (XmlElementSpanner xmlElementSpanner9 = new XmlElementSpanner(this.m_XmlWriter, "StackFrame"))
														{
															this.m_XmlWriter.WriteAttributeString("MethodID", value.ToString());
															this.m_XmlWriter.WriteAttributeString("ExceptionStackDepth", num1.ToString());
															int indexForAssemblyID = this.GetIndexForAssemblyID(typeInformation);
															if (indexForAssemblyID > 0)
															{
																this.m_XmlWriter.WriteAttributeString("Assembly", indexForAssemblyID.ToString());
															}
															if (value1 != -1)
															{
																this.m_XmlWriter.WriteAttributeString("ILOffset", value1.ToString());
															}
															object[] objArray1 = objArray;
															for (int j = 0; j < (int)objArray1.Length; j++)
															{
																object obj3 = objArray1[j];
																try
																{
																	this.SaveObjectInformation(new ObjectAndType(obj3, true), null);
																}
																catch
																{
																}
															}
														}
													}
												}
											}
											else
											{
												using (XmlElementSpanner xmlElementSpanner10 = new XmlElementSpanner(this.m_XmlWriter, "RemovedFrames"))
												{
													this.m_XmlWriter.WriteAttributeString("TotalFramesCount", count.ToString());
												}
											}
										}
										catch
										{
										}
									}
								}
							}
							catch
							{
							}
						}
					}
					this.WriteObjects();
					using (XmlElementSpanner xmlElementSpanner11 = new XmlElementSpanner(this.m_XmlWriter, "TypeNames"))
					{
						int count1 = this.m_TypeNames.Count;
						this.m_XmlWriter.WriteAttributeString("Count", count1.ToString());
						for (int k = 0; k < this.m_TypeNames.Count; k++)
						{
							try
							{
								str = this.m_TypeNames[k].ToString();
							}
							catch (Exception exception2)
							{
								Exception exception1 = exception2;
								str = string.Concat((char)34, exception1.Message, (char)34);
							}
							this.m_XmlWriter.WriteElementString("TypeName", str);
						}
					}
					using (XmlElementSpanner xmlElementSpanner12 = new XmlElementSpanner(this.m_XmlWriter, "AssemblyIDs"))
					{
						int count2 = this.m_AssemblyIDs.Count;
						this.m_XmlWriter.WriteAttributeString("Count", count2.ToString());
						for (int l = 0; l < this.m_AssemblyIDs.Count; l++)
						{
							using (XmlElementSpanner xmlElementSpanner13 = new XmlElementSpanner(this.m_XmlWriter, "AssemblyID"))
							{
								ErrorReportSender.AssemblyInformation assemblyInformation = this.m_AssemblyIDs[l];
								this.m_XmlWriter.WriteAttributeString("ID", assemblyInformation.AssemblyId);
								if (assemblyInformation.AssemblyFullName.Length > 0)
								{
									this.m_XmlWriter.WriteAttributeString("FullName", assemblyInformation.AssemblyFullName);
								}
							}
						}
					}
				}
				this.m_XmlWriter.WriteEndDocument();
				this.m_XmlWriter.Flush();
				this.m_MemoryStream.Flush();
				this.m_ReportData = this.m_MemoryStream.ToArray();
				return this.m_ReportData;
			}
			else
			{
				return this.m_ReportData;
			}
		}

		internal byte[] GetReportRawData()
		{
			return this.GetReportData();
		}

		private static ErrorReportSender.TypeInformation GetTypeInformation(Type objectType)
		{
			ErrorReportSender.TypeInformation empty = ErrorReportSender.TypeInformation.Empty;
			if (objectType != null && objectType.Assembly.GetType("SmartAssembly.Attributes.PoweredByAttribute") != null)
			{
				int metadataToken = (objectType.MetadataToken & 16777215) - 1;
				empty.ID = metadataToken.ToString();
				Assembly assembly = objectType.Assembly;
				Guid moduleVersionId = assembly.ManifestModule.ModuleVersionId;
				empty.AssemblyInformation = new ErrorReportSender.AssemblyInformation(moduleVersionId.ToString("B"), assembly.FullName);
			}
			return empty;
		}

		private static string GetValidatedInformation(string value)
		{
			if (!value.StartsWith("\"<RSAKeyValue>") || !value.EndsWith("</RSAKeyValue>\""))
			{
				return value;
			}
			else
			{
				return "*** Information not reported for security reasons ***";
			}
		}

		private void InvokeDebuggerLaunched()
		{
			EventHandler eventHandler = this.DebuggerLaunched;
			if (eventHandler != null)
			{
				eventHandler(this, EventArgs.Empty);
			}
		}

		public void InvokeFatalException(FatalExceptionEventArgs e)
		{
			FatalExceptionEventHandler fatalExceptionEventHandler = this.FatalException;
			if (fatalExceptionEventHandler != null)
			{
				fatalExceptionEventHandler(this, e);
			}
		}

		internal void LaunchDebugger()
		{
			try
			{
				string tempFileName = Path.GetTempFileName();
				this.SaveEncryptedReport(tempFileName);
				string str = AppPathFinder.ReadInstalledSaPath();
				Process.Start(Path.Combine(str, "SmartAssembly.exe"), string.Concat("/AddExceptionReport \"", tempFileName, "\""));
				if (this.DebuggerLaunched != null)
				{
					this.DebuggerLaunched(this, EventArgs.Empty);
				}
			}
			catch (ThreadAbortException threadAbortException)
			{
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				this.InvokeFatalException(new FatalExceptionEventArgs(exception));
			}
		}

		internal bool SaveEncryptedReport(string fileName)
		{
			byte[] numArray;
			bool flag;
			try
			{
				byte[] reportData = this.GetReportData();
				try
				{
					numArray = SimpleZip.Zip(reportData);
				}
				catch
				{
					numArray = null;
				}
				byte[] numArray1 = Encryption.Encrypt(numArray, "<RSAKeyValue><Modulus>s3i8v1TIvLPXY9D2QXApSYXgdpiFbD5n3PGcGKNDDrbc1rSAkgu0So/uBn6kUoGcSP9zlHOlyWKpCHz+pMuRQd7kg2lgu7h3pN0levcjuMfqqCYW710dnaniMevPoC9MgoYz9M0QmWg9Sug1VvuCwLrki9nF+/3WY5R0JE9nOOU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>");
				FileStream fileStream = File.OpenWrite(fileName);
				byte[] bytes = Encoding.ASCII.GetBytes("{727AFF2A-2466-4D47-9015-42C37D73C405}");
				fileStream.Write(bytes, 0, (int)bytes.Length);
				fileStream.Write(numArray1, 0, (int)numArray1.Length);
				fileStream.Close();
				flag = true;
			}
			catch (ThreadAbortException threadAbortException)
			{
				flag = false;
			}
			catch (Exception exception)
			{
				flag = false;
			}
			return flag;
		}

		private void SaveObjectAndWriteIDAttribute(ObjectAndType objectAndType)
		{
			object obj = objectAndType.GetObject();
			int count = -1;
			int num = 0;
			while (num < this.m_ExceptionObjects.Count)
			{
				if (!object.ReferenceEquals(this.m_ExceptionObjects[num].GetObject(), obj))
				{
					num++;
				}
				else
				{
					count = num;
					break;
				}
			}
			if (count == -1)
			{
				count = this.m_ExceptionObjects.Count;
				this.m_ExceptionObjects.Add(objectAndType);
			}
			this.m_XmlWriter.WriteAttributeString("ID", count.ToString());
		}

		private void SaveObjectInformation(ObjectAndType objectAndType, FieldInfo fieldType)
		{
			string name;
			string str;
			if (fieldType == null)
			{
				name = null;
			}
			else
			{
				name = fieldType.Name;
			}
			string str1 = name;
			if (fieldType == null)
			{
				str = "Object";
			}
			else
			{
				str = "Field";
			}
			string str2 = str;
			object obj = objectAndType.GetObject();
			if (obj != null)
			{
				Type type = objectAndType.GetObject().GetType();
				string fullName = null;
				string lower = null;
				if (obj is string)
				{
					fullName = "System.String";
				}
				if (fullName == null)
				{
					if (type.IsPrimitive || obj as IntPtr != 0 || obj as UIntPtr != 0)
					{
						fullName = type.FullName;
						if (obj as char != 0)
						{
							int num = (char)obj;
							StringBuilder stringBuilder = new StringBuilder();
							if (num >= 32)
							{
								stringBuilder.Append('\'');
								stringBuilder.Append((char)obj);
								stringBuilder.Append("' ");
							}
							stringBuilder.Append("(0x");
							stringBuilder.Append(num.ToString("x"));
							stringBuilder.Append(')');
							lower = stringBuilder.ToString();
						}
						if (obj as bool)
						{
							lower = obj.ToString().ToLower();
						}
						if (lower == null)
						{
							string hexValue = ErrorReportSender.GetHexValue(obj);
							if (hexValue.Length <= 0)
							{
								lower = obj.ToString();
							}
							else
							{
								StringBuilder stringBuilder1 = new StringBuilder();
								stringBuilder1.Append(obj.ToString());
								stringBuilder1.Append(" (0x");
								stringBuilder1.Append(hexValue);
								stringBuilder1.Append(')');
								lower = stringBuilder1.ToString();
							}
						}
					}
					else
					{
						if (type.IsValueType && type.Module != base.GetType().Module)
						{
							fullName = type.FullName;
						}
					}
				}
				using (XmlElementSpanner xmlElementSpanner = new XmlElementSpanner(this.m_XmlWriter, str2))
				{
					if (fieldType != null && fieldType.IsStatic)
					{
						this.m_XmlWriter.WriteAttributeString("Static", "1");
					}
					if (fullName == null)
					{
						if (fieldType != null)
						{
							this.AddObjectTypeAttribute(fieldType.FieldType);
						}
						this.SaveObjectAndWriteIDAttribute(objectAndType);
						if (str1 != null)
						{
							this.AddNameAttribute(str1);
						}
					}
					else
					{
						this.AddObjectTypeAttribute(type);
						if (str1 != null)
						{
							this.AddNameAttribute(str1);
						}
						if (type.IsEnum)
						{
							lower = obj.ToString();
						}
						if (obj is Guid)
						{
							lower = string.Concat("{", obj, "}");
						}
						if (lower == null)
						{
							lower = string.Concat("\"", obj, "\"");
						}
						this.m_XmlWriter.WriteAttributeString("Value", ErrorReportSender.GetValidatedInformation(lower));
					}
				}
				return;
			}
			else
			{
				using (XmlElementSpanner xmlElementSpanner1 = new XmlElementSpanner(this.m_XmlWriter, str2))
				{
					if (fieldType != null)
					{
						if (fieldType.IsStatic)
						{
							this.m_XmlWriter.WriteAttributeString("Static", "1");
						}
						Type type1 = fieldType.FieldType;
						if (type1 == null || !type1.HasElementType)
						{
							this.AddObjectTypeAttribute(type1);
						}
						else
						{
							this.AddObjectTypeAttribute(type1.GetElementType());
							if (type1.IsByRef)
							{
								this.m_XmlWriter.WriteAttributeString("ByRef", "1");
							}
							if (type1.IsPointer)
							{
								this.m_XmlWriter.WriteAttributeString("Pointer", "1");
							}
							if (type1.IsArray)
							{
								int arrayRank = type1.GetArrayRank();
								this.m_XmlWriter.WriteAttributeString("Rank", arrayRank.ToString());
							}
						}
					}
					if (str1 != null)
					{
						this.AddNameAttribute(str1);
					}
					this.m_XmlWriter.WriteAttributeString("Null", "1");
				}
				return;
			}
		}

		[ReportUsage("Unhandled Exception Report Sent")]
		internal bool SendReport()
		{
			byte[] reportData;
			bool flag;
			try
			{
				base.InvokeSendingReportFeedback(SendingReportStep.PreparingReport);
				try
				{
					reportData = this.GetReportData();
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					int lOffset = -1;
					try
					{
						StackTrace stackTrace = new StackTrace(exception);
						if (stackTrace.FrameCount > 0)
						{
							StackFrame frame = stackTrace.GetFrame(stackTrace.FrameCount - 1);
							lOffset = frame.GetILOffset();
						}
					}
					catch
					{
					}
					base.InvokeSendingReportFeedback(SendingReportStep.PreparingReport, string.Format("ERR 2006: {0} @ 0x{1:x4}", exception.Message, lOffset));
					flag = false;
					return flag;
				}
				ReportSender.NotificationEmailSettings notificationEmailSetting = new ReportSender.NotificationEmailSettings("0x10cdevkit@gmail.com", "DevKit", "v1.7.5.0 from 04/06/2012 16:30:37");
				flag = base.SendReport(reportData, notificationEmailSetting);
			}
			catch (ThreadAbortException threadAbortException)
			{
				flag = false;
			}
			catch (Exception exception3)
			{
				Exception exception2 = exception3;
				this.InvokeFatalException(new FatalExceptionEventArgs(exception2));
				flag = false;
			}
			return flag;
		}

		private void WriteException(Exception exceptionToWrite)
		{
			using (XmlElementSpanner xmlElementSpanner = new XmlElementSpanner(this.m_XmlWriter, "Exception"))
			{
				try
				{
					Type type = exceptionToWrite.GetType();
					this.AddObjectTypeAttribute(type);
					string message = "N/A";
					try
					{
						message = exceptionToWrite.Message;
					}
					catch
					{
					}
					this.m_XmlWriter.WriteAttributeString("Message", message);
					string str = exceptionToWrite.StackTrace.Trim();
					this.m_XmlWriter.WriteAttributeString("ExceptionStackTrace", str);
					int num = str.IndexOf(' ');
					str = str.Substring(num + 1);
					num = str.IndexOf("\r\n");
					if (num != -1)
					{
						str = str.Substring(0, num);
					}
					this.m_XmlWriter.WriteAttributeString("Method", str);
					this.SaveObjectAndWriteIDAttribute(new ObjectAndType(exceptionToWrite, true));
				}
				catch
				{
				}
			}
		}

		private void WriteObjectFields(ObjectAndType objectToWrite)
		{
			FieldInfo[] fields = objectToWrite.GetType().GetFields(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			FieldInfo[] fieldInfoArray = fields;
			for (int i = 0; i < (int)fieldInfoArray.Length; i++)
			{
				FieldInfo fieldInfo = fieldInfoArray[i];
				try
				{
					if (!fieldInfo.IsLiteral)
					{
						if (!fieldInfo.IsStatic || !fieldInfo.IsInitOnly)
						{
							bool flag = true;
							object[] customAttributes = fieldInfo.GetCustomAttributes(true);
							int num = 0;
							while (num < (int)customAttributes.Length)
							{
								Attribute attribute = (Attribute)customAttributes[num];
								if (attribute.GetType().Name != "DoNotCaptureAttribute")
								{
									num++;
								}
								else
								{
									flag = false;
									break;
								}
							}
							if (flag)
							{
								this.SaveObjectInformation(new ObjectAndType(fieldInfo.GetValue(objectToWrite.GetObject()), false), fieldInfo);
							}
						}
					}
				}
				catch
				{
				}
			}
			objectToWrite = new ObjectAndType(objectToWrite.GetObject(), objectToWrite.GetType().BaseType, objectToWrite.FirstLevel);
			if (objectToWrite.GetType() != null)
			{
				using (XmlElementSpanner xmlElementSpanner = new XmlElementSpanner(this.m_XmlWriter, "Field"))
				{
					this.AddNameAttribute("__base");
					int count = this.m_ExceptionObjects.Count;
					this.m_XmlWriter.WriteAttributeString("ID", count.ToString());
				}
				this.m_ExceptionObjects.Add(objectToWrite);
				return;
			}
			else
			{
				return;
			}
		}

		private void WriteObjects()
		{
			using (XmlElementSpanner xmlElementSpanner = new XmlElementSpanner(this.m_XmlWriter, "Objects"))
			{
				for (int i = 0; i < this.m_ExceptionObjects.Count; i++)
				{
					ObjectAndType item = this.m_ExceptionObjects[i];
					object obj = item.GetObject();
					Type type = item.GetType();
					using (XmlElementSpanner xmlElementSpanner1 = new XmlElementSpanner(this.m_XmlWriter, "ObjectDef"))
					{
						this.m_XmlWriter.WriteAttributeString("ID", i.ToString());
						string str = null;
						bool flag = true;
						char[] chrArray = new char[1];
						chrArray[0] = ',';
						string[] strArrays = "".Split(chrArray);
						int num = 0;
						while (num < (int)strArrays.Length)
						{
							string str1 = strArrays[num];
							if (!(str1 != "") || !type.FullName.StartsWith(str1))
							{
								num++;
							}
							else
							{
								flag = false;
								break;
							}
						}
						object[] customAttributes = type.GetCustomAttributes(true);
						int num1 = 0;
						while (num1 < (int)customAttributes.Length)
						{
							Attribute attribute = (Attribute)customAttributes[num1];
							string name = attribute.GetType().Name;
							if (!(name != "DoNotCaptureFieldsAttribute") || !(name != "DoNotCaptureAttribute"))
							{
								flag = false;
								break;
							}
							else
							{
								num1++;
							}
						}
						if (flag)
						{
							try
							{
								str = obj.ToString();
								if (str != type.FullName)
								{
									if (!type.IsEnum)
									{
										if (obj as Guid == null)
										{
											str = string.Concat("\"", str, "\"");
										}
										else
										{
											str = string.Concat("{", str, "}");
										}
									}
									else
									{
										str = Enum.Format(type, obj, "d");
									}
								}
								else
								{
									str = null;
								}
							}
							catch
							{
							}
							if (str != null)
							{
								this.m_XmlWriter.WriteAttributeString("Value", ErrorReportSender.GetValidatedInformation(str));
							}
						}
						if (!type.HasElementType)
						{
							this.AddObjectTypeAttribute(type);
							if (item.FirstLevel && flag)
							{
								try
								{
									if (obj is IEnumerable)
									{
										using (XmlElementSpanner xmlElementSpanner2 = new XmlElementSpanner(this.m_XmlWriter, "IEnumerable"))
										{
											int num2 = 0;
											foreach (object obj2 in (IEnumerable)obj)
											{
												if (num2 <= 20)
												{
													this.SaveObjectInformation(new ObjectAndType(obj2, false), null);
													num2++;
												}
												else
												{
													this.m_XmlWriter.WriteElementString("More", string.Empty);
													break;
												}
											}
										}
									}
								}
								catch
								{
								}
								this.WriteObjectFields(item);
							}
						}
						else
						{
							this.AddObjectTypeAttribute(type.GetElementType());
							if (type.IsByRef)
							{
								this.m_XmlWriter.WriteAttributeString("ByRef", "1");
							}
							if (type.IsPointer)
							{
								this.m_XmlWriter.WriteAttributeString("Pointer", "1");
							}
							if (type.IsArray)
							{
								Array arrays = (Array)obj;
								int rank = arrays.Rank;
								this.m_XmlWriter.WriteAttributeString("Rank", rank.ToString());
								StringBuilder stringBuilder = new StringBuilder();
								for (int j = 0; j < arrays.Rank; j++)
								{
									if (j > 0)
									{
										stringBuilder.Append(',');
									}
									stringBuilder.Append(arrays.GetLength(j));
								}
								this.m_XmlWriter.WriteAttributeString("Length", stringBuilder.ToString());
								if (arrays.Rank == 1)
								{
									int length = arrays.Length;
									for (int k = 0; k < length; k++)
									{
										if (k == 10 && length > 16)
										{
											k = length - 5;
										}
										try
										{
											this.SaveObjectInformation(new ObjectAndType(arrays.GetValue(k), false), null);
										}
										catch
										{
										}
									}
								}
							}
						}
					}
				}
			}
		}

		public event EventHandler DebuggerLaunched;

		public event FatalExceptionEventHandler FatalException;

		private struct AssemblyInformation
		{
			public readonly string AssemblyId;

			public readonly string AssemblyFullName;

			public AssemblyInformation(string assemblyID, string assemblyFullName)
			{
				this.AssemblyId = assemblyID;
				this.AssemblyFullName = assemblyFullName;
			}
		}

		private struct AttachedFile
		{
			public readonly string FileName;

			public readonly string Data;

			public readonly string Error;

			public readonly int Length;

			public AttachedFile(string fileName)
			{
				byte[] numArray;
				this.FileName = string.Empty;
				this.Data = string.Empty;
				this.Error = string.Empty;
				this.Length = 0;
				try
				{
					FileInfo fileInfo = new FileInfo(fileName);
					this.FileName = Path.GetFileName(fileName);
					this.Length = (int)fileInfo.Length;
					byte[] numArray1 = new byte[this.Length];
					FileStream fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					using (fileStream)
					{
						fileStream.Read(numArray1, 0, this.Length);
						fileStream.Close();
					}
					try
					{
						numArray = SimpleZip.Zip(numArray1);
					}
					catch
					{
						numArray = null;
					}
					this.Data = Convert.ToBase64String(numArray);
				}
				catch (Exception exception1)
				{
					Exception exception = exception1;
					this.Error = exception.Message;
				}
			}
		}

		private struct TypeInformation
		{
			public string ID;

			public ErrorReportSender.AssemblyInformation AssemblyInformation;

			public static ErrorReportSender.TypeInformation Empty
			{
				get
				{
					return new ErrorReportSender.TypeInformation(string.Empty, string.Empty, string.Empty);
				}
			}

			public bool IsEmpty
			{
				get
				{
					return this.ID.Length == 0;
				}
			}

			private TypeInformation(string id, string assemblyID, string assemblyFullName)
			{
				this.ID = id;
				this.AssemblyInformation = new ErrorReportSender.AssemblyInformation(assemblyID, assemblyFullName);
			}
		}
	}
}