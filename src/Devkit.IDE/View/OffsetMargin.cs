using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Editing;
using ICSharpCode.AvalonEdit.Rendering;
using ICSharpCode.AvalonEdit.Utils;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace Devkit.IDE.View
{
	public class OffsetMargin : AbstractMargin, IWeakEventListener
	{
		public static DependencyProperty OffsetStringsProperty;

		private TextArea textArea;

		private Typeface typeface;

		private double emSize;

		private int maxLineNumberLength;

		private AnchorSegment selectionStart;

		private bool selecting;

		public IEnumerable<string> OffsetStrings
		{
			get
			{
				IEnumerable<string> value;
				try
				{
					value = (IEnumerable<string>)base.GetValue(OffsetMargin.OffsetStringsProperty);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return value;
			}
			set
			{
				try
				{
					base.SetValue(OffsetMargin.OffsetStringsProperty, value);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		static OffsetMargin()
		{
			try
			{
				OffsetMargin.OffsetStringsProperty = DependencyProperty.Register("OffsetStrings", typeof(IEnumerable<string>), typeof(OffsetMargin), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender, new PropertyChangedCallback(OffsetMargin.OffsetStringsPropertyChangedCallback)));
				FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(OffsetMargin), new FrameworkPropertyMetadata(typeof(OffsetMargin)));
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException0(exception);
				throw;
			}
		}

		public OffsetMargin()
		{
			this.maxLineNumberLength = 1;
		}

		private void ExtendSelection(SimpleSegment currentSeg)
		{
			try
			{
				if (currentSeg.Offset >= this.selectionStart.get_Offset())
				{
					this.textArea.get_Caret().set_Offset(currentSeg.Offset + currentSeg.Length);
					this.textArea.set_Selection(new SimpleSelection(this.selectionStart.get_Offset(), currentSeg.Offset + currentSeg.Length));
				}
				else
				{
					this.textArea.get_Caret().set_Offset(currentSeg.Offset);
					this.textArea.set_Selection(new SimpleSelection(currentSeg.Offset, this.selectionStart.get_Offset() + this.selectionStart.get_Length()));
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, currentSeg);
				throw;
			}
		}

		private SimpleSegment GetTextLineSegment(MouseEventArgs e)
		{
			Point position;
			VisualLine visualLineFromVisualTop;
			TextLine textLineByVisualYPosition;
			int textLineVisualStartColumn;
			int length;
			int offset;
			int relativeOffset;
			int delimiterLength;
			SimpleSegment simpleSegment;
			try
			{
				position = e.GetPosition(base.get_TextView());
				position.X = 0;
				position.Y = position.Y + base.get_TextView().get_VerticalOffset();
				visualLineFromVisualTop = base.get_TextView().GetVisualLineFromVisualTop(position.Y);
				if (visualLineFromVisualTop != null)
				{
					textLineByVisualYPosition = visualLineFromVisualTop.GetTextLineByVisualYPosition(position.Y);
					textLineVisualStartColumn = visualLineFromVisualTop.GetTextLineVisualStartColumn(textLineByVisualYPosition);
					length = textLineVisualStartColumn + textLineByVisualYPosition.Length;
					offset = visualLineFromVisualTop.get_FirstDocumentLine().get_Offset();
					relativeOffset = visualLineFromVisualTop.GetRelativeOffset(textLineVisualStartColumn) + offset;
					delimiterLength = visualLineFromVisualTop.GetRelativeOffset(length) + offset;
					if (delimiterLength == visualLineFromVisualTop.get_LastDocumentLine().get_Offset() + visualLineFromVisualTop.get_LastDocumentLine().get_Length())
					{
						delimiterLength = delimiterLength + visualLineFromVisualTop.get_LastDocumentLine().get_DelimiterLength();
					}
					simpleSegment = new SimpleSegment(relativeOffset, delimiterLength - relativeOffset);
				}
				else
				{
					simpleSegment = SimpleSegment.Invalid;
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException10(exception, position, visualLineFromVisualTop, textLineByVisualYPosition, textLineVisualStartColumn, length, offset, relativeOffset, delimiterLength, this, e);
				throw;
			}
			return simpleSegment;
		}

		protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
		{
			HitTestResult pointHitTestResult;
			try
			{
				pointHitTestResult = new PointHitTestResult(this, hitTestParameters.HitPoint);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, hitTestParameters);
				throw;
			}
			return pointHitTestResult;
		}

		protected override Size MeasureOverride(Size availableSize)
		{
			FormattedText formattedText;
			Size size;
			try
			{
				this.typeface = ExtensionMethods.CreateTypeface(this);
				this.emSize = (double)base.GetValue(TextBlock.FontSizeProperty);
				formattedText = TextFormatterFactory.CreateFormattedText(this, "FFFFF", this.typeface, new double?(this.emSize), (Brush)base.GetValue(Control.ForegroundProperty));
				size = new Size(formattedText.Width, 0);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, formattedText, this, availableSize);
				throw;
			}
			return size;
		}

		private static void OffsetStringsPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
		{
		}

		protected override void OnDocumentChanged(TextDocument oldDocument, TextDocument newDocument)
		{
			try
			{
				if (oldDocument != null)
				{
					PropertyChangedEventManager.RemoveListener(oldDocument, this, "LineCount");
				}
				base.OnDocumentChanged(oldDocument, newDocument);
				if (newDocument != null)
				{
					PropertyChangedEventManager.AddListener(newDocument, this, "LineCount");
				}
				this.OnDocumentLineCountChanged();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, oldDocument, newDocument);
				throw;
			}
		}

		private void OnDocumentLineCountChanged()
		{
			int num;
			int length;
			int lineCount;
			try
			{
				if (base.get_Document() != null)
				{
					lineCount = base.get_Document().get_LineCount();
				}
				else
				{
					lineCount = 1;
				}
				num = lineCount;
				length = num.ToString(CultureInfo.CurrentCulture).Length;
				if (length < 2)
				{
					length = 2;
				}
				if (length != this.maxLineNumberLength)
				{
					this.maxLineNumberLength = length;
					base.InvalidateMeasure();
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, num, length, this);
				throw;
			}
		}

		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			SimpleSegment textLineSegment;
			SimpleSelection selection;
			try
			{
				base.OnMouseLeftButtonDown(e);
				if (!e.Handled && base.get_TextView() != null && this.textArea != null)
				{
					e.Handled = true;
					this.textArea.Focus();
					textLineSegment = this.GetTextLineSegment(e);
					if (textLineSegment != SimpleSegment.Invalid)
					{
						this.textArea.get_Caret().set_Offset(textLineSegment.Offset + textLineSegment.Length);
						if (base.CaptureMouse())
						{
							this.selecting = true;
							this.selectionStart = new AnchorSegment(base.get_Document(), textLineSegment.Offset, textLineSegment.Length);
							if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
							{
								selection = this.textArea.get_Selection() as SimpleSelection;
								if (selection != null)
								{
									this.selectionStart = new AnchorSegment(base.get_Document(), selection);
								}
							}
							this.textArea.set_Selection(new SimpleSelection(this.selectionStart));
							if ((Keyboard.Modifiers & ModifierKeys.Shift) == ModifierKeys.Shift)
							{
								this.ExtendSelection(textLineSegment);
							}
						}
					}
					else
					{
						return;
					}
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, textLineSegment, selection, this, e);
				throw;
			}
		}

		protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
		{
			try
			{
				if (this.selecting)
				{
					this.selecting = false;
					this.selectionStart = null;
					base.ReleaseMouseCapture();
					e.Handled = true;
				}
				base.OnMouseLeftButtonUp(e);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, e);
				throw;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			SimpleSegment textLineSegment;
			try
			{
				if (this.selecting && this.textArea != null && base.get_TextView() != null)
				{
					e.Handled = true;
					textLineSegment = this.GetTextLineSegment(e);
					if (textLineSegment != SimpleSegment.Invalid)
					{
						this.ExtendSelection(textLineSegment);
					}
					else
					{
						return;
					}
				}
				base.OnMouseMove(e);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, textLineSegment, this, e);
				throw;
			}
		}

		protected override void OnRender(DrawingContext drawingContext)
		{
			TextView textView;
			Brush value;
			VisualLine visualLine = null;
			int lineNumber;
			string str;
			FormattedText formattedText;
			IEnumerator<VisualLine> enumerator = null;
			string array;
			try
			{
				textView = base.get_TextView();
				base.RenderSize;
				if (this.OffsetStrings != null)
				{
					if (textView != null && textView.get_VisualLinesValid())
					{
						value = (Brush)base.GetValue(Control.ForegroundProperty);
						foreach (VisualLine visualLine in textView.get_VisualLines())
						{
							lineNumber = visualLine.get_FirstDocumentLine().get_LineNumber();
							if (lineNumber > this.OffsetStrings.Count<string>())
							{
								array = null;
							}
							else
							{
								array = this.OffsetStrings.ToArray<string>()[lineNumber - 1];
							}
							str = array;
							if (str == null)
							{
								continue;
							}
							formattedText = TextFormatterFactory.CreateFormattedText(this, str, this.typeface, new double?(this.emSize), value);
							drawingContext.DrawText(formattedText, new Point(0, visualLine.get_VisualTop() - textView.get_VerticalOffset()));
						}
					}
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException9(exception, textView, value, visualLine, lineNumber, str, formattedText, enumerator, this, drawingContext);
				throw;
			}
		}

		protected override void OnTextViewChanged(TextView oldTextView, TextView newTextView)
		{
			try
			{
				if (oldTextView != null)
				{
					oldTextView.remove_VisualLinesChanged(new EventHandler(this.TextViewVisualLinesChanged));
				}
				base.OnTextViewChanged(oldTextView, newTextView);
				if (newTextView == null)
				{
					this.textArea = null;
				}
				else
				{
					newTextView.add_VisualLinesChanged(new EventHandler(this.TextViewVisualLinesChanged));
					this.textArea = newTextView.get_Services().GetService(typeof(TextArea)) as TextArea;
				}
				base.InvalidateVisual();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, oldTextView, newTextView);
				throw;
			}
		}

		protected virtual bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			bool flag;
			try
			{
				if (managerType != typeof(PropertyChangedEventManager))
				{
					flag = false;
				}
				else
				{
					this.OnDocumentLineCountChanged();
					flag = true;
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, this, managerType, sender, e);
				throw;
			}
			return flag;
		}

		bool System.Windows.IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
		{
			bool flag;
			try
			{
				flag = this.ReceiveWeakEvent(managerType, sender, e);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, this, managerType, sender, e);
				throw;
			}
			return flag;
		}

		private void TextViewVisualLinesChanged(object sender, EventArgs e)
		{
			try
			{
				base.InvalidateVisual();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}
	}
}