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
	public class FindAndReplace : Window, IComponentConnector
	{
		internal TextBox searchTerm;

		private bool _contentLoaded;

		public FindAndReplace()
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

		private void Close_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				base.Close();
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
					uri = new Uri("/Devkit.IDE;component/view/findandreplace.xaml", UriKind.Relative);
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
						this.searchTerm = (TextBox)target;
						return;
					}
					case 2:
					{
						(Button)target.Click += new RoutedEventHandler(this.Close_Click);
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
	}
}