using System;
using System.Collections.Generic;
using lexCalculator.Calculation;

namespace lexCalculator.Types.Operations
{
	// Represents binary operation which can be written like an operator. Example: "a + b"
	public sealed class BinaryOperatorOperation : BinaryOperation
	{
		public readonly string Operator;
		public readonly int Precedence;
		public readonly bool IsLeftAssociative;

		private static readonly Dictionary<string, BinaryOperatorOperation> operatorDictionary = new Dictionary<string, BinaryOperatorOperation>();
		public static IReadOnlyDictionary<string, BinaryOperatorOperation> OperatorDictionary => operatorDictionary;

		private BinaryOperatorOperation(string functionName, ResultFunction function, int precedence, bool isLeftAssociative, string operatorString)
			: base(functionName, function,
				  (operatorString != null)
				  ? String.Format("{0}{2}{1}", "{0}", "{1}", operatorString)
				  : throw new ArgumentNullException(nameof(operatorString)),
				  true)
		{
			Operator = operatorString;
			Precedence = precedence;
			IsLeftAssociative = isLeftAssociative;
			operatorDictionary.Add(operatorString, this);
		}

		public static readonly BinaryOperatorOperation Addition			= new BinaryOperatorOperation("add",	MoreMath.Sum,			 0, true, "+");
		public static readonly BinaryOperatorOperation Substraction		= new BinaryOperatorOperation("sub",	MoreMath.Sub,			 0, true, "-");
		public static readonly BinaryOperatorOperation Multiplication	= new BinaryOperatorOperation("mul",	MoreMath.Mul,			 1, true, "*");
		public static readonly BinaryOperatorOperation Division			= new BinaryOperatorOperation("div",	MoreMath.Div,			 1, true, "/");
		public static readonly BinaryOperatorOperation Power			= new BinaryOperatorOperation("pow",	Math.Pow,				 3, false, "^");
		public static readonly BinaryOperatorOperation Remainder		= new BinaryOperatorOperation("rem",	Math.IEEERemainder,		 2, false, "%");

		public static readonly BinaryOperatorOperation More				= new BinaryOperatorOperation("more",	MoreMath.More,			 4, true, ">");
		public static readonly BinaryOperatorOperation Less				= new BinaryOperatorOperation("less",	MoreMath.Less,			 4, true, "<");
		public static readonly BinaryOperatorOperation MoreEqual		= new BinaryOperatorOperation("mequal",	MoreMath.MoreOrEqual,	 4, true, ">=");
		public static readonly BinaryOperatorOperation LessEqual		= new BinaryOperatorOperation("lequal",	MoreMath.LessOrEqual,	 4, true, "<=");
		public static readonly BinaryOperatorOperation Equal			= new BinaryOperatorOperation("equal",	MoreMath.Equal,			 4, true, "==");
		public static readonly BinaryOperatorOperation NotEqual			= new BinaryOperatorOperation("nequal", MoreMath.NotEqual,		 4, true, "!=");

		public static readonly BinaryOperatorOperation And				= new BinaryOperatorOperation("and",	MoreMath.And,			-1, true, "&&");
		public static readonly BinaryOperatorOperation Or				= new BinaryOperatorOperation("or",		MoreMath.Or,			-1, true, "??");
		public static readonly BinaryOperatorOperation Xor				= new BinaryOperatorOperation("xor",	MoreMath.Xor,			-1, true, "!!");
	}
}
