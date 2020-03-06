using System;
using System.Collections.Generic;
using lexCalculator.Calculation;

namespace lexCalculator.Types.Operations
{
	// Represents binary operation which can be written like an operator. Example: "a + b"
	public sealed class BinaryOperatorOperation : BinaryOperation
	{
		public readonly int Precedence;
		public readonly bool IsLeftAssociative;

		private static Dictionary<string, BinaryOperatorOperation> stringToOperation = new Dictionary<string, BinaryOperatorOperation>();
		public static readonly IReadOnlyDictionary<string, BinaryOperatorOperation> OperatorDictionary = stringToOperation;

		private BinaryOperatorOperation(string functionName, ResultFunction function, int precedence, bool isLeftAssociative, string operatorString)
			: base(functionName, function,
				  (operatorString != null)
				  ? String.Format("{0}{2}{1}", "{0}", "{1}", operatorString)
				  : throw new ArgumentNullException(nameof(operatorString)),
				  true)
		{
			Precedence = precedence;
			IsLeftAssociative = isLeftAssociative;
			stringToOperation.Add(operatorString, this);
		}

		public static readonly BinaryOperatorOperation Addition = new BinaryOperatorOperation("add", MoreMath.Sum, 0, true, "+");
		public static readonly BinaryOperatorOperation Substraction = new BinaryOperatorOperation("sub", MoreMath.Sub, 0, true, "-");
		public static readonly BinaryOperatorOperation Multiplication = new BinaryOperatorOperation("mul", MoreMath.Mul, 1, true, "*");
		public static readonly BinaryOperatorOperation Division = new BinaryOperatorOperation("div", MoreMath.Div, 1, true, "/");
		public static readonly BinaryOperatorOperation Power = new BinaryOperatorOperation("pow", Math.Pow, 3, false, "^");
		public static readonly BinaryOperatorOperation Remainder = new BinaryOperatorOperation("rem", Math.IEEERemainder, 2, false, "%");
	}
}
