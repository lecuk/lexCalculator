namespace lexCalculator.Types
{
	// Represents an unary operator or function with exactly one parameter.
	public enum UnaryOperation
	{
		Negative,				// -a
		Sign,					// sign(a)

		Sine,					// sin(a)
		Cosine,					// cos(a)
		Tangent,				// tan(a)
		Cotangent,              // cot(a)
		Secant,                 // sec(a)
		Cosecant,               // csc(a)

		ArcSine,				// arcsin(a)
		ArcCosine,				// arccos(a)
		ArcTangent,				// arctan(a)
		ArcCotangent,           // arccot(a)
		ArcSecant,              // arcsec(a)
		ArcCosecant,            // arccsc(a)

		SineHyperbolic,			// sinh(a)
		CosineHyperbolic,		// cos(a)
		TangentHyperbolic,		// tan(a)
		CotangentHyperbolic,    // cot(a)
		SecantHyperbolic,       // sech(a)
		CosecantHyperbolic,     // csch(a)

		Exponent,				// exp(a) == e^a
		NaturalLogarithm,		// ln(a) = log(a, e)

		SquareRoot,				// sqrt(a)
		CubeRoot,				// cbrt(a)

		Floor,					// floor(a)
		Ceil,					// ceil(a)
		AbsoluteValue,			// abs(a) == |a|

		Factorial				// a!
	}

	// Represents an arithmetic binary operation or function with exactly two parameters.
	public enum BinaryOperation
	{
		Addition,				// a + b
		Substraction,			// a - b
		Multiplication,			// a * b
		Division,				// a / b
		Power,					// a ^ b
		Remainder,				// a % b

		Logarithm,				// log(a, p)

		NRoot,					// nrt(a, n)
	}

	// Represents a function with variable number of arguments or list as single argument.
	public enum ListOperation
	{
		Max,					// max(a1, a2, ...)
		Min,					// min(a1, a2, ...)
		Mean,					// mean(a1, a2, ...)
		
		GreatestCommonDivisor,	// gcd(a1, a2, ...)
		LeastCommonDenominator	// lcd(a1, a2, ...)
	}
}
