using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Types.TreeNodes
{
	public class FunctionParameterTreeNode : TreeNode
	{
		public int Index { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => true;

		public FunctionParameterTreeNode(int index, TreeNode parent = null) : base(parent)
		{
			Index = index;
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new FunctionParameterTreeNode(Index, parent);
		}

		public override string ToString()
		{
			return String.Format("${0}", Index);
		}

		public override bool Equals(TreeNode other)
		{
			return (other is FunctionParameterTreeNode fpNode)
				&& (Index == fpNode.Index);
		}
	}
}
