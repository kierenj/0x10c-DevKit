using Devkit.Assembler;
using Devkit.IDE.Converters;
using Devkit.Workspace.ViewModel.Debugger;
using ICSharpCode.AvalonEdit;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Threading;

namespace Devkit.IDE.View
{
	public class Disassembly : UserControl, IComponentConnector
	{
		private readonly OffsetMargin _offsetMargin;

		private Disassembler _disasm;

		internal TextEditor editor;

		private bool _contentLoaded;

		public Disassembly()
		{
			try
			{
				this.InitializeComponent();
				base.DataContextChanged += new DependencyPropertyChangedEventHandler(this.DisassemblyDataContextChanged);
				this._offsetMargin = new OffsetMargin();
				this.editor.get_TextArea().get_LeftMargins().Add(this._offsetMargin);
				this.editor.set_SyntaxHighlighting(new StrategyHighlighter(DevKit10cProvider.get_EditorStrategy()));
				this._disasm = base.DataContext as Disassembler;
				this.BindDisasm();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
		}

		private void BindDisasm()
		{
			try
			{
				if (this._disasm != null)
				{
					this._disasm.add_PropertyChanged(new PropertyChangedEventHandler(this.DisasmPropertyChanged));
					this._disasm.get_DecorationInfo().add_PropertyChanged(new PropertyChangedEventHandler(this.DecorationInfoPropertyChanged));
					this.editor.set_Text(this._disasm.get_CurrentText());
					OffsetMargin offsetMargin = this._offsetMargin;
					int[] currentOffsets = this._disasm.get_CurrentOffsets();
					offsetMargin.OffsetStrings = (IEnumerable<int>)currentOffsets.Select<int, string>((int o) => {
						string str;
						try
						{
							string str = string.Format("{0:X4}", o);
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException1(exception, o);
							throw;
						}
						return str;
					}
					);
					this.editor.get_TextArea().get_TextView().get_BackgroundRenderers().Add(new CodeBackgroundRenderer(this._disasm.get_DecorationInfo()));
				}
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException1(exception1, this);
				throw;
			}
		}

		private void DecorationInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Action action;
			Disassembly.Disassembly variable = null;
			string str;
			try
			{
				action = null;
				if (base.Dispatcher.CheckAccess())
				{
					string propertyName = e.PropertyName;
					str = propertyName;
					if (propertyName != null && str == "CurrentLineNumbers")
					{
						if (this._disasm.get_DecorationInfo().get_CurrentLineNumbers() != null)
						{
							if (this._disasm.get_DecorationInfo().get_CurrentLineNumbers().Any<int>())
							{
								this.editor.ScrollToLine(this._disasm.get_DecorationInfo().get_CurrentLineNumbers().First<int>());
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
				StackFrameHelper.CreateException6(exception1, action, variable, str, this, sender, e);
				throw;
			}
		}

		private void DisasmPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Action action;
			Disassembly.Disassembly variable = null;
			string str1;
			try
			{
				action = null;
				if (base.Dispatcher.CheckAccess())
				{
					string propertyName = e.PropertyName;
					str1 = propertyName;
					if (propertyName != null)
					{
						if (str1 == "CurrentText")
						{
							this.editor.set_Text(this._disasm.get_CurrentText());
							return;
						}
						else
						{
							if (str1 == "CurrentOffsets")
							{
								OffsetMargin array = this._offsetMargin;
								int[] currentOffsets = this._disasm.get_CurrentOffsets();
								array.OffsetStrings = (IEnumerable<int>)currentOffsets.Select<int, string>((int o) => {
									string str;
									try
									{
										string str = string.Format("{0:X4}", o);
									}
									catch (Exception exception)
									{
										StackFrameHelper.CreateException1(exception, o);
										throw;
									}
									return str;
								}
								).ToArray<string>();
							}
							else
							{
								return;
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
								this.DisasmPropertyChanged(sender, e);
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
				StackFrameHelper.CreateException6(exception1, action, variable, str1, this, sender, e);
				throw;
			}
		}

		private void DisassemblyDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
		{
			try
			{
				if (e.NewValue != null)
				{
					this._disasm = base.DataContext as Disassembler;
					this.BindDisasm();
				}
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
					uri = new Uri("/Devkit.IDE;component/view/disassembly.xaml", UriKind.Relative);
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
					this.editor = (TextEditor)target;
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