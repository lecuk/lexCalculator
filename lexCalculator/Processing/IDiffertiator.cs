using lexCalculator.Types;

namespace lexCalculator.Processing
{
	public interface IDifferentiator
	{
		FinishedFunction FindDifferential(FinishedFunction func, int index);
	}
}
