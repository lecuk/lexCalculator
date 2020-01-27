using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Construction
{
	public enum UnaryOperation
	{
		Negative,			// -a

		Sine,               // sin(a)
		Cosine,             // cos(a)
		Tangent,            // tan(a)
		Cotangent,          // cot(a)

		Exponent,           // exp(a) == e^a
		NaturalLogarithm,   // ln(a) = log(a, e)

		SquareRoot,         // sqrt(a)
		CubeRoot,           // cbrt(a)

		Floor,              // floor(a)
		Ceil,               // ceil(a)
		Absolute,			// abs(a) == |a|

		Factorial           // a!
	}

	public enum BinaryOperation
	{
		Addition,           // a + b
		Substraction,       // a - b
		Multiplication,     // a * b
		Division,           // a / b
		Power,              // a ^ b
		Remainder,          // a % b
		
		Logarithm,          // log(a, p)

		NRoot				// nrt(a, n)
	}
}
