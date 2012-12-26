using System;
using System.IO;
using System.Security.Cryptography;

namespace SmartAssembly.SmartExceptionsCore
{
	internal class Encryption
	{
		public static string ExceptionMessage;

		public Encryption()
		{
		}

		public static byte[] Encrypt(byte[] data, string xmlPublicKey)
		{
			byte[] array;
			if (!xmlPublicKey.StartsWith("{"))
			{
				RijndaelManaged rijndaelManaged = null;
				RSACryptoServiceProvider rSACryptoServiceProvider = null;
				MemoryStream memoryStream = null;
				CryptoStream cryptoStream = null;
				try
				{
					try
					{
						rijndaelManaged = new RijndaelManaged();
						rSACryptoServiceProvider = new RSACryptoServiceProvider();
						rSACryptoServiceProvider.FromXmlString(xmlPublicKey);
						rijndaelManaged.GenerateKey();
						rijndaelManaged.GenerateIV();
						byte[] numArray = new byte[48];
						Buffer.BlockCopy(rijndaelManaged.Key, 0, numArray, 0, 32);
						Buffer.BlockCopy(rijndaelManaged.IV, 0, numArray, 32, 16);
						memoryStream = new MemoryStream();
						try
						{
							byte[] numArray1 = rSACryptoServiceProvider.Encrypt(numArray, false);
							memoryStream.WriteByte(1);
							memoryStream.WriteByte(Convert.ToByte((int)numArray1.Length / 8));
							memoryStream.Write(numArray1, 0, (int)numArray1.Length);
						}
						catch (CryptographicException cryptographicException1)
						{
							try
							{
								byte[] numArray2 = new byte[16];
								byte[] numArray3 = new byte[16];
								Buffer.BlockCopy(rijndaelManaged.Key, 0, numArray2, 0, 16);
								Buffer.BlockCopy(rijndaelManaged.Key, 16, numArray3, 0, 16);
								byte[] numArray4 = rSACryptoServiceProvider.Encrypt(numArray2, false);
								byte[] numArray5 = rSACryptoServiceProvider.Encrypt(numArray3, false);
								byte[] numArray6 = rSACryptoServiceProvider.Encrypt(rijndaelManaged.IV, false);
								memoryStream.WriteByte(2);
								memoryStream.WriteByte(Convert.ToByte((int)numArray4.Length / 8));
								memoryStream.Write(numArray4, 0, (int)numArray4.Length);
								memoryStream.Write(numArray5, 0, (int)numArray5.Length);
								memoryStream.Write(numArray6, 0, (int)numArray6.Length);
							}
							catch (CryptographicException cryptographicException)
							{
								Encryption.ExceptionMessage = "ERR 2005: The 128-bit encryption is not available on this computer. You need to install the High Encryption Pack in order to use the reporting feature.";
								array = null;
								return array;
							}
						}
						cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write);
						cryptoStream.Write(data, 0, (int)data.Length);
						cryptoStream.FlushFinalBlock();
						array = memoryStream.ToArray();
					}
					catch (Exception exception1)
					{
						Exception exception = exception1;
						Encryption.ExceptionMessage = string.Concat("ERR 2004: ", exception.Message);
						array = null;
					}
				}
				finally
				{
					if (rijndaelManaged != null)
					{
						rijndaelManaged.Clear();
					}
					if (rSACryptoServiceProvider != null)
					{
						rSACryptoServiceProvider.Clear();
					}
					if (memoryStream != null)
					{
						memoryStream.Close();
					}
					if (cryptoStream != null)
					{
						cryptoStream.Close();
					}
				}
				return array;
			}
			else
			{
				Encryption.ExceptionMessage = "ERR 2006: This template was not properly processed by SmartAssembly";
				return null;
			}
		}
	}
}