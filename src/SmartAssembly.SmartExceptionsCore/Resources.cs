using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace SmartAssembly.SmartExceptionsCore
{
	internal class Resources
	{
		public Resources()
		{
		}

		public static Bitmap GetBitmap(string key)
		{
			Bitmap bitmap;
			Bitmap bitmap1;
			try
			{
				Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Concat("SmartAssembly.SmartExceptionsCore.Resources.", key, ".png"));
				if (manifestResourceStream == null)
				{
					bitmap1 = null;
				}
				else
				{
					bitmap1 = new Bitmap(manifestResourceStream);
				}
				bitmap = bitmap1;
			}
			catch
			{
				bitmap = null;
			}
			return bitmap;
		}

		public static Icon GetIcon(string key)
		{
			Icon icon;
			Icon icon1;
			try
			{
				Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Concat("SmartAssembly.SmartExceptionsCore.Resources.", key, ".ico"));
				if (manifestResourceStream == null)
				{
					icon1 = null;
				}
				else
				{
					icon1 = new Icon(manifestResourceStream);
				}
				icon = icon1;
			}
			catch
			{
				icon = null;
			}
			return icon;
		}
	}
}