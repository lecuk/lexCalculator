using System;
using System.Collections.Generic;

namespace lexCalculator.Linking
{
	static class StandardVariables
	{
		public static IReadOnlyDictionary<string, double> Variables = new Dictionary<string, double>()
		{
			{ "pi", Math.PI },
			{ "e", Math.E }
		};
	}
}
