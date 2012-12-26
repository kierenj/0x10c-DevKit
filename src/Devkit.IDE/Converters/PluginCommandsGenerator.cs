using Devkit.Interfaces;
using Devkit.Workspace.Commands;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace Devkit.IDE.Converters
{
	public class PluginCommandsGenerator : IValueConverter
	{
		public PluginCommandsGenerator()
		{
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			IPlugin plugin;
			PluginInfo pluginInfo;
			object commands;
			try
			{
				plugin = value as IPlugin;
				if (plugin == null)
				{
					pluginInfo = value as PluginInfo;
					if (pluginInfo == null)
					{
						commands = null;
					}
					else
					{
						commands = this.GetCommands(pluginInfo);
					}
				}
				else
				{
					commands = this.GetCommands(plugin);
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException7(exception, plugin, pluginInfo, this, value, targetType, parameter, culture);
				throw;
			}
			return commands;
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

		private IEnumerable<RelayCommand> GetCommands(IPlugin plugin)
		{
			PluginCommandsGenerator.u003cGetCommandsu003ed__6 variable;
			IEnumerable<RelayCommand> relayCommands;
			try
			{
				variable = new PluginCommandsGenerator.u003cGetCommandsu003ed__6(-2);
				variable.u003cu003e4__this = this;
				variable.u003cu003e3__plugin = plugin;
				relayCommands = variable;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, variable, this, plugin);
				throw;
			}
			return relayCommands;
		}

		private IEnumerable<RelayCommand> GetCommands(PluginInfo plugin)
		{
			PluginCommandsGenerator.u003cGetCommandsu003ed__f variable;
			IEnumerable<RelayCommand> relayCommands;
			try
			{
				variable = new PluginCommandsGenerator.u003cGetCommandsu003ed__f(-2);
				variable.u003cu003e4__this = this;
				variable.u003cu003e3__plugin = plugin;
				relayCommands = variable;
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException3(exception, variable, this, plugin);
				throw;
			}
			return relayCommands;
		}
	}
}