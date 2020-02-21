using lexCalculator.Types;

namespace lexCalculator.Processing
{
	public interface IOptimizer
	{
		FinishedFunction Optimize(FinishedFunction unoptimized);
		FinishedFunction OptimizeWithTable(FinishedFunction unoptimized);
	}
}
