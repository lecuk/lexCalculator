namespace lexCalculator.Types.Tokens
{
	public class IdentifierToken : Token
	{
		public string Identifier { get; set; }

		public IdentifierToken(string identifier)
		{
			Identifier = identifier;
		}

		public override string ToString()
		{
			return Identifier;
		}
	}
}
