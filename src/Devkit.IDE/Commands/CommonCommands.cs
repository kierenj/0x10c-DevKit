using Devkit.Workspace;
using Devkit.Workspace.Commands;
using Devkit.Workspace.Services;
using Ninject;
using Ninject.Parameters;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.Windows.Input;

namespace Devkit.IDE.Commands
{
	public class CommonCommands
	{
		public static RelayCommand Copy
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					relayCommand = new RelayCommand("Copy", ApplicationCommands.Copy);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(23));
					relayCommand.set_ToolTip("Copy (Ctrl+C)");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, relayCommand);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand Cut
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					relayCommand = new RelayCommand("Cut", ApplicationCommands.Cut);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(22));
					relayCommand.set_ToolTip("Cut (Ctrl+X)");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, relayCommand);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand Paste
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					relayCommand = new RelayCommand("Paste", ApplicationCommands.Paste);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(24));
					relayCommand.set_ToolTip("Paste (Ctrl+V)");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, relayCommand);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand Redo
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					relayCommand = new RelayCommand("Redo", ApplicationCommands.Redo);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(26));
					relayCommand.set_ToolTip("Redo (Ctrl+Y)");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, relayCommand);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand Undo
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					relayCommand = new RelayCommand("Undo", ApplicationCommands.Undo);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(25));
					relayCommand.set_ToolTip("Undo (Ctrl+Z)");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, relayCommand);
					throw;
				}
				return relayCommand1;
			}
		}

		public CommonCommands()
		{
		}
	}
}