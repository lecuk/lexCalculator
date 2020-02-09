using System;

namespace lexCalculator.Types
{
	public class FinishedFunction
	{
		public TreeNode TopNode { get; set; }
		public VariableTable VariableTable { get; set; }
		public int ParameterCount { get; set; }

		public FinishedFunction(TreeNode topNode, VariableTable variableTable, int parameterCount = 0)
		{
			if (!topNode.IsFinished) throw new ArgumentException("Tree is not finished");
			TopNode = topNode ?? throw new ArgumentNullException(nameof(topNode));
			VariableTable = variableTable;
			ParameterCount = parameterCount;
		}
	}
}
