using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Types.TreeNodes
{
	class DerivativeTreeNode : TreeNode
	{
		public TreeNode Child { get; set; }

		public override bool HasChildren => true;

		public override bool IsFinished => Child.IsFinished;

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new DerivativeTreeNode(Child, parent);
		}

		public override bool Equals(TreeNode otherNode)
		{
			return (otherNode is DerivativeTreeNode otherDerivativeNode && (Child == otherDerivativeNode.Child));
		}

		public DerivativeTreeNode(TreeNode child, TreeNode parent = null) : base(parent)
		{
			Child = child;
		}
	}
}
