using SmartAssembly.SmartExceptionsCore;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Devkit.IDE.Converters
{
	public class WordConverter : IValueConverter
	{
		public WordConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object obj;
			try
			{
				if (value != null)
				{
					obj = string.Format("{0:X4}", value);
				}
				else
				{
					obj = null;
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException5(exception, this, value, targetType, parameter, culture);
				throw;
			}
			return obj;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object num;
			try
			{
				num = Convert.ToInt32(value.ToString(), 16);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException5(exception, this, value, targetType, parameter, culture);
				throw;
			}
			return num;
		}
	}
}