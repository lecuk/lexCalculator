using lexCalculator.Calculation;
using lexCalculator.Linking;
using lexCalculator.Parsing;
using lexCalculator.Types;
using lexCalculator.Types.Operations;
using lexCalculator.Types.Tokens;
using lexCalculator.Types.TreeNodes;
using System;
using System.Collections.Generic;

namespace lexInterpreter
{
	class StatementParser
	{
		public class ConstructionContext
		{
			Token[][] lines;
			public int currentToken;

			public bool TryGetNextNonEmptyLine(out Token[] line)
			{
				do
				{
					if (currentToken > lines.Length)
					{
						line = null;
						return false;
					}

					line = lines[currentToken - 1];
					++currentToken;
				}
				while (line.Length == 0);

				return true;
			}

			public void PutBackLine()
			{
				if (currentToken > 0) --currentToken;
			}
			
			public ConstructionContext(Token[][] lines)
			{
				this.lines = (Token[][])lines.Clone();
				this.currentToken = 1;
			}
		}

		DefaultParser expressionParser;
		DefaultLinker expressionLinker;
		ExecutionContext executionContext;

		private static readonly IReadOnlyDictionary<string, BinaryOperatorOperation> AssignOperationDictionary 
			= new Dictionary<string, BinaryOperatorOperation>
		{
			{ "=", null },
			{ "+=", BinaryOperatorOperation.Addition },
			{ "-=", BinaryOperatorOperation.Substraction },
			{ "*=", BinaryOperatorOperation.Multiplication },
			{ "/=", BinaryOperatorOperation.Division },
			{ "^=", BinaryOperatorOperation.Power },
			{ "%=", BinaryOperatorOperation.Remainder }
		};

		public StatementParser()
		{
			// i know, horrible code. Forgive me
			expressionParser = new DefaultParser();
			expressionLinker = new DefaultLinker(true);
			executionContext = new ExecutionContext(
				StandardLibrary.GenerateStandardContext(),
				new ExpressionLexer(),
				expressionParser,
				expressionLinker,
				new TreeCalculator());
		}

		public Statement ParseLine(ConstructionContext constructionContext, Token[] line)
		{
			// empty line
			if (line.Length == 0) throw new Exception("Empty line was passed to parser");

			// firstly, we determine type of token
			Token firstToken = line[0];

			switch (firstToken)
			{
				case KeywordToken keywordToken:
					return ParseKeywordToken(constructionContext, line, keywordToken);

				case SymbolToken symbolToken:
					return ParseSymbolToken(constructionContext, line, symbolToken);

				case IdentifierToken identifierToken:
					return ParseIdentifierToken(constructionContext, line, identifierToken);

				default:
					throw new Exception("Unexpected token type");
			}
		}

		private bool IsAssignmentToken(Token token, out SymbolToken outAssignToken)
		{
			if ((token is SymbolToken assignToken)
			&& ((assignToken.SymbolString == "=")
			|| (assignToken.SymbolString == "+=")
			|| (assignToken.SymbolString == "-=")
			|| (assignToken.SymbolString == "*=")
			|| (assignToken.SymbolString == "/=")
			|| (assignToken.SymbolString == "^=")
			|| (assignToken.SymbolString == "%=")))
			{
				outAssignToken = assignToken;
				return true;
			}
			outAssignToken = null;
			return false;
		}

		private Statement ParseIdentifierToken(ConstructionContext constructionContext, Token[] line, IdentifierToken identifierToken)
		{
			if (line.Length < 2) throw new Exception("Lone identifier token is invalid");

			// if next token is assign symbol, then we know we define a variable here
			if (IsAssignmentToken(line[1], out SymbolToken assignToken))
			{
				return ParseVariableAssignment(constructionContext, line, identifierToken, assignToken);
			}

			// the hardert part: parse function definition
			return ParseFunctionAssignment(constructionContext, line, identifierToken);
		}

		private Statement ParseVariableAssignment(ConstructionContext constructionContext, Token[] line, IdentifierToken identifierToken, SymbolToken assignToken)
		{
			Token[] expressionTokens = new Token[line.Length - 2];
			for (int i = 2; i < line.Length; ++i)
			{
				expressionTokens[i - 2] = line[i];
			}

			TreeNode expression = expressionParser.ParseExpression(new DefaultParser.ConstructionContext(expressionTokens));
			
			return new VariableAssignmentStatement(executionContext, identifierToken.Identifier,
				AssignOperationDictionary[assignToken.SymbolString], expression);
		}

