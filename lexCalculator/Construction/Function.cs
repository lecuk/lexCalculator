using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Construction
{
	class Function
	{
		public string[] ParameterNames { get; set; }
		public ExpressionTreeNode ExpressionTopNode { get; set; }

		public Function(string[] parameterNames, ExpressionTreeNode expressionTopNode)
		{
			ParameterNames = (string[])parameterNames.Clone();
			ExpressionTopNode = expressionTopNode ?? throw new ArgumentNullException(nameof(expressionTopNode));
		}
	}
}
