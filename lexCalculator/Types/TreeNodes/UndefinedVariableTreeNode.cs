using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Types.TreeNodes
{
	public class UndefinedVariableTreeNode : TreeNode
	{
		public string Name { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => false;

		public UndefinedVariableTreeNode(string name, TreeNode parent = null) : base(parent)
		{
			Name = name;
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new UndefinedVariableTreeNode(Name, parent);
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(TreeNode other)
		{
			return (other is UndefinedVariableTreeNode uvNode)
				&& (Name == uvNode.Name);
		}
	}
}
