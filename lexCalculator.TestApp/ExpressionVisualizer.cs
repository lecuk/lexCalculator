using lexCalculator.Types;
using System;

namespace lexCalculator.TestApp
{
	class ExpressionVisualizer
	{
		readonly string[] UnaryToString = new string[]
		{
			"-",
			"sign",
			"sin",
			"cos",
			"tan",
			"cot",
			"sec",
			"csc",
			"arcsin",
			"arccos",
			"arctan",
			"arccot",
			"arcsec",
			"arccsc",
			"sinh",
			"cosh",
			"tanh",
			"coth",
			"sech",
			"csch",
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

		public void VisualizeAsTree(TreeNode node, string colorStr)
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

				case FunctionParameterTreeNode fpTreeNode:
				{
					Console.ForegroundColor = ConsoleColor.DarkGreen;
					Console.WriteLine(fpTreeNode);
					Console.ResetColor();
					break;
				}

				case IndexTreeNode iTreeNode:
				{
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine(iTreeNode);
					Console.ResetColor();
					break;
				}

				case FunctionTreeNode fTreeNode:
				{
					Console.ForegroundColor = ConsoleColor.Magenta;
					Console.WriteLine(String.Format("{0}()", fTreeNode.Name));
					Console.ResetColor();
					foreach (TreeNode child in fTreeNode.Parameters)
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

		public void VisualizeAsPrefixEquation(TreeNode node)
		{
			switch (node)
			{
				case LiteralTreeNode lTreeNode:
				{
					Console.Write(lTreeNode.Value.ToString("G6", System.Globalization.CultureInfo.InvariantCulture));
					Console.Write(' ');
					break;
				}

				case VariableTreeNode vTreeNode:
				{
					Console.Write(vTreeNode.Name);
					Console.Write(' ');
					break;
				}

				case FunctionParameterTreeNode fpTreeNode:
				{
					Console.Write(fpTreeNode);
					Console.Write(' ');
					break;
				}

				case IndexTreeNode iTreeNode:
				{
					Console.Write(iTreeNode);
					Console.Write(' ');
					break;
				}

				case FunctionTreeNode fTreeNode:
				{
					Console.Write(fTreeNode.Name);
					Console.Write(' ');
					foreach (TreeNode child in fTreeNode.Parameters)
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

		public void VisualizeAsPostfixEquation(TreeNode node)
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

				case FunctionParameterTreeNode fpTreeNode:
				{
					Console.Write(fpTreeNode);
					Console.Write(' ');
					break;
				}

				case IndexTreeNode iTreeNode:
				{
					Console.Write(iTreeNode);
					Console.Write(' ');
					break;
				}

				case FunctionTreeNode fTreeNode:
				{
					foreach (TreeNode child in fTreeNode.Parameters)
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
