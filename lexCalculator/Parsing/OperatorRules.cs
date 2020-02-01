using System;
using lexCalculator.Types;

namespace lexCalculator.Parsing
{
	static class OperatorRules
	{
		public static int GetOperatorPriority(char symbol)
		{
			if (!ParserRules.IsBinaryOperatorChar(symbol)) throw new ArgumentException("Not a binary operator");

			switch (symbol)
			{
				case '+':
				case '-':
					return 0;
				case '*':
				case '/':
					return 1;
				case '%':
					return 2;
				case '^':
					return 3;
				default:
					throw new ArgumentException("Unknown binary operator");
			}
		}

		public static bool IsOperatorLeftAssociative(char symbol)
		{
			if (!ParserRules.IsBinaryOperatorChar(symbol)) throw new ArgumentException("Not a binary operator");

			switch (symbol)
			{
				case '+':
				case '-':
				case '*':
				case '/':
					return true;
				case '%':
				case '^':
					return false;
				default:
					throw new ArgumentException("Unknown binary operator");
			}
		}

		public static BinaryOperation CharToBinaryOperator(char symbol)
		{
			if (!ParserRules.IsBinaryOperatorChar(symbol)) throw new ArgumentException("Not a binary operator");

			switch (symbol)
			{
				case '+': return BinaryOperation.Addition;
				case '-': return BinaryOperation.Substraction;
				case '*': return BinaryOperation.Multiplication;
				case '/': return BinaryOperation.Division;
				case '^': return BinaryOperation.Power;
				case '%': return BinaryOperation.Remainder;
				default:
					throw new ArgumentException("Unknown binary operator");
			}
		}
	}
}
