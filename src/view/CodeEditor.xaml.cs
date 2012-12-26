using Devkit.IDE.Converters;
using Devkit.Interfaces.Build;
using Devkit.Workspace.ViewModel;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Devkit.IDE.View
{
	public class CodeEditor : UserControl, IComponentConnector
	{
		private OpenFile _openFile;

		private bool _loading;

		private CodeEditorStrategy _strategy;

		internal TextEditor editor;

		private bool _contentLoaded;

		public CodeEditor(CodeEditorStrategy strategy)
		{
			try
			{
				this.InitializeComponent();
				base.DataContextChanged += new DependencyPropertyChangedEventHandler(this.CodeEditorDataContextChanged);
				base.Loaded += new RoutedEventHandler(this.SetFocusWhenLoaded);
				this.editor.get_TextArea().get_Caret().add_PositionChanged(new EventHandler(this.CaretPositionChanged));
				this.editor.get_TextArea().add_SelectionChanged(new EventHandler(this.TextAreaSelectionChanged));
				this.editor.get_Document().add_TextChanged(new EventHandler(this.DocumentTextChanged));
				this.editor.set_SyntaxHighlighting(new StrategyHighlighter(strategy));
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, strategy);
				throw;
			}
		}

		public CodeEditor() : this(null)
		{
			try
			{
				this._loading = true;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
		}

		private void CaretPositionChanged(object sender, EventArgs e)
		{
			try
			{
				if (this._openFile != null && !this._loading)
				{
					this._openFile.set_CurrentLine(this.editor.get_TextArea().get_Caret().get_Line());
					this._openFile.set_CurrentOffset(this.editor.get_TextArea().get_Caret().get_Offset());
					if (!this.editor.get_TextArea().get_Selection().get_Segments().Any<ISegment>())
					{
						this._openFile.set_Selection(new Tuple<int, int>(this.editor.get_TextArea().get_Caret().get_Offset(), 0));
					}
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}

		private void CodeEditorDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			bool flag;
			OpenFile oldValue;
			Action action;
			try
			{
				action = null;
				flag = this._loading;
				this._loading = true;
				if (e.OldValue != null)
				{
					oldValue = (OpenFile)e.OldValue;
					oldValue.remove_ReplaceSelectionRequested(new OpenFile.ReplaceSelectionRequestedHandler(this, FileReplaceSelectionRequested));
					oldValue.remove_PropertyChanged(new PropertyChangedEventHandler(this.FilePropertyChanged));
					oldValue.get_DecorationInfo().remove_PropertyChanged(new PropertyChangedEventHandler(this.DecorationInfoPropertyChanged));
					this.editor.get_TextArea().get_TextView().get_BackgroundRenderers().Clear();
				}
				if (e.NewValue != null)
				{
					this._openFile = (OpenFile)e.NewValue;
					this._openFile.add_ReplaceSelectionRequested(new OpenFile.ReplaceSelectionRequestedHandler(this, FileReplaceSelectionRequested));
					this._openFile.add_PropertyChanged(new PropertyChangedEventHandler(this.FilePropertyChanged));
					this._openFile.get_DecorationInfo().add_PropertyChanged(new PropertyChangedEventHandler(this.DecorationInfoPropertyChanged));
					this.editor.set_Text(this._openFile.get_Content());
					this.editor.get_TextArea().get_TextView().get_BackgroundRenderers().Add(new CodeBackgroundRenderer(this._openFile.get_DecorationInfo()));
					Dispatcher dispatcher = base.Dispatcher;
					int num = 9;
					if (action == null)
					{
						action = () => {
							try
							{
								this.ScrollToAndPosition(this._openFile.get_CurrentLine());
							}
							catch (Exception exception)
							{
								StackFrameHelper.CreateException1(exception, this);
								throw;
							}
						}
						;
					}
					dispatcher.BeginInvoke((DispatcherPriority)num, action);
				}
				this._loading = flag;
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException6(exception1, flag, oldValue, action, this, sender, e);
				throw;
			}
		}

		private void DecorationInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Action action;
			CodeEditor.CodeEditor variable = null;
			try
			{
				action = null;
				if (base.Dispatcher.CheckAccess())
				{
					this.editor.get_TextArea().get_TextView().InvalidateLayer(0);
				}
				else
				{
					Dispatcher dispatcher = base.Dispatcher;
					if (action == null)
					{
						action = () => {
							try
							{
								this.DecorationInfoPropertyChanged(sender, e);
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
				StackFrameHelper.CreateException5(exception1, action, variable, this, sender, e);
				throw;
			}
		}

		private void DocumentTextChanged(object sender, EventArgs e)
		{
			try
			{
				if (this._openFile != null && !this._loading)
				{
					this._openFile.set_Content(this.editor.get_Document().get_Text());
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}

		private void FilePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			int currentOffset;
			int currentLine;
			Tuple<int, int> selection;
			int item1;
			int item2;
			Action action;
			CodeEditor.CodeEditor variable = null;
			string str;
			try
			{
				action = null;
				if (base.Dispatcher.CheckAccess())
				{
					if (this._openFile != null)
					{
						string propertyName = e.PropertyName;
						str = propertyName;
						if (propertyName != null)
						{
							if (str == "Content")
							{
								if (this._openFile.get_Content() != this.editor.get_Document().get_Text())
								{
									this.editor.get_Document().set_Text(this._openFile.get_Content());
									return;
								}
							}
							else
							{
								if (str == "CurrentOffset")
								{
									currentOffset = this._openFile.get_CurrentOffset();
									if (this.editor.get_TextArea().get_Caret().get_Offset() != currentOffset)
									{
										this.editor.get_TextArea().get_Caret().set_Offset(currentOffset);
										this.editor.ScrollToLine(this.editor.get_TextArea().get_Document().GetLineByOffset(currentOffset).get_LineNumber());
										this.editor.get_TextArea().Focus();
										return;
									}
								}
								else
								{
									if (str == "CurrentLine")
									{
										currentLine = this._openFile.get_CurrentLine();
										if (this.editor.get_TextArea().get_Caret().get_Line() != currentLine)
										{
											this.ScrollToAndPosition(currentLine);
											return;
										}
									}
									else
									{
										if (str == "Selection")
										{
											selection = this._openFile.get_Selection();
											item1 = selection.Item1;
											item2 = item1 + selection.Item2;
											this.editor.get_TextArea().set_Selection(new SimpleSelection(item1, item2));
											this.editor.get_TextArea().Focus();
										}
										else
										{
											return;
										}
									}
								}
							}
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
								this.FilePropertyChanged(sender, e);
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
				object[] objArray = new object[11];
				objArray[0] = currentOffset;
				objArray[1] = currentLine;
				objArray[2] = selection;
				objArray[3] = item1;
				objArray[4] = item2;
				objArray[5] = action;
				objArray[6] = variable;
				objArray[7] = str;
				objArray[8] = this;
				objArray[9] = sender;
				objArray[10] = e;
				StackFrameHelper.CreateExceptionN(exception1, objArray);
				throw;
			}
		}

		private void FileReplaceSelectionRequested(string replaceText)
		{
			try
			{
				this.editor.get_TextArea().get_Selection().ReplaceSelectionWithText(this.editor.get_TextArea(), replaceText);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, replaceText);
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
					uri = new Uri("/Devkit.IDE;component/view/codeeditor.xaml", UriKind.Relative);
					Application.LoadComponent(this, uri);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, uri, this);
				throw;
			}
		}

		private void ScrollToAndPosition(int line)
		{
			try
			{
				this.editor.get_TextArea().get_Caret().set_Line(line);
				this.editor.ScrollToLine(line);
				this.editor.get_TextArea().Focus();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, line);
				throw;
			}
		}

		private void SetFocusWhenLoaded(object sender, RoutedEventArgs e)
		{
			try
			{
				this.editor.get_TextArea().Focus();
				this._loading = false;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
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
					this.editor = (TextEditor)target;
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, num, this, connectionId, target);
				throw;
			}
		}

		private void TextAreaSelectionChanged(object sender, EventArgs e)
		{
			SimpleSelection simpleSelection;
			try
			{
				if (this._openFile != null && !this._loading)
				{
					simpleSelection = this.editor.get_TextArea().get_Selection().get_Segments().FirstOrDefault<ISegment>() as SimpleSelection;
					if (simpleSelection != null)
					{
						this._openFile.set_Selection(new Tuple<int, int>(simpleSelection.get_StartOffset(), simpleSelection.get_EndOffset() - simpleSelection.get_StartOffset()));
					}
					else
					{
						this._openFile.set_Selection(new Tuple<int, int>(this.editor.get_TextArea().get_Caret().get_Offset(), 0));
					}
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, simpleSelection, this, sender, e);
				throw;
			}
		}
	}
}