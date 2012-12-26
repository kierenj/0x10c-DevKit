using Devkit.Workspace.ViewModel;
using SmartAssembly.SmartExceptionsCore;
using System;

namespace Devkit.IDE.ViewModel
{
	public class ItemPropertyPage : DocPaneItem
	{
		private readonly object _item;

		private bool _isOpen;

		public override string DisplayTitle
		{
			get
			{
				string str;
				try
				{
					str = this._item.ToString();
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return str;
			}
		}

		public override bool IsOpen
		{
			get
			{
				bool flag;
				try
				{
					flag = this._isOpen;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return flag;
			}
			set
			{
				try
				{
					this._isOpen = value;
					if (!this._isOpen)
					{
						this.OnClosed();
					}
					this.OnPropertyChanged("IsOpen");
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public object Item
		{
			get
			{
				object obj;
				try
				{
					obj = this._item;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return obj;
			}
		}

		public ItemPropertyPage(object item)
		{
			try
			{
				this._item = item;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, item);
				throw;
			}
		}

		private void OnClosed()
		{
		}
	}
}