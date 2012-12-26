using Devkit.IDE.View;
using Devkit.Interfaces.Build;
using ICSharpCode.AvalonEdit.Highlighting;
using SmartAssembly.SmartExceptionsCore;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Media;

namespace Devkit.IDE.Converters
{
	public class StrategyHighlighter : IHighlightingDefinition
	{
		private readonly HighlightingRuleSet _ruleSet;

		public HighlightingRuleSet MainRuleSet
		{
			get
			{
				HighlightingRuleSet highlightingRuleSet;
				try
				{
					highlightingRuleSet = this._ruleSet;
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return highlightingRuleSet;
			}
		}

		public string Name
		{
			get
			{
				string str;
				try
				{
					str = "DevKit Highlighting";
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
				return str;
			}
		}

		public IEnumerable<HighlightingColor> NamedHighlightingColors
		{
			get
			{
				try
				{
					throw new NotImplementedException();
				}
				catch (Exception exception)
				{
					StackFrameHelper.CreateException1(exception, this);
					throw;
				}
			}
		}

		public StrategyHighlighter(CodeEditorStrategy strategy)
		{
			Tuple<Regex, Brush> highlightingRule = null;
			HighlightingRule highlightingRule1;
			HighlightingColor highlightingColor;
			HighlightingRuleSet highlightingRuleSet;
			IEnumerator<Tuple<Regex, Brush>> enumerator = null;
			try
			{
				highlightingRuleSet = new HighlightingRuleSet();
				highlightingRuleSet.set_Name(this.Name);
				this._ruleSet = highlightingRuleSet;
				if (strategy != null)
				{
					foreach (Tuple<Regex, Brush> highlightingRule in strategy.HighlightingRules)
					{
						highlightingRule1 = new HighlightingRule();
						highlightingRule1.set_Regex(highlightingRule.Item1);
						highlightingColor = new HighlightingColor();
						highlightingColor.set_Foreground(new StandardBrushFactory(highlightingRule.Item2));
						highlightingRule1.set_Color(highlightingColor);
						this._ruleSet.get_Rules().Add(highlightingRule1);
					}
				}
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException7(exception, highlightingRule, highlightingRule1, highlightingColor, highlightingRuleSet, enumerator, this, strategy);
				throw;
			}
		}

		public HighlightingColor GetNamedColor(string name)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, name);
				throw;
			}
		}

		public HighlightingRuleSet GetNamedRuleSet(string name)
		{
			try
			{
				throw new NotImplementedException();
			}
			catch (Exception exception)
			{
				StackFrameHelper.CreateException2(exception, this, name);
				throw;
			}
		}
	}
}