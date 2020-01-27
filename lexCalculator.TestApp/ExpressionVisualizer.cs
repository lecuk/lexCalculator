using lexCalculator.Construction;
using System;

namespace lexCalculator.TestApp
{
	class ExpressionVisualizer
	{
		readonly string[] UnaryToString = new string[]
		{
			"-",
			"sin",
			"cos",
			"tan",
			"cot",
			"exp",
			"ln",
			"sqrt",
			"cbrt",
			"floor",
			"ceil",
			"abs",
			"!"
		};

		readonly string[] BinaryToString = new string[]
		{
			"+",
			"-",
			"*",
			"/",
			"^",
			"%",
			"log",
			"nrt"
		};

		public void VisualizeAsTree(ExpressionTreeNode node, string colorStr)
		{
			for (int i = 0; i < colorStr.Length; ++i)
			{
				if (colorStr[i] == '0') Console.ForegroundColor = ConsoleColor.Magenta;
				if (colorStr[i] == '1') Console.ForegroundColor = ConsoleColor.Red;
				if (colorStr[i] == '2') Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write("|   ");
				Console.ResetColor();
			}

			switch (node)
			{
				case LiteralTreeNode lTreeNode:
				{
					Console.WriteLine(lTreeNode.Value.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture));
					break;
				}

				case VariableTreeNode vTreeNode:
				{
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine(vTreeNode.Name);
					Console.ResetColor();
					break;
				}
				
				case FunctionTreeNode fTreeNode:
				{
					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.WriteLine(String.Format("{0}()", fTreeNode.Name));
					Console.ResetColor();
					foreach (ExpressionTreeNode child in fTreeNode.Parameters)
					{
						VisualizeAsTree(child, colorStr + '0');
					}
					break;
				}

				case UnaryOperationTreeNode uTreeNode:
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(String.Format("[{0}]", UnaryToString[(int)uTreeNode.Operation]));
					Console.ResetColor();
					VisualizeAsTree(uTreeNode.Child, colorStr + '1');
					break;
				}

				case BinaryOperationTreeNode bTreeNode:
				{
					Console.ForegroundColor = ConsoleColor.Yellow;
					Console.WriteLine(String.Format("[{0}]", BinaryToString[(int)bTreeNode.Operation]));
					Console.ResetColor();
					VisualizeAsTree(bTreeNode.LeftChild, colorStr + '2');
					VisualizeAsTree(bTreeNode.RightChild, colorStr + '2');
					break;
				}
			}

			/*
			for (int i = 0; i < level; ++i)
			{
				Console.Write("|   ");
			}
			Console.WriteLine();
			*/
		}

		public void VisualizeAsNormalEquation(ExpressionTreeNode node)
		{
			switch (node)
			{
				case LiteralTreeNode lTreeNode:
				{
					Console.Write(lTreeNode.Value.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture));
					break;
				}

				case VariableTreeNode vTreeNode:
				{
					Console.Write(vTreeNode.Name);
					break;
				}

				case FunctionTreeNode fTreeNode:
				{
					Console.Write(fTreeNode.Name);
					Console.Write('(');
					if (fTreeNode.Parameters.Length > 0) VisualizeAsNormalEquation(fTreeNode.Parameters[0]);
					for (int i = 1; i < fTreeNode.Parameters.Length; ++i)
					{
						Console.Write(", ");
						VisualizeAsNormalEquation(fTreeNode.Parameters[i]);
					}
					Console.Write(")");
					break;
				}

				case UnaryOperationTreeNode uTreeNode:
				{
					if (uTreeNode.Operation == UnaryOperation.Factorial)
					{
						Console.Write("(");
						VisualizeAsNormalEquation(uTreeNode.Child);
						Console.Write(")!");
					}
					else
					{
						Console.Write(String.Format("{0}(", UnaryToString[(int)uTreeNode.Operation]));
						VisualizeAsNormalEquation(uTreeNode.Child);
						Console.Write(")");
					}
					break;
				}

				case BinaryOperationTreeNode bTreeNode:
				{
					if (bTreeNode.Operation == BinaryOperation.Logarithm || bTreeNode.Operation == BinaryOperation.NRoot)
					{
						Console.Write(BinaryToString[(int)bTreeNode.Operation]);
						Console.Write('(');
						VisualizeAsNormalEquation(bTreeNode.LeftChild);
						Console.Write(", ");
						VisualizeAsNormalEquation(bTreeNode.RightChild);
						Console.Write(")");
					}
					else
					{
						VisualizeAsNormalEquation(bTreeNode.LeftChild);
						Console.Write(String.Format(" {0} ", BinaryToString[(int)bTreeNode.Operation]));
						VisualizeAsNormalEquation(bTreeNode.RightChild);
					}
					break;
				}
			}
		}

		public void VisualizeAsPrefixEquation(ExpressionTreeNode node)
		{
			switch (node)
			{
				case LiteralTreeNode lTreeNode:
				{
					Console.Write(lTreeNode.Value.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture));
					Console.Write(' ');
					break;
				}

				case VariableTreeNode vTreeNode:
				{
					Console.Write(vTreeNode.Name);
					Console.Write(' ');
					break;
				}

				case FunctionTreeNode fTreeNode:
				{
					Console.Write(fTreeNode.Name);
					Console.Write(' ');
					foreach (ExpressionTreeNode child in fTreeNode.Parameters)
					{
						VisualizeAsPrefixEquation(child);
					}
					break;
				}

				case UnaryOperationTreeNode uTreeNode:
				{
					Console.Write(UnaryToString[(int)uTreeNode.Operation]);
					Console.Write(' ');
					VisualizeAsPrefixEquation(uTreeNode.Child);
					break;
				}

				case BinaryOperationTreeNode bTreeNode:
				{
					Console.Write(BinaryToString[(int)bTreeNode.Operation]);
					Console.Write(' ');
					VisualizeAsPrefixEquation(bTreeNode.LeftChild);
					VisualizeAsPrefixEquation(bTreeNode.RightChild);
					break;
				}
			}
		}

		public void VisualizeAsPostfixEquation(ExpressionTreeNode node)
		{
			switch (node)
			{
				case LiteralTreeNode lTreeNode:
				{
					Console.Write(lTreeNode.Value.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture));
					Console.Write(' ');
					break;
				}

				case VariableTreeNode vTreeNode:
				{
					Console.Write(vTreeNode.Name);
					Console.Write(' ');
					break;
				}

				case FunctionTreeNode fTreeNode:
				{
					foreach (ExpressionTreeNode child in fTreeNode.Parameters)
					{
						VisualizeAsPostfixEquation(child);
					}
					Console.Write(fTreeNode.Name);
					Console.Write(' ');
					break;
				}

				case UnaryOperationTreeNode uTreeNode:
				{
					VisualizeAsPostfixEquation(uTreeNode.Child);
					Console.Write(UnaryToString[(int)uTreeNode.Operation]);
					Console.Write(' ');
					break;
				}

				case BinaryOperationTreeNode bTreeNode:
				{
					VisualizeAsPostfixEquation(bTreeNode.LeftChild);
					VisualizeAsPostfixEquation(bTreeNode.RightChild);
					Console.Write(BinaryToString[(int)bTreeNode.Operation]);
					Console.Write(' ');
					break;
				}
			}
		}
	}
}
