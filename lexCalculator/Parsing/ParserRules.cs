using System;

namespace lexCalculator.Parsing
{
	public static class ParserRules
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
			return (symbol == '+' || symbol == '-');
		}
		
		public static bool IsBracketChar(char symbol)
		{
			return ((symbol == '(')|| (symbol == ')') || (symbol == '|'));
		}

		public static bool IsBinaryOperatorChar(char symbol)
		{
			switch (symbol)
			{
				case '+':
				case '-':
				case '*':
				case '/':
				case '^':
				case '%':
					return true;
			}
			return false;
		}
		
		public static bool IsValidNumberFirstChar(char symbol)
		{
			return (Char.IsDigit(symbol) || IsDecimalPointChar(symbol));
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
			return (Char.IsLetter(symbol) || symbol == '_' || symbol == '#');
		}

		public static bool IsValidIdentifierChar(char symbol)
		{
			return (Char.IsLetterOrDigit(symbol) || symbol == '_' || symbol == '#');
		}

		public static bool IsValidSymbolChar(char symbol)
		{
			const string allSymbols = "+-*/^%!()|,";
			return (allSymbols.IndexOf(symbol) != -1);
		}

		public static bool IsStopForIdentifierOrLiteralChar(char symbol)
		{
			return (Char.IsWhiteSpace(symbol) || IsValidSymbolChar(symbol));
		}
	}
}
