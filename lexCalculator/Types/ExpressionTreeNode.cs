using System;

namespace lexCalculator.Types
{
	public abstract class ExpressionTreeNode
	{
		public ExpressionTreeNode Parent { get; set; }

		public abstract bool HasChildren { get; }
		public abstract bool IsFinished { get; }

		public ExpressionTreeNode(ExpressionTreeNode parent = null)
		{
			Parent = parent;
		}

		public abstract ExpressionTreeNode Clone(ExpressionTreeNode parent = null);
	}

	public class LiteralTreeNode : ExpressionTreeNode
	{
		public double Value { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => true;

		public LiteralTreeNode(double value, ExpressionTreeNode parent = null) : base(parent)
		{
			Value = value;
		}

		public override ExpressionTreeNode Clone(ExpressionTreeNode parent = null)
		{
			return new LiteralTreeNode(Value, parent);
		}

		public override string ToString()
		{
			return Value.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture);
		}
	}

	public class VariableTreeNode : ExpressionTreeNode
	{
		public string Name { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => false;

		public VariableTreeNode(string name, ExpressionTreeNode parent = null) : base(parent)
		{
			Name = name;
		}

		public override ExpressionTreeNode Clone(ExpressionTreeNode parent = null)
		{
			return new VariableTreeNode(Name, parent);
		}

		public override string ToString()
		{
			return Name;
		}
	}

	public class FunctionParameterTreeNode : ExpressionTreeNode
	{
		public int Index { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => true;

		public FunctionParameterTreeNode(int index,  ExpressionTreeNode parent = null) : base(parent)
		{
			Index = index;
		}

		public override ExpressionTreeNode Clone(ExpressionTreeNode parent = null)
		{
			return new FunctionParameterTreeNode(Index, parent);
		}

		public override string ToString()
		{
			return String.Format("${0}", Index);
		}
	}

	public class IndexTreeNode : ExpressionTreeNode
	{
		public int Index { get; set; }
		public override bool HasChildren => false;
		public override bool IsFinished => true;

		public IndexTreeNode(int index, ExpressionTreeNode parent = null) : base(parent)
		{
			Index = index;
		}

		public override ExpressionTreeNode Clone(ExpressionTreeNode parent = null)
		{
			return new IndexTreeNode(Index, parent);
		}

		public override string ToString()
		{
			return String.Format("#[{0}]", Index);
		}
	}

	public class FunctionTreeNode : ExpressionTreeNode
	{
		public string Name { get; set; }
		public ExpressionTreeNode[] Parameters { get; set; }
		public override bool HasChildren => true;
		public override bool IsFinished => false;

		public FunctionTreeNode(string name, ExpressionTreeNode[] parameters, ExpressionTreeNode parent = null) : base(parent)
		{
			Name = name;
			Parameters = new ExpressionTreeNode[parameters.Length];
			for (int i = 0; i < parameters.Length; ++i)
			{
				Parameters[i] = parameters[i];
				Parameters[i].Parent = this;
			}
		}

		public override ExpressionTreeNode Clone(ExpressionTreeNode parent = null)
		{
			var newParameters = new ExpressionTreeNode[Parameters.Length];

			for (int i = 0; i < newParameters.Length; ++i)
			{
				newParameters[i] = Parameters[i].Clone(this);
			}
			return new FunctionTreeNode(Name, newParameters, parent);
		}

		public override string ToString()
		{
			return String.Format("{0}({1} params)", Name, Parameters.Length);
		}
	}

	public class UnaryOperationTreeNode : ExpressionTreeNode
	{
		public UnaryOperation Operation { get; set; }
		public ExpressionTreeNode Child { get; set; }
		public override bool HasChildren => true;
		public override bool IsFinished => Child.IsFinished;

		public UnaryOperationTreeNode(UnaryOperation operation, ExpressionTreeNode child, ExpressionTreeNode parent = null) : base(parent)
		{
			Operation = operation;
			Child = child;
			Child.Parent = this;
		}

		public override ExpressionTreeNode Clone(ExpressionTreeNode parent = null)
		{
			return new UnaryOperationTreeNode(Operation, Child.Clone(this), parent);
		}

		public override string ToString()
		{
			return Operation.ToString();
		}
	}

	public class BinaryOperationTreeNode : ExpressionTreeNode
	{
		public BinaryOperation Operation { get; set; }
		public ExpressionTreeNode LeftChild { get; set; }
		public ExpressionTreeNode RightChild { get; set; }
		public override bool HasChildren => true;
		public override bool IsFinished => LeftChild.IsFinished && RightChild.IsFinished;

		public BinaryOperationTreeNode(BinaryOperation operation, ExpressionTreeNode leftChild, ExpressionTreeNode rightChild, ExpressionTreeNode parent = null) : base(parent)
		{
			Operation = operation;
			LeftChild = leftChild;
			LeftChild.Parent = this;
			RightChild = rightChild;
			RightChild.Parent = this;
		}

		public override ExpressionTreeNode Clone(ExpressionTreeNode parent = null)
		{
			return new BinaryOperationTreeNode(Operation, LeftChild.Clone(this), RightChild.Clone(this), parent);
		}

		public override string ToString()
		{
			return Operation.ToString();
		}
	}

	public class ListOperationTreeNode : ExpressionTreeNode
	{
		public ListOperation Operation { get; set; }
		public ExpressionTreeNode[] Parameters { get; set; }
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

		public ListOperationTreeNode(ListOperation operation, ExpressionTreeNode[] parameters, ExpressionTreeNode parent = null) : base(parent)
		{
			Operation = operation;
			Parameters = new ExpressionTreeNode[parameters.Length];
			for (int i = 0; i < parameters.Length; ++i)
			{
				Parameters[i] = parameters[i];
				Parameters[i].Parent = this;
			}
		}

		public override ExpressionTreeNode Clone(ExpressionTreeNode parent = null)
		{
			var newParameters = new ExpressionTreeNode[Parameters.Length];

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
