using System;
using System.Collections.Generic;
using System.Linq;
using lexCalculator.Types;

namespace lexCalculator.Parsing
{
	// it's a horrible class, but is's "Shitty" after all :D
	public class ShittyExpressionConstructor : IExpressionConstructor
	{
		class ConstructionContext
		{
			Token[] tokens;
			public int currentToken;

			public bool TryGetNextToken(out Token token)
			{
				if (currentToken > tokens.Length)
				{
					token = null;
					return false;
				}

				token = tokens[currentToken - 1];
				++currentToken;
				return true;
			}

			public bool TryPeekNextToken(out Token token)
			{
				if (currentToken > tokens.Length)
				{
					token = null;
					return false;
				}

				token = tokens[currentToken - 1];
				return true;
			}

			public void PutBackToken()
			{
				if (currentToken <= 0)
				{
					throw new Exception("No tokens to put back");
				}

				--currentToken;
			}

			public void EatLastToken()
			{
				if (currentToken > tokens.Length)
				{
					throw new Exception("No tokens to eat");
				}

				++currentToken;
			}

			public ConstructionContext(Token[] tokens)
			{
				this.tokens = (Token[])tokens.Clone();
				this.currentToken = 1;
			}
		}

		TreeNode ParseTerm(ConstructionContext context)
		{
			if (!context.TryGetNextToken(out Token token))
			{
				throw new Exception("No tokens for term to parse");
			}

			switch (token)
			{
				case SymbolToken symbolToken:
				{
					// (expression)
					if (symbolToken.Symbol == '(')
					{
						TreeNode node = ParseExpression(context, ')');
						if (context.TryPeekNextToken(out Token parentnesisEndToken) &&
						parentnesisEndToken is SymbolToken parentnesisEndSymbolToken &&
						parentnesisEndSymbolToken.Symbol == ')')
						{
							context.EatLastToken(); // eat right brackets
							return node;
						}
						throw new Exception("Mismatched brackets");
					}
					// |expression|
					else if (symbolToken.Symbol == '|')
					{
						TreeNode node = ParseExpression(context, '|');
						if (context.TryPeekNextToken(out Token parentnesisEndToken) && 
							parentnesisEndToken is SymbolToken parentnesisEndSymbolToken &&
							parentnesisEndSymbolToken.Symbol == '|')
						{
							context.EatLastToken(); // eat right brackets
							return new UnaryOperationTreeNode(UnaryOperation.AbsoluteValue, node);
						}
						throw new Exception("Mismatched brackets");
					}
					// -term
					else if (symbolToken.Symbol == '-')
					{
						return new UnaryOperationTreeNode(UnaryOperation.Negative, ParseTermWithPossibleFactorial(context));
					}
					// +term
					else if (symbolToken.Symbol == '+')
					{
						return ParseTermWithPossibleFactorial(context);
					}
					throw new Exception(String.Format("Unexpected symbol token when parsing term: '{0}'", symbolToken.Symbol));
				}

				case IdentifierToken identifierToken:
				{
					// variable
					if (!context.TryPeekNextToken(out Token nextToken))
					{
						return new VariableTreeNode(identifierToken.Identifier);
					}
					else if (!(nextToken is SymbolToken nextSymbolToken && nextSymbolToken.Symbol == '('))
					{
						return new VariableTreeNode(identifierToken.Identifier);
					}

					// if we are here, then it's a function
					context.EatLastToken(); // eat left brackets

					// check if it has any parameters
					// func()
					if (context.TryPeekNextToken(out Token emptyFuncToken) &&
						emptyFuncToken is SymbolToken emptyFuncSymbolToken &&
						emptyFuncSymbolToken.Symbol == ')')
					{
						context.EatLastToken(); // eat right brackets
						return new FunctionTreeNode(identifierToken.Identifier, new TreeNode[0]);
					}

					// parse each parameter
					// func(expression, ...)
					List<TreeNode> parameters = new List<TreeNode>();
					while (true)
					{
						TreeNode parameter = ParseExpression(context, ',', ')');
						parameters.Add(parameter);

						if (context.TryGetNextToken(out Token delimToken) && delimToken is SymbolToken delimSymbolToken)
						{
							if (delimSymbolToken.Symbol == ',') continue;
							if (delimSymbolToken.Symbol == ')') break;
							throw new Exception(String.Format("Can't parse function (unknown delimeter token '{0}')", delimSymbolToken));
						}
						// if we're here, then something is wrong
						throw new Exception(String.Format("Can't parse function (no delimeter token)"));
					}
					
					return new FunctionTreeNode(identifierToken.Identifier, parameters.ToArray());
				}

				// literal
				case NumberToken literalToken:
				{
					return new LiteralTreeNode(literalToken.Value);
				}
			}

			throw new Exception(String.Format("Unknown token type: '{0}'", token));
		}

