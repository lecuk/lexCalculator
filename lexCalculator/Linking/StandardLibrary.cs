using lexCalculator.Types;
using System.Collections.Generic;

namespace lexCalculator.Linking
{
	public static class StandardLibrary
	{
		public static CalculationContext GetContext()
		{
			CalculationContext context = new CalculationContext();

			foreach (KeyValuePair<string, double> pair in StandardVariables.Variables)
			{
				context.VariableTable.AssignNewItem(pair.Key, pair.Value);
			}

			foreach (KeyValuePair<string, UnaryOperation> pair in StandardFunctions.UnaryFunctions)
			{
				context.FunctionTable.AssignNewItem(pair.Key, new FinishedFunction( 
					new UnaryOperationTreeNode(pair.Value, new FunctionParameterTreeNode(0)), 
					context.VariableTable,
					context.FunctionTable,
					1));
			}

			foreach (KeyValuePair<string, BinaryOperation> pair in StandardFunctions.BinaryFunctions)
			{
				context.FunctionTable.AssignNewItem(pair.Key, new FinishedFunction(
					new BinaryOperationTreeNode(pair.Value, new FunctionParameterTreeNode(0), new FunctionParameterTreeNode(1)),
					context.VariableTable,
					context.FunctionTable,
					2));
			}

			foreach (KeyValuePair<string, ListOperation> pair in StandardFunctions.ListFunctions)
			{
				//context.AssignFunction(pair.Key, new Function(new string[] { "x" }, new UnaryOperationTreeNode(pair.Value, new VariableTreeNode("x"))));
			}

			return context;
		}
	}
}
