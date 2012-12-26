using Devkit.Workspace.Services;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Devkit.IDE.View
{
	public class NewItem : InfoRequestViewBase<NewFileInfo>, IComponentConnector
	{
		internal TextBox textBoxName;

		internal TextBox textBoxFilename;

		private bool _contentLoaded;

		public NewItem()
		{
			try
			{
				this.InitializeComponent();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		internal Delegate _CreateDelegate(Type delegateType, string handler)
		{
			Delegate @delegate;
			try
			{
				@delegate = Delegate.CreateDelegate(delegateType, this, handler);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, delegateType, handler);
				throw;
			}
			return @delegate;
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			string str;
			try
			{
				str = Path.Combine(base.InfoRequest.get_Folder(), base.InfoRequest.get_Filename());
				if (!File.Exists(str))
				{
					base.DialogResult = new bool?(true);
					base.Close();
				}
				else
				{
					MessageBox.Show("File already exists - choose another name!", "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, str, this, sender, e);
				throw;
			}
		}

		[DebuggerNonUserCode]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		public void InitializeComponent()
		{
			Uri uri;
			try
			{
				if (!this._contentLoaded)
				{
					this._contentLoaded = true;
					uri = new Uri("/Devkit.IDE;component/view/newitem.xaml", UriKind.Relative);
					Application.LoadComponent(this, uri);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, uri, this);
				throw;
			}
		}

		[DebuggerNonUserCode]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
		{
			int num;
			try
			{
				num = connectionId;
				switch (num)
				{
					case 1:
					{
						this.textBoxName = (TextBox)target;
						return;
					}
					case 2:
					{
						this.textBoxFilename = (TextBox)target;
						return;
					}
					case 3:
					{
						(Button)target.Click += new RoutedEventHandler(this.Button_Click);
						return;
					}
				}
				this._contentLoaded = true;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, num, this, connectionId, target);
				throw;
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				base.Dispatcher.BeginInvoke(() => {
					try
					{
						this.textBoxName.Focus();
						this.textBoxName.SelectAll();
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
				}
				, DispatcherPriority.Background, new object[0]);
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException3(exception1, this, sender, e);
				throw;
			}
		}
	}
}