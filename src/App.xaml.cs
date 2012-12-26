using SmartAssembly.SmartExceptionsCore;
using SmartAssembly.SmartExceptionsWithAdvancedUI;
using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Windows;

namespace Devkit.IDE
{
	public class App : Application
	{
		private static bool _commandLineRead;

		private bool _contentLoaded;

		static App()
		{
		}

		public App()
		{
		}

		public static string[] GetPendingCommandline()
		{
			string[] commandLineArgs;
			try
			{
				if (!App._commandLineRead)
				{
					App._commandLineRead = true;
					commandLineArgs = Environment.GetCommandLineArgs();
				}
				else
				{
					commandLineArgs = null;
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException0(exception);
				throw;
			}
			return commandLineArgs;
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
					base.StartupUri = new Uri("MainWindow.xaml", UriKind.Relative);
					uri = new Uri("/Devkit.IDE;component/app.xaml", UriKind.Relative);
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
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		[STAThread]
		public static void Main()
		{
			App app;
			try
			{
				if (UnhandledExceptionHandlerWithAdvancedUI.AttachApp())
				{
					app = new App();
					app.InitializeComponent();
					app.Run();
				}
			}
			catch (Exception exception)
			{
				object[] objArray = new object[1];
				objArray[0] = app;
				UnhandledExceptionHandler.EntryPointException(exception, objArray);
			}
		}

		protected override void OnStartup(StartupEventArgs e)
		{
			try
			{
				base.OnStartup(e);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, e);
				throw;
			}
		}
	}
}