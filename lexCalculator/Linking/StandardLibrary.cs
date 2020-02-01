using lexCalculator.Types;
using System.Collections.Generic;

namespace lexCalculator.Linking
{
	public static class StandardLibrary
	{
		public static ExpressionContext GetContext()
		{
			ExpressionContext context = new ExpressionContext();

			foreach (KeyValuePair<string, UnaryOperation> pair in StandardFunctions.UnaryFunctions)
			{
				context.AssignFunction(pair.Key, new Function( 
					new UnaryOperationTreeNode(pair.Value, new FunctionParameterTreeNode(0)), 
					1));
			}

			foreach (KeyValuePair<string, BinaryOperation> pair in StandardFunctions.BinaryFunctions)
			{
				context.AssignFunction(pair.Key, new Function(
					new BinaryOperationTreeNode(pair.Value, new FunctionParameterTreeNode(0), new FunctionParameterTreeNode(1)),
					2));
			}

			foreach (KeyValuePair<string, ListOperation> pair in StandardFunctions.ListFunctions)
			{
				//context.AssignFunction(pair.Key, new Function(new string[] { "x" }, new UnaryOperationTreeNode(pair.Value, new VariableTreeNode("x"))));
			}

			foreach (KeyValuePair<string, double> pair in StandardVariables.Variables)
			{
				context.VariableTable.AssignNewVariable(pair.Key, pair.Value);
			}

			return context;
		}
	}
}
