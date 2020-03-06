using System;
using lexCalculator.Static;
using lexCalculator.Types.Operations;

namespace lexCalculator.Types.TreeNodes
{
	public class UnaryOperationTreeNode : TreeNode
	{
		public UnaryOperation Operation { get; set; }
		public TreeNode Child { get; set; }
		public override bool HasChildren => true;
		public override bool IsFinished => Child.IsFinished;

		public UnaryOperationTreeNode(UnaryOperation operation, TreeNode child, TreeNode parent = null) : base(parent)
		{
			Operation = operation;
			Child = child;
			Child.Parent = this;
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new UnaryOperationTreeNode(Operation, Child.Clone(this), parent);
		}

		public override string ToString()
		{
			if (Operation.HasSpecialFormat)
			{
				return String.Format(
					String.Format(NeedBrackets() ? "({0})" : "{0}", Operation.SpecialFormat),
					Child);
			}
			else return String.Format("{0}({1})", Operation.FunctionName, Child);
		}

		public override bool Equals(TreeNode other)
		{
			return (other is UnaryOperationTreeNode uNode)
				&& (Operation == uNode.Operation)
				&& (Child == uNode.Child);
		}
	}
}
