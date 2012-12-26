using Devkit.Interfaces;
using Devkit.Workspace;
using Devkit.Workspace.ViewModel;
using Ninject;
using Ninject.Parameters;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;

namespace Devkit.IDE.View
{
	public class Display : Window, IComponentConnector, ISystemUI
	{
		private bool _hasBeenShown;

		private List<IDisplayContentProvider> _providers;

		private KeyEventHandler KeyEvent;

		private Delegates.DisplayContentProviderChangedHandler DisplayContentProviderChanged;

		internal StackPanel contentPanel;

		private bool _contentLoaded;

		public Display()
		{
			try
			{
				this._providers = new List<IDisplayContentProvider>();
				this.InitializeComponent();
				base.Loaded += new RoutedEventHandler(this.OnLoaded);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
		}

		public void AddDisplayContentProvider(IDisplayContentProvider provider)
		{
			try
			{
				this._providers.Add(provider);
				this.OnDisplayContentProviderChanged();
				this.CreateContentIfNecessary();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, provider);
				throw;
			}
		}

		private void CreateContentIfNecessary()
		{
			IDisplayContentProvider _provider = null;
			List<IDisplayContentProvider>.Enumerator enumerator;
			try
			{
				if (this.contentPanel.Children.Count != this._providers.Count)
				{
					this.contentPanel.Children.Clear();
					foreach (IDisplayContentProvider _provider in this._providers)
					{
						this.contentPanel.Children.Add(_provider.CreateUIElement());
					}
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, _provider, enumerator, this);
				throw;
			}
		}

		public IEnumerable<IDisplayContentProvider> GetDisplayContentProviders()
		{
			IEnumerable<IDisplayContentProvider> displayContentProviders;
			try
			{
				displayContentProviders = this._providers;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
			return displayContentProviders;
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
					uri = new Uri("/Devkit.IDE;component/view/display.xaml", UriKind.Relative);
					Application.LoadComponent(this, uri);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, uri, this);
				throw;
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			try
			{
				e.Cancel = true;
				base.Hide();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, e);
				throw;
			}
		}

		private void OnDisplayContentProviderChanged()
		{
			Delegates.DisplayContentProviderChangedHandler displayContentProviderChanged;
			try
			{
				displayContentProviderChanged = this.DisplayContentProviderChanged;
				if (displayContentProviderChanged != null)
				{
					displayContentProviderChanged(this._providers);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, displayContentProviderChanged, this);
				throw;
			}
		}

		private void OnKeyEvent(object sender, KeyEventArgs e)
		{
			KeyEventHandler keyEvent;
			try
			{
				keyEvent = this.KeyEvent;
				if (keyEvent != null)
				{
					keyEvent(sender, e);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, keyEvent, this, sender, e);
				throw;
			}
		}

		private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
		{
			try
			{
				this.CreateContentIfNecessary();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, routedEventArgs);
				throw;
			}
		}

		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			IDisplayContentProvider current;
			IDisplayContentProvider _provider = null;
			RuntimeManager runtimeManager;
			Visibility visibility;
			List<IDisplayContentProvider>.Enumerator enumerator;
			List<IDisplayContentProvider>.Enumerator enumerator1;
			try
			{
				base.OnPropertyChanged(e);
				if (e.Property.Name == "Visibility" && e.NewValue != e.OldValue)
				{
					visibility = base.Visibility;
					if (visibility == Visibility.Visible)
					{
						this._hasBeenShown = true;
						enumerator = this._providers.GetEnumerator();
						try
						{
							while (enumerator.MoveNext())
							{
								current = enumerator.Current;
								current.DisplayOpened();
							}
							goto Label1;
						}
						finally
						{
							enumerator.Dispose();
						}
					}
					else if (visibility == Visibility.Hidden || visibility == Visibility.Collapsed)
					{
						foreach (IDisplayContentProvider _provider in this._providers)
						{
							_provider.DisplayClosed();
						}
						if (this._hasBeenShown)
						{
							runtimeManager = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]).get_RuntimeManager();
							if (runtimeManager.get_State() == 3 || MessageBox.Show(base.Owner, "System is still running: would you like to stop execution?", "System active", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
							{
								goto Label1;
							}
							runtimeManager.Stop();
							goto Label1;
						}
						else
						{
							return;
						}
					}
					return;
				}
			Label1:
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException8(exception, current, _provider, runtimeManager, visibility, enumerator, enumerator1, this, e);
				throw;
			}
		}

		public void RemoveDisplayContentProvider(IDisplayContentProvider provider)
		{
			try
			{
				this._providers.Remove(provider);
				this.OnDisplayContentProviderChanged();
				this.CreateContentIfNecessary();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, provider);
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
						(Display)target.KeyDown += new KeyEventHandler(this.WindowKey);
						(Display)target.KeyUp += new KeyEventHandler(this.WindowKey);
						return;
					}
					case 2:
					{
						this.contentPanel = (StackPanel)target;
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

		private void WindowKey(object sender, KeyEventArgs e)
		{
			try
			{
				this.OnKeyEvent(sender, e);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}

		public event Delegates.DisplayContentProviderChangedHandler DisplayContentProviderChanged
		{
			add
			{
				Delegates.DisplayContentProviderChangedHandler displayContentProviderChanged;
				Delegates.DisplayContentProviderChangedHandler displayContentProviderChangedHandler;
				Delegates.DisplayContentProviderChangedHandler displayContentProviderChangedHandler1;
				try
				{
					displayContentProviderChanged = this.DisplayContentProviderChanged;
					do
					{
						displayContentProviderChangedHandler = displayContentProviderChanged;
						displayContentProviderChangedHandler1 = (Delegates.DisplayContentProviderChangedHandler)Delegate.Combine(displayContentProviderChangedHandler, value);
						displayContentProviderChanged = Interlocked.CompareExchange<Delegates.DisplayContentProviderChangedHandler>(ref this.DisplayContentProviderChanged, displayContentProviderChangedHandler1, displayContentProviderChangedHandler);
					}
					while (displayContentProviderChanged != displayContentProviderChangedHandler);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, displayContentProviderChanged, displayContentProviderChangedHandler, displayContentProviderChangedHandler1, this, value);
					throw;
				}
			}
			remove
			{
				Delegates.DisplayContentProviderChangedHandler displayContentProviderChanged;
				Delegates.DisplayContentProviderChangedHandler displayContentProviderChangedHandler;
				Delegates.DisplayContentProviderChangedHandler displayContentProviderChangedHandler1;
				try
				{
					displayContentProviderChanged = this.DisplayContentProviderChanged;
					do
					{
						displayContentProviderChangedHandler = displayContentProviderChanged;
						displayContentProviderChangedHandler1 = (Delegates.DisplayContentProviderChangedHandler)Delegate.Remove(displayContentProviderChangedHandler, value);
						displayContentProviderChanged = Interlocked.CompareExchange<Delegates.DisplayContentProviderChangedHandler>(ref this.DisplayContentProviderChanged, displayContentProviderChangedHandler1, displayContentProviderChangedHandler);
					}
					while (displayContentProviderChanged != displayContentProviderChangedHandler);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, displayContentProviderChanged, displayContentProviderChangedHandler, displayContentProviderChangedHandler1, this, value);
					throw;
				}
			}
		}

		public event KeyEventHandler KeyEvent
		{
			add
			{
				KeyEventHandler keyEvent;
				KeyEventHandler keyEventHandler;
				KeyEventHandler keyEventHandler1;
				try
				{
					keyEvent = this.KeyEvent;
					do
					{
						keyEventHandler = keyEvent;
						keyEventHandler1 = (KeyEventHandler)Delegate.Combine(keyEventHandler, value);
						keyEvent = Interlocked.CompareExchange<KeyEventHandler>(ref this.KeyEvent, keyEventHandler1, keyEventHandler);
					}
					while (keyEvent != keyEventHandler);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, keyEvent, keyEventHandler, keyEventHandler1, this, value);
					throw;
				}
			}
			remove
			{
				KeyEventHandler keyEvent;
				KeyEventHandler keyEventHandler;
				KeyEventHandler keyEventHandler1;
				try
				{
					keyEvent = this.KeyEvent;
					do
					{
						keyEventHandler = keyEvent;
						keyEventHandler1 = (KeyEventHandler)Delegate.Remove(keyEventHandler, value);
						keyEvent = Interlocked.CompareExchange<KeyEventHandler>(ref this.KeyEvent, keyEventHandler1, keyEventHandler);
					}
					while (keyEvent != keyEventHandler);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, keyEvent, keyEventHandler, keyEventHandler1, this, value);
					throw;
				}
			}
		}
	}
}