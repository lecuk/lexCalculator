using System;
using System.Collections.Generic;
using lexCalculator.Construction;
using lexCalculator.Parsing;

namespace lexCalculator.TestApp
{
	class Program
	{
		static void Main(string[] args)
		{
			ITokenizer tokenizer = new ShittyTokenizer();
			IExpressionConstructor constructor = new ShittyExpressionConstructor();
			ExpressionVisualizer visualizer = new ExpressionVisualizer();

			while (true)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("> ");
				string input = Console.ReadLine();
				Console.ResetColor();

				try
				{
					Token[] tokens = tokenizer.Tokenize(input);
					ExpressionTreeNode tree = constructor.Construct(tokens);

					Console.WriteLine();
					Console.Write("Tokens: [ ");
					foreach (Token token in tokens)
					{
						switch (token)
						{
							case SymbolToken _:
								Console.ForegroundColor = ConsoleColor.Yellow;
								break;
							case IdentifierToken _:
								Console.ForegroundColor = ConsoleColor.Cyan;
								break;
							case LiteralToken _:
							default:
								Console.ForegroundColor = ConsoleColor.Gray;
								break;
						}
						Console.Write(token.ToString());
						Console.ResetColor();
						Console.Write(' ');
					}
					Console.WriteLine("]");
					Console.WriteLine();

					Console.WriteLine("Normal form:");
					visualizer.VisualizeAsNormalEquation(tree);
					Console.WriteLine();
					Console.WriteLine();
					Console.WriteLine("Postfix form:");
					visualizer.VisualizeAsPostfixEquation(tree);
					Console.WriteLine();
					Console.WriteLine();
					Console.WriteLine("Prefix form:");
					visualizer.VisualizeAsPrefixEquation(tree);
					Console.WriteLine();
					Console.WriteLine();
					Console.WriteLine("Tree form:");
					visualizer.VisualizeAsTree(tree, "");
				}
				catch (Exception ex)
				{
					Console.WriteLine(String.Format("Error: {0}", ex.Message));
				}
				Console.WriteLine();
			}
		}
	}
}
