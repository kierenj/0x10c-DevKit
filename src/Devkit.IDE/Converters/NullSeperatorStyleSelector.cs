using SmartAssembly.SmartExceptionsCore;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Devkit.IDE.Converters
{
	public class NullSeperatorStyleSelector : StyleSelector
	{
		public Style SeparatorStyle
		{
			get
			{
				Style u003cSeparatorStyleu003ek_BackingField;
				try
				{
					u003cSeparatorStyleu003ek_BackingField = this.u003cSeparatorStyleu003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cSeparatorStyleu003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cSeparatorStyleu003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public NullSeperatorStyleSelector()
		{
		}

		public override Style SelectStyle(object item, DependencyObject container)
		{
			Style separatorStyle;
			try
			{
				if (item != null)
				{
					separatorStyle = base.SelectStyle(item, container);
				}
				else
				{
					separatorStyle = this.SeparatorStyle;
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, item, container);
				throw;
			}
			return separatorStyle;
		}
	}
}