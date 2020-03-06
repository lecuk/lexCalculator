using System;
using lexCalculator.Static;
using lexCalculator.Types.Operations;

namespace lexCalculator.Types.TreeNodes
{
	public class BinaryOperationTreeNode : TreeNode
	{
		public BinaryOperation Operation { get; set; }
		public TreeNode LeftChild { get; set; }
		public TreeNode RightChild { get; set; }
		public override bool HasChildren => true;
		public override bool IsFinished => LeftChild.IsFinished && RightChild.IsFinished;

		public BinaryOperationTreeNode(BinaryOperation operation, TreeNode leftChild, TreeNode rightChild, TreeNode parent = null) : base(parent)
		{
			Operation = operation;
			LeftChild = leftChild;
			LeftChild.Parent = this;
			RightChild = rightChild;
			RightChild.Parent = this;
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new BinaryOperationTreeNode(Operation, LeftChild.Clone(this), RightChild.Clone(this), parent);
		}

		public override string ToString()
		{
			if (Operation.HasSpecialFormat)
			{
				return String.Format(
					String.Format(NeedBrackets() ? "({0})" : "{0}", Operation.SpecialFormat),
					LeftChild, RightChild);
			}
			else return String.Format("{0}({1}, {2})", Operation.FunctionName, LeftChild, RightChild);
		}

		public override bool Equals(TreeNode other)
		{
			return (other is BinaryOperationTreeNode bNode)
				&& (Operation == bNode.Operation)
				&& (LeftChild == bNode.LeftChild)
				&& (RightChild == bNode.RightChild);
		}
	}
}
