using Devkit.Workspace;
using Devkit.Workspace.Commands;
using Devkit.Workspace.Services;
using Ninject;
using Ninject.Parameters;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;

namespace Devkit.IDE.View
{
	public class StartPage : UserControl, IComponentConnector
	{
		internal WebBrowser browser;

		private bool _contentLoaded;

		public RelayCommand BackCommand
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					relayCommand = new RelayCommand("Back", (object p) => {
						try
						{
							this.browser.GoBack();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, p);
							throw;
						}
					}
					, (object p) => {
						bool canGoBack;
						try
						{
							bool canGoBack = this.browser.CanGoBack;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, p);
							throw;
						}
						return canGoBack;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(35));
					relayCommand.set_ToolTip("Back");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, this);
					throw;
				}
				return relayCommand1;
			}
		}

		public RelayCommand ForwardCommand
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					relayCommand = new RelayCommand("Forward", (object p) => {
						try
						{
							this.browser.GoForward();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, p);
							throw;
						}
					}
					, (object p) => {
						bool canGoForward;
						try
						{
							bool canGoForward = this.browser.CanGoForward;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, p);
							throw;
						}
						return canGoForward;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(36));
					relayCommand.set_ToolTip("Forward");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, this);
					throw;
				}
				return relayCommand1;
			}
		}

		public RelayCommand ReloadCommand
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					relayCommand = new RelayCommand("Reload", (object p) => {
						try
						{
							this.browser.Refresh();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, p);
							throw;
						}
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(37));
					relayCommand.set_ToolTip("Reload");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, this);
					throw;
				}
				return relayCommand1;
			}
		}

		public StartPage()
		{
			try
			{
				this.InitializeComponent();
				this.browser.Source = new Uri(App.StartPageUrl);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
		}

		private void browser_Navigated(object sender, NavigationEventArgs e)
		{
			try
			{
				CommandManager.InvalidateRequerySuggested();
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
					uri = new Uri("/Devkit.IDE;component/view/startpage.xaml", UriKind.Relative);
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
					this.browser = (WebBrowser)target;
					this.browser.Navigated += new NavigatedEventHandler(this.browser_Navigated);
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