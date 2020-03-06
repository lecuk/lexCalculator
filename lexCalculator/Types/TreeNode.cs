using lexCalculator.Types.TreeNodes;
using lexCalculator.Types.Operations;

namespace lexCalculator.Types
{
	public abstract class TreeNode
	{
		public TreeNode Parent { get; set; }

		public abstract bool HasChildren { get; }
		public abstract bool IsFinished { get; }

		public TreeNode(TreeNode parent = null)
		{
			Parent = parent;
		}

		public abstract TreeNode Clone(TreeNode parent = null);
		public abstract bool Equals(TreeNode otherNode);

		public bool NeedBrackets()
		{
			if (Parent == null) return false;

			if (this is BinaryOperationTreeNode bNode && bNode.Operation is BinaryOperatorOperation bOperation)
			{
				switch (Parent)
				{
					case UnaryOperationTreeNode puNode:
						return bOperation.ChildrenInSpecialFormatMayNeedBrackets;

					case BinaryOperationTreeNode pbNode:
						return (pbNode.Operation is BinaryOperatorOperation pbOperation) 
							&& (pbOperation.ChildrenInSpecialFormatMayNeedBrackets)
							&& (pbOperation.Precedence > bOperation.Precedence);

					default: return false;
				}
			}
			else return false;
		}
	}
}
