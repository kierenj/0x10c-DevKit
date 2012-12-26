using SmartAssembly.SmartExceptionsCore;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Devkit.IDE.Converters
{
	public class DiscreteOptionConverter : IValueConverter
	{
		private const double EPSILON = 0.01;

		public DiscreteOptionConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			double num;
			object obj;
			try
			{
				if (value as double != 0)
				{
					num = (double)value;
					obj = Math.Abs(num - double.Parse(parameter.ToString())) < 0.01;
				}
				else
				{
					obj = (bool)0;
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException6(exception, num, this, value, targetType, parameter, culture);
				throw;
			}
			return obj;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object obj;
			try
			{
				if (value as bool)
				{
					if ((bool)value)
					{
						obj = double.Parse(parameter.ToString());
					}
					else
					{
						obj = null;
					}
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
	}
}