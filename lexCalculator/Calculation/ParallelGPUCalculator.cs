using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Calculation
{
	class ParallelGPUCalculator<T> : ICalculator<T>
	{
		public ICalculator<T> CalculatorToUse { get; set; }

		public double Calculate(T obj, params double[] parameters)
		{
			return CalculatorToUse.Calculate(obj, parameters);
		}

		public double[] CalculateMultiple(T obj, double[][] parameters)
		{
			throw new NotImplementedException();
		}

		public ParallelGPUCalculator(ICalculator<T> calculatorToUse)
		{
			CalculatorToUse = calculatorToUse ?? throw new ArgumentNullException(nameof(calculatorToUse));
		}
	}
}