		// stupid factorial. Why should it be after term, not before it?
		TreeNode ParseTermWithPossibleFactorial(ConstructionContext context)
		{
			TreeNode term = ParseTerm(context);

			while (context.TryPeekNextToken(out Token factorialToken) &&
				factorialToken is SymbolToken factorialSymbolToken &&
				factorialSymbolToken.Symbol == '!')
			{
				context.TryGetNextToken(out factorialToken); // eat factorial token

				term = new UnaryOperationTreeNode(UnaryOperation.Factorial, term);
			}

			return term;
		}

		void PopOperatorAndPushResult(Stack<TreeNode> termStack, Stack<char> operatorStack)
		{
			BinaryOperation operation = OperatorRules.CharToBinaryOperator(operatorStack.Pop());
			// note: operators are popped in the reverse order because stack.
			TreeNode rightChild = termStack.Pop();
			TreeNode leftChild = termStack.Pop();
			TreeNode result = new BinaryOperationTreeNode(operation, leftChild, rightChild);
			termStack.Push(result);
		}

		// expression is parsed using Dijkstra's shunting-yard algorithm.
		TreeNode ParseExpression(ConstructionContext context, params char[] stopSymbols)
		{
			Stack<TreeNode> termStack = new Stack<TreeNode>();
			Stack<char> operatorStack = new Stack<char>();
			termStack.Push(ParseTermWithPossibleFactorial(context)); // expression should have at least one term

			// then there should be exactly one binary operator and one term every iteration
			while (context.TryPeekNextToken(out Token token))
			{
				if (token is SymbolToken symbolToken)
				{
					if (ParserRules.IsBinaryOperatorChar(symbolToken.Symbol))
					{
						context.EatLastToken(); // eat binary operator

						char curOperator = symbolToken.Symbol;
						while (operatorStack.Count > 0)
						{
							char topOperator = operatorStack.Peek();
							if ((OperatorRules.GetOperatorPriority(topOperator) > OperatorRules.GetOperatorPriority(curOperator)) ||
								((OperatorRules.GetOperatorPriority(topOperator) == OperatorRules.GetOperatorPriority(curOperator)) &&
								OperatorRules.IsOperatorLeftAssociative(curOperator)))
							{
								PopOperatorAndPushResult(termStack, operatorStack);
							}
							else break;
						}
						operatorStack.Push(curOperator);

						termStack.Push(ParseTermWithPossibleFactorial(context));
						continue;
					}
					else if (stopSymbols.Contains(symbolToken.Symbol))
					{
						while (operatorStack.Count > 0)
						{
							PopOperatorAndPushResult(termStack, operatorStack);
						}
						return termStack.Pop();
					}
				}
				throw new Exception(String.Format("Unexpected token: '{0}', expected binary operator or stop-symbol", token));
			}

			if (stopSymbols.Length > 0) throw new Exception(String.Format("Unfinished expression: expected stop symbol '{0}'", stopSymbols[0]));

			while (operatorStack.Count > 0)
			{
				PopOperatorAndPushResult(termStack, operatorStack);
			}
			return termStack.Pop();
		}

		public TreeNode Construct(Token[] tokens)
		{
			TreeNode unfinishedTree = ParseExpression(new ConstructionContext(tokens));

			return unfinishedTree;
		}
	}
}
