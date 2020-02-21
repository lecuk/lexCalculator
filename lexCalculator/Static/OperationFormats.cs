using System;
using System.Collections.Generic;
using lexCalculator.Types;

namespace lexCalculator.Static
{
	public static class OperationFormats
	{
		public static readonly IReadOnlyDictionary<UnaryOperation, OperationFormatInfo> UnaryOperationFormats 
			= new Dictionary<UnaryOperation, OperationFormatInfo>
		{
			{ UnaryOperation.Negative,
				new OperationFormatInfo("minus", "neg", "-{0}") },

			{ UnaryOperation.Sign,
				new OperationFormatInfo("sign", "sgn") },

			{ UnaryOperation.Sine,
				new OperationFormatInfo("sin", "sin") },
			
			{ UnaryOperation.Cosine,
				new OperationFormatInfo("cos", "cos") },

			{ UnaryOperation.Tangent,
				new OperationFormatInfo("tan", "tan") },

			{ UnaryOperation.Cotangent,
				new OperationFormatInfo("cot", "cot") },

			{ UnaryOperation.Secant,
				new OperationFormatInfo("sec", "sec") },

			{ UnaryOperation.Cosecant,
				new OperationFormatInfo("csc", "csc") },

			{ UnaryOperation.ArcSine,
				new OperationFormatInfo("arcsin", "asin") },

			{ UnaryOperation.ArcCosine,
				new OperationFormatInfo("arccos", "acos") },

			{ UnaryOperation.ArcTangent,
				new OperationFormatInfo("arctan", "atan") },

			{ UnaryOperation.ArcCotangent,
				new OperationFormatInfo("arccot", "acot") },

			{ UnaryOperation.ArcSecant,
				new OperationFormatInfo("arcsec", "asec") },

			{ UnaryOperation.ArcCosecant,
				new OperationFormatInfo("arccsc", "acsc") },

			{ UnaryOperation.SineHyperbolic,
				new OperationFormatInfo("sinh", "sinh") },

			{ UnaryOperation.CosineHyperbolic,
				new OperationFormatInfo("cosh", "cosh") },

			{ UnaryOperation.TangentHyperbolic,
				new OperationFormatInfo("tanh", "tanh") },

			{ UnaryOperation.CotangentHyperbolic,
				new OperationFormatInfo("coth", "coth") },

			{ UnaryOperation.SecantHyperbolic,
				new OperationFormatInfo("sech", "sech") },

			{ UnaryOperation.CosecantHyperbolic,
				new OperationFormatInfo("csch", "csch") },

			{ UnaryOperation.Exponent,
				new OperationFormatInfo("exp", "exp", "e^{0}") },

			{ UnaryOperation.NaturalLogarithm,
				new OperationFormatInfo("ln", "ln") },

			{ UnaryOperation.SquareRoot,
				new OperationFormatInfo("sqrt", "sqrt") },

			{ UnaryOperation.CubeRoot,
				new OperationFormatInfo("cbrt", "cbrt") },

			{ UnaryOperation.Square,
				new OperationFormatInfo("sqr", "sqr", "{0}^2") },

			{ UnaryOperation.Cube,
				new OperationFormatInfo("cube", "cube", "{0}^3") },

			{ UnaryOperation.Floor,
				new OperationFormatInfo("floor", "floor") },

			{ UnaryOperation.Ceiling,
				new OperationFormatInfo("ceil", "ceil") },

			{ UnaryOperation.AbsoluteValue,
				new OperationFormatInfo("abs", "abs", "|{0}|", false) },

			{ UnaryOperation.Factorial,
				new OperationFormatInfo("factorial", "fact", "{0}!") }
		};

		public static readonly IReadOnlyDictionary<BinaryOperation, OperationFormatInfo> BinaryOperationFormats
			= new Dictionary<BinaryOperation, OperationFormatInfo>
		{
			{ BinaryOperation.Addition,
				new OperationFormatInfo("add", "add", "{0} + {1}") },

			{ BinaryOperation.Substraction,
				new OperationFormatInfo("sub", "sub", "{0} - {1}") },

			{ BinaryOperation.Multiplication,
				new OperationFormatInfo("mul", "mul", "{0} * {1}") },

			{ BinaryOperation.Division,
				new OperationFormatInfo("div", "div", "{0} / {1}") },

			{ BinaryOperation.Power,
				new OperationFormatInfo("pow", "pow", "{0} ^ {1}") },

			{ BinaryOperation.Remainder,
				new OperationFormatInfo("rem", "rem", "{0} % {1}") },

			{ BinaryOperation.Logarithm,
				new OperationFormatInfo("log", "log") },

			{ BinaryOperation.NRoot,
				new OperationFormatInfo("nroot", "nrt") }
		};

		public static bool TreeNodeNeedsBrackets(TreeNode node)
		{
			TreeNode parent = node.Parent;

			if (parent == null) return false;

			if (node is BinaryOperationTreeNode bNode)
			{
				switch (parent)
				{
					case UnaryOperationTreeNode puNode:
						return BinaryOperationFormats[bNode.Operation].HasSpecialFormat &&
							UnaryOperationFormats[puNode.Operation].ChildrenMayNeedBrackets;

					case BinaryOperationTreeNode pbNode:
						return BinaryOperationFormats[bNode.Operation].HasSpecialFormat &&
							BinaryOperationFormats[pbNode.Operation].ChildrenMayNeedBrackets &&
							pbNode.Operation > bNode.Operation;

					default: return false;
				}
			}
			else return false;
		}
	}
}
