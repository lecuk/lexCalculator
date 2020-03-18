using lexCalculator.Calculation;

namespace lexCalculator.Types.Operations
{
	// Represents an arithmetic ternary operation or function with exactly three parameters.
	public class TernaryOperation : Operation
	{
		public delegate double ResultFunction(double a, double b, double c);

		public readonly ResultFunction Function;

		protected TernaryOperation(string functionName, ResultFunction function, string specialFormat, bool childrenMayNeedBrackets)
			: base(ArgumentType.Ternary, functionName, specialFormat, childrenMayNeedBrackets)
		{
			Function = function;
		}

		protected TernaryOperation(string functionName, ResultFunction function)
			: this(functionName, function, null, false) {  } 

		public static readonly TernaryOperation Conditional = new TernaryOperation("ifelse", MoreMath.TernaryConditional);
		public static readonly TernaryOperation CheckInRange = new TernaryOperation("range", MoreMath.TernaryRange);
	}
}
