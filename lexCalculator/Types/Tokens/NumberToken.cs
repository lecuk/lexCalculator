namespace lexCalculator.Types.Tokens
{
	public sealed class NumberToken : Token
	{
		public double Value { get; set; }

		public NumberToken(double value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Value.ToString("G7", System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}
