using lexCalculator.Types;
using System.Collections.Generic;

namespace lexCalculator.Linking
{
	static class StandardFunctions
	{
		public static IReadOnlyDictionary<string, UnaryOperation> UnaryFunctions = new Dictionary<string, UnaryOperation>()
		{
			{ "sign", UnaryOperation.Sign },
			{ "abs", UnaryOperation.AbsoluteValue },

			{ "sin", UnaryOperation.Sine },
			{ "cos", UnaryOperation.Cosine },
			{ "tan", UnaryOperation.Tangent },
			{ "cot", UnaryOperation.Cotangent },
			{ "sec", UnaryOperation.Secant },
			{ "csc", UnaryOperation.Cosecant },

			{ "arcsin", UnaryOperation.ArcSine },
			{ "arccos", UnaryOperation.ArcCosine },
			{ "arctan", UnaryOperation.ArcTangent },
			{ "arccot", UnaryOperation.ArcCotangent },
			{ "arcsec", UnaryOperation.ArcSecant },
			{ "arccsc", UnaryOperation.ArcCosecant },

			{ "sinh", UnaryOperation.SineHyperbolic },
			{ "cosh", UnaryOperation.CosineHyperbolic },
			{ "tanh", UnaryOperation.TangentHyperbolic },
			{ "coth", UnaryOperation.CotangentHyperbolic },
			{ "sech", UnaryOperation.SecantHyperbolic },
			{ "csch", UnaryOperation.CosecantHyperbolic },

			{ "exp", UnaryOperation.Exponent },
			{ "ln", UnaryOperation.NaturalLogarithm },

			{ "sqrt", UnaryOperation.SquareRoot },
			{ "cbrt", UnaryOperation.CubeRoot },

			{ "floor", UnaryOperation.Floor },
			{ "ceil", UnaryOperation.Ceiling }
		};

		public static IReadOnlyDictionary<string, BinaryOperation> BinaryFunctions = new Dictionary<string, BinaryOperation>()
		{
			{ "log", BinaryOperation.Logarithm },
			{ "nrt", BinaryOperation.NRoot }
		};

		public static IReadOnlyDictionary<string, ListOperation> ListFunctions = new Dictionary<string, ListOperation>()
		{
			{ "max", ListOperation.Max },
			{ "min", ListOperation.Min },
			{ "mean", ListOperation.Mean },

			{ "gcd", ListOperation.GreatestCommonDivisor },
			{ "lcd", ListOperation.LeastCommonDenominator }
		};
	}
}
