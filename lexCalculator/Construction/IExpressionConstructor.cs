using lexCalculator.Parsing;

namespace lexCalculator.Construction
{
	public interface IExpressionConstructor
	{
		ExpressionTreeNode Construct(Token[] tokens);
	}
}
