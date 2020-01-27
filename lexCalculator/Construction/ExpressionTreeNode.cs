using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Construction
{
	public abstract class ExpressionTreeNode
	{
		public ExpressionTreeNode Parent { get; set; }

		public abstract bool HasChildren { get; }

		public ExpressionTreeNode(ExpressionTreeNode parent = null)
		{
			Parent = parent;
		}
	}

	public class LiteralTreeNode : ExpressionTreeNode
	{
		public double Value { get; set; }
		public override bool HasChildren => false;

		public LiteralTreeNode(double value, ExpressionTreeNode parent = null) : base(parent)
		{
			Value = value;
		}
	}

	public class VariableTreeNode : ExpressionTreeNode
	{
		public string Name { get; set; }
		public override bool HasChildren => false;

		public VariableTreeNode(string name, ExpressionTreeNode parent = null) : base(parent)
		{
			Name = name;
		}
	}

	public class FunctionTreeNode : ExpressionTreeNode
	{
		public string Name { get; set; }
		public ExpressionTreeNode[] Parameters { get; set; }
		public override bool HasChildren => true;

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
	}

	public class UnaryOperationTreeNode : ExpressionTreeNode
	{
		public UnaryOperation Operation { get; set; }
		public ExpressionTreeNode Child { get; set; }
		public override bool HasChildren => true;

		public UnaryOperationTreeNode(UnaryOperation operation, ExpressionTreeNode child, ExpressionTreeNode parent = null) : base(parent)
		{
			Operation = operation;
			Child = child;
			Child.Parent = this;
		}
	}

	public class BinaryOperationTreeNode : ExpressionTreeNode
	{
		public BinaryOperation Operation { get; set; }
		public ExpressionTreeNode LeftChild { get; set; }
		public ExpressionTreeNode RightChild { get; set; }
		public override bool HasChildren => true;

		public BinaryOperationTreeNode(BinaryOperation operation, ExpressionTreeNode leftChild, ExpressionTreeNode rightChild, ExpressionTreeNode parent = null) : base(parent)
		{
			Operation = operation;
			LeftChild = leftChild;
			LeftChild.Parent = this;
			RightChild = rightChild;
			RightChild.Parent = this;
		}
	}
}
