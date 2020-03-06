using System;
using System.Text;

namespace lexCalculator.Types.TreeNodes
{
	public class FunctionIndexTreeNode : TreeNode
	{
		public int Index { get; set; }
		public TreeNode[] Parameters { get; set; }
		public override bool HasChildren => true;
		public override bool IsFinished => true;

		public FunctionIndexTreeNode(int index, TreeNode[] parameters, TreeNode parent = null) : base(parent)
		{
			Index = index;
			Parameters = new TreeNode[parameters.Length];
			for (int i = 0; i < parameters.Length; ++i)
			{
				Parameters[i] = parameters[i];
				Parameters[i].Parent = this;
			}
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new FunctionIndexTreeNode(Index, Parameters, parent);
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.AppendFormat("F:{0}(", Index);
			if (Parameters.Length > 0) builder.Append(Parameters[0]);
			for (int i = 1; i < Parameters.Length; ++i)
			{
				builder.Append(", ");
				builder.Append(Parameters[i]);
			}
			builder.Append(")");
			return builder.ToString();
		}

		public override bool Equals(TreeNode other)
		{
			if (other is FunctionIndexTreeNode fiNode)
			{
				if (Index != fiNode.Index) return false;
				if (Parameters.Length != fiNode.Parameters.Length) return false;

				for (int i = 0; i < Parameters.Length; ++i)
				{
					if (Parameters[i] != fiNode.Parameters[i]) return false;
				}

				return true;
			}
			else return false;
		}
	}
}
