using Devkit.Workspace.ViewModel;
using Microsoft.Win32;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace Devkit.IDE.View
{
	public class SolutionProperties : UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public SolutionProperties Properties
		{
			get
			{
				SolutionProperties dataContext;
				try
				{
					dataContext = (SolutionProperties)base.DataContext;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return dataContext;
			}
		}

		public SolutionProperties()
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

		private void BrowseClick(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog;
			bool? nullable;
			OpenFileDialog loadBinaryFilename;
			try
			{
				loadBinaryFilename = new OpenFileDialog();
				loadBinaryFilename.Title = "Browse for emulator memory file";
				loadBinaryFilename.CheckFileExists = true;
				loadBinaryFilename.DefaultExt = ".bin";
				loadBinaryFilename.Filter = "Binary files (*.bin)|*.bin|All files (*.*)|*.*";
				loadBinaryFilename.FileName = this.Properties.get_LoadBinaryFilename();
				openFileDialog = loadBinaryFilename;
				nullable = openFileDialog.ShowDialog();
				if (nullable.HasValue && nullable.Value)
				{
					this.Properties.set_LoadBinaryFilename(openFileDialog.FileName);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException6(exception, openFileDialog, nullable, loadBinaryFilename, this, sender, e);
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
					uri = new Uri("/Devkit.IDE;component/view/solutionproperties.xaml", UriKind.Relative);
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
				if (num != 1)
				{
					this._contentLoaded = true;
				}
				else
				{
					(Button)target.Click += new RoutedEventHandler(this.BrowseClick);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, num, this, connectionId, target);
				throw;
			}
		}
	}
}