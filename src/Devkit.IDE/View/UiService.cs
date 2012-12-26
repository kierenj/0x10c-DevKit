using Devkit.IDE;
using Devkit.IDE.Resources;
using Devkit.IDE.ViewModel;
using Devkit.Interfaces;
using Devkit.Interfaces.Build;
using Devkit.Workspace;
using Devkit.Workspace.Services;
using Devkit.Workspace.ViewModel;
using Devkit.Workspace.ViewModel.Debugger;
using Microsoft.Win32;
using Ninject;
using Ninject.Parameters;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;

namespace Devkit.IDE.View
{
	public class UiService : IUiService
	{
		private MainWindow _mainWindow;

		private Display _ui;

		private FindAndReplace _findAndReplace;

		private Dictionary<object, ItemPropertyPage> _propertyPageCache;

		public UiService(MainWindow mainWindow)
		{
			Display display;
			try
			{
				this._mainWindow = mainWindow;
				this._mainWindow.Loaded += new RoutedEventHandler(this.MainWindowLoaded);
				display = new Display();
				display.Visibility = Visibility.Hidden;
				this._ui = display;
				this._propertyPageCache = new Dictionary<object, ItemPropertyPage>();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, display, this, mainWindow);
				throw;
			}
		}

		public object CreateBuiltinProjectPropertiesControl(IProjectProperties project)
		{
			CodeProjectProperties codeProjectProperty;
			Action action;
			UiService.UiService variable = null;
			object obj;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					codeProjectProperty = new CodeProjectProperties();
					codeProjectProperty.DataContext = project;
					obj = codeProjectProperty;
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.CreateBuiltinProjectPropertiesControl(project);
							}
							catch (Exception exception)
							{
								StackFrameHelper.CreateException1(exception, this);
								throw;
							}
						}
						;
					}
					obj = dispatcher.Invoke(action, new object[0]);
				}
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException5(exception1, codeProjectProperty, action, variable, this, project);
				throw;
			}
			return obj;
		}

		public object CreateSolutionPropertiesControl(SolutionProperties solutionProps)
		{
			SolutionProperties solutionProperty;
			Action action;
			UiService.UiService variable = null;
			object obj;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					solutionProperty = new SolutionProperties();
					solutionProperty.DataContext = solutionProps;
					obj = solutionProperty;
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.CreateSolutionPropertiesControl(solutionProps);
							}
							catch (Exception exception)
							{
								StackFrameHelper.CreateException1(exception, this);
								throw;
							}
						}
						;
					}
					obj = dispatcher.Invoke(action, new object[0]);
				}
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException5(exception1, solutionProperty, action, variable, this, solutionProps);
				throw;
			}
			return obj;
		}

		public void ExitApplication()
		{
			Action action;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					this.HideDisplay();
					Application.Current.Shutdown(0);
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.ExitApplication();
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
				StackFrameHelper.CreateException2(exception1, action, this);
				throw;
			}
		}

		public string FileDialog(string title, string filetypeFilter, bool saveDialog)
		{
			bool? nullable;
			SaveFileDialog saveFileDialog;
			SaveFileDialog saveFileDialog1;
			OpenFileDialog openFileDialog;
			OpenFileDialog openFileDialog1;
			Action action;
			UiService.UiService variable = null;
			string fileName;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					if (!saveDialog)
					{
						openFileDialog1 = new OpenFileDialog();
						openFileDialog1.Filter = filetypeFilter;
						openFileDialog1.Title = title;
						openFileDialog = openFileDialog1;
						nullable = openFileDialog.ShowDialog();
						if (!nullable.HasValue || !nullable.Value)
						{
							fileName = null;
						}
						else
						{
							fileName = openFileDialog.FileName;
						}
					}
					else
					{
						saveFileDialog1 = new SaveFileDialog();
						saveFileDialog1.Filter = filetypeFilter;
						saveFileDialog1.Title = title;
						saveFileDialog = saveFileDialog1;
						nullable = saveFileDialog.ShowDialog();
						if (!nullable.HasValue || !nullable.Value)
						{
							fileName = null;
						}
						else
						{
							fileName = saveFileDialog.FileName;
						}
					}
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.FileDialog(title, filetypeFilter, saveDialog);
							}
							catch (Exception exception)
							{
								StackFrameHelper.CreateException1(exception, this);
								throw;
							}
						}
						;
					}
					fileName = (string)dispatcher.Invoke(action, new object[0]);
				}
			}
			catch (Exception exception1)
			{
				object[] objArray = new object[11];
				objArray[0] = nullable;
				objArray[1] = saveFileDialog;
				objArray[2] = saveFileDialog1;
				objArray[3] = openFileDialog;
				objArray[4] = openFileDialog1;
				objArray[5] = action;
				objArray[6] = variable;
				objArray[7] = this;
				objArray[8] = title;
				objArray[9] = filetypeFilter;
				objArray[10] = saveDialog;
				StackFrameHelper.CreateExceptionN(exception1, objArray);
				throw;
			}
			return fileName;
		}

		public void FocusMultipleItems(IEnumerable<DocPaneItem> items)
		{
			Action action;
			UiService.UiService variable = null;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					this._mainWindow.MultiFocus(items);
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.FocusMultipleItems(items);
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
				StackFrameHelper.CreateException4(exception1, action, variable, this, items);
				throw;
			}
		}

		public object GetImage(ImageType imageType)
		{
			object value;
			try
			{
				value = typeof(Images).GetProperty(imageType.ToString()).GetValue(null, new object[0]);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, imageType);
				throw;
			}
			return value;
		}

		public ISystemUI GetSystemUI()
		{
			ISystemUI systemUI;
			try
			{
				systemUI = this._ui;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
			return systemUI;
		}

		public void HideDisplay()
		{
			Action action;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					this._ui.Hide();
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.HideDisplay();
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
				StackFrameHelper.CreateException2(exception1, action, this);
				throw;
			}
		}

		private void MainWindowLoaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this._ui.Owner = this._mainWindow;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}

		public void OpenItemProperties(object item)
		{
			Action action;
			UiService.UiService variable = null;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					if (!this._propertyPageCache.ContainsKey(item))
					{
						this._propertyPageCache[item] = new ItemPropertyPage(item);
					}
					this._mainWindow.ShowCustomView(this._propertyPageCache[item], false);
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.OpenItemProperties(item);
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
				StackFrameHelper.CreateException4(exception1, action, variable, this, item);
				throw;
			}
		}

		public void RegisterFileOpened(string path, bool success)
		{
			Action action;
			UiService.UiService variable = null;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					this._mainWindow.recentFileList.InsertFile(path);
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.RegisterFileOpened(path, success);
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
				StackFrameHelper.CreateException5(exception1, action, variable, this, path, success);
				throw;
			}
		}

		public void RegisterSolutionOpened(string path, bool success)
		{
			Action action;
			UiService.UiService variable = null;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					if (!success)
					{
						this._mainWindow.recentSolutionList.RemoveFile(path);
					}
					else
					{
						this._mainWindow.recentSolutionList.InsertFile(path);
					}
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.RegisterSolutionOpened(path, success);
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
				StackFrameHelper.CreateException5(exception1, action, variable, this, path, success);
				throw;
			}
		}

		public void RequestInformation(InfoRequest request, Action<RequestResult, InfoRequest> continuation)
		{
			NewFolderedTypedItem newFolderedTypedItem;
			bool? nullable;
			NewFolderedTypedItem newFolderedTypedItem1;
			NewFolderedItem newFolderedItem;
			bool? nullable1;
			NewFolderedItem newFolderedItem1;
			NewItem newItem;
			bool? nullable2;
			NewItem newItem1;
			NewFile newFile;
			bool? nullable3;
			NewFile newFile1;
			SaveFileInfo saveFileInfo;
			SaveFileDialog saveFileDialog;
			bool? nullable4;
			SaveFileDialog defaultExtension;
			OpenFileInfo openFileInfo;
			OpenFileDialog openFileDialog;
			bool? nullable5;
			OpenFileDialog title;
			MessageRequest messageRequest;
			MessageBoxButton messageBoxButton;
			MessageBoxImage messageBoxImage;
			MessageBoxResult messageBoxResult;
			Action action;
			UiService.UiService variable = null;
			string[] filetypeDescription;
			string[] extension;
			RequestResult requestResult;
			RequestResult requestResult1;
			RequestResult requestResult2;
			RequestResult requestResult3;
			RequestResult requestResult4;
			RequestResult requestResult5;
			RequestResult requestResult6;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					if (request as NewFolderedTypedItemInfo == null)
					{
						if (request as NewFolderedItemInfo == null)
						{
							if (request as NewItemInfo == null)
							{
								if (request as NewFileInfo == null)
								{
									if (request as SaveFileInfo == null)
									{
										if (request as OpenFileInfo == null)
										{
											if (request is MessageRequest)
											{
												messageRequest = (MessageRequest)request;
												messageBoxButton = (MessageBoxButton)Enum.Parse(typeof(MessageBoxButton), messageRequest.get_Buttons().ToString());
												messageBoxImage = (MessageBoxImage)Enum.Parse(typeof(MessageBoxImage), messageRequest.get_Image().ToString());
												messageBoxResult = MessageBox.Show(messageRequest.get_Message(), messageRequest.get_Title(), messageBoxButton, messageBoxImage);
												Action<RequestResult, InfoRequest> action1 = continuation;
												if (messageBoxResult == MessageBoxResult.OK || messageBoxResult == MessageBoxResult.Yes)
												{
													requestResult = 0;
												}
												else
												{
													requestResult = 1;
												}
												action1(requestResult, request);
											}
										}
										else
										{
											openFileInfo = (OpenFileInfo)request;
											title = new OpenFileDialog();
											OpenFileDialog openFileDialog1 = title;
											string filter = openFileInfo.get_Filter();
											string str = filter;
											if (filter == null)
											{
												extension = new string[6];
												extension[0] = openFileInfo.get_FiletypeDescription();
												extension[1] = " (*";
												extension[2] = openFileInfo.get_Extension();
												extension[3] = ")|*";
												extension[4] = openFileInfo.get_Extension();
												extension[5] = "|All files (*.*)|*.*";
												str = string.Concat(extension);
											}
											openFileDialog1.Filter = str;
											title.Title = openFileInfo.get_Title();
											title.CheckFileExists = true;
											openFileDialog = title;
											nullable5 = openFileDialog.ShowDialog();
											openFileInfo.set_Path(openFileDialog.FileName);
											Action<RequestResult, InfoRequest> action2 = continuation;
											if (!nullable5.HasValue || !nullable5.Value)
											{
												requestResult1 = 1;
											}
											else
											{
												requestResult1 = 0;
											}
											action2(requestResult1, request);
										}
									}
									else
									{
										saveFileInfo = (SaveFileInfo)request;
										defaultExtension = new SaveFileDialog();
										defaultExtension.AddExtension = true;
										defaultExtension.DefaultExt = saveFileInfo.get_DefaultExtension();
										filetypeDescription = new string[6];
										filetypeDescription[0] = saveFileInfo.get_FiletypeDescription();
										filetypeDescription[1] = " (*";
										filetypeDescription[2] = saveFileInfo.get_DefaultExtension();
										filetypeDescription[3] = ")|*";
										filetypeDescription[4] = saveFileInfo.get_DefaultExtension();
										filetypeDescription[5] = "|All files (*.*)|*.*";
										defaultExtension.Filter = string.Concat(filetypeDescription);
										defaultExtension.Title = saveFileInfo.get_Title();
										defaultExtension.OverwritePrompt = true;
										saveFileDialog = defaultExtension;
										nullable4 = saveFileDialog.ShowDialog();
										saveFileInfo.set_Path(saveFileDialog.FileName);
										Action<RequestResult, InfoRequest> action3 = continuation;
										if (!nullable4.HasValue || !nullable4.Value)
										{
											requestResult2 = 1;
										}
										else
										{
											requestResult2 = 0;
										}
										action3(requestResult2, request);
									}
								}
								else
								{
									newFile1 = new NewFile();
									newFile1.DataContext = request;
									newFile = newFile1;
									nullable3 = newFile.ShowDialog();
									Action<RequestResult, InfoRequest> action4 = continuation;
									if (!nullable3.HasValue || !nullable3.Value)
									{
										requestResult3 = 1;
									}
									else
									{
										requestResult3 = 0;
									}
									action4(requestResult3, request);
								}
							}
							else
							{
								newItem1 = new NewItem();
								newItem1.DataContext = request;
								newItem = newItem1;
								nullable2 = newItem.ShowDialog();
								Action<RequestResult, InfoRequest> action5 = continuation;
								if (!nullable2.HasValue || !nullable2.Value)
								{
									requestResult4 = 1;
								}
								else
								{
									requestResult4 = 0;
								}
								action5(requestResult4, request);
							}
						}
						else
						{
							newFolderedItem1 = new NewFolderedItem();
							newFolderedItem1.DataContext = request;
							newFolderedItem = newFolderedItem1;
							nullable1 = newFolderedItem.ShowDialog();
							Action<RequestResult, InfoRequest> action6 = continuation;
							if (!nullable1.HasValue || !nullable1.Value)
							{
								requestResult5 = 1;
							}
							else
							{
								requestResult5 = 0;
							}
							action6(requestResult5, request);
						}
					}
					else
					{
						newFolderedTypedItem1 = new NewFolderedTypedItem();
						newFolderedTypedItem1.DataContext = request;
						newFolderedTypedItem = newFolderedTypedItem1;
						nullable = newFolderedTypedItem.ShowDialog();
						Action<RequestResult, InfoRequest> action7 = continuation;
						if (!nullable.HasValue || !nullable.Value)
						{
							requestResult6 = 1;
						}
						else
						{
							requestResult6 = 0;
						}
						action7(requestResult6, request);
					}
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.RequestInformation(request, continuation);
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
				object[] objArray = new object[31];
				objArray[0] = newFolderedTypedItem;
				objArray[1] = nullable;
				objArray[2] = newFolderedTypedItem1;
				objArray[3] = newFolderedItem;
				objArray[4] = nullable1;
				objArray[5] = newFolderedItem1;
				objArray[6] = newItem;
				objArray[7] = nullable2;
				objArray[8] = newItem1;
				objArray[9] = newFile;
				objArray[10] = nullable3;
				objArray[11] = newFile1;
				objArray[12] = saveFileInfo;
				objArray[13] = saveFileDialog;
				objArray[14] = nullable4;
				objArray[15] = defaultExtension;
				objArray[16] = openFileInfo;
				objArray[17] = openFileDialog;
				objArray[18] = nullable5;
				objArray[19] = title;
				objArray[20] = messageRequest;
				objArray[21] = messageBoxButton;
				objArray[22] = messageBoxImage;
				objArray[23] = messageBoxResult;
				objArray[24] = action;
				objArray[25] = variable;
				objArray[26] = filetypeDescription;
				objArray[27] = extension;
				objArray[28] = this;
				objArray[29] = request;
				objArray[30] = continuation;
				StackFrameHelper.CreateExceptionN(exception1, objArray);
				throw;
			}
		}

		public object ShowAbout()
		{
			Welcome welcome;
			object obj;
			try
			{
				if (Application.Current.Dispatcher.CheckAccess())
				{
					welcome = new Welcome();
					welcome.Owner = this._mainWindow;
					welcome.Show();
					obj = welcome;
				}
				else
				{
					obj = Application.Current.Dispatcher.Invoke(new Func<object>(this.ShowAbout), new object[0]);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, welcome, this);
				throw;
			}
			return obj;
		}

		public void ShowConfirmation(string message, string confirmText, string cancelText)
		{
		}

		public void ShowDialog(ViewModelBase viewModel)
		{
			FindAndReplace findAndReplace;
			try
			{
				if (viewModel is FindAndReplace)
				{
					if (this._findAndReplace == null || !this._findAndReplace.IsLoaded)
					{
						findAndReplace = new FindAndReplace();
						findAndReplace.Owner = this._mainWindow;
						this._findAndReplace = findAndReplace;
					}
					this._findAndReplace.DataContext = viewModel;
					this._findAndReplace.Show();
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, findAndReplace, this, viewModel);
				throw;
			}
		}

		public void ShowDisassembly()
		{
			Disassembler disassembler;
			Action action;
			bool flag;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					disassembler = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]).get_Debugger().get_Disassembler();
					flag = true;
					this._mainWindow.ShowCustomView(disassembler, flag);
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.ShowDisassembly();
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
				StackFrameHelper.CreateException4(exception1, disassembler, action, flag, this);
				throw;
			}
		}

		public void ShowDisplay(bool focus)
		{
			Action action;
			UiService.UiService variable = null;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					this._ui.ShowActivated = focus;
					this._ui.Show();
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.ShowDisplay(focus);
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
				StackFrameHelper.CreateException4(exception1, action, variable, this, focus);
				throw;
			}
		}

		public void ShowDocumentationWindow(string title, string data)
		{
			DocumentationViewModel documentationViewModel;
			DocumentationWindow documentationWindow;
			DocumentationWindow documentationWindow1;
			Action action;
			UiService.UiService variable = null;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					documentationViewModel = new DocumentationViewModel(title, data);
					documentationWindow1 = new DocumentationWindow();
					documentationWindow1.DataContext = documentationViewModel;
					documentationWindow1.Owner = this._mainWindow;
					documentationWindow = documentationWindow1;
					documentationWindow.Show();
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.ShowDocumentationWindow(title, data);
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
				StackFrameHelper.CreateException8(exception1, documentationViewModel, documentationWindow, documentationWindow1, action, variable, this, title, data);
				throw;
			}
		}

		public void ShowError(string message)
		{
			Action action;
			UiService.UiService variable = null;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					MessageBox.Show(this._mainWindow, message, "Error", MessageBoxButton.OK, MessageBoxImage.Hand);
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.ShowError(message);
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
				StackFrameHelper.CreateException4(exception1, action, variable, this, message);
				throw;
			}
		}

		public void ShowMemoryView()
		{
			MemoryView memoryView;
			bool flag;
			try
			{
				if (Application.Current.Dispatcher.CheckAccess())
				{
					memoryView = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]).get_Debugger().get_MemoryView();
					flag = true;
					this._mainWindow.ShowCustomView(memoryView, flag);
				}
				else
				{
					Application.Current.Dispatcher.Invoke(new Action(this.ShowMemoryView), new object[0]);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, memoryView, flag, this);
				throw;
			}
		}

		public void ShowMessage(string message)
		{
			Action action;
			UiService.UiService variable = null;
			try
			{
				action = null;
				if (Application.Current.Dispatcher.CheckAccess())
				{
					MessageBox.Show(this._mainWindow, message, "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk);
				}
				else
				{
					Dispatcher dispatcher = Application.Current.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.ShowMessage(message);
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
				StackFrameHelper.CreateException4(exception1, action, variable, this, message);
				throw;
			}
		}

		public void ShowStackView()
		{
			StackView stackView;
			bool flag;
			try
			{
				if (Application.Current.Dispatcher.CheckAccess())
				{
					stackView = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]).get_Debugger().get_StackView();
					flag = true;
					this._mainWindow.ShowCustomView(stackView, flag);
				}
				else
				{
					Application.Current.Dispatcher.Invoke(new Action(this.ShowStackView), new object[0]);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, stackView, flag, this);
				throw;
			}
		}
	}
}