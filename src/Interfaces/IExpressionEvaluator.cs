using System;

/// <summary>
/// Used to evaluate an experssion or set the target of an expression
/// </summary>
namespace Devkit.Interfaces
{
	public interface IExpressionEvaluator
	{
		/// <summary>
		/// Compiles and evaluates the given source string and returns the current value
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		object Evaluate(string source);

		/// <summary>
		/// Compiles the given source string and sets the target of the expression to a given value
		/// </summary>
		/// <param name="source"></param>
		/// <param name="val"></param>
		void Set(string source, object val);
	}
}