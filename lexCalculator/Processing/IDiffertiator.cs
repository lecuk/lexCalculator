using lexCalculator.Types;

namespace lexCalculator.Processing
{
	interface IDiffertiator
	{
		FinishedFunction FindDifferential(FinishedFunction func, int index);
	}
}
