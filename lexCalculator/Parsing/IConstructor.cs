using lexCalculator.Types;

namespace lexCalculator.Parsing
{
	public interface IConstructor
	{
		TreeNode Construct(Token[] tokens);
	}
}