		private Statement ParseFunctionAssignment(ConstructionContext constructionContext, Token[] line, IdentifierToken identifierToken)
		{
			// firstly we divide statement into 2 parts
			int assignIndex = -1;
			for (int i = 2; i < line.Length; ++i)
			{
				if (IsAssignmentToken(line[i], out SymbolToken assignFunctionToken) && assignFunctionToken.SymbolString == "=")
				{
					assignIndex = i;
					break;
				}
			}

			if (assignIndex == -1) throw new Exception("No assignment token");

			Token[] definitionPart = new Token[assignIndex];
			for (int i = 0; i < assignIndex; ++i)
			{
				definitionPart[i] = line[i];
			}
			TreeNode functionDefinition = expressionParser.ParseExpression(new DefaultParser.ConstructionContext(definitionPart));

			// if first part looks like a function with parameters
			if (functionDefinition is UndefinedFunctionTreeNode ufTreeNode)
			{
				string[] parameterNames = new string[ufTreeNode.Parameters.Length];
				for (int i = 0; i < parameterNames.Length; ++i)
				{
					if (ufTreeNode.Parameters[i] is UndefinedVariableTreeNode parameterNode)
					{
						parameterNames[i] = parameterNode.Name;
					}
					else throw new Exception("Invalid parameter in function definition");
				}

				Token[] treePart = new Token[line.Length - assignIndex - 1];
				for (int i = assignIndex + 1; i < line.Length; ++i)
				{
					treePart[i - assignIndex - 1] = line[i];
				}

				TreeNode functionTree = expressionParser.ParseExpression(new DefaultParser.ConstructionContext(treePart));

				return new FunctionDefinitionStatement(executionContext, ufTreeNode.Name, parameterNames, functionTree);
			}
			else throw new Exception("Invalid assignment, what are you trying to define?");
		}

		private Statement ParseKeywordToken(ConstructionContext constructionContext, Token[] tokens, KeywordToken firstToken)
		{
			switch (firstToken.KeywordType)
			{
				case KeywordToken.Type.Import:
					return ParseImportStatement(constructionContext, tokens, firstToken);

				case KeywordToken.Type.Input:
					return ParseInputStatement(constructionContext, tokens, firstToken);

				case KeywordToken.Type.Output:
					return ParseOutputStatement(constructionContext, tokens, firstToken);

				case KeywordToken.Type.If:
					return ParseIf(constructionContext, tokens, firstToken);

				case KeywordToken.Type.Else:
					throw new Exception("Else keyword is not expected here");

				case KeywordToken.Type.While:
					return ParseWhile(constructionContext, tokens, firstToken);

				case KeywordToken.Type.Exit:
					return new ExitStatement(executionContext);

				case KeywordToken.Type.Newline:
					return new OutputNewLineStatement(executionContext);

				default:
					throw new Exception("Unknown keyword token");
			}
		}

		private Statement ParseSymbolToken(ConstructionContext constructionContext, Token[] tokens, SymbolToken firstToken)
		{
			if (firstToken.SymbolString == "{")
			{
				return ParseMultiStatement(constructionContext);
			}
			else throw new Exception("Unexpected symbol at the start of line");
		}

		private MultiStatement ParseMultiStatement(ConstructionContext constructionContext)
		{
			List<Statement> statements = new List<Statement>();

			while (constructionContext.TryGetNextNonEmptyLine(out Token[] line))
			{
				if (line[0] is SymbolToken symbolToken && symbolToken.SymbolString == "}") break;

				Statement statement = ParseLine(constructionContext, line);
				statements.Add(statement);
			}

			return new MultiStatement(executionContext, statements.ToArray());
		}

		private ImportStatement ParseImportStatement(ConstructionContext constructionContext, Token[] tokens, KeywordToken firstToken)
		{
			if (tokens.Length < 2) throw new Exception("Not enough tokens for import statement");
			if (tokens.Length > 2) throw new Exception("Excess tokens for import statement");

			if (tokens[1] is StringToken pathToken)
			{
				return new ImportStatement(executionContext, pathToken.Text);
			}
			else throw new Exception("Import path is not a string");
		}

