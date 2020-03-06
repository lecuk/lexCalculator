using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Types.TreeNodes
{
	public class NumberTreeNode : TreeNode
	{
		public double Value { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => true;

		public NumberTreeNode(double value, TreeNode parent = null) : base(parent)
		{
			Value = value;
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new NumberTreeNode(Value, parent);
		}

		public override string ToString()
		{
			return Value.ToString("G7", System.Globalization.CultureInfo.InvariantCulture);
		}

		public override bool Equals(TreeNode other)
		{
			return (other is NumberTreeNode nNode)
				&& (Value == nNode.Value);
		}
	}
}
