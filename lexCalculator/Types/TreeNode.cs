using System;
using System.Text;
using lexCalculator.Static;

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
	}

	public class LiteralTreeNode : TreeNode
	{
		public double Value { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => true;

		public LiteralTreeNode(double value, TreeNode parent = null) : base(parent)
		{
			Value = value;
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new LiteralTreeNode(Value, parent);
		}

		public override string ToString()
		{
			return Value.ToString("G7", System.Globalization.CultureInfo.InvariantCulture);
		}
	}

	public class UnknownVariableTreeNode : TreeNode
	{
		public string Name { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => false;

		public UnknownVariableTreeNode(string name, TreeNode parent = null) : base(parent)
		{
			Name = name;
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new UnknownVariableTreeNode(Name, parent);
		}

		public override string ToString()
		{
			return Name;
		}
	}

	public class FunctionParameterTreeNode : TreeNode
	{
		public int Index { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => true;

		public FunctionParameterTreeNode(int index,  TreeNode parent = null) : base(parent)
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
	}

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
	}

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

		public override TreeNode Clone (TreeNode parent = null)
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
	}

	public class UnknownFunctionTreeNode : TreeNode
	{
		public string Name { get; set; }
		public TreeNode[] Parameters { get; set; }
		public override bool HasChildren => true;
		public override bool IsFinished => false;

		public UnknownFunctionTreeNode(string name, TreeNode[] parameters, TreeNode parent = null) : base(parent)
		{
			Name = name;
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
			return new UnknownFunctionTreeNode(Name, newParameters, parent);
		}

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			builder.Append(Name);
			builder.Append('(');
			if (Parameters.Length > 0) builder.Append(Parameters[0]);
			for (int i = 1; i < Parameters.Length; ++i)
			{
				builder.Append(", ");
				builder.Append(Parameters[i]);
			}
			builder.Append(")");
			return builder.ToString();
		}
	}

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
			if (!OperationFormats.UnaryOperationFormats.ContainsKey(Operation))
			{
				throw new Exception(String.Format("Not implemented string format of unary operator: {0}", Operation.ToString()));
			}

			OperationFormatInfo format = OperationFormats.UnaryOperationFormats[Operation];
			if (format.HasSpecialFormat)
			{
				return String.Format(
					String.Format(OperationFormats.TreeNodeNeedsBrackets(this) ? "({0})" : "{0}", format.SpecialFormat),
					Child);
			}
			else return String.Format("{0}({1})", format.FunctionName, Child);
		}
	}

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
			if (!OperationFormats.BinaryOperationFormats.ContainsKey(Operation))
			{
				throw new Exception(String.Format("Not implemented string format of binary operator: {0}", Operation.ToString()));
			}

			OperationFormatInfo format = OperationFormats.BinaryOperationFormats[Operation];
			if (format.HasSpecialFormat)
			{
				return String.Format(
					String.Format(OperationFormats.TreeNodeNeedsBrackets(this) ? "({0})" : "{0}", format.SpecialFormat),
					LeftChild, RightChild);
			}
			else return String.Format("{0}({1}, {2})", format.FunctionName, LeftChild, RightChild);
		}
	}

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
	}
}
