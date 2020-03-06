namespace lexCalculator.Parsing
{
	public interface ILexer
    {
		Token[] Tokenize(string expression);
    }
}