		private InputStatement ParseInputStatement(ConstructionContext constructionContext, Token[] tokens, KeywordToken firstToken)
		{
			if (tokens.Length < 2) throw new Exception("Not enough tokens for input statement");
			if (tokens.Length > 2) throw new Exception("Too many tokens for input statement");

			if (tokens[1] is IdentifierToken variableToken)
			{
				return new InputStatement(executionContext, variableToken.Identifier);
			}
			else throw new Exception("Input target is not a variable");
		}

		private Statement ParseOutputStatement(ConstructionContext constructionContext, Token[] tokens, KeywordToken firstToken)
		{
			if (tokens.Length < 2) throw new Exception("Not enough tokens for output statement");

			if (tokens[1] is StringToken stringToken)
			{
				if (tokens.Length > 2) throw new Exception("Too many tokens for output statement");
				return new OutputStringStatement(executionContext, stringToken.Text);
			}

			Token[] expressionTokens = new Token[tokens.Length - 1];
			for (int i = 1; i < tokens.Length; ++i)
			{
				expressionTokens[i - 1] = tokens[i];
			}

			TreeNode expression = expressionParser.ParseExpression(new DefaultParser.ConstructionContext(expressionTokens));

			return new OutputStatement(executionContext, expression);
		}

		private Statement ParseIf(ConstructionContext constructionContext, Token[] tokens, KeywordToken firstToken)
		{
			if (tokens.Length < 2) throw new Exception("Not enough tokens for if statement");

			Token[] expressionTokens = new Token[tokens.Length - 1];
			for (int i = 1; i < tokens.Length; ++i)
			{
				expressionTokens[i - 1] = tokens[i];
			}

			TreeNode condition = expressionParser.ParseExpression(new DefaultParser.ConstructionContext(expressionTokens));
			if (constructionContext.TryGetNextNonEmptyLine(out Token[] thenLine))
			{
				Statement ifTrue = ParseLine(constructionContext, thenLine);

				// if ~else exists
				if (constructionContext.TryGetNextNonEmptyLine(out Token[] elseKeywordLine))
				{
					if ((elseKeywordLine[0] is KeywordToken elseKeyword)
					&& (elseKeyword.KeywordType == KeywordToken.Type.Else))
					{
						if (elseKeywordLine.Length > 1) throw new Exception("Else line should not contain anything except keyword \"~else\"");

						if (constructionContext.TryGetNextNonEmptyLine(out Token[] elseLine))
						{
							Statement ifFalse = ParseLine(constructionContext, elseLine);
							return new ConditionalStatement(executionContext, condition, ifTrue, ifFalse);
						}
						else throw new Exception("No else statement");
					}
					else constructionContext.PutBackLine();
				}
				return new ConditionalStatement(executionContext, condition, ifTrue);
			}
			else throw new Exception("No then statement");
		}

		private Statement ParseWhile(ConstructionContext constructionContext, Token[] tokens, KeywordToken firstToken)
		{
			if (tokens.Length < 2) throw new Exception("Not enough tokens for while statement");

			Token[] expressionTokens = new Token[tokens.Length - 1];
			for (int i = 1; i < tokens.Length; ++i)
			{
				expressionTokens[i - 1] = tokens[i];
			}

			TreeNode condition = expressionParser.ParseExpression(new DefaultParser.ConstructionContext(expressionTokens));
			if (constructionContext.TryGetNextNonEmptyLine(out Token[] thenLine))
			{
				Statement whileTrue = ParseLine(constructionContext, thenLine);

				return new LoopStatement(executionContext, condition, whileTrue);
			}
			else throw new Exception("No while body statement");
		}

		public Statement ParseLines(Token[][] lines)
		{
			List<Statement> statements = new List<Statement>();
			ConstructionContext constructionContext = new ConstructionContext(lines);

			while (constructionContext.TryGetNextNonEmptyLine(out Token[] line))
			{
				Statement statement = ParseLine(constructionContext, line);
				statements.Add(statement);
			}

			if (statements.Count == 0) return new EmptyStatement(executionContext);
			if (statements.Count == 1) return statements[0];

			return new MultiStatement(executionContext, statements.ToArray());
		}
	}
}
