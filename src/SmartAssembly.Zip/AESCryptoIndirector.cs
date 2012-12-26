using SmartAssembly.SmartExceptionsCore;
using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;

namespace SmartAssembly.Zip
{
	public sealed class AESCryptoIndirector : IDisposable
	{
		private readonly Type m_AcspType;

		private readonly object m_AESCryptoServiceProvider;

		public AESCryptoIndirector()
		{
			Assembly assembly;
			try
			{
				try
				{
					assembly = Assembly.Load("System.Core, Version=2.0.5.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e");
					this.m_AcspType = assembly.GetType("System.Security.Cryptography.AesManaged");
				}
				catch (FileNotFoundException fileNotFoundException)
				{
					assembly = Assembly.Load("mscorlib");
					this.m_AcspType = assembly.GetType("System.Security.Cryptography.RijndaelManaged");
				}
				this.m_AESCryptoServiceProvider = Activator.CreateInstance(this.m_AcspType);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, assembly, this);
				throw;
			}
		}

		public void Clear()
		{
			try
			{
				this.m_AcspType.GetMethod("Clear").Invoke(this.m_AESCryptoServiceProvider, new object[0]);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
		}

		public void Dispose()
		{
			try
			{
				this.Clear();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
		}

		public ICryptoTransform GetAESCryptoTransform(byte[] key, byte[] iv, bool decrypt)
		{
			MethodInfo method;
			object[] objArray;
			ICryptoTransform cryptoTransform;
			string str;
			try
			{
				objArray = new object[1];
				objArray[0] = key;
				this.m_AcspType.GetProperty("Key").GetSetMethod().Invoke(this.m_AESCryptoServiceProvider, objArray);
				objArray = new object[1];
				objArray[0] = iv;
				this.m_AcspType.GetProperty("IV").GetSetMethod().Invoke(this.m_AESCryptoServiceProvider, objArray);
				Type mAcspType = this.m_AcspType;
				if (decrypt)
				{
					str = "CreateDecryptor";
				}
				else
				{
					str = "CreateEncryptor";
				}
				method = mAcspType.GetMethod(str, new Type[0]);
				cryptoTransform = (ICryptoTransform)method.Invoke(this.m_AESCryptoServiceProvider, new object[0]);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException6(exception, method, objArray, this, key, iv, decrypt);
				throw;
			}
			return cryptoTransform;
		}
	}
}