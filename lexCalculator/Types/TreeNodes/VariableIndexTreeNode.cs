using System;

namespace lexCalculator.Types.TreeNodes
{
	public class VariableIndexTreeNode : TreeNode
	{
		public int Index { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => true;

		public VariableIndexTreeNode(int index, TreeNode parent = null) : base(parent)
		{
			Index = index;
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new VariableIndexTreeNode(Index, parent);
		}

		public override string ToString()
		{
			return String.Format("V:{0}", Index);
		}

		public override bool Equals(TreeNode other)
		{
			return (other is VariableIndexTreeNode viNode)
				&& (Index == viNode.Index);
		}
	}
}
