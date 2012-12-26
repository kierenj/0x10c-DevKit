using Devkit.Build;
using Devkit.Workspace;
using Devkit.Workspace.Commands;
using Devkit.Workspace.ViewModel;
using Microsoft.Win32;
using Ninject;
using Ninject.Parameters;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace Devkit.IDE.Controls
{
	public class RecentFileList : Separator
	{
		private EventHandler<RecentFileList.MenuClickEventArgs> MenuClick;

		private Separator _Separator;

		private List<RecentFileList.RecentFile> _RecentFiles;

		public string Category
		{
			get
			{
				string u003cCategoryu003ek_BackingField;
				try
				{
					u003cCategoryu003ek_BackingField = this.u003cCategoryu003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cCategoryu003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cCategoryu003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public MenuItem FileMenu
		{
			get
			{
				MenuItem u003cFileMenuu003ek_BackingField;
				try
				{
					u003cFileMenuu003ek_BackingField = this.u003cFileMenuu003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cFileMenuu003ek_BackingField;
			}
			private set
			{
				try
				{
					this.u003cFileMenuu003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public RecentFileList.GetMenuItemTextDelegate GetMenuItemTextHandler
		{
			get
			{
				RecentFileList.GetMenuItemTextDelegate u003cGetMenuItemTextHandleru003ek_BackingField;
				try
				{
					u003cGetMenuItemTextHandleru003ek_BackingField = this.u003cGetMenuItemTextHandleru003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cGetMenuItemTextHandleru003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cGetMenuItemTextHandleru003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public int MaxNumberOfFiles
		{
			get
			{
				int u003cMaxNumberOfFilesu003ek_BackingField;
				try
				{
					u003cMaxNumberOfFilesu003ek_BackingField = this.u003cMaxNumberOfFilesu003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cMaxNumberOfFilesu003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cMaxNumberOfFilesu003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public int MaxPathLength
		{
			get
			{
				int u003cMaxPathLengthu003ek_BackingField;
				try
				{
					u003cMaxPathLengthu003ek_BackingField = this.u003cMaxPathLengthu003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cMaxPathLengthu003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cMaxPathLengthu003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public string MenuItemFormatOneToNine
		{
			get
			{
				string u003cMenuItemFormatOneToNineu003ek_BackingField;
				try
				{
					u003cMenuItemFormatOneToNineu003ek_BackingField = this.u003cMenuItemFormatOneToNineu003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cMenuItemFormatOneToNineu003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cMenuItemFormatOneToNineu003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public string MenuItemFormatTenPlus
		{
			get
			{
				string u003cMenuItemFormatTenPlusu003ek_BackingField;
				try
				{
					u003cMenuItemFormatTenPlusu003ek_BackingField = this.u003cMenuItemFormatTenPlusu003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cMenuItemFormatTenPlusu003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cMenuItemFormatTenPlusu003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public RecentFileList.IPersist Persister
		{
			get
			{
				RecentFileList.IPersist u003cPersisteru003ek_BackingField;
				try
				{
					u003cPersisteru003ek_BackingField = this.u003cPersisteru003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cPersisteru003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cPersisteru003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public List<string> RecentFiles
		{
			get
			{
				List<string> strs;
				try
				{
					strs = this.Persister.RecentFiles(this.MaxNumberOfFiles, this.Category);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return strs;
			}
		}

		public static IEnumerable<RecentFileList.RecentFileItem> RecentSolutionItems
		{
			get
			{
				IEnumerable<RecentFileList.RecentFileItem> recentFileItems;
				try
				{
					List<string> strs = (new RecentFileList.RegistryPersister()).RecentFiles(9, "Solution");
					recentFileItems = strs.Select<string, RecentFileList.RecentFileItem>((string f) => {
						RecentFileList.RecentFileItem recentFileItem;
						RecentFileList.RecentFileItem recentFileItem1;
						try
						{
							RecentFileList.RecentFileItem recentFileItem = new RecentFileList.RecentFileItem();
							recentFileItem.Path = f;
							recentFileItem.Display = SolutionFile.GetDisplayName(f);
							RecentFileList.RecentFileItem recentFileItem1 = recentFileItem;
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException2(exception, recentFileItem, f);
							throw;
						}
						return recentFileItem1;
					}
					);
				}
				catch (Exception exception1)
				{
					StackFrameHelper.CreateException0(exception1);
					throw;
				}
				return recentFileItems;
			}
		}

		public RecentFileList()
		{
			RoutedEventHandler routedEventHandler = null;
			base();
			try
			{
				this.Persister = new RecentFileList.RegistryPersister();
				this.MaxNumberOfFiles = 9;
				this.MaxPathLength = 50;
				this.MenuItemFormatOneToNine = "_{0}:  {2}";
				this.MenuItemFormatTenPlus = "{0}:  {2}";
				RecentFileList recentFileList = this;
				if (routedEventHandler == null)
				{
					routedEventHandler = (object s, RoutedEventArgs e) => {
						try
						{
							this.HookFileMenu();
						}
						catch (Exception exception)
						{
							StackFrameHelper.CreateException3(exception, this, s, e);
							throw;
						}
					}
					;
				}
				recentFileList.Loaded += routedEventHandler;
			}
			catch (Exception exception1)
			{
				StackFrameHelper.CreateException2(exception1, routedEventHandler, this);
				throw;
			}
		}

		private void _FileMenu_SubmenuOpened(object sender, RoutedEventArgs e)
		{
			try
			{
				this.SetMenuItems();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, this, sender, e);
				throw;
			}
		}

		private string GetFilepath(MenuItem menuItem)
		{
			RecentFileList.RecentFile current;
			string filepath;
			List<RecentFileList.RecentFile>.Enumerator enumerator;
			string empty;
			try
			{
				enumerator = this._RecentFiles.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						current = enumerator.Current;
						if (current.MenuItem != menuItem)
						{
							continue;
						}
						filepath = current.Filepath;
						goto Label1;
					}
					empty = string.Empty;
					return empty;
				}
				finally
				{
					enumerator.Dispose();
				}
			Label1:
				empty = filepath;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException5(exception, current, filepath, enumerator, this, menuItem);
				throw;
			}
			return empty;
		}

		private string GetMenuItemText(int index, string filepath, string displaypath)
		{
			RecentFileList.GetMenuItemTextDelegate getMenuItemTextHandler;
			string str;
			string str1;
			string str2;
			string menuItemFormatOneToNine;
			try
			{
				getMenuItemTextHandler = this.GetMenuItemTextHandler;
				if (getMenuItemTextHandler == null)
				{
					if (index < 10)
					{
						menuItemFormatOneToNine = this.MenuItemFormatOneToNine;
					}
					else
					{
						menuItemFormatOneToNine = this.MenuItemFormatTenPlus;
					}
					str = menuItemFormatOneToNine;
					str1 = RecentFileList.ShortenPathname(displaypath, this.MaxPathLength);
					str2 = string.Format(str, index, filepath, str1);
				}
				else
				{
					str2 = getMenuItemTextHandler(index, filepath);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException7(exception, getMenuItemTextHandler, str, str1, this, index, filepath, displaypath);
				throw;
			}
			return str2;
		}

		private void HookFileMenu()
		{
			MenuItem parent;
			try
			{
				parent = base.Parent as MenuItem;
				if (parent != null)
				{
					if (this.FileMenu != parent)
					{
						if (this.FileMenu != null)
						{
							this.FileMenu.SubmenuOpened -= new RoutedEventHandler(this._FileMenu_SubmenuOpened);
						}
						this.FileMenu = parent;
						this.FileMenu.SubmenuOpened += new RoutedEventHandler(this._FileMenu_SubmenuOpened);
					}
				}
				else
				{
					throw new ApplicationException("Parent must be a MenuItem");
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, parent, this);
				throw;
			}
		}

		public void InsertFile(string filepath)
		{
			try
			{
				this.Persister.InsertFile(filepath, this.MaxNumberOfFiles, this.Category);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, filepath);
				throw;
			}
		}

		private void InsertMenuItems()
		{
			int num;
			RecentFileList.RecentFile _RecentFile = null;
			string menuItemText;
			MenuItem menuItem;
			List<RecentFileList.RecentFile>.Enumerator enumerator;
			try
			{
				if (this._RecentFiles != null)
				{
					if (this._RecentFiles.Count != 0)
					{
						num = this.FileMenu.Items.IndexOf(this);
						foreach (RecentFileList.RecentFile _RecentFile in this._RecentFiles)
						{
							menuItemText = this.GetMenuItemText(_RecentFile.Number + 1, _RecentFile.Filepath, _RecentFile.DisplayPath);
							menuItem = new MenuItem();
							menuItem.Header = menuItemText;
							_RecentFile.MenuItem = menuItem;
							_RecentFile.MenuItem.Click += new RoutedEventHandler(this.MenuItem_Click);
							int num1 = num + 1;
							num = num1;
							this.FileMenu.Items.Insert(num1, _RecentFile.MenuItem);
						}
					}
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException6(exception, num, _RecentFile, menuItemText, menuItem, enumerator, this);
				throw;
			}
		}

		private void LoadRecentFiles()
		{
			try
			{
				this._RecentFiles = this.LoadRecentFilesCore();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
		}

		private List<RecentFileList.RecentFile> LoadRecentFilesCore()
		{
			List<string> recentFiles;
			List<RecentFileList.RecentFile> recentFiles1;
			int num;
			string recentFile = null;
			List<string>.Enumerator enumerator;
			List<RecentFileList.RecentFile> recentFiles2;
			try
			{
				recentFiles = this.RecentFiles;
				recentFiles1 = new List<RecentFileList.RecentFile>(recentFiles.Count);
				num = 0;
				foreach (string recentFile in recentFiles)
				{
					int num1 = num;
					num = num1 + 1;
					recentFiles1.Add(new RecentFileList.RecentFile(num1, recentFile));
				}
				recentFiles2 = recentFiles1;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException6(exception, recentFiles, recentFiles1, num, recentFile, enumerator, this);
				throw;
			}
			return recentFiles2;
		}

		private void MenuItem_Click(object sender, EventArgs e)
		{
			MenuItem menuItem;
			try
			{
				menuItem = sender as MenuItem;
				this.OnMenuClick(menuItem);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, menuItem, this, sender, e);
				throw;
			}
		}

		protected virtual void OnMenuClick(MenuItem menuItem)
		{
			string filepath;
			EventHandler<RecentFileList.MenuClickEventArgs> menuClick;
			try
			{
				filepath = this.GetFilepath(menuItem);
				if (!string.IsNullOrEmpty(filepath))
				{
					menuClick = this.MenuClick;
					if (menuClick != null)
					{
						menuClick(menuItem, new RecentFileList.MenuClickEventArgs(filepath));
					}
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException4(exception, filepath, menuClick, this, menuItem);
				throw;
			}
		}

		public void RemoveFile(string filepath)
		{
			try
			{
				this.Persister.RemoveFile(filepath, this.MaxNumberOfFiles, this.Category);
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, filepath);
				throw;
			}
		}

		private void RemoveMenuItems()
		{
			try
			{
				this.FileMenu.Items.Clear();
				this._Separator = null;
				this._RecentFiles = null;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
		}

		private void SetMenuItems()
		{
			try
			{
				this.RemoveMenuItems();
				this.LoadRecentFiles();
				this.InsertMenuItems();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException1(exception, this);
				throw;
			}
		}

		public static string ShortenPathname(string pathname, int maxLength)
		{
			string pathRoot;
			string[] strArrays;
			int length;
			int num;
			int length1;
			int num1;
			int i;
			int length2;
			int num2;
			int j;
			int k;
			char[] directorySeparatorChar;
			string str;
			try
			{
				if (pathname.Length > maxLength)
				{
					pathRoot = Path.GetPathRoot(pathname);
					if (pathRoot.Length > 3)
					{
						pathRoot = string.Concat(pathRoot, Path.DirectorySeparatorChar);
					}
					directorySeparatorChar = new char[2];
					directorySeparatorChar[0] = Path.DirectorySeparatorChar;
					directorySeparatorChar[1] = Path.AltDirectorySeparatorChar;
					strArrays = pathname.Substring(pathRoot.Length).Split(directorySeparatorChar);
					length = strArrays.GetLength(0) - 1;
					if (strArrays.GetLength(0) != 1)
					{
						if (pathRoot.Length + 4 + strArrays[length].Length <= maxLength)
						{
							if (strArrays.GetLength(0) != 2)
							{
								length1 = 0;
								num1 = 0;
								for (i = 0; i < length; i++)
								{
									if (strArrays[i].Length > length1)
									{
										num1 = i;
										length1 = strArrays[i].Length;
									}
								}
								length2 = pathname.Length - length1 + 3;
								num2 = num1 + 1;
								do
								{
									if (length2 <= maxLength)
									{
										break;
									}
									if (num1 > 0)
									{
										int num3 = num1 - 1;
										num1 = num3;
										length2 = length2 - (strArrays[num3].Length - 1);
									}
									if (length2 <= maxLength)
									{
										break;
									}
									if (num2 >= length)
									{
										continue;
									}
									int num4 = num2 + 1;
									num2 = num4;
									length2 = length2 - (strArrays[num4].Length - 1);
								}
								while (num1 != 0 || num2 != length);
								for (j = 0; j < num1; j++)
								{
									pathRoot = string.Concat(pathRoot, strArrays[j], (char)92);
								}
								pathRoot = string.Concat(pathRoot, "...\\");
								for (k = num2; k < length; k++)
								{
									pathRoot = string.Concat(pathRoot, strArrays[k], (char)92);
								}
								str = string.Concat(pathRoot, strArrays[length]);
							}
							else
							{
								str = string.Concat(pathRoot, "...\\", strArrays[1]);
							}
						}
						else
						{
							pathRoot = string.Concat(pathRoot, "...\\");
							num = strArrays[length].Length;
							if (num >= 6)
							{
								if (pathRoot.Length + 6 < maxLength)
								{
									num = maxLength - pathRoot.Length - 3;
								}
								else
								{
									num = 3;
								}
								str = string.Concat(pathRoot, strArrays[length].Substring(0, num), "...");
							}
							else
							{
								str = string.Concat(pathRoot, strArrays[length]);
							}
						}
					}
					else
					{
						if (strArrays[0].Length <= 5)
						{
							str = pathname;
						}
						else
						{
							if (pathRoot.Length + 6 < maxLength)
							{
								str = string.Concat(pathname.Substring(0, maxLength - 3), "...");
							}
							else
							{
								str = string.Concat(pathRoot, strArrays[0].Substring(0, 3), "...");
							}
						}
					}
				}
				else
				{
					str = pathname;
				}
			}
			catch (Exception exception)
			{
				object[] objArray = new object[14];
				objArray[0] = pathRoot;
				objArray[1] = strArrays;
				objArray[2] = length;
				objArray[3] = num;
				objArray[4] = length1;
				objArray[5] = num1;
				objArray[6] = i;
				objArray[7] = length2;
				objArray[8] = num2;
				objArray[9] = j;
				objArray[10] = k;
				objArray[11] = directorySeparatorChar;
				objArray[12] = pathname;
				objArray[13] = maxLength;
				StackFrameHelper.CreateExceptionN(exception, objArray);
				throw;
			}
			return str;
		}

		public event EventHandler<RecentFileList.MenuClickEventArgs> MenuClick
		{
			add
			{
				EventHandler<RecentFileList.MenuClickEventArgs> menuClick;
				EventHandler<RecentFileList.MenuClickEventArgs> eventHandler;
				EventHandler<RecentFileList.MenuClickEventArgs> eventHandler1;
				try
				{
					menuClick = this.MenuClick;
					do
					{
						eventHandler = menuClick;
						eventHandler1 = (EventHandler<RecentFileList.MenuClickEventArgs>)Delegate.Combine(eventHandler, value);
						menuClick = Interlocked.CompareExchange<EventHandler<RecentFileList.MenuClickEventArgs>>(ref this.MenuClick, eventHandler1, eventHandler);
					}
					while (menuClick != eventHandler);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, menuClick, eventHandler, eventHandler1, this, value);
					throw;
				}
			}
			remove
			{
				EventHandler<RecentFileList.MenuClickEventArgs> menuClick;
				EventHandler<RecentFileList.MenuClickEventArgs> eventHandler;
				EventHandler<RecentFileList.MenuClickEventArgs> eventHandler1;
				try
				{
					menuClick = this.MenuClick;
					do
					{
						eventHandler = menuClick;
						eventHandler1 = (EventHandler<RecentFileList.MenuClickEventArgs>)Delegate.Remove(eventHandler, value);
						menuClick = Interlocked.CompareExchange<EventHandler<RecentFileList.MenuClickEventArgs>>(ref this.MenuClick, eventHandler1, eventHandler);
					}
					while (menuClick != eventHandler);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, menuClick, eventHandler, eventHandler1, this, value);
					throw;
				}
			}
		}

		private static class ApplicationAttributes
		{
			private readonly static Assembly _Assembly;

			private readonly static AssemblyTitleAttribute _Title;

			private readonly static AssemblyCompanyAttribute _Company;

			private readonly static AssemblyCopyrightAttribute _Copyright;

			private readonly static AssemblyProductAttribute _Product;

			private static Version _Version;

			public static string CompanyName
			{
				get
				{
					string u003cCompanyNameu003ek_BackingField;
					try
					{
						u003cCompanyNameu003ek_BackingField = RecentFileList.ApplicationAttributes.u003cCompanyNameu003ek__BackingField;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException0(exception);
						throw;
					}
					return u003cCompanyNameu003ek_BackingField;
				}
				private set
				{
					try
					{
						RecentFileList.ApplicationAttributes.u003cCompanyNameu003ek__BackingField = value;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, value);
						throw;
					}
				}
			}

			public static string Copyright
			{
				get
				{
					string u003cCopyrightu003ek_BackingField;
					try
					{
						u003cCopyrightu003ek_BackingField = RecentFileList.ApplicationAttributes.u003cCopyrightu003ek__BackingField;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException0(exception);
						throw;
					}
					return u003cCopyrightu003ek_BackingField;
				}
				private set
				{
					try
					{
						RecentFileList.ApplicationAttributes.u003cCopyrightu003ek__BackingField = value;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, value);
						throw;
					}
				}
			}

			public static string ProductName
			{
				get
				{
					string u003cProductNameu003ek_BackingField;
					try
					{
						u003cProductNameu003ek_BackingField = RecentFileList.ApplicationAttributes.u003cProductNameu003ek__BackingField;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException0(exception);
						throw;
					}
					return u003cProductNameu003ek_BackingField;
				}
				private set
				{
					try
					{
						RecentFileList.ApplicationAttributes.u003cProductNameu003ek__BackingField = value;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, value);
						throw;
					}
				}
			}

			public static string Title
			{
				get
				{
					string u003cTitleu003ek_BackingField;
					try
					{
						u003cTitleu003ek_BackingField = RecentFileList.ApplicationAttributes.u003cTitleu003ek__BackingField;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException0(exception);
						throw;
					}
					return u003cTitleu003ek_BackingField;
				}
				private set
				{
					try
					{
						RecentFileList.ApplicationAttributes.u003cTitleu003ek__BackingField = value;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, value);
						throw;
					}
				}
			}

			public static string Version
			{
				get
				{
					string u003cVersionu003ek_BackingField;
					try
					{
						u003cVersionu003ek_BackingField = RecentFileList.ApplicationAttributes.u003cVersionu003ek__BackingField;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException0(exception);
						throw;
					}
					return u003cVersionu003ek_BackingField;
				}
				private set
				{
					try
					{
						RecentFileList.ApplicationAttributes.u003cVersionu003ek__BackingField = value;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, value);
						throw;
					}
				}
			}

			static ApplicationAttributes()
			{
				object[] customAttributes;
				object obj;
				Type type;
				object[] objArray;
				int i;
				try
				{
					try
					{
						RecentFileList.ApplicationAttributes.Title = string.Empty;
						RecentFileList.ApplicationAttributes.CompanyName = string.Empty;
						RecentFileList.ApplicationAttributes.Copyright = string.Empty;
						RecentFileList.ApplicationAttributes.ProductName = string.Empty;
						RecentFileList.ApplicationAttributes.Version = string.Empty;
						RecentFileList.ApplicationAttributes._Assembly = Assembly.GetEntryAssembly();
						if (RecentFileList.ApplicationAttributes._Assembly != null)
						{
							customAttributes = RecentFileList.ApplicationAttributes._Assembly.GetCustomAttributes(false);
							objArray = customAttributes;
							for (i = 0; i < (int)objArray.Length; i++)
							{
								obj = objArray[i];
								type = obj.GetType();
								if (type == typeof(AssemblyTitleAttribute))
								{
									RecentFileList.ApplicationAttributes._Title = (AssemblyTitleAttribute)obj;
								}
								if (type == typeof(AssemblyCompanyAttribute))
								{
									RecentFileList.ApplicationAttributes._Company = (AssemblyCompanyAttribute)obj;
								}
								if (type == typeof(AssemblyCopyrightAttribute))
								{
									RecentFileList.ApplicationAttributes._Copyright = (AssemblyCopyrightAttribute)obj;
								}
								if (type == typeof(AssemblyProductAttribute))
								{
									RecentFileList.ApplicationAttributes._Product = (AssemblyProductAttribute)obj;
								}
							}
							RecentFileList.ApplicationAttributes._Version = RecentFileList.ApplicationAttributes._Assembly.GetName().Version;
						}
						if (RecentFileList.ApplicationAttributes._Title != null)
						{
							RecentFileList.ApplicationAttributes.Title = RecentFileList.ApplicationAttributes._Title.Title;
						}
						if (RecentFileList.ApplicationAttributes._Company != null)
						{
							RecentFileList.ApplicationAttributes.CompanyName = RecentFileList.ApplicationAttributes._Company.Company;
						}
						if (RecentFileList.ApplicationAttributes._Copyright != null)
						{
							RecentFileList.ApplicationAttributes.Copyright = RecentFileList.ApplicationAttributes._Copyright.Copyright;
						}
						if (RecentFileList.ApplicationAttributes._Product != null)
						{
							RecentFileList.ApplicationAttributes.ProductName = RecentFileList.ApplicationAttributes._Product.Product;
						}
						if (RecentFileList.ApplicationAttributes._Version != null)
						{
							RecentFileList.ApplicationAttributes.Version = RecentFileList.ApplicationAttributes._Version.ToString();
						}
					}
					catch
					{
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException5(exception, customAttributes, obj, type, objArray, i);
					throw;
				}
			}
		}

		public delegate string GetMenuItemTextDelegate(int index, string filepath);

		public interface IPersist
		{
			void InsertFile(string filepath, int max, string category = "Default");

			List<string> RecentFiles(int max, string category = "Default");

			void RemoveFile(string filepath, int max, string category = "Default");
		}

		public class MenuClickEventArgs : EventArgs
		{
			public string Filepath
			{
				get
				{
					string u003cFilepathu003ek_BackingField;
					try
					{
						u003cFilepathu003ek_BackingField = this.u003cFilepathu003ek__BackingField;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return u003cFilepathu003ek_BackingField;
				}
				private set
				{
					try
					{
						this.u003cFilepathu003ek__BackingField = value;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException2(exception, this, value);
						throw;
					}
				}
			}

			public MenuClickEventArgs(string filepath)
			{
				try
				{
					this.Filepath = filepath;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, filepath);
					throw;
				}
			}
		}

		private class RecentFile
		{
			public int Number;

			public string Filepath;

			public MenuItem MenuItem;

			public string DisplayPath
			{
				get
				{
					string str;
					try
					{
						str = Path.Combine(Path.GetDirectoryName(this.Filepath), Path.GetFileNameWithoutExtension(this.Filepath));
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return str;
				}
			}

			public RecentFile(int number, string filepath)
			{
				this.Filepath = "";
				try
				{
					this.Number = number;
					this.Filepath = filepath;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException3(exception, this, number, filepath);
					throw;
				}
			}
		}

		public class RecentFileItem
		{
			public string Display
			{
				get
				{
					string u003cDisplayu003ek_BackingField;
					try
					{
						u003cDisplayu003ek_BackingField = this.u003cDisplayu003ek__BackingField;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return u003cDisplayu003ek_BackingField;
				}
				set
				{
					try
					{
						this.u003cDisplayu003ek__BackingField = value;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException2(exception, this, value);
						throw;
					}
				}
			}

			public RelayCommand OpenCommand
			{
				get
				{
					RelayCommand relayCommand;
					try
					{
						relayCommand = new RelayCommand("Open", (object o) => {
							try
							{
								ResolutionExtensions.Get<Workspace>(App.Kernel, new IParameter[0]).OpenSolution(this.Path);
							}
							catch (Exception exception)
							{
								StackFrameHelper.CreateException2(exception, this, o);
								throw;
							}
						}
						);
					}
					catch (Exception exception1)
					{
						StackFrameHelper.CreateException1(exception1, this);
						throw;
					}
					return relayCommand;
				}
			}

			public string Path
			{
				get
				{
					string u003cPathu003ek_BackingField;
					try
					{
						u003cPathu003ek_BackingField = this.u003cPathu003ek__BackingField;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return u003cPathu003ek_BackingField;
				}
				set
				{
					try
					{
						this.u003cPathu003ek__BackingField = value;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException2(exception, this, value);
						throw;
					}
				}
			}

			public RecentFileItem()
			{
			}
		}

		private class RegistryPersister : RecentFileList.IPersist
		{
			public string RegistryKey
			{
				get
				{
					string u003cRegistryKeyu003ek_BackingField;
					try
					{
						u003cRegistryKeyu003ek_BackingField = this.u003cRegistryKeyu003ek__BackingField;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException1(exception, this);
						throw;
					}
					return u003cRegistryKeyu003ek_BackingField;
				}
				set
				{
					try
					{
						this.u003cRegistryKeyu003ek__BackingField = value;
					}
					catch (Exception exception)
					{
						StackFrameHelper.CreateException2(exception, this, value);
						throw;
					}
				}
			}

			public RegistryPersister()
			{
				string[] companyName;
				try
				{
					companyName = new string[5];
					companyName[0] = "Software\\";
					companyName[1] = RecentFileList.ApplicationAttributes.CompanyName;
					companyName[2] = "\\";
					companyName[3] = RecentFileList.ApplicationAttributes.ProductName;
					companyName[4] = "\\RecentFileList";
					this.RegistryKey = string.Concat(companyName);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, companyName, this);
					throw;
				}
			}

			public RegistryPersister(string key)
			{
				try
				{
					this.RegistryKey = key;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, key);
					throw;
				}
			}

			public void InsertFile(string filepath, int max, string category = "Default")
			{
				RegistryKey registryKey;
				int i;
				string str;
				string str1;
				object value;
				try
				{
					registryKey = Registry.CurrentUser.OpenSubKey(string.Concat(this.RegistryKey, "\\", category));
					if (registryKey == null)
					{
						Registry.CurrentUser.CreateSubKey(string.Concat(this.RegistryKey, "\\", category));
					}
					registryKey = Registry.CurrentUser.OpenSubKey(string.Concat(this.RegistryKey, "\\", category), true);
					this.RemoveFile(filepath, max, category);
					for (i = max - 2; i >= 0; i--)
					{
						str = this.Key(i);
						str1 = this.Key(i + 1);
						value = registryKey.GetValue(str);
						if (value != null)
						{
							registryKey.SetValue(str1, value);
						}
					}
					registryKey.SetValue(this.Key(0), filepath);
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException9(exception, registryKey, i, str, str1, value, this, filepath, max, category);
					throw;
				}
			}

			private string Key(int i)
			{
				string str;
				try
				{
					str = i.ToString("00");
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, i);
					throw;
				}
				return str;
			}

			public List<string> RecentFiles(int max, string category = "Default")
			{
				RegistryKey registryKey;
				List<string> strs;
				int i;
				string value;
				List<string> strs1;
				try
				{
					registryKey = Registry.CurrentUser.OpenSubKey(string.Concat(this.RegistryKey, "\\", category));
					if (registryKey == null)
					{
						registryKey = Registry.CurrentUser.CreateSubKey(string.Concat(this.RegistryKey, "\\", category));
					}
					strs = new List<string>(max);
					for (i = 0; i < max; i++)
					{
						value = (string)registryKey.GetValue(this.Key(i));
						if (string.IsNullOrEmpty(value))
						{
							break;
						}
						strs.Add(value);
					}
					strs1 = strs;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException7(exception, registryKey, strs, i, value, this, max, category);
					throw;
				}
				return strs1;
			}

			public void RemoveFile(string filepath, int max, string category = "Default")
			{
				RegistryKey registryKey;
				int i;
				string value;
				try
				{
					registryKey = Registry.CurrentUser.OpenSubKey(string.Concat(this.RegistryKey, "\\", category));
					if (registryKey != null)
					{
						for (i = 0; i < max; i++)
						{
							while (true)
							{
								value = (string)registryKey.GetValue(this.Key(i));
								if (value == null || !value.Equals(filepath, StringComparison.CurrentCultureIgnoreCase))
								{
									break;
								}
								this.RemoveFile(i, max, category);
							}
						}
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException7(exception, registryKey, i, value, this, filepath, max, category);
					throw;
				}
			}

			private void RemoveFile(int index, int max, string category = "Default")
			{
				RegistryKey registryKey;
				int num;
				string str;
				string str1;
				object value;
				try
				{
					registryKey = Registry.CurrentUser.OpenSubKey(string.Concat(this.RegistryKey, "\\", category), true);
					if (registryKey != null)
					{
						registryKey.DeleteValue(this.Key(index), false);
						num = index;
						while (num < max - 1)
						{
							str = this.Key(num);
							str1 = this.Key(num + 1);
							value = registryKey.GetValue(str1);
							if (value != null)
							{
								registryKey.SetValue(str, value);
								registryKey.DeleteValue(str1);
								num++;
							}
							else
							{
								return;
							}
						}
					}
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException9(exception, registryKey, num, str, str1, value, this, index, max, category);
					throw;
				}
			}
		}
	}
}