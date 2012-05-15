using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using Devkit.Interfaces.Build;

namespace Devkit.DCPUToolchainPlugin
{
    public static class Highlighting
    {
        public static void Apply(CodeEditorStrategy strat)
        {
            strat.HighlightingRules.Add(new Tuple<Regex, Brush>(
                new Regex(@"(?i:\b(do|for|while|break|continue|goto|return|catch|throw|try|asm|delete|new|sizeof|typeof|typeid|typedef|extern|inline|int|const|register|bool|char|unsigned|union|struct|class|virtual|double|float|long|short|void|signed|unsigned|enum|asm)\b)"),
                Brushes.Blue));

            strat.HighlightingRules.Add(new Tuple<Regex, Brush>(
                new Regex("(?s:\".*?\")"),
                Brushes.DarkOrange));

            strat.HighlightingRules.Add(new Tuple<Regex, Brush>(
                new Regex("(?s:\'.*?\')"),
                Brushes.DarkOrange));

            strat.HighlightingRules.Add(new Tuple<Regex, Brush>(
                new Regex(@"(?i:\b(((0x)((\d)|a|b|c|d|e|f)+)|(\d+))\b)"),
                Brushes.Indigo));

            strat.HighlightingRules.Add(new Tuple<Regex, Brush>(
                new Regex(@"(\#define)|(\#include)|(\#ifdef)|(\#undef)|(\#else)|(\#pragma)"),
                Brushes.Red));

            strat.HighlightingRules.Add(new Tuple<Regex, Brush>(
                new Regex(@"//.*"),
                Brushes.Green));

            strat.HighlightingRules.Add(new Tuple<Regex, Brush>(
                new Regex(@"/\*.*?\*/"),
                Brushes.Green));

            strat.HighlightingRules.Add(new Tuple<Regex, Brush>(
                new Regex(@"[?,.;()\[\]{}+\-/%*<>^=~!&]+"),
                Brushes.Maroon));
        }
    }
}
