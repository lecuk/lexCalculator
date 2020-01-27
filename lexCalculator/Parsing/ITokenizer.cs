namespace lexCalculator.Parsing
{
	public interface ITokenizer
    {
		Token[] Tokenize(string expression);
    }
}
