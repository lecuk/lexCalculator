using lexCalculator.Types;

namespace lexCalculator.Parsing
{
	public interface IParser
	{
		TreeNode Construct(Token[] tokens);
	}
}
