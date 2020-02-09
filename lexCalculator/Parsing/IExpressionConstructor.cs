using lexCalculator.Types;

namespace lexCalculator.Parsing
{
	public interface IExpressionConstructor
	{
		TreeNode Construct(Token[] tokens);
	}
}
