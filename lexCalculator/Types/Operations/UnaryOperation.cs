using lexCalculator.Calculation;
using System;

namespace lexCalculator.Types.Operations
{
	// Represents an unary operator or function with exactly one parameter.
	public sealed class UnaryOperation : Operation
	{
		public delegate double ResultFunction(double x);

		public readonly ResultFunction Function;

		private UnaryOperation(string functionName, ResultFunction function, string specialFormat, bool childrenInSpecialFormatMayNeedBrackets) 
			: base(ArgumentType.Unary, functionName, specialFormat, childrenInSpecialFormatMayNeedBrackets)
		{
			Function = function;
		}

		private UnaryOperation(string functionName, ResultFunction function)
			: this(functionName, function, null, false) {  }

		public static readonly UnaryOperation Negative = new UnaryOperation("neg", MoreMath.Negative, "-{0}", true);
		public static readonly UnaryOperation AbsoluteValue = new UnaryOperation("abs", Math.Abs, "|{0}|", false);
		public static readonly UnaryOperation Sign = new UnaryOperation("sign", MoreMath.FSign);

		public static readonly UnaryOperation Sine = new UnaryOperation("sin", Math.Sin);
		public static readonly UnaryOperation Cosine = new UnaryOperation("cos", Math.Cos);
		public static readonly UnaryOperation Tangent = new UnaryOperation("tan", Math.Tan);
		public static readonly UnaryOperation Cotangent = new UnaryOperation("cot", MoreMath.Cot);
		public static readonly UnaryOperation Secant = new UnaryOperation("sec", MoreMath.Sec);
		public static readonly UnaryOperation Cosecant = new UnaryOperation("csc", MoreMath.Csc);

		public static readonly UnaryOperation ArcSine = new UnaryOperation("arcsin", Math.Asin);
		public static readonly UnaryOperation ArcCosine = new UnaryOperation("arccos", Math.Acos);
		public static readonly UnaryOperation ArcTangent = new UnaryOperation("arctan", Math.Tan);
		public static readonly UnaryOperation ArcCotangent = new UnaryOperation("arccot", MoreMath.Acot);
		public static readonly UnaryOperation ArcSecant = new UnaryOperation("arcsec", MoreMath.Asec);
		public static readonly UnaryOperation ArcCosecant = new UnaryOperation("arccsc", MoreMath.Acsc);

		public static readonly UnaryOperation SineHyperbolic = new UnaryOperation("sinh", Math.Sinh);
		public static readonly UnaryOperation CosineHyperbolic = new UnaryOperation("cosh", Math.Cosh);
		public static readonly UnaryOperation TangentHyperbolic = new UnaryOperation("tanh", Math.Tanh);
		public static readonly UnaryOperation CotangentHyperbolic = new UnaryOperation("coth", MoreMath.Coth);
		public static readonly UnaryOperation SecantHyperbolic = new UnaryOperation("sech", MoreMath.Sech);
		public static readonly UnaryOperation CosecantHyperbolic = new UnaryOperation("csch", MoreMath.Csch);

		public static readonly UnaryOperation NaturalLogarithm = new UnaryOperation("ln", Math.Log);
		public static readonly UnaryOperation Exponent = new UnaryOperation("exp", Math.Exp, "e^{0}", true);
			    
		public static readonly UnaryOperation Square = new UnaryOperation("sqr", MoreMath.Square, "{0}^2", true);
		public static readonly UnaryOperation Cube = new UnaryOperation("cube", MoreMath.Cube, "{0}^3", true);
		public static readonly UnaryOperation SquareRoot = new UnaryOperation("sqrt", Math.Sqrt);
		public static readonly UnaryOperation CubeRoot = new UnaryOperation("cbrt", MoreMath.Cbrt);

		public static readonly UnaryOperation Floor = new UnaryOperation("floor", Math.Floor);
		public static readonly UnaryOperation Ceiling = new UnaryOperation("ceil", Math.Ceiling);

		public static readonly UnaryOperation Factorial = new UnaryOperation("fact", MoreMath.Factorial, "{0}!", true);
	}
}
