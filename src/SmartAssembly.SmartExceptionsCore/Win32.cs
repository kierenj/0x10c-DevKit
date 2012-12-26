using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SmartAssembly.SmartExceptionsCore
{
	public class Win32
	{
		private const int DT_WORDBREAK = 16;

		private const int DT_CALCRECT = 1024;

		private const int DT_NOPREFIX = 2048;

		private const int VER_NT_WORKSTATION = 1;

		private const int SM_SERVERR2 = 89;

		private const int PROCESSOR_ARCHITECTURE_AMD64 = 9;

		private static bool s_ReadVersionInfo;

		private static Win32.OSVERSIONINFO s_VersionInfo;

		internal static bool IsServerR2
		{
			get
			{
				bool systemMetrics;
				try
				{
					systemMetrics = Win32.GetSystemMetrics(89) != 0;
				}
				catch
				{
					systemMetrics = false;
				}
				return systemMetrics;
			}
		}

		internal static bool IsWorkstation
		{
			get
			{
				return Win32.VersionInfo.wProductType == 1;
			}
		}

		internal static bool IsX64
		{
			get
			{
				bool flag;
				try
				{
					Win32.SYSTEM_INFO sYSTEMINFO = new Win32.SYSTEM_INFO();
					Win32.GetSystemInfo(ref sYSTEMINFO);
					flag = sYSTEMINFO.wProcessorArchitecture == 9;
				}
				catch
				{
					flag = false;
				}
				return flag;
			}
		}

		internal static string ServicePack
		{
			get
			{
				return Win32.VersionInfo.szCSDVersion;
			}
		}

		private static Win32.OSVERSIONINFO VersionInfo
		{
			get
			{
				if (!Win32.s_ReadVersionInfo)
				{
					Win32.s_VersionInfo = new Win32.OSVERSIONINFO();
					try
					{
						Win32.s_VersionInfo.dwOSVersionInfoSize = Marshal.SizeOf(typeof(Win32.OSVERSIONINFO));
						Win32.GetVersionEx(ref Win32.s_VersionInfo);
						Win32.s_ReadVersionInfo = true;
					}
					catch
					{
					}
				}
				return Win32.s_VersionInfo;
			}
		}

		public Win32()
		{
		}

		[DllImport("user32", CharSet=CharSet.Unicode)]
		private static extern int DrawText(IntPtr hDC, string text, int textLength, ref Win32.RECT lpRect, int format);

		[DllImport("shell32", CharSet=CharSet.None)]
		private static extern int ExtractIconEx(string lpszFile, int nIconIndex, ref int phiconLarge, ref int phiconSmall, int nIcons);

		public static Icon GetApplicationIcon()
		{
			Icon applicationIconInternal;
			try
			{
				applicationIconInternal = Win32.GetApplicationIconInternal();
			}
			catch (Exception exception)
			{
				applicationIconInternal = Resources.GetIcon("default");
			}
			return applicationIconInternal;
		}

		private static Icon GetApplicationIconInternal()
		{
			int num = 0;
			int num1 = 0;
			int num2 = Win32.ExtractIconEx(Application.ExecutablePath, -1, ref num1, ref num1, 1);
			if (num2 > 0)
			{
				Win32.ExtractIconEx(Application.ExecutablePath, 0, ref num, ref num1, 1);
				if (num != 0)
				{
					return Icon.FromHandle(new IntPtr(num));
				}
			}
			return null;
		}

		[DllImport("kernel32.dll", CharSet=CharSet.None)]
		private static extern void GetSystemInfo(ref Win32.SYSTEM_INFO lpSystemInfo);

		[DllImport("user32.dll", CharSet=CharSet.None)]
		private static extern int GetSystemMetrics(int smIndex);

		internal static int GetTextHeight(Graphics graphics, string text, Font font, int width)
		{
			int textHeightGDIInternal;
			try
			{
				textHeightGDIInternal = Win32.GetTextHeightGDIInternal(graphics, text, font, width);
			}
			catch (Exception exception1)
			{
				try
				{
					textHeightGDIInternal = Convert.ToInt32((double)Win32.GetTextHeightGDIPlusInternal(graphics, text, font, width) * 1.1);
					return textHeightGDIInternal;
				}
				catch (Exception exception)
				{
				}
				return 0;
			}
			return textHeightGDIInternal;
		}

		private static int GetTextHeightGDIInternal(Graphics graphics, string text, Font font, int width)
		{
			Win32.RECT rECT = new Win32.RECT(new Rectangle(0, 0, width, 10000));
			int num = 3088;
			IntPtr hdc = graphics.GetHdc();
			IntPtr hfont = font.ToHfont();
			IntPtr intPtr = Win32.SelectObject(hdc, hfont);
			Win32.DrawText(hdc, text, -1, ref rECT, num);
			Win32.SelectObject(hdc, intPtr);
			graphics.ReleaseHdc(hdc);
			return rECT.Bottom - rECT.Top;
		}

		private static int GetTextHeightGDIPlusInternal(Graphics graphics, string text, Font font, int width)
		{
			Size size = Size.Ceiling(graphics.MeasureString(text, font, width));
			return size.Height;
		}

		[DllImport("kernel32.Dll", CharSet=CharSet.None)]
		private static extern short GetVersionEx(ref Win32.OSVERSIONINFO o);

		[DllImport("gdi32.dll", CharSet=CharSet.None)]
		private static extern IntPtr SelectObject(IntPtr hDC, IntPtr hGdiObj);

		private struct OSVERSIONINFO
		{
			public int dwOSVersionInfoSize;

			public uint dwMajorVersion;

			public uint dwMinorVersion;

			public uint dwBuildNumber;

			public uint dwPlatformId;

			public string szCSDVersion;

			public ushort wServicePackMajor;

			public ushort wServicePackMinor;

			public ushort wSuiteMask;

			public byte wProductType;

			private byte wReserved;
		}

		private struct RECT
		{
			public int Left;

			public int Top;

			public int Right;

			public int Bottom;

			public RECT(Rectangle rectangle)
			{
				this.Left = rectangle.Left;
				this.Top = rectangle.Top;
				this.Bottom = rectangle.Bottom;
				this.Right = rectangle.Right;
			}
		}

		public struct SYSTEM_INFO
		{
			public ushort wProcessorArchitecture;

			private ushort wReserved;

			public uint dwPageSize;

			public IntPtr lpMinimumApplicationAddress;

			public IntPtr lpMaximumApplicationAddress;

			public IntPtr dwActiveProcessorMask;

			public uint dwNumberOfProcessors;

			public uint dwProcessorType;

			public uint dwAllocationGranularity;

			public ushort wProcessorLevel;

			public ushort wProcessorRevision;
		}
	}
}