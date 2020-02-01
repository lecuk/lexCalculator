using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace lexCalculator.Parsing
{
	public class ShittyTokenizer : ITokenizer
	{
		Token GetSymbol(StringReader reader)
		{
			char symbol = (char)reader.Read();
			return new SymbolToken(symbol);
		}

		Token GetNumber(StringReader reader)
		{
			StringBuilder literalBuilder = new StringBuilder();
			
			char symbol, lastSymbol = (char)0;
			bool pointWasPut = false, exponentSignWasPut = false, exponentWasPut = false;

			while (true)
			{
				int peekResult = reader.Peek();
				if (peekResult == -1) break;
				symbol = (char)peekResult;
				
				if (ParserRules.IsValidNumberChar(symbol, lastSymbol, pointWasPut, exponentSignWasPut, exponentWasPut))
				{
					if (ParserRules.IsDecimalPointChar(symbol)) pointWasPut = true;
					if (ParserRules.IsSignChar(symbol))  exponentSignWasPut = true;
					if (ParserRules.IsExponentChar(symbol))  exponentWasPut = true;

					literalBuilder.Append(symbol);
					lastSymbol = symbol;
					reader.Read();
					continue;
				}

				if (ParserRules.IsStopForIdentifierOrLiteralChar(symbol)) break;

				throw new ArgumentException(String.Format("Unexpected character in number token: \"{0}\"", symbol));
			}

			return new NumberToken(Double.Parse(literalBuilder.ToString(), System.Globalization.NumberStyles.AllowExponent | System.Globalization.NumberStyles.AllowDecimalPoint, System.Globalization.CultureInfo.InvariantCulture));
		}

		Token GetIdentifier(StringReader reader)
		{
			StringBuilder identifierBuilder = new StringBuilder();

			char symbol;
			
			while (true)
			{
				int peekResult = reader.Peek();
				if (peekResult == -1) break;
				symbol = (char)peekResult;

				if (ParserRules.IsValidIdentifierChar(symbol))
				{
					identifierBuilder.Append(symbol);
					reader.Read();
					continue;
				}

				if (ParserRules.IsStopForIdentifierOrLiteralChar(symbol)) break;

				throw new ArgumentException(String.Format("Unexpected character in identifier token: \"{0}\"", symbol));
			}

			return new IdentifierToken(identifierBuilder.ToString());
		}

		Token GetNextToken(StringReader reader)
		{
			int peekResult = reader.Peek();
			if (peekResult == -1) throw new InvalidOperationException("Stream is empty");

			char firstSymbol = (char)peekResult;

			if (ParserRules.IsValidNumberFirstChar(firstSymbol))
			{
				return GetNumber(reader);
			}
			if (ParserRules.IsValidIdentifierFirstChar(firstSymbol))
			{
				return GetIdentifier(reader);
			}
			if (ParserRules.IsValidSymbolChar(firstSymbol))
			{
				return GetSymbol(reader);
			}

			throw new ArgumentException(String.Format("Unexpected token: \"{0}\"", firstSymbol));
		}

		void SkipWhiteSpaces(StringReader reader)
		{
			int readResult = reader.Peek();
			while (readResult != -1 && Char.IsWhiteSpace((char)readResult))
			{
				reader.Read();
				readResult = reader.Peek();
			}
		}

		public Token[] Tokenize(string expression)
		{
			List<Token> tokens = new List<Token>();
			using (var reader = new StringReader(expression))
			{
				SkipWhiteSpaces(reader);
				while (reader.Peek() != -1)
				{
					Token token = GetNextToken(reader);
					tokens.Add(token);
					SkipWhiteSpaces(reader);
				}
			}
			return tokens.ToArray();
		}
	}
}
