using Devkit.Workspace.ViewModel;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace Devkit.IDE.Converters
{
	public class ContextMenuConverter : IValueConverter
	{
		public ContextMenu FileContextMenu
		{
			get
			{
				ContextMenu u003cFileContextMenuu003ek_BackingField;
				try
				{
					u003cFileContextMenuu003ek_BackingField = this.u003cFileContextMenuu003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cFileContextMenuu003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cFileContextMenuu003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public ContextMenu ProjectContextMenu
		{
			get
			{
				ContextMenu u003cProjectContextMenuu003ek_BackingField;
				try
				{
					u003cProjectContextMenuu003ek_BackingField = this.u003cProjectContextMenuu003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cProjectContextMenuu003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cProjectContextMenuu003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public ContextMenu ReferenceContextMenu
		{
			get
			{
				ContextMenu u003cReferenceContextMenuu003ek_BackingField;
				try
				{
					u003cReferenceContextMenuu003ek_BackingField = this.u003cReferenceContextMenuu003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cReferenceContextMenuu003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cReferenceContextMenuu003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public ContextMenu ReferencesCollectionContextMenu
		{
			get
			{
				ContextMenu u003cReferencesCollectionContextMenuu003ek_BackingField;
				try
				{
					u003cReferencesCollectionContextMenuu003ek_BackingField = this.u003cReferencesCollectionContextMenuu003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cReferencesCollectionContextMenuu003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cReferencesCollectionContextMenuu003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public ContextMenu SolutionContextMenu
		{
			get
			{
				ContextMenu u003cSolutionContextMenuu003ek_BackingField;
				try
				{
					u003cSolutionContextMenuu003ek_BackingField = this.u003cSolutionContextMenuu003ek__BackingField;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return u003cSolutionContextMenuu003ek_BackingField;
			}
			set
			{
				try
				{
					this.u003cSolutionContextMenuu003ek__BackingField = value;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException2(exception, this, value);
					throw;
				}
			}
		}

		public ContextMenuConverter()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			object referenceContextMenu;
			try
			{
				if (value != null)
				{
					if (value as File == null)
					{
						if (value as Project == null)
						{
							if (value as Solution == null)
							{
								if (value as ReferencesCollection == null)
								{
									if (value as Reference == null)
									{
										referenceContextMenu = null;
									}
									else
									{
										referenceContextMenu = this.ReferenceContextMenu;
									}
								}
								else
								{
									referenceContextMenu = this.ReferencesCollectionContextMenu;
								}
							}
							else
							{
								referenceContextMenu = this.SolutionContextMenu;
							}
						}
						else
						{
							referenceContextMenu = this.ProjectContextMenu;
						}
					}
					else
					{
						referenceContextMenu = this.FileContextMenu;
					}
				}
				else
				{
					referenceContextMenu = null;
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException5(exception, this, value, targetType, parameter, culture);
				throw;
			}
			return referenceContextMenu;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException5(exception, this, value, targetType, parameter, culture);
				throw;
			}
		}
	}
}