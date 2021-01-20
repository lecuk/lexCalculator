using System;
using System.Runtime.CompilerServices;
using System.Numerics;

namespace lexCalculator.Calculation
{
	public static class MoreMath
	{
		public static double Negative(double x)
		{
			return -x;
		}

		public static double FSign(double x)
		{
			return (double)Math.Sign(x);
		}

		public static double Cot(double x)
		{
			return 1.0 / Math.Tan(x);
		}

		public static double Sec(double x)
		{
			return 1.0 / Math.Cos(x);
		}

		public static double Csc(double x)
		{
			return 1.0 / Math.Sin(x);
		}

		public static double Acot(double x)
		{
			return Math.Atan(1.0 / x);
		}

		public static double Asec(double x)
		{
			return Math.Acos(1.0 / x);
		}

		public static double Acsc(double x)
		{
			return Math.Asin(1.0 / x);
		}

		public static double Coth(double x)
		{
			return 1.0 / Math.Tanh(x);
		}

		public static double Sech(double x)
		{
			return 1.0 / Math.Cosh(x);
		}

		public static double Csch(double x)
		{
			return 1.0 / Math.Sinh(x);
		}

		// wat do
		public static double Cbrt(double x)
		{
			return Math.Pow(x, 0.3333333333333333333333);
		}
	
		public static bool IsWhole(double x)
		{
			return Math.Abs(x % 1.0) <= (Double.Epsilon * 100);
		}

		// using S.Ramanujan's factorial approximation formula
		public static double Factorial(double x)
		{
			return IsWhole(x) ? WholeFactorial((long)x) : RamanujanApproxFactorial(x);
		}
		
		public static double RamanujanApproxFactorial(double x)
		{
			return (x < 0) ? Double.NaN
				: (x == 0) ? 1.0
				: Math.Sqrt(2 * Math.PI * x) * Math.Pow(x / Math.E, x) * Math.Exp(1.0 / (12.0 * x) - 1 / (360.0 * x * x * x));
		}

		public static double WholeFactorial(long x)
		{
			double product = 1;
			for (long i = 2; i <= x; ++i)
			{
				product *= i;
			}
			return product;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Sum(double x, double y)
		{
			return x + y;
		}

		public static double Square(double arg)
		{
			return arg * arg;
		}

		public static double Cube(double arg)
		{
			return arg * arg * arg;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Sub(double x, double y)
		{
			return x - y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Mul(double x, double y)
		{
			return x * y;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static double Div(double x, double y)
		{
			return x / y;
		}

		// log_p(x) = ln(x) / ln(p)
		public static double Log(double p, double x)
		{
			return Math.Log(x) / Math.Log(p);
		}

		public static double NthRoot(double p, double x)
		{
			return Math.Pow(x, 1.0 / p);
		}

		// stolen here because i am lazy to write my own function:
		// https://www.sanfoundry.com/csharp-program-gcd/
		public static double GCD(double x, double y)
		{
			while (y > 0.000001)
			{
				double rem = Math.IEEERemainder(x, y);
				x = y;
				y = rem;
			}

			return x;
		}
	}
}
