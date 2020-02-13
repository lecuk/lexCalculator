using System;
using System.Text;

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

	public class VariableTreeNode : TreeNode
	{
		public string Name { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => false;

		public VariableTreeNode(string name, TreeNode parent = null) : base(parent)
		{
			Name = name;
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new VariableTreeNode(Name, parent);
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

	public class IndexTreeNode : TreeNode
	{
		public int Index { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => true;

		public IndexTreeNode(int index, TreeNode parent = null) : base(parent)
		{
			Index = index;
		}

		public override TreeNode Clone(TreeNode parent = null)
		{
			return new IndexTreeNode(Index, parent);
		}

		public override string ToString()
		{
			return String.Format("#[{0}]", Index);
		}
	}

	public class FunctionTreeNode : TreeNode
	{
		public string Name { get; set; }
		public TreeNode[] Parameters { get; set; }
		public override bool HasChildren => true;
		public override bool IsFinished => false;

		public FunctionTreeNode(string name, TreeNode[] parameters, TreeNode parent = null) : base(parent)
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
			return new FunctionTreeNode(Name, newParameters, parent);
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

		private readonly string[] UnaryToString = new string[]
		{
			"-",
			"sign",
			"sin",
			"cos",
			"tan",
			"cot",
			"sec",
			"csc",
			"arcsin",
			"arccos",
			"arctan",
			"arccot",
			"arcsec",
			"arccsc",
			"sinh",
			"cosh",
			"tanh",
			"coth",
			"sech",
			"csch",
			"exp",
			"ln",
			"sqrt",
			"cbrt",
			"floor",
			"ceil",
			"abs",
			"!"
		};

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			if (Operation == UnaryOperation.Factorial)
			{
				builder.Append('(');
				builder.Append(Child);
				builder.Append(")!");
			}
			if (Operation == UnaryOperation.Negative)
			{
				bool needBrackets = (Child is BinaryOperationTreeNode bChild && (
					bChild.Operation == BinaryOperation.Addition ||
					bChild.Operation == BinaryOperation.Substraction ||
					bChild.Operation == BinaryOperation.Multiplication ||
					bChild.Operation == BinaryOperation.Division));
				builder.Append('-');
				if (needBrackets) builder.Append('(');
				builder.Append(Child);
				if (needBrackets) builder.Append(')');
			}
			else if (Operation == UnaryOperation.AbsoluteValue)
			{
				builder.Append('|');
				builder.Append(Child);
				builder.Append('|');
			}
			else
			{
				builder.Append(String.Format("{0}(", UnaryToString[(int)Operation]));
				builder.Append(Child);
				builder.Append(")");
			}
			return builder.ToString();
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

		private readonly string[] BinaryToString = new string[]
		{
			"+",
			"-",
			"*",
			"/",
			"^",
			"%",
			"log",
			"nrt"
		};

		public override string ToString()
		{
			StringBuilder builder = new StringBuilder();
			bool isFunction = (Operation == BinaryOperation.Logarithm || Operation == BinaryOperation.NRoot);
			bool needBrackets = !isFunction &&
				(Parent is BinaryOperationTreeNode pbTreeNode) &&
				(pbTreeNode.Operation > Operation);

			if (isFunction)
			{
				builder.AppendFormat("{0}({1}, {2})", BinaryToString[(int)Operation], LeftChild, RightChild);
			}
			else
			{
				if (needBrackets) builder.Append('(');
				builder.AppendFormat("{1} {0} {2}", BinaryToString[(int)Operation], LeftChild, RightChild);
				if (needBrackets) builder.Append(')');
			}

			return builder.ToString();
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
