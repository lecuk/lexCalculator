using lexCalculator.Types;

namespace lexCalculator.Parsing
{
	public interface IExpressionConstructor
	{
		ExpressionTreeNode Construct(Token[] tokens);
	}
}
