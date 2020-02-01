using System;

namespace lexCalculator.Types
{
	public class Function
	{
		public ExpressionTreeNode TopNode { get; set; }
		public int ParameterCount { get; set; }

		public Function(ExpressionTreeNode topNode, int parameterCount = 0)
		{
			TopNode = topNode ?? throw new ArgumentNullException(nameof(topNode));
			ParameterCount = parameterCount;
		}
	}
}
