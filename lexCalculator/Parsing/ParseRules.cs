using System;

namespace lexCalculator.Parsing
{
	public static class ParseRules
	{
		public static bool IsDecimalPointChar(char symbol)
		{
			return (symbol == '.');
		}

		public static bool IsExponentChar(char symbol)
		{
			return (Char.ToLower(symbol) == 'e');
		}

		public static bool IsSignChar(char symbol)
		{
			return (symbol == '+') || (symbol == '-');
		}
		
		public static bool IsValidNumberFirstChar(char symbol)
		{
			return Char.IsDigit(symbol) || IsDecimalPointChar(symbol);
		}

		public static bool IsValidNumberChar(char symbol, char lastSymbol, bool pointWasPut, bool exponentSignWasPut, bool exponentWasPut)
		{
			return (
				Char.IsDigit(symbol) || 
				(IsDecimalPointChar(symbol) && !exponentWasPut && !pointWasPut) || 
				(IsSignChar(symbol) && IsExponentChar(lastSymbol)) ||
				(IsExponentChar(symbol) && !exponentWasPut && Char.IsDigit(lastSymbol)));
		}

		public static bool IsValidIdentifierFirstChar(char symbol)
		{
			return (Char.IsLetter(symbol) || symbol == '_');
		}

		public static bool IsValidIdentifierChar(char symbol)
		{
			return (Char.IsLetterOrDigit(symbol) || symbol == '_');
		}

		public static bool IsValidSymbolChar(char symbol)
		{
			const string allSymbols = "+-*/^%!()|,?:=><{}&'";
			return (allSymbols.IndexOf(symbol) != -1);
		}

		// for example, + can be paired with = to make +=
		public static bool SymbolCanBePairedForAnotherSymbolToken(char symbol)
		{
			const string symbols = "+-*/^%?!=><&";
			return (symbols.IndexOf(symbol) != -1);
		}

		public static bool IsStopForIdentifierOrLiteralChar(char symbol)
		{
			return (Char.IsWhiteSpace(symbol) || IsValidSymbolChar(symbol));
		}
	}
}
