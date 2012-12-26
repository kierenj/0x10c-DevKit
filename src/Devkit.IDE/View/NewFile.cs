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
	public class NewFile : InfoRequestViewBase<NewFileInfo>, IComponentConnector
	{
		private readonly static string DefaultFilename;

		internal TextBox textBox;

		private bool _contentLoaded;

		static NewFile()
		{
			try
			{
				NewFile.DefaultFilename = "untitled";
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException0(exception);
				throw;
			}
		}

		public NewFile()
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

		private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				this.UpdateExtensionAndSelection();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
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
					uri = new Uri("/Devkit.IDE;component/view/newfile.xaml", UriKind.Relative);
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
						this.textBox = (TextBox)target;
						return;
					}
					case 2:
					{
						(ComboBox)target.SelectionChanged += new SelectionChangedEventHandler(this.ComboBox_SelectionChanged);
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

		private void UpdateExtensionAndSelection()
		{
			int num;
			string str;
			try
			{
				num = 0;
				str = "";
				do
				{
					base.InfoRequest.set_Filename(string.Format("{0}{1}{2}", Path.GetFileNameWithoutExtension(base.InfoRequest.get_Filename()), str, base.InfoRequest.get_SelectedExtension()));
					num++;
					str = string.Concat("-", num);
				}
				while (File.Exists(Path.Combine(base.InfoRequest.get_Folder(), base.InfoRequest.get_Filename())));
				base.Dispatcher.BeginInvoke(() => {
					try
					{
						this.textBox.Focus();
						this.textBox.SelectionStart = 0;
						this.textBox.SelectionLength = base.InfoRequest.get_Filename().Length - base.InfoRequest.get_SelectedExtension().Length;
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
				StackFrameHelper.CreateException3(exception1, num, str, this);
				throw;
			}
		}

		private void Window_Loaded(object sender, RoutedEventArgs e)
		{
			try
			{
				base.InfoRequest.set_Filename(NewFile.DefaultFilename);
				this.UpdateExtensionAndSelection();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}
	}
}