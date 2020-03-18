using System;
using System.Collections.Generic;

namespace lexCalculator.Types.Tokens
{
	public class SymbolToken : Token
	{
		public string SymbolString { get; private set; }

		public SymbolToken(string symbolString)
		{
			SymbolString = symbolString;
		}

		public override string ToString()
		{
			return SymbolString;
		}
	}
}
