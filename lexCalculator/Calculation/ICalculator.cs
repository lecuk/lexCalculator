using lexCalculator.Types;

namespace lexCalculator.Calculation
{
	// Calculator uses T data structure to calculate the final result of a function.
	public interface ICalculator<T>
	{
		double Calculate(T obj, VariableIndexTable table, params double[] parameters);
		double[] CalculateMultiple(T obj, VariableIndexTable table, double[,] parameters);
	}
}
