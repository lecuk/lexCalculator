using lexCalculator.Calculation;
using System;
using System.Collections.Generic;

namespace lexCalculator.Types.Operations
{
	// Represents an arithmetic binary operation or function with exactly two parameters.
	public class BinaryOperation : Operation
	{
		public delegate double ResultFunction(double a, double b);

		public readonly ResultFunction Function;

		protected BinaryOperation(string functionName, ResultFunction function, string specialFormat, bool childrenMayNeedBrackets)
			: base(ArgumentType.Binary, functionName, specialFormat, childrenMayNeedBrackets)
		{
			Function = function;
		}

		protected BinaryOperation(string functionName, ResultFunction function)
			: this(functionName, function, null, false) {  }

		public static readonly BinaryOperation Logarithm = new BinaryOperation("log", MoreMath.Log);
		public static readonly BinaryOperation NthRoot = new BinaryOperation("nroot", MoreMath.NthRoot);
	}
}
