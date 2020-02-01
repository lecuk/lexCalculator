using System;

namespace lexCalculator.Types
{
	public class Expression
	{
		public Expression(ExpressionTreeNode topNode, VariableIndexTable variableTable = null)
		{
			TopNode = topNode ?? throw new ArgumentNullException(nameof(topNode));
			VariableTable = variableTable;
		}

		public ExpressionTreeNode TopNode { get; set; }
		public VariableIndexTable VariableTable { get; set; }
	}
}
