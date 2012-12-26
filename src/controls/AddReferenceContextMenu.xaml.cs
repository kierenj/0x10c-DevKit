using Devkit.Interfaces.Build;
using Devkit.Workspace;
using Devkit.Workspace.Commands;
using Devkit.Workspace.ViewModel;
using Ninject;
using Ninject.Parameters;
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

namespace Devkit.IDE.Controls
{
	public class AddReferenceContextMenu : ContextMenu, IComponentConnector
	{
		private bool _contentLoaded;

		public AddReferenceContextMenu()
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
					uri = new Uri("/Devkit.IDE;component/controls/addreferencecontextmenu.xaml", UriKind.Relative);
					Application.LoadComponent(this, uri);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, uri, this);
				throw;
			}
		}

		protected override void OnOpened(RoutedEventArgs e)
		{
			Workspace workspace;
			IProjectTypeProvider projectTypeProvider;
			MenuItem menuItem;
			MenuItem menuItem1;
			Action<object> action;
			AddReferenceContextMenu.AddReferenceContextMenu variable = null;
			Func<ProjectReference, bool> func;
			AddReferenceContextMenu.AddReferenceContextMenu variable1 = null;
			MenuItem menuItem2;
			AddReferenceContextMenu.AddReferenceContextMenu variable2 = null;
			IEnumerator<Project> enumerator;
			try
			{
				base.OnOpened(e);
				workspace = ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]);
				base.Items.Clear();
				Project currentProject = workspace.get_Solution().get_CurrentProject();
				projectTypeProvider = workspace.get_BuildManager().GetProjectTypeProvider(currentProject.get_TypeCode());
				enumerator = workspace.get_Solution().get_Projects().Where<Project>((Project p) => {
					bool flag;
					try
					{
						bool flag = p != currentProject;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException2(exception, this, p);
						throw;
					}
					return flag;
				}
				).GetEnumerator();
				using (enumerator)
				{
					func = null;
					while (enumerator.MoveNext())
					{
						Project current = enumerator.Current;
						action = null;
						ReferencesCollection references = currentProject.get_References();
						IEnumerable<ProjectReference> projectReferences = references.Select<Reference, Reference>((Reference r) => {
							Reference innerObject;
							try
							{
								Reference innerObject = r.get_InnerObject();
							}
							catch (Exception exception)
							{
								StackFrameHelper.CreateException1(exception, r);
								throw;
							}
							return innerObject;
						}
						).OfType<ProjectReference>();
						if (func == null)
						{
							func = (ProjectReference pr) => {
								bool projectName;
								try
								{
									bool projectName = pr.ProjectName == current.get_Name();
								}
								catch (Exception exception)
								{
									StackFrameHelper.CreateException2(exception, this, pr);
									throw;
								}
								return projectName;
							}
							;
						}
						if (projectReferences.Any<ProjectReference>(func))
						{
							continue;
						}
						Project project = current;
						if (!projectTypeProvider.CanAddReference(currentProject, current))
						{
							menuItem1 = new MenuItem();
							menuItem1.Header = string.Concat("Cannot add reference to ", current.get_Name());
							menuItem1.IsEnabled = false;
							base.Items.Add(menuItem1);
						}
						else
						{
							ItemCollection items = base.Items;
							menuItem = new MenuItem();
							menuItem.Header = string.Concat("Add reference to ", current.get_Name());
							MenuItem relayCommand = menuItem;
							string str = "Add reference";
							if (action == null)
							{
								action = (object param) => {
									try
									{
										currentProject.AddReference(project);
									}
									catch (Exception exception)
									{
										StackFrameHelper.CreateException2(exception, this, param);
										throw;
									}
								}
								;
							}
							relayCommand.Command = new RelayCommand(str, action);
							items.Add(menuItem);
						}
					}
				}
				if (base.Items.Count == 0)
				{
					menuItem2 = new MenuItem();
					menuItem2.Header = "No other projects to reference";
					menuItem2.IsEnabled = false;
					base.Items.Add(menuItem2);
				}
			}
			catch (Exception exception1)
			{
				object[] objArray = new object[13];
				objArray[0] = workspace;
				objArray[1] = projectTypeProvider;
				objArray[2] = menuItem;
				objArray[3] = menuItem1;
				objArray[4] = action;
				objArray[5] = variable;
				objArray[6] = func;
				objArray[7] = variable1;
				objArray[8] = menuItem2;
				objArray[9] = variable2;
				objArray[10] = enumerator;
				objArray[11] = this;
				objArray[12] = e;
				StackFrameHelper.CreateExceptionN(exception1, objArray);
				throw;
			}
		}

		[DebuggerNonUserCode]
		[EditorBrowsable(EditorBrowsableState.Never)]
		[GeneratedCode("PresentationBuildTasks", "4.0.0.0")]
		void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
		{
			try
			{
				this._contentLoaded = true;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, connectionId, target);
				throw;
			}
		}
	}
}