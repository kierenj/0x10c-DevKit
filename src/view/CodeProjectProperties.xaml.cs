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
	public class CodeProjectProperties : UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public CodeProjectProperties()
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
		public void InitializeComponent()
		{
			Uri uri;
			try
			{
				if (!this._contentLoaded)
				{
					this._contentLoaded = true;
					uri = new Uri("/Devkit.IDE;component/view/codeprojectproperties.xaml", UriKind.Relative);
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
			try
			{
				this._contentLoaded = true;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, connectionId, target);
				throw;
			}
		}
	}
}