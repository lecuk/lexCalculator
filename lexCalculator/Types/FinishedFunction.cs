using System;

namespace lexCalculator.Types
{
	public class FinishedFunction
	{
		public TreeNode TopNode { get; set; }
		public IReadOnlyVariableTable VariableTable { get; set; }
		public int ParameterCount { get; set; }

		public FinishedFunction(TreeNode topNode, IReadOnlyVariableTable variableTable, int parameterCount = 0)
		{
			if (!topNode.IsFinished) throw new ArgumentException("Tree is not finished");
			TopNode = topNode ?? throw new ArgumentNullException(nameof(topNode));
			VariableTable = variableTable;
			ParameterCount = parameterCount;
		}
	}
}
