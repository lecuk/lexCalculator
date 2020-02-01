using lexCalculator.Types;

namespace lexCalculator.Linking
{
	// Replaces function nodes with their own trees
	public interface IFunctionBuilder
	{
		Function BuildFunction(ExpressionTreeNode topNode, ExpressionContext context, string[] parameterNames);
	}
}
