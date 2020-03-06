using System;
using lexCalculator.Types.Operations;

namespace lexCalculator.Types.TreeNodes
{
	public class ListOperationTreeNode : TreeNode
	{
		public ListOperation Operation { get; set; }
		public TreeNode[] Parameters { get; set; }
		public override bool HasChildren => true;
		public override bool IsFinished
		{
			get
			{
				for (int i = 0; i < Parameters.Length; ++i)
				{
					if (!Parameters[i].IsFinished) return false;
				}
				return true;
			}
		}

		public ListOperationTreeNode(ListOperation operation, TreeNode[] parameters, TreeNode parent = null) : base(parent)
		{
			Operation = operation;
			Parameters = new TreeNode[parameters.Length];
			for (int i = 0; i < parameters.Length; ++i)
			{
				Parameters[i] = parameters[i];
				Parameters[i].Parent = this;
			}
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			var newParameters = new TreeNode[Parameters.Length];

			for (int i = 0; i < newParameters.Length; ++i)
			{
				newParameters[i] = Parameters[i].Clone(this);
			}
			return new ListOperationTreeNode(Operation, newParameters, parent);
		}

		public override string ToString()
		{
			return Operation.ToString();
		}

		public override bool Equals(TreeNode other)
		{
			if (other is ListOperationTreeNode lNode)
			{
				if (Operation != lNode.Operation) return false;
				if (Parameters.Length != lNode.Parameters.Length) return false;

				for (int i = 0; i < Parameters.Length; ++i)
				{
					if (Parameters[i] != lNode.Parameters[i]) return false;
				}
				return true;
			}
			else return false;
		}
	}
}
