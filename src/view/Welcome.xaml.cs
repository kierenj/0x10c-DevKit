using SmartAssembly.SmartExceptionsCore;
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace Devkit.IDE.View
{
	public class Welcome : Window, IComponentConnector
	{
		private bool closeStoryBoardCompleted;

		private Storyboard closeStoryBoard;

		internal TextBlock message;

		internal TextBox txtTerms;

		private bool _contentLoaded;

		public Welcome()
		{
			Storyboard storyboard;
			DoubleAnimation doubleAnimation;
			try
			{
				this.InitializeComponent();
				this.closeStoryBoard = (Storyboard)base.FindResource("closeStoryBoard");
				storyboard = new Storyboard();
				doubleAnimation = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromSeconds(1)));
				Storyboard.SetTarget(doubleAnimation, this);
				Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(UIElement.OpacityProperty));
				storyboard.Children.Add(doubleAnimation);
				storyboard.Begin();
				this.txtTerms.Text = "©Copyright 2012 Team Chicken.\r\nDo not use this software if you obtained it from any other than the official 0x10c-devkit.com site.  If you downloaded it elsewhere, DELETE IT NOW and please get in contact via the site: safety is not guaranteed if you continue.\r\n\r\nBy using this software, you agree to the following terms and conditions:\r\n\r\n1. You agree not to reverse-engineer or modify the software.\r\n2. You agree not to redistribute, resell or repackage the software.\r\n3. You agree not to use the software for malicious purposes.\r\n\r\nDeveloped by Kieren Johnstone and David Price (Team Chicken).  Dedicated to Kat and Lex!\r\n\r\nThanks to:\r\n\r\n- Jon (https://github.com/twistedtwig)\r\n- notch (http://mojang.com/notch/)\r\n- The reddit community (http://reddit.com/r/0x10c and /r/dcpu16)\r\n- Red River Software (http://red-river-software.co.uk/)\r\n- Daniel Keep (daniel.keep@gmail.com) for BIEF reference code\r\n\r\n==============================================\r\nUses the AvalonDock component, license follows:\r\n==============================================\r\nCopyright (c) 2007-2012, Adolfo Marinucci\r\nAll rights reserved.\r\n\r\nRedistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:\r\n\r\n* Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.\r\n\r\n* Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.\r\n\r\n* Neither the name of Adolfo Marinucci nor the names of its contributors may be used to endorse or promote products derived from this software without specific prior written permission.\r\n\r\nTHIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS \"AS IS\" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.\r\n==============================================\r\n\r\nhttp://0x10c-devkit.com/\r\n\r\n";
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, storyboard, doubleAnimation, this);
				throw;
			}
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				base.Close();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}

		private void closeStoryBoard_Completed(object sender, EventArgs e)
		{
			try
			{
				this.closeStoryBoardCompleted = true;
				base.Close();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}

		private void Hyperlink_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Process.Start(((Hyperlink)sender).Tag.ToString());
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
					uri = new Uri("/Devkit.IDE;component/view/welcome.xaml", UriKind.Relative);
					Application.LoadComponent(this, uri);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, uri, this);
				throw;
			}
		}

		public void ShowMessage(string message)
		{
			Action action;
			Welcome.Welcome variable = null;
			try
			{
				action = null;
				if (base.Dispatcher.CheckAccess())
				{
					this.message.Text = message;
				}
				else
				{
					Dispatcher dispatcher = base.Dispatcher;
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
						(Welcome)target.MouseDown += new MouseButtonEventHandler(this.Window_MouseDown);
						(Welcome)target.Closing += new CancelEventHandler(this.Window_Closing);
						return;
					}
					case 2:
					{
						(Storyboard)target.Completed += new EventHandler(this.closeStoryBoard_Completed);
						return;
					}
					case 3:
					{
						this.message = (TextBlock)target;
						return;
					}
					case 4:
					{
						(Hyperlink)target.Click += new RoutedEventHandler(this.Hyperlink_Click);
						return;
					}
					case 5:
					{
						this.txtTerms = (TextBox)target;
						return;
					}
					case 6:
					{
						(Button)target.Click += new RoutedEventHandler(this.Button_Click);
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

		private void Window_Closing(object sender, CancelEventArgs e)
		{
			try
			{
				if (!this.closeStoryBoardCompleted)
				{
					this.closeStoryBoard.Begin(this);
					e.Cancel = true;
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			try
			{
				base.DragMove();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}
	}
}