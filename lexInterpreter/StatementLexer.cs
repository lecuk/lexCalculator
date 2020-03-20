using lexCalculator.Parsing;
using lexCalculator.Types;
using lexCalculator.Types.Tokens;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace lexInterpreter
{
	class StatementLexer : ILexer
	{
		ExpressionLexer expressionLexer = new ExpressionLexer();

		public Token[] Tokenize(string expression)
		{
			List<Token> tokens = new List<Token>();
			using (var reader = new StringReader(expression))
			{
				expressionLexer.SkipWhiteSpaces(reader);
				while (reader.Peek() != -1)
				{
					Token token = GetNextToken(reader);
					tokens.Add(token);
					expressionLexer.SkipWhiteSpaces(reader);
				}
			}
			return tokens.ToArray();
		}

		private Token GetNextToken(StringReader reader)
		{
			int peekResult = reader.Peek();
			if (peekResult == -1) throw new InvalidOperationException("Stream is empty");

			char firstSymbol = (char)peekResult;

			if (ParseRules.IsValidNumberFirstChar(firstSymbol))
			{
				return expressionLexer.GetNumber(reader);
			}
			if (ParseRules.IsValidIdentifierFirstChar(firstSymbol))
			{
				return expressionLexer.GetIdentifier(reader);
			}
			if (ParseRules.IsValidSymbolChar(firstSymbol))
			{
				return expressionLexer.GetSymbol(reader);
			}
			if (firstSymbol == '~')
			{
				return GetKeyword(reader);
			}
			if (firstSymbol == '\"')
			{
				return GetString(reader);
			}

			throw new ArgumentException(String.Format("Unexpected token: \"{0}\"", firstSymbol));
		}
		
		private Token GetString(StringReader reader)
		{
			reader.Read(); //eat '\"'
			StringBuilder builder = new StringBuilder();
			while (true)
			{
				int readResult = reader.Read();
				if (readResult == -1) throw new InvalidOperationException("Stream is empty");

				char c = (char)readResult;
				if (c == '\"') break;
				builder.Append(c);
			}
			return new StringToken(builder.ToString());
		}

		private Token GetKeyword(StringReader reader)
		{
			reader.Read(); //eat '~'
			Token pseudoIdentifier = expressionLexer.GetIdentifier(reader);

			if (pseudoIdentifier is IdentifierToken identifier)
			{
				if (identifier.Identifier == "input")
					return new KeywordToken(KeywordToken.Type.Input, identifier.Identifier);

				if (identifier.Identifier == "output")
					return new KeywordToken(KeywordToken.Type.Output, identifier.Identifier);

				if (identifier.Identifier == "if")
					return new KeywordToken(KeywordToken.Type.If, identifier.Identifier);

				if (identifier.Identifier == "else")
					return new KeywordToken(KeywordToken.Type.Else, identifier.Identifier);

				if (identifier.Identifier == "while")
					return new KeywordToken(KeywordToken.Type.While, identifier.Identifier);

				if (identifier.Identifier == "import")
					return new KeywordToken(KeywordToken.Type.Import, identifier.Identifier);

				if (identifier.Identifier == "exit")
					return new KeywordToken(KeywordToken.Type.Exit, identifier.Identifier);

				if (identifier.Identifier == "newline")
					return new KeywordToken(KeywordToken.Type.Newline, identifier.Identifier);
			}

			throw new Exception("Invalid keyword");
		}
	}
}
