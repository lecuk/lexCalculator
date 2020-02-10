namespace lexCalculator.Types
{
	public interface IReadOnlyVariableTable
	{
		double this[int i] { get; }
	}
}