using lexCalculator.Types;

namespace lexCalculator.Linking
{
	// Replaces function nodes with their own trees
	public interface ILinker
	{
		FinishedFunction BuildFunction(TreeNode topNode, CalculationContext context, string[] parameterNames);
	}
}
