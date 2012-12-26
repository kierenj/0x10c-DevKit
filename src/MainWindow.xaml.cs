using AvalonDock;
using Devkit.Core;
using Devkit.IDE.Controls;
using Devkit.IDE.View;
using Devkit.IDE.ViewModel;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;
using Devkit.Workspace;
using Devkit.Workspace.Services;
using Devkit.Workspace.ViewModel;
using Devkit.Workspace.ViewModel.Debugger;
using Ninject;
using Ninject.Parameters;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Devkit.IDE
{
	public class MainWindow : Window, IComponentConnector, IStyleConnector
	{
		private readonly Dictionary<DocPaneItem, DockableContent> _itemContent;

		private Welcome _about;

		internal RecentFileList recentFileList;

		internal RecentFileList recentSolutionList;

		internal DockingManager dockingManager;

		internal DockablePane panelSymbolsNav;

		internal ResizingPanel documentPaneContainer;

		internal DocumentPane documentsPane;

		internal DockablePane infoPane;

		internal DockableContent outputContent;

		internal DockableContent errorsContent;

		internal DockableContent cpuContent;

		private bool _contentLoaded;

		public Workspace Workspace
		{
			get
			{
				Workspace dataContext;
				try
				{
					dataContext = (Workspace)base.DataContext;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return dataContext;
			}
		}

		public MainWindow()
		{
			try
			{
				App.Kernel.Bind<IUiService>().ToConstant<UiService>(new UiService(this));
				this.InitializeComponent();
				this._itemContent = new Dictionary<DocPaneItem, DockableContent>();
				this.Workspace.add_PropertyChanged(new PropertyChangedEventHandler(this.Workspace_PropertyChanged));
				this.Workspace.get_BuildManager().add_PropertyChanged(new PropertyChangedEventHandler(this.BuildManager_PropertyChanged));
				this.Workspace.get_RuntimeManager().add_ExecutionBreak(new Delegates.ExecutionBreakHandler(this.RuntimeManager_ExecutionBreak));
				this.RebindFileCollectionChanged();
				this.CheckFiles();
				base.Loaded += new RoutedEventHandler(this.MainWindow_Loaded);
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

		private void BuildManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			BuildManager buildManager;
			Action action;
			MainWindow.MainWindow variable = null;
			string str;
			BuildStatus status;
			try
			{
				action = null;
				if (base.Dispatcher.CheckAccess())
				{
					buildManager = (BuildManager)sender;
					string propertyName = e.PropertyName;
					str = propertyName;
					if (propertyName != null)
					{
						if (str == "Status")
						{
							status = buildManager.get_Status();
							if (status == BuildStatus.BuildFailed)
							{
								this.infoPane.SelectedItem = this.errorsContent;
							}
							else
							{
								return;
							}
						}
						else
						{
							return;
						}
					}
				}
				else
				{
					Dispatcher dispatcher = base.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.BuildManager_PropertyChanged(sender, e);
							}
							catch (Exception exception)
							{
								StackFrameHelper.CreateException1(exception, this);
								throw;
							}
						}
						;
					}
					dispatcher.Invoke(action, new object[0]);
				}
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException8(exception1, buildManager, action, variable, str, status, this, sender, e);
				throw;
			}
		}

		private void buildOutput_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextBox textBox;
			try
			{
				textBox = (TextBox)sender;
				textBox.ScrollToEnd();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, textBox, this, sender, e);
				throw;
			}
		}

		private void CheckFiles()
		{
			Workspace workspace;
			Dictionary<DocPaneItem, DockableContent>.ValueCollection values;
			object[] array;
			DockableContent value = null;
			OpenFile openFile = null;
			Dictionary<DocPaneItem, DockableContent>.ValueCollection.Enumerator enumerator;
			IEnumerator<OpenFile> enumerator1 = null;
			try
			{
				workspace = this.Workspace;
				values = this._itemContent.Values;
				Dictionary<DocPaneItem, DockableContent>.ValueCollection docPaneItems = values;
				array = docPaneItems.Select<DockableContent, object>((DockableContent dc) => {
					object dataContext;
					try
					{
						object dataContext = dc.DataContext;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, dc);
						throw;
					}
					return dataContext;
				}
				).ToArray<object>();
				foreach (DockableContent value in values)
				{
					if (value.DataContext as OpenFile == null || workspace.get_OpenFiles().Contains<object>(value.DataContext))
					{
						continue;
					}
					this.documentsPane.Items.Remove(value);
				}
				foreach (OpenFile openFile in workspace.get_OpenFiles())
				{
					if (array.Contains<object>(openFile))
					{
						continue;
					}
					this.documentsPane.Items.Add(this.GetContent(openFile));
					this.documentsPane.SelectedIndex = this.documentsPane.Items.Count - 1;
				}
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException8(exception1, workspace, values, array, value, openFile, enumerator, enumerator1, this);
				throw;
			}
		}

		private object CreateCodeEditorControl(IOpenFile openFile, CodeEditorStrategy strategy)
		{
			object codeEditor;
			try
			{
				codeEditor = new CodeEditor(strategy);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, openFile, strategy);
				throw;
			}
			return codeEditor;
		}

		private DockableContent CreateContent(DocPaneItem dataItem)
		{
			DataTemplate dataTemplate;
			DockableContent dockableContent;
			IOpenFile openFile;
			IFileTypeProvider fileBuildProvider;
			IEditorControlStrategy editorControlStrategy;
			CodeEditorStrategy codeEditorStrategy;
			CustomEditorControlStrategy customEditorControlStrategy;
			ItemPropertyPage itemPropertyPage;
			object item;
			IProjectProperties projectProperty;
			IProjectTypeProvider projectTypeProvider;
			SolutionProperties solutionProperty;
			MainWindow.MainWindow variable = null;
			DockableContent dockableContent1;
			try
			{
				dataTemplate = (DataTemplate)base.FindResource("documentPaneTemplate");
				dockableContent = (DockableContent)dataTemplate.LoadContent();
				dockableContent.DataContext = dataItem;
				openFile = dataItem as IOpenFile;
				if (openFile != null)
				{
					fileBuildProvider = this.Workspace.get_BuildManager().GetFileBuildProvider(openFile.File);
					if (fileBuildProvider != null)
					{
						editorControlStrategy = fileBuildProvider.EditorControlStrategy;
						if (editorControlStrategy as CodeEditorStrategy == null)
						{
							if (editorControlStrategy is CustomEditorControlStrategy)
							{
								customEditorControlStrategy = (CustomEditorControlStrategy)editorControlStrategy;
								((ContentPresenter)dockableContent.FindName("contentPresenter")).Content = customEditorControlStrategy.CreateEditorControl(openFile);
							}
						}
						else
						{
							codeEditorStrategy = (CodeEditorStrategy)editorControlStrategy;
							((ContentPresenter)dockableContent.FindName("contentPresenter")).Content = this.CreateCodeEditorControl(openFile, codeEditorStrategy);
						}
					}
				}
				itemPropertyPage = dataItem as ItemPropertyPage;
				if (itemPropertyPage != null)
				{
					item = itemPropertyPage.Item;
					projectProperty = item as IProjectProperties;
					if (projectProperty != null)
					{
						projectTypeProvider = this.Workspace.get_BuildManager().GetProjectTypeProvider(projectProperty.TypeCode);
						if (projectTypeProvider != null)
						{
							((ContentPresenter)dockableContent.FindName("contentPresenter")).Content = projectTypeProvider.CreatePropertiesControl(projectProperty);
						}
					}
					solutionProperty = item as SolutionProperties;
					if (solutionProperty != null)
					{
						((ContentPresenter)dockableContent.FindName("contentPresenter")).Content = ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).CreateSolutionPropertiesControl(solutionProperty);
					}
				}
				dockableContent.add_Closed((object s, EventArgs e) => {
					try
					{
						this.DocPaneItemClosed(dataItem);
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException3(exception, this, s, e);
						throw;
					}
				}
				);
				dataItem.set_IsOpen(true);
				dockableContent1 = dockableContent;
			}
			catch (Exception exception1)
			{
				object[] objArray = new object[15];
				objArray[0] = dataTemplate;
				objArray[1] = dockableContent;
				objArray[2] = openFile;
				objArray[3] = fileBuildProvider;
				objArray[4] = editorControlStrategy;
				objArray[5] = codeEditorStrategy;
				objArray[6] = customEditorControlStrategy;
				objArray[7] = itemPropertyPage;
				objArray[8] = item;
				objArray[9] = projectProperty;
				objArray[10] = projectTypeProvider;
				objArray[11] = solutionProperty;
				objArray[12] = variable;
				objArray[13] = this;
				objArray[14] = dataItem;
				StackFrameHelper.CreateExceptionN(exception1, objArray);
				throw;
			}
			return dockableContent1;
		}

		private void DocPaneItemClosed(DocPaneItem item)
		{
			Workspace workspace;
			try
			{
				workspace = this.Workspace;
				item.set_IsOpen(false);
				workspace.NotifyItemClosed(item);
				this._itemContent.Remove(item);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, workspace, this, item);
				throw;
			}
		}

		private void documentsPane_GotFocus(object sender, RoutedEventArgs e)
		{
			Workspace workspace;
			KeyValuePair<DocPaneItem, DockableContent> keyValuePair;
			ISolutionFile solutionFile;
			try
			{
				workspace = this.Workspace;
				keyValuePair = this._itemContent.SingleOrDefault<KeyValuePair<DocPaneItem, DockableContent>>((KeyValuePair<DocPaneItem, DockableContent> x) => {
					bool key;
					try
					{
						if (x.Value != this.documentsPane.SelectedItem)
						{
							bool key = false;
						}
						else
						{
							key = x.Key is OpenFile;
						}
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException2(exception, this, x);
						throw;
					}
					return key;
				}
				);
				Workspace workspace1 = workspace;
				if (keyValuePair.Key != null)
				{
					solutionFile = (OpenFile)keyValuePair.Key;
				}
				else
				{
					solutionFile = null;
				}
				workspace1.set_SelectedSolutionFile(solutionFile);
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException5(exception1, workspace, keyValuePair, this, sender, e);
				throw;
			}
		}

		private void documentsPane_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			Workspace workspace;
			KeyValuePair<DocPaneItem, DockableContent> keyValuePair;
			DocPaneItem dataContext;
			OpenFile openFile;
			try
			{
				if (base.IsLoaded)
				{
					workspace = this.Workspace;
					this._itemContent.SingleOrDefault<KeyValuePair<DocPaneItem, DockableContent>>((KeyValuePair<DocPaneItem, DockableContent> x) => {
						bool value;
						try
						{
							bool value = x.Value == this.documentsPane.SelectedItem;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, x);
							throw;
						}
						return value;
					}
					);
					keyValuePair = this._itemContent.SingleOrDefault<KeyValuePair<DocPaneItem, DockableContent>>((KeyValuePair<DocPaneItem, DockableContent> x) => {
						bool key;
						try
						{
							if (x.Value != this.documentsPane.SelectedItem)
							{
								bool key = false;
							}
							else
							{
								key = x.Key is OpenFile;
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, x);
							throw;
						}
						return key;
					}
					);
					Workspace workspace1 = workspace;
					if (this.documentsPane.SelectedItem == null)
					{
						dataContext = null;
					}
					else
					{
						dataContext = ((FrameworkElement)this.documentsPane.SelectedItem).DataContext as DocPaneItem;
					}
					workspace1.set_CurrentItem(dataContext);
					Workspace workspace2 = workspace;
					if (keyValuePair.Key == null)
					{
						openFile = null;
					}
					else
					{
						openFile = (OpenFile)keyValuePair.Key;
					}
					workspace2.set_CurrentFile(openFile);
				}
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException5(exception1, workspace, keyValuePair, this, sender, e);
				throw;
			}
		}

		private DockableContent GetContent(DocPaneItem item)
		{
			DockableContent dockableContent = null;
			DockableContent dockableContent1;
			try
			{
				if (!this._itemContent.TryGetValue(item, out dockableContent))
				{
					DockableContent dockableContent2 = this.CreateContent(item);
					dockableContent = dockableContent2;
					this._itemContent[item] = dockableContent2;
				}
				dockableContent1 = dockableContent;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, dockableContent, this, item);
				throw;
			}
			return dockableContent1;
		}

		public static IEnumerable<T> GetVisualTreeChildren<T>(DependencyObject parent)
		where T : DependencyObject
		{
			MainWindow.u003cGetVisualTreeChildrenu003ed__12<T> variable;
			IEnumerable<T> ts;
			try
			{
				variable = new MainWindow.u003cGetVisualTreeChildrenu003ed__12<T>(-2);
				variable.u003cu003e3__parent = parent;
				ts = variable;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, variable, parent);
				throw;
			}
			return ts;
		}

		private void HandleGenericGridItemDoubleClick(object sender, MouseButtonEventArgs e)
		{
			DataGrid dataGrid;
			ICompileMessage selectedItem;
			Breakpoint breakpoint;
			Symbol symbol;
			SourceReference sourceRef;
			MethodInfo method;
			try
			{
				dataGrid = sender as DataGrid;
				if (dataGrid != null)
				{
					selectedItem = dataGrid.SelectedItem as ICompileMessage;
					if (selectedItem != null)
					{
						ResolutionExtensions.Get<IOpenFileProvider>(App.Kernel, new IParameter[0]).SwitchToOpenFile(1, selectedItem.Filename, Utilities.ObjectAsEnumerable<int>(selectedItem.Line));
					}
					breakpoint = dataGrid.SelectedItem as Breakpoint;
					if (breakpoint != null)
					{
						ResolutionExtensions.Get<IOpenFileProvider>(App.Kernel, new IParameter[0]).SwitchToOpenFile(2, breakpoint.get_Path(), Utilities.ObjectAsEnumerable<int>(breakpoint.get_LineNumber()));
					}
					symbol = dataGrid.SelectedItem as Symbol;
					if (symbol != null)
					{
						sourceRef = symbol.SourceRef;
						if (sourceRef != null)
						{
							ResolutionExtensions.Get<IOpenFileProvider>(App.Kernel, new IParameter[0]).SwitchToOpenFile(2, sourceRef.File, Utilities.ObjectAsEnumerable<int>(sourceRef.Line));
						}
					}
					method = this.dockingManager.GetType().GetMethod("HideFlyoutWindow", BindingFlags.Instance | BindingFlags.NonPublic);
					method.Invoke(this.dockingManager, new object[0]);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException9(exception, dataGrid, selectedItem, breakpoint, symbol, sourceRef, method, this, sender, e);
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
					uri = new Uri("/Devkit.IDE;component/mainwindow.xaml", UriKind.Relative);
					Application.LoadComponent(this, uri);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, uri, this);
				throw;
			}
		}

		private void MainWindow_Loaded(object sender, RoutedEventArgs e)
		{
			string[] pendingCommandline;
			string str;
			char[] chrArray;
			char[] chrArray1;
			try
			{
				if (!this.panelSymbolsNav.get_IsAutoHidden())
				{
					this.panelSymbolsNav.ToggleAutoHide();
				}
				pendingCommandline = App.GetPendingCommandline();
				if (pendingCommandline.Count<string>() <= 1)
				{
					this._about = (Welcome)ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).ShowAbout();
				}
				else
				{
					this.documentsPane.Items.Clear();
					chrArray = new char[1];
					chrArray[0] = '\"';
					chrArray1 = new char[1];
					chrArray1[0] = '\"';
					str = pendingCommandline[1].TrimStart(chrArray).TrimEnd(chrArray1);
					this.Workspace.OpenSolution(str);
				}
				this.StartPluginSystem();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException7(exception, pendingCommandline, str, chrArray, chrArray1, this, sender, e);
				throw;
			}
		}

		private void MenuItem_Initialized(object sender, EventArgs e)
		{
		}

		public void MultiFocus(IEnumerable<DocPaneItem> items)
		{
			DocPaneItem item = null;
			DockableContent content;
			IEnumerator<DocPaneItem> enumerator = null;
			try
			{
				foreach (DocPaneItem item in items)
				{
					content = this.GetContent(item);
					if (this.documentsPane.Items.Contains(content))
					{
						continue;
					}
					this.documentsPane.Items.Add(content);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException5(exception, item, content, enumerator, this, items);
				throw;
			}
		}

		private void OpenFiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			try
			{
				if (base.Dispatcher.CheckAccess())
				{
					this.CheckFiles();
				}
				else
				{
					base.Dispatcher.Invoke(new Action(this.CheckFiles), new object[0]);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}

		private void PluginManagerMessage(string message)
		{
			try
			{
				if (this._about != null && this._about.IsVisible)
				{
					this._about.ShowMessage(message);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, message);
				throw;
			}
		}

		private void RebindFileCollectionChanged()
		{
			try
			{
				this.Workspace.get_OpenFiles().CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OpenFiles_CollectionChanged);
				this.Workspace.get_OpenFiles().CollectionChanged += new NotifyCollectionChangedEventHandler(this.OpenFiles_CollectionChanged);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
		}

		private void RecentFileList_MenuClick(object sender, RecentFileList.MenuClickEventArgs e)
		{
		}

		private void RecentSolutionList_MenuClick(object sender, RecentFileList.MenuClickEventArgs e)
		{
			try
			{
				this.Workspace.OpenSolution(e.Filepath);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}

		private void RuntimeManager_ExecutionBreak(bool stoppedCompletely)
		{
			Action action;
			MainWindow.MainWindow variable = null;
			try
			{
				action = null;
				if (base.Dispatcher.CheckAccess())
				{
					if (!stoppedCompletely && this.Workspace.get_RuntimeManager().get_CpuState().get_Status() == CpuStatus.Faulted)
					{
						this.infoPane.SelectedItem = this.cpuContent;
					}
				}
				else
				{
					Dispatcher dispatcher = base.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.RuntimeManager_ExecutionBreak(stoppedCompletely);
							}
							catch (Exception exception)
							{
								StackFrameHelper.CreateException1(exception, this);
								throw;
							}
						}
						;
					}
					dispatcher.Invoke(action, new object[0]);
				}
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException4(exception1, action, variable, this, stoppedCompletely);
				throw;
			}
		}

		public void ShowCustomView(DocPaneItem dataContext, bool uniqueByType = false)
		{
			DockableContent[] array;
			object[] objArray;
			DockableContent item = null;
			DockableContent content;
			IEnumerator enumerator = null;
			IDisposable disposable = null;
			try
			{
				array = MainWindow.GetVisualTreeChildren<DockableContent>(this).ToArray<DockableContent>();
				DockableContent[] dockableContentArray = array;
				objArray = dockableContentArray.Select<DockableContent, object>((DockableContent dc) => {
					object obj;
					try
					{
						object obj = dc.DataContext;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, dc);
						throw;
					}
					return obj;
				}
				).ToArray<object>();
				if (uniqueByType)
				{
					foreach (DockableContent item in this.documentsPane.Items)
					{
						if (item.DataContext.GetType() != dataContext.GetType())
						{
							continue;
						}
						this.documentsPane.SelectedItem = item;
						goto Label0;
					}
				}
				content = this.GetContent(dataContext);
				if (!objArray.Contains<object>(dataContext))
				{
					this.documentsPane.Items.Add(content);
				}
				this.documentsPane.SelectedItem = content;
			Label0:
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException9(exception1, array, objArray, item, content, enumerator, disposable, this, dataContext, uniqueByType);
				throw;
			}
		}

		private void StartPluginSystem()
		{
			MainWindow.MainWindow variable = null;
			try
			{
				PluginManager pluginManager = this.Workspace.get_PluginManager();
				pluginManager.add_PluginManagerMessage(new PluginManager.PluginManagerMessageHandler(this, PluginManagerMessage));
				Task.Factory.StartNew(() => {
					Exception exception;
					try
					{
						try
						{
							try
							{
								this.PluginManagerMessage("Loading...");
								Thread.Sleep(1000);
								pluginManager.Startup();
								this.PluginManagerMessage(string.Concat("Finished loading ", pluginManager.get_LoadedPlugins().Count, " plugins."));
							}
							catch (Exception exception1)
							{
								Exception exception = exception1;
								this.PluginManagerMessage("Error loading plugins");
								MessageBox.Show(string.Concat("Error starting plugin system:\n\n", exception.ToString()), "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
							}
						}
						finally
						{
							pluginManager.remove_PluginManagerMessage(new PluginManager.PluginManagerMessageHandler(this, PluginManagerMessage));
						}
					}
					catch (Exception exception2)
					{
						StackFrameHelper.CreateException2(exception2, exception, this);
						throw;
					}
				}
				);
			}
			catch (Exception exception3)
			{
				StackFrameHelper.CreateException2(exception3, variable, this);
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
						this.recentFileList = (RecentFileList)target;
						break;
					}
					case 2:
					{
						this.recentSolutionList = (RecentFileList)target;
						break;
					}
					case 3:
					{
						this.dockingManager = (DockingManager)target;
						break;
					}
					case 4:
					{
						this.panelSymbolsNav = (DockablePane)target;
						break;
					}
					case 5:
					{
						(DataGrid)target.MouseDoubleClick += new MouseButtonEventHandler(this.HandleGenericGridItemDoubleClick);
						break;
					}
					case 6:
					{
						this.documentPaneContainer = (ResizingPanel)target;
						break;
					}
					case 7:
					{
						this.documentsPane = (DocumentPane)target;
						this.documentsPane.SelectionChanged += new SelectionChangedEventHandler(this.documentsPane_SelectionChanged);
						this.documentsPane.GotFocus += new RoutedEventHandler(this.documentsPane_GotFocus);
						break;
					}
					case 8:
					{
						(TreeView)target.SelectedItemChanged += new RoutedPropertyChangedEventHandler<object>(this.TreeView_SelectedItemChanged);
						break;
					}
					case 9:
					{
					Label0:
						this._contentLoaded = true;
						break;
					}
					case 10:
					{
						this.infoPane = (DockablePane)target;
						break;
					}
					case 11:
					{
						this.outputContent = (DockableContent)target;
						break;
					}
					case 12:
					{
						(TextBox)target.TextChanged += new TextChangedEventHandler(this.buildOutput_TextChanged);
						break;
					}
					case 13:
					{
						this.errorsContent = (DockableContent)target;
						break;
					}
					case 14:
					{
						(DataGrid)target.MouseDoubleClick += new MouseButtonEventHandler(this.HandleGenericGridItemDoubleClick);
						break;
					}
					case 15:
					{
						(DataGrid)target.MouseDoubleClick += new MouseButtonEventHandler(this.HandleGenericGridItemDoubleClick);
						break;
					}
					case 16:
					{
						this.cpuContent = (DockableContent)target;
						break;
					}
					default:
					{
						goto Label0;
					}
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, num, this, connectionId, target);
				throw;
			}
		}

		[DebuggerNonUserCode]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target)
		{
			EventSetter eventSetter;
			int num;
			try
			{
				num = connectionId;
				if (num == 9)
				{
					eventSetter = new EventSetter();
					eventSetter.Event = UIElement.MouseRightButtonDownEvent;
					eventSetter.Handler = new MouseButtonEventHandler(this.TreeViewItem_MouseRightButtonDown);
					((Style)target).Setters.Add(eventSetter);
					eventSetter = new EventSetter();
					eventSetter.Event = Control.MouseDoubleClickEvent;
					eventSetter.Handler = new MouseButtonEventHandler(this.TreeViewItem_MouseDoubleClick);
					((Style)target).Setters.Add(eventSetter);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException5(exception, eventSetter, num, this, connectionId, target);
				throw;
			}
		}

		private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			try
			{
				if (e.NewValue != null)
				{
					this.Workspace.set_SelectedItem(e.NewValue as ViewModelBase);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}

		private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			TreeViewItem treeViewItem;
			IWorkspaceItem dataContext;
			try
			{
				treeViewItem = sender as TreeViewItem;
				if (treeViewItem != null)
				{
					dataContext = treeViewItem.DataContext as IWorkspaceItem;
					if (dataContext != null)
					{
						if (dataContext.get_DefaultCommand() != null && dataContext.get_DefaultCommand().CanExecute(null))
						{
							dataContext.get_DefaultCommand().Execute(null);
						}
					}
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException5(exception, treeViewItem, dataContext, this, sender, e);
				throw;
			}
		}

		private void TreeViewItem_MouseRightButtonDown(object sender, MouseEventArgs e)
		{
			TreeViewItem treeViewItem;
			try
			{
				treeViewItem = sender as TreeViewItem;
				if (treeViewItem != null)
				{
					treeViewItem.Focus();
					e.Handled = true;
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, treeViewItem, this, sender, e);
				throw;
			}
		}

		private void Workspace_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Action action;
			Action action1;
			MainWindow.MainWindow variable = null;
			string str;
			try
			{
				action1 = null;
				string propertyName = e.PropertyName;
				str = propertyName;
				if (propertyName != null)
				{
					if (str == "OpenFiles")
					{
						this.RebindFileCollectionChanged();
						this.CheckFiles();
						return;
					}
					else
					{
						if (str == "CurrentFile")
						{
							if (((Workspace)sender).get_CurrentFile() != null)
							{
								if (action1 == null)
								{
									action1 = () => {
										DockableContent content;
										try
										{
											DockableContent content = this.GetContent(((Workspace)sender).get_CurrentFile());
											DocumentPane documentPane.SelectedItem = content;
										}
										catch (Exception exception)
										{
											StackFrameHelper.CreateException2(exception, content, this);
											throw;
										}
									}
									;
								}
								action = action1;
								if (base.Dispatcher.CheckAccess())
								{
									action();
								}
								else
								{
									base.Dispatcher.Invoke(action, new object[0]);
									return;
								}
							}
							else
							{
								return;
							}
						}
						else
						{
							return;
						}
					}
				}
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException7(exception1, action, action1, variable, str, this, sender, e);
				throw;
			}
		}
	}
}