using SmartAssembly.SmartExceptionsCore;
using System;
using System.Reflection;
using System.Security.Cryptography;

namespace SmartAssembly.Zip
{
	public sealed class DESCryptoIndirector : IDisposable
	{
		private readonly Type m_DcspType;

		private readonly object m_DESCryptoServiceProvider;

		public DESCryptoIndirector()
		{
			Assembly assembly;
			try
			{
				assembly = Assembly.Load("mscorlib");
				this.m_DcspType = assembly.GetType("System.Security.Cryptography.DESCryptoServiceProvider");
				this.m_DESCryptoServiceProvider = Activator.CreateInstance(this.m_DcspType);
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
				this.m_DcspType.GetMethod("Clear").Invoke(this.m_DESCryptoServiceProvider, new object[0]);
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

		public ICryptoTransform GetDESCryptoTransform(byte[] key, byte[] iv, bool decrypt)
		{
			MethodInfo method;
			object[] objArray;
			ICryptoTransform cryptoTransform;
			string str;
			try
			{
				objArray = new object[1];
				objArray[0] = key;
				this.m_DcspType.GetProperty("Key").GetSetMethod().Invoke(this.m_DESCryptoServiceProvider, objArray);
				objArray = new object[1];
				objArray[0] = iv;
				this.m_DcspType.GetProperty("IV").GetSetMethod().Invoke(this.m_DESCryptoServiceProvider, objArray);
				Type mDcspType = this.m_DcspType;
				if (decrypt)
				{
					str = "CreateDecryptor";
				}
				else
				{
					str = "CreateEncryptor";
				}
				method = mDcspType.GetMethod(str, new Type[0]);
				cryptoTransform = (ICryptoTransform)method.Invoke(this.m_DESCryptoServiceProvider, new object[0]);
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