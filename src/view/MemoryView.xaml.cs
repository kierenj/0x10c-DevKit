using Devkit.Workspace.ViewModel.Debugger;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;

namespace Devkit.IDE.View
{
	public class MemoryView : UserControl, IComponentConnector
	{
		private bool _contentLoaded;

		public MemoryView()
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

		private MemoryViewBase.DelegateObjectWrapper GetListViewItemFromEvent(ListView lv, object originalSource)
		{
			DependencyObject dependencyObject;
			DependencyObject parent;
			MemoryViewBase.DelegateObjectWrapper dataContext;
			MemoryViewBase.DelegateObjectWrapper delegateObjectWrapper;
			try
			{
				dependencyObject = originalSource as DependencyObject;
				if (dependencyObject != null)
				{
					parent = dependencyObject;
					while (parent != null && parent != lv)
					{
						dataContext = ((FrameworkElement)parent).DataContext as MemoryViewBase.DelegateObjectWrapper;
						if (dataContext == null)
						{
							parent = VisualTreeHelper.GetParent(parent);
						}
						else
						{
							delegateObjectWrapper = dataContext;
							return delegateObjectWrapper;
						}
					}
				}
				delegateObjectWrapper = null;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException6(exception, dependencyObject, parent, dataContext, this, lv, originalSource);
				throw;
			}
			return delegateObjectWrapper;
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
					uri = new Uri("/Devkit.IDE;component/view/memoryview.xaml", UriKind.Relative);
					Application.LoadComponent(this, uri);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, uri, this);
				throw;
			}
		}

		private void ListView_ScrollChanged(object sender, ScrollChangedEventArgs e)
		{
			ListView listView;
			HitTestResult hitTestResult;
			HitTestResult hitTestResult1;
			MemoryViewBase.DelegateObjectWrapper listViewItemFromEvent;
			MemoryViewBase.DelegateObjectWrapper delegateObjectWrapper;
			try
			{
				listView = (ListView)sender;
				hitTestResult = VisualTreeHelper.HitTest(listView, new Point(5, 5));
				hitTestResult1 = VisualTreeHelper.HitTest(listView, new Point(5, listView.ActualHeight - 5));
				listViewItemFromEvent = this.GetListViewItemFromEvent(listView, hitTestResult.VisualHit);
				delegateObjectWrapper = this.GetListViewItemFromEvent(listView, hitTestResult1.VisualHit);
				if (listViewItemFromEvent != null && delegateObjectWrapper != null)
				{
					((MemoryViewBase)base.DataContext).SetChangeHookRange(listViewItemFromEvent.Index, delegateObjectWrapper.Index);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException8(exception, listView, hitTestResult, hitTestResult1, listViewItemFromEvent, delegateObjectWrapper, this, sender, e);
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
					((ListView)target).AddHandler(ScrollViewer.ScrollChangedEvent, new ScrollChangedEventHandler(this.ListView_ScrollChanged));
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