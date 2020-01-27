using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using lexCalculator.Construction;

namespace lexCalculator.Linking
{
	public class Expression
	{
		public Expression(ExpressionTreeNode topNode)
		{
			TopNode = topNode ?? throw new ArgumentNullException(nameof(topNode));
		}

		public ExpressionTreeNode TopNode { get; set; }
	}
}
