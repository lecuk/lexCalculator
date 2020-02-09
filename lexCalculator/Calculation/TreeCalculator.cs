using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexCalculator.Types;

namespace lexCalculator.Calculation
{
	// Tree calculator is calculating result from tree recursively. It is easier to understand but is much slower.
	class TreeCalculator : ICalculator<FinishedFunction>
	{
		public double Calculate(FinishedFunction function, params double[] parameters)
		{
			throw new NotImplementedException();
		}

		public double[] CalculateMultiple(FinishedFunction function, double[,] parameters)
		{
			throw new NotImplementedException();
		}
	}
}
