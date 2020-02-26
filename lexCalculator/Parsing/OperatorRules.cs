using System;
using lexCalculator.Types;

namespace lexCalculator.Parsing
{
	static class OperatorRules
	{
		public static int GetOperatorPriority(BinaryOperation operation)
		{
			switch (operation)
			{
				case BinaryOperation.Addition:
				case BinaryOperation.Substraction:
					return 0;
				case BinaryOperation.Multiplication:
				case BinaryOperation.Division:
					return 1;
				case BinaryOperation.Remainder:
					return 2;
				case BinaryOperation.Power:
					return 3;
				default:
					throw new ArgumentException("Unknown binary operator");
			}
		}

		public static bool IsOperatorLeftAssociative(BinaryOperation operation)
		{
			switch (operation)
			{
				case BinaryOperation.Addition:
				case BinaryOperation.Substraction:
				case BinaryOperation.Multiplication:
				case BinaryOperation.Division:
					return true;
				case BinaryOperation.Remainder:
				case BinaryOperation.Power:
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
