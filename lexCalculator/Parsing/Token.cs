using System;

namespace lexCalculator.Parsing
{
	public abstract class Token
	{
		public override string ToString()
		{
			return "?";
		}
	}

	public class SymbolToken : Token
	{
		public char Symbol { get; set; }

		public SymbolToken(char symbol)
		{
			Symbol = symbol;
		}

		public override string ToString()
		{
			return Symbol.ToString();
		}
	}
	
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

	public class LiteralToken : Token
	{
		public double Value { get; set; }

		public LiteralToken(double value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Value.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture);
		}
	}
}
