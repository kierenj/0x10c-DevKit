using Devkit.Workspace.Services;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.Windows;

namespace Devkit.IDE.View
{
	public class InfoRequestViewBase<T> : Window
	where T : InfoRequest
	{
		public T InfoRequest
		{
			get
			{
				T dataContext;
				try
				{
					dataContext = (T)(base.DataContext as T);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return dataContext;
			}
		}

		public InfoRequestViewBase()
		{
		}
	}
}