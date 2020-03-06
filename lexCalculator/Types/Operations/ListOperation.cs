using lexCalculator.Calculation;
using System;

namespace lexCalculator.Types.Operations
{
	// Represents a function with variable number of arguments or list as single argument.
	public class ListOperation : Operation
	{
		public delegate double ResultFunction(params double[] args);

		public ResultFunction Function;

		public ListOperation(string functionName, ResultFunction function) 
			: base(ArgumentType.List, functionName, null, false)
		{
			Function = function;
		}

		public static readonly ListOperation Sum = new ListOperation("sum", (double[] arr) =>
		{
			if (arr.Length == 0) return Double.NaN;
			double sum = 0;
			for (int i = 0; i < arr.Length; ++i)
			{
				sum += arr[i];
			}
			return sum;
		});

		public static readonly ListOperation Product = new ListOperation("prod", (double[] arr) =>
		{
			if (arr.Length == 0) return Double.NaN;
			double prod = 1.0;
			for (int i = 0; i < arr.Length; ++i)
			{
				prod *= arr[i];
			}
			return prod;
		});

		public static readonly ListOperation Max = new ListOperation("max", (double[] arr) =>
		{
			if (arr.Length == 0) return Double.NaN;
			double max = Double.NegativeInfinity;
			for (int i = 0; i < arr.Length; ++i)
			{
				if (arr[i] > max) max = arr[i];
			}
			return max;
		});

		public static readonly ListOperation Min = new ListOperation("min", (double[] arr) =>
		{
			if (arr.Length == 0) return Double.NaN;
			double min = Double.PositiveInfinity;
			for (int i = 0; i < arr.Length; ++i)
			{
				if (arr[i] < min) min = arr[i];
			}
			return min;
		});

		public static readonly ListOperation Mean = new ListOperation("mean", (double[] arr) =>
		{
			if (arr.Length == 0) return Double.NaN;
			double sum = 0;
			for (int i = 0; i < arr.Length; ++i)
			{
				sum += arr[i];
			}
			return sum / arr.Length;
		});
		
		public static readonly ListOperation GreatestCommonDivisor = new ListOperation("gcd", (double[] arr) =>
		{
			if (arr.Length == 0) return Double.NaN;
			double gcd = arr[0];
			for (int i = 1; i < arr.Length; ++i)
			{
				gcd = MoreMath.GCD(arr[i], gcd);
			}
			return gcd;
		});

		public static readonly ListOperation LowestCommonMultiple = new ListOperation("lcm", (double[] arr) =>
		{
			throw new NotImplementedException();
		});
	}
}
