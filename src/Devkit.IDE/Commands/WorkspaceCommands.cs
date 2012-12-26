using Devkit.Workspace;
using Devkit.Workspace.Commands;
using Devkit.Workspace.Services;
using Devkit.Workspace.ViewModel;
using Ninject;
using Ninject.Parameters;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Devkit.IDE.Commands
{
	public static class WorkspaceCommands
	{
		public static RelayCommand AddExistingFile
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Add Existing File", (object param) => {
						Project project;
						try
						{
							Project project1 = param as Project;
							Project currentProject = project1;
							if (project1 == null)
							{
								currentProject = this.workspace.get_Solution().get_CurrentProject();
							}
							Project project = currentProject;
							project.AddExistingFile();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, project, this, param);
							throw;
						}
					}
					, (object param) => {
						bool flag;
						try
						{
							if (workspace.get_Solution() == null)
							{
								bool flag = false;
							}
							else
							{
								Project project = param as Project;
								object currentProject = project;
								if (project == null)
								{
									currentProject = workspace.get_Solution().get_CurrentProject();
								}
								flag = currentProject != null;
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return flag;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(14));
					relayCommand.set_ToolTip("Add Existing File");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand AddNewFile
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Add File", (object param) => {
						Project project;
						try
						{
							Project project1 = param as Project;
							Project currentProject = project1;
							if (project1 == null)
							{
								currentProject = this.workspace.get_Solution().get_CurrentProject();
							}
							Project project = currentProject;
							project.AddNewFile();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, project, this, param);
							throw;
						}
					}
					, (object param) => {
						bool flag;
						try
						{
							if (workspace.get_Solution() == null)
							{
								bool flag = false;
							}
							else
							{
								Project project = param as Project;
								object currentProject = project;
								if (project == null)
								{
									currentProject = workspace.get_Solution().get_CurrentProject();
								}
								flag = currentProject != null;
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return flag;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(14));
					relayCommand.set_ToolTip("Add New File");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand AddNewProject
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Add Project", (object param) => {
						try
						{
							this.workspace.get_Solution().AddNewProject();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool solution;
						try
						{
							bool solution = workspace.get_Solution() != null;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return solution;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(9));
					relayCommand.set_ToolTip("Add New Project");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand Break
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Break", (object param) => {
						try
						{
							this.workspace.get_RuntimeManager().Break();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool state;
						try
						{
							bool state = workspace.get_RuntimeManager().get_State() == 0;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return state;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(30));
					relayCommand.set_ToolTip("Break");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand BuildSolution
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Build Solution", (object param) => {
						try
						{
							this.workspace.BuildSolution();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool solution;
						try
						{
							bool solution = workspace.get_Solution() != null;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return solution;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(28));
					relayCommand.set_ToolTip("Build Solution (F6)");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand Close
		{
			get
			{
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Close", (object param) => {
						try
						{
							this.workspace.get_CurrentFile().Close();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool currentFile;
						try
						{
							bool currentFile = workspace.get_CurrentFile() != null;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return currentFile;
					}
					);
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException1(exception1, variable);
					throw;
				}
				return relayCommand;
			}
		}

		public static RelayCommand CloseSolution
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					string str = "Close Solution";
					Action<object> action = (object param) => {
						try
						{
							ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]).CloseSolution();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException1(exception, param);
							throw;
						}
					}
					;
					relayCommand = new RelayCommand(str, action, (object param) => {
						bool hasSolution;
						try
						{
							bool hasSolution = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]).get_HasSolution();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException1(exception, param);
							throw;
						}
						return hasSolution;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(7));
					relayCommand.set_ToolTip("Close Solution");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException1(exception1, relayCommand);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand CreateNewSolution
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					string str = "New Solution";
					Action<object> action = (object param) => {
						try
						{
							ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]).CreateNewSolution();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException1(exception, param);
							throw;
						}
					}
					;
					relayCommand = new RelayCommand(str, action, (object param) => {
						bool hasSolution;
						try
						{
							bool hasSolution = !ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]).get_HasSolution();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException1(exception, param);
							throw;
						}
						return hasSolution;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(2));
					relayCommand.set_LargeIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(3));
					relayCommand.set_ToolTip("New Solution");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException1(exception1, relayCommand);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand DeleteFile
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Delete", (object param) => {
						File file;
						try
						{
							File file1 = param as File;
							File selectedItem = file1;
							if (file1 == null)
							{
								selectedItem = this.workspace.get_SelectedItem() as File;
							}
							File file = selectedItem;
							if (file != null)
							{
								file.get_Project().DeleteFile(file);
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, file, this, param);
							throw;
						}
					}
					, (object param) => {
						File file;
						bool flag;
						try
						{
							File file1 = param as File;
							File selectedItem = file1;
							if (file1 == null)
							{
								selectedItem = workspace.get_SelectedItem() as File;
							}
							File file = selectedItem;
							bool flag = file != null;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, file, this, param);
							throw;
						}
						return flag;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(21));
					relayCommand.set_ToolTip("Delete");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand DeleteOrRemove
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Delete/Remove", (object param) => {
						try
						{
							if (this.workspace.get_SelectedSolutionFile() as File == null)
							{
								if (this.workspace.get_SelectedSolutionFile() is Project)
								{
									WorkspaceCommands.RemoveProject.Execute(null);
								}
							}
							else
							{
								WorkspaceCommands.DeleteFile.Execute(null);
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool flag;
						try
						{
							if (workspace.get_SelectedSolutionFile() as File == null)
							{
								if (workspace.get_SelectedSolutionFile() as Project == null)
								{
									bool flag = false;
								}
								else
								{
									flag = WorkspaceCommands.RemoveProject.CanExecute(null);
								}
							}
							else
							{
								flag = WorkspaceCommands.DeleteFile.CanExecute(null);
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return flag;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(21));
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static IEnumerable<RelayCommand> FileCommands
		{
			get
			{
				WorkspaceCommands.u003cget_FileCommandsu003ed__3a variable;
				IEnumerable<RelayCommand> relayCommands;
				try
				{
					variable = new WorkspaceCommands.u003cget_FileCommandsu003ed__3a(-2);
					relayCommands = variable;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, variable);
					throw;
				}
				return relayCommands;
			}
		}

		public static RelayCommand FindNext
		{
			get
			{
				Workspace workspace;
				RelayCommand findNextCommand;
				try
				{
					workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					findNextCommand = workspace.get_FindAndReplace().get_FindNextCommand();
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, workspace);
					throw;
				}
				return findNextCommand;
			}
		}

		public static RelayCommand OpenFile
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					string str = "Open File";
					Action<object> action = (object param) => {
					}
					;
					relayCommand = new RelayCommand(str, action, (object param) => {
						bool flag;
						try
						{
							bool flag = false;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException1(exception, param);
							throw;
						}
						return flag;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(16));
					relayCommand.set_ToolTip("Open File");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException1(exception1, relayCommand);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand OpenFolderInWindowsExplorer
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Open Folder in Windows Explorer", (object param) => {
						ISolutionFile solutionFile;
						try
						{
							ISolutionFile solutionFile1 = param as ISolutionFile;
							ISolutionFile selectedSolutionFile = solutionFile1;
							if (solutionFile1 == null)
							{
								selectedSolutionFile = this.workspace.get_SelectedSolutionFile();
							}
							ISolutionFile solutionFile = selectedSolutionFile;
							if (solutionFile != null)
							{
								Process.Start(Path.GetDirectoryName(solutionFile.get_AbsolutePath()));
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, solutionFile, this, param);
							throw;
						}
					}
					, (object param) => {
						ISolutionFile solutionFile;
						bool flag;
						try
						{
							ISolutionFile solutionFile1 = param as ISolutionFile;
							ISolutionFile selectedSolutionFile = solutionFile1;
							if (solutionFile1 == null)
							{
								selectedSolutionFile = workspace.get_SelectedSolutionFile();
							}
							ISolutionFile solutionFile = selectedSolutionFile;
							bool flag = solutionFile != null;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, solutionFile, this, param);
							throw;
						}
						return flag;
					}
					);
					relayCommand.set_ToolTip("Open Folder in Windows Explorer");
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(27));
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand OpenSolution
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					string str = "Open Solution";
					Action<object> action = (object param) => {
						try
						{
							ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]).OpenSolution(string.Empty);
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException1(exception, param);
							throw;
						}
					}
					;
					relayCommand = new RelayCommand(str, action, (object param) => {
						bool hasSolution;
						try
						{
							bool hasSolution = !ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]).get_HasSolution();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException1(exception, param);
							throw;
						}
						return hasSolution;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(4));
					relayCommand.set_LargeIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(5));
					relayCommand.set_ToolTip("Open Solution");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException1(exception1, relayCommand);
					throw;
				}
				return relayCommand1;
			}
		}

		public static IEnumerable<RelayCommand> ProjectCommands
		{
			get
			{
				WorkspaceCommands.u003cget_ProjectCommandsu003ed__24 variable;
				IEnumerable<RelayCommand> relayCommands;
				try
				{
					variable = new WorkspaceCommands.u003cget_ProjectCommandsu003ed__24(-2);
					relayCommands = variable;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, variable);
					throw;
				}
				return relayCommands;
			}
		}

		public static RelayCommand ProjectProperties
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Properties", (object param) => {
						try
						{
							workspace.get_Solution().get_CurrentProject().OpenProperties();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					);
					relayCommand.set_ToolTip("Properties");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static IEnumerable<RelayCommand> ReferenceCommands
		{
			get
			{
				WorkspaceCommands.u003cget_ReferenceCommandsu003ed__3 variable;
				IEnumerable<RelayCommand> relayCommands;
				try
				{
					variable = new WorkspaceCommands.u003cget_ReferenceCommandsu003ed__3(-2);
					relayCommands = variable;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, variable);
					throw;
				}
				return relayCommands;
			}
		}

		public static RelayCommand RemoveFile
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Exclude From Project", (object param) => {
						File file;
						try
						{
							File file1 = param as File;
							File selectedItem = file1;
							if (file1 == null)
							{
								selectedItem = this.workspace.get_SelectedItem() as File;
							}
							File file = selectedItem;
							if (file != null)
							{
								file.get_Project().RemoveFile(file);
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, file, this, param);
							throw;
						}
					}
					, (object param) => {
						File file;
						bool flag;
						try
						{
							File file1 = param as File;
							File selectedItem = file1;
							if (file1 == null)
							{
								selectedItem = workspace.get_SelectedItem() as File;
							}
							File file = selectedItem;
							bool flag = file != null;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, file, this, param);
							throw;
						}
						return flag;
					}
					);
					relayCommand.set_ToolTip("Exclude From Project");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand RemoveProject
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Remove", (object param) => {
						Project project;
						try
						{
							Project project1 = param as Project;
							Project currentProject = project1;
							if (project1 == null)
							{
								currentProject = this.workspace.get_Solution().get_CurrentProject();
							}
							Project project = currentProject;
							if (project != null)
							{
								project.get_Solution().RemoveProject(project);
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, project, this, param);
							throw;
						}
					}
					, (object param) => {
						Project project;
						bool flag;
						try
						{
							Project project1 = param as Project;
							Project currentProject = project1;
							if (project1 == null)
							{
								currentProject = workspace.get_Solution().get_CurrentProject();
							}
							Project project = currentProject;
							bool flag = project != null;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, project, this, param);
							throw;
						}
						return flag;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(21));
					relayCommand.set_ToolTip("Remove Project");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand RemoveReference
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					string str = "Remove";
					relayCommand = new RelayCommand(str, (object param) => {
						Reference reference;
						Project parent;
						try
						{
							Reference reference = param as Reference;
							if (reference != null)
							{
								Project parent = reference.get_Parent() as Project;
								parent.RemoveReference(reference);
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, reference, parent, param);
							throw;
						}
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(21));
					relayCommand.set_ToolTip("Remove");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException1(exception1, relayCommand);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand Run
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Run", (object param) => {
						try
						{
							this.workspace.Run();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool state;
						try
						{
							if (workspace.get_Solution() == null)
							{
								bool state = false;
							}
							else
							{
								state = workspace.get_RuntimeManager().get_State() != 0;
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return state;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(29));
					relayCommand.set_ToolTip("Run");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand Save
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Save", (object param) => {
						ISolutionFile solutionFile;
						try
						{
							ISolutionFile solutionFile1 = param as ISolutionFile;
							ISolutionFile selectedSolutionFile = solutionFile1;
							if (solutionFile1 == null)
							{
								selectedSolutionFile = this.workspace.get_SelectedSolutionFile();
							}
							ISolutionFile solutionFile = selectedSolutionFile;
							solutionFile.Save();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, solutionFile, this, param);
							throw;
						}
					}
					, (object param) => {
						ISolutionFile solutionFile;
						bool isDirty;
						try
						{
							ISolutionFile solutionFile1 = param as ISolutionFile;
							ISolutionFile selectedSolutionFile = solutionFile1;
							if (solutionFile1 == null)
							{
								selectedSolutionFile = workspace.get_SelectedSolutionFile();
							}
							ISolutionFile solutionFile = selectedSolutionFile;
							if (solutionFile == null)
							{
								bool isDirty = false;
							}
							else
							{
								isDirty = solutionFile.get_IsDirty();
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, solutionFile, this, param);
							throw;
						}
						return isDirty;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(19));
					relayCommand.set_ToolTip("Save");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand SaveAll
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Save All", (object param) => {
						try
						{
							this.workspace.SaveAll();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool hasDirtyFiles;
						try
						{
							bool hasDirtyFiles = workspace.get_HasDirtyFiles();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return hasDirtyFiles;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(20));
					relayCommand.set_ToolTip("Save All (Ctrl+Shift+S)");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand SaveAllProjectFiles
		{
			get
			{
				RelayCommand relayCommand;
				RelayCommand relayCommand1;
				try
				{
					string str = "Save All";
					Action<object> action = (object param) => {
						try
						{
							(param as Project).SaveAll();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException1(exception, param);
							throw;
						}
					}
					;
					relayCommand = new RelayCommand(str, action, (object param) => {
						bool isDirty;
						try
						{
							bool isDirty = (param as Project).get_IsDirty();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException1(exception, param);
							throw;
						}
						return isDirty;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(20));
					relayCommand.set_ToolTip("Save All (Ctrl+Shift+S)");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException1(exception1, relayCommand);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand SaveAs
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Save As", (object param) => {
						try
						{
							this.workspace.get_CurrentFile().SaveAs();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool currentFile;
						try
						{
							bool currentFile = workspace.get_CurrentFile() != null;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return currentFile;
					}
					);
					relayCommand.set_ToolTip("Save As");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand SaveProject
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Save Project", (object param) => {
						Project project;
						try
						{
							Project project1 = param as Project;
							Project currentProject = project1;
							if (project1 == null)
							{
								currentProject = this.workspace.get_Solution().get_CurrentProject();
							}
							Project project = currentProject;
							project.Save();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, project, this, param);
							throw;
						}
					}
					, (object param) => {
						Project project;
						bool isDirty;
						try
						{
							Project project1 = param as Project;
							Project currentProject = project1;
							if (project1 == null)
							{
								currentProject = workspace.get_Solution().get_CurrentProject();
							}
							Project project = currentProject;
							if (project == null)
							{
								bool isDirty = false;
							}
							else
							{
								isDirty = project.get_IsDirty();
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, project, this, param);
							throw;
						}
						return isDirty;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(19));
					relayCommand.set_ToolTip("Save Project");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand SaveSolution
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Save Solution", (object param) => {
						try
						{
							this.workspace.get_Solution().Save();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool isDirty;
						try
						{
							if (workspace.get_Solution() == null)
							{
								bool isDirty = false;
							}
							else
							{
								isDirty = workspace.get_Solution().get_IsDirty();
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return isDirty;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(19));
					relayCommand.set_ToolTip("Save Solution");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand ShowDisassembly
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Show Disassembly", (object param) => {
						try
						{
							workspace.ShowDisassembly();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					);
					relayCommand.set_ToolTip("Show Disassembly");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand ShowFindAndReplace
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Find and Replace...", (object param) => {
						try
						{
							workspace.ShowFindAndReplace();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					);
					relayCommand.set_ToolTip("Find and Replace");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand ShowMemoryView
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Show Memory View", (object param) => {
						try
						{
							workspace.ShowMemoryView();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					);
					relayCommand.set_ToolTip("Show Memory View");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand ShowNextStatement
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Show Next Statement", (object param) => {
						try
						{
							this.workspace.get_Debugger().ShowNextStatement();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool state;
						try
						{
							bool state = workspace.get_RuntimeManager().get_State() != 3;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return state;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(33));
					relayCommand.set_ToolTip("Show Next Statement");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand ShowStackView
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Show Stack View", (object param) => {
						try
						{
							workspace.ShowStackView();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					);
					relayCommand.set_ToolTip("Show Stack View");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static IEnumerable<RelayCommand> SolutionCommands
		{
			get
			{
				WorkspaceCommands.u003cget_SolutionCommandsu003ed__9 variable;
				IEnumerable<RelayCommand> relayCommands;
				try
				{
					variable = new WorkspaceCommands.u003cget_SolutionCommandsu003ed__9(-2);
					relayCommands = variable;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, variable);
					throw;
				}
				return relayCommands;
			}
		}

		public static RelayCommand SolutionProperties
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Properties", (object param) => {
						try
						{
							workspace.get_Solution().OpenProperties();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					);
					relayCommand.set_ToolTip("Properties");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static IEnumerable<RelayCommand> StartPageCommands
		{
			get
			{
				WorkspaceCommands.u003cget_StartPageCommandsu003ed__0 variable;
				IEnumerable<RelayCommand> relayCommands;
				try
				{
					variable = new WorkspaceCommands.u003cget_StartPageCommandsu003ed__0(-2);
					relayCommands = variable;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, variable);
					throw;
				}
				return relayCommands;
			}
		}

		public static RelayCommand Step
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Step", (object param) => {
						try
						{
							this.workspace.Step();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool state;
						try
						{
							if (workspace.get_Solution() == null)
							{
								bool state = false;
							}
							else
							{
								state = workspace.get_RuntimeManager().get_State() != 0;
							}
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return state;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(32));
					relayCommand.set_ToolTip("Step");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand Stop
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Stop", (object param) => {
						try
						{
							this.workspace.get_RuntimeManager().Stop();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool state;
						try
						{
							bool state = workspace.get_RuntimeManager().get_State() != 3;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return state;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(31));
					relayCommand.set_ToolTip("Stop");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

		public static RelayCommand ToggleBreakpoint
		{
			get
			{
				RelayCommand relayCommand;
				WorkspaceCommands.WorkspaceCommands variable = null;
				RelayCommand relayCommand1;
				try
				{
					Workspace workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
					relayCommand = new RelayCommand("Toggle Breakpoint", (object param) => {
						try
						{
							this.workspace.get_Debugger().ToggleBreakpoint(this.workspace.get_CurrentFile());
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
					}
					, (object param) => {
						bool currentFile;
						try
						{
							bool currentFile = workspace.get_CurrentFile() != null;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, this, param);
							throw;
						}
						return currentFile;
					}
					);
					relayCommand.set_SmallIcon(ResolutionExtensions.Get<IUiService>(App.Kernel, new IParameter[0]).GetImage(34));
					relayCommand.set_ToolTip("Toggle Breakpoint (F9)");
					relayCommand1 = relayCommand;
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException2(exception1, relayCommand, variable);
					throw;
				}
				return relayCommand1;
			}
		}

	}
}