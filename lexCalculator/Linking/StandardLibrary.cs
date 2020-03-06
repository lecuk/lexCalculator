using lexCalculator.Types;
using lexCalculator.Types.TreeNodes;
using lexCalculator.Types.Operations;
using System;

namespace lexCalculator.Linking
{
	public static class StandardLibrary
	{
		static void AssignFunction(CalculationContext context, Operation operation)
		{
			FinishedFunction funcToAssign;
			switch (operation)
			{
				case UnaryOperation uOperation:
					funcToAssign = new FinishedFunction(
						new UnaryOperationTreeNode(uOperation, new FunctionParameterTreeNode(0)),
						null, null, 1);
					break;

				case BinaryOperation bOperation:
					funcToAssign = new FinishedFunction(
						new BinaryOperationTreeNode(bOperation, new FunctionParameterTreeNode(0), new FunctionParameterTreeNode(1)),
						null, null, 2);
					break;

				default: return;
			}

			context.FunctionTable.AssignNewItem(operation.FunctionName, funcToAssign);
		}

		public static CalculationContext GenerateStandardContext()
		{
			CalculationContext context = new CalculationContext();

			context.VariableTable.AssignNewItem("pi", Math.PI);
			context.VariableTable.AssignNewItem("e", Math.E);
			context.VariableTable.AssignNewItem("phi", (Math.Sqrt(5) + 1.0) / 2);
			
			AssignFunction(context, UnaryOperation.Sign);
			AssignFunction(context, UnaryOperation.AbsoluteValue);

			AssignFunction(context, UnaryOperation.Sine);
			AssignFunction(context, UnaryOperation.Cosine);
			AssignFunction(context, UnaryOperation.Tangent);
			AssignFunction(context, UnaryOperation.Cotangent);
			AssignFunction(context, UnaryOperation.Secant);
			AssignFunction(context, UnaryOperation.Cosecant);

			AssignFunction(context, UnaryOperation.Exponent);
			AssignFunction(context, UnaryOperation.NaturalLogarithm);
			AssignFunction(context, BinaryOperation.Logarithm);

			AssignFunction(context, UnaryOperation.SquareRoot);
			AssignFunction(context, UnaryOperation.CubeRoot);
			AssignFunction(context, BinaryOperation.NthRoot);

			return context;
		}
	}
}
