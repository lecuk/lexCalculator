using System;

namespace lexCalculator.Types
{
	public class FinishedFunction
	{
		public TreeNode TopNode { get; set; }
		public IReadOnlyTable<double> VariableTable { get; set; }
		public IReadOnlyTable<FinishedFunction> FunctionTable { get; set; }
		public int ParameterCount { get; set; }

		public FinishedFunction(TreeNode topNode, IReadOnlyTable<double> variableTable, IReadOnlyTable<FinishedFunction> functionTable, int parameterCount = 0)
		{
			if (!topNode.IsFinished) throw new ArgumentException("Tree is not finished");
			TopNode = topNode ?? throw new ArgumentNullException(nameof(topNode));
			VariableTable = variableTable;
			FunctionTable = functionTable;
			ParameterCount = parameterCount;
		}
	}
}
