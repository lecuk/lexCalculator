using System;
using System.Collections.Generic;
using lexCalculator.Types;
using lexCalculator.Types.TreeNodes;
using lexCalculator.Types.Operations;

namespace lexCalculator.Processing
{
	public class DefaultDifferentiator : IDifferentiator
	{
		static readonly TreeNode X = new FunctionParameterTreeNode(0);
		static readonly TreeNode dX = new FunctionParameterTreeNode(1);
		static readonly TreeNode Y = new FunctionParameterTreeNode(2);
		static readonly TreeNode dY = new FunctionParameterTreeNode(3);

		static readonly Dictionary<UnaryOperation, TreeNode> UnaryDifferential = new Dictionary<UnaryOperation, TreeNode>()
		{
			// (-u)' = -1
			{ UnaryOperation.Negative,
				new NumberTreeNode(-1)},
			
			// sign'(u) = ({-1, 0, 1})' = 0
			{ UnaryOperation.Sign,
				new TernaryOperationTreeNode(TernaryOperation.Conditional,
					new BinaryOperationTreeNode(BinaryOperatorOperation.Equal,
						X,  
						new NumberTreeNode(0)),
					new NumberTreeNode(Double.NaN),
					new NumberTreeNode(0))},
			
			// sin'(u) = u' * cos(u)
			{ UnaryOperation.Sine,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
					dX,
					new UnaryOperationTreeNode(UnaryOperation.Cosine,
						X))},
			
			// cos'(u) = u' * (-sin(u))
			{ UnaryOperation.Cosine,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
					dX,
					new UnaryOperationTreeNode(UnaryOperation.Negative,
						new UnaryOperationTreeNode(UnaryOperation.Sine,
							X)))},
			
			// tan'(u) = u' / cos(u)^2
			{ UnaryOperation.Tangent,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Division,
					dX,
					new BinaryOperationTreeNode(BinaryOperatorOperation.Power,
						new UnaryOperationTreeNode(UnaryOperation.Cosine,
							X),
						new NumberTreeNode(2)))},
			
			// cot'(u) = u' / (-sin(u)^2)
			{ UnaryOperation.Cotangent,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Division,
					dX,
					new UnaryOperationTreeNode(UnaryOperation.Negative,
						new BinaryOperationTreeNode(BinaryOperatorOperation.Power,
							new UnaryOperationTreeNode(UnaryOperation.Sine,
								X),
							new NumberTreeNode(2))))},
			
			// sec'(u) = u' * sec(u)*tan(u)
			{ UnaryOperation.Secant,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
					dX,
					new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
						new UnaryOperationTreeNode(UnaryOperation.Secant,
							X),
						new UnaryOperationTreeNode(UnaryOperation.Tangent,
							X)))},

			// csc'(u) = u' * (-csc(u)*cot(u))
			{ UnaryOperation.Cosecant,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
					dX,
					new UnaryOperationTreeNode(UnaryOperation.Negative,
						new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
							new UnaryOperationTreeNode(UnaryOperation.Cosecant,
								X),
							new UnaryOperationTreeNode(UnaryOperation.Cotangent,
								X))))},

			/*
			{ UnaryOperation.ArcSine,               Math.Asin},
			{ UnaryOperation.ArcCosine,             Math.Acos},
			{ UnaryOperation.ArcTangent,            Math.Atan},
			{ UnaryOperation.ArcCotangent,          MoreMath.Acot },
			{ UnaryOperation.ArcSecant,             MoreMath.Asec },
			{ UnaryOperation.ArcCosecant,           MoreMath.Acsc },
			{ UnaryOperation.SineHyperbolic,        Math.Sinh},
			{ UnaryOperation.CosineHyperbolic,      Math.Cosh},
			{ UnaryOperation.TangentHyperbolic,     Math.Tanh},
			{ UnaryOperation.CotangentHyperbolic,   MoreMath.Coth },
			{ UnaryOperation.SecantHyperbolic,      MoreMath.Sech },
			{ UnaryOperation.CosecantHyperbolic,    MoreMath.Csch },
			*/

			// (e^u)' = u' * e^u
			{ UnaryOperation.Exponent,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
					dX,
					new UnaryOperationTreeNode(UnaryOperation.Exponent,
						X))},

			// ln'(u) = u' / u
			{ UnaryOperation.NaturalLogarithm,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Division,
					dX,
					X)},

			// sqrt'(u) = u' / (2*sqrt(u))
			{ UnaryOperation.SquareRoot,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Division,
					dX,
					new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
						new NumberTreeNode(2),
						new UnaryOperationTreeNode(UnaryOperation.SquareRoot,
							X)))},

			// cbrt'(u) = u' / (3 * cbrt(u^2))
			{ UnaryOperation.CubeRoot,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Division,
					dX,
					new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
						new NumberTreeNode(3),
						new UnaryOperationTreeNode(UnaryOperation.CubeRoot,
							new BinaryOperationTreeNode(BinaryOperatorOperation.Power,
								X,
								new NumberTreeNode(2)))))},
			
			{ UnaryOperation.Floor,
				new NumberTreeNode(0)},
			
			{ UnaryOperation.Ceiling,
				new NumberTreeNode(0)},
			
			{ UnaryOperation.AbsoluteValue,
				new UnaryOperationTreeNode(UnaryOperation.Sign,
					X)},

			/*
			{ UnaryOperation.Factorial,             MoreMath.Factorial }
			*/
		};

		static readonly Dictionary<BinaryOperation, TreeNode> BinaryDifferential = new Dictionary<BinaryOperation, TreeNode>()
		{
			// (u + v)' = u' + v'
			{ BinaryOperatorOperation.Addition,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Addition,
					dX,
					dY) },
			
			// (u - v)' = u' - v'
			{ BinaryOperatorOperation.Substraction,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Substraction,
					dX,
					dY) },
			
			// (u * v)' = u'v + uv'
			{ BinaryOperatorOperation.Multiplication,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Addition,
					new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
						dX,
						Y),
					new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
						X,
						dY)) },
			
			// (u / v)' = (u'v - uv') / (v * v)
			{ BinaryOperatorOperation.Division,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Division,
					new BinaryOperationTreeNode(BinaryOperatorOperation.Substraction,
						new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
							dX,
							Y),
						new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
							X,
							dY)),
					new BinaryOperationTreeNode(BinaryOperatorOperation.Power,
						Y,
						new NumberTreeNode(2))) },
			
			// (u ^ v)' = (u ^ v) * ((v * u') / u + v' * ln(u)) 
			{ BinaryOperatorOperation.Power,
				new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
					new BinaryOperationTreeNode(BinaryOperatorOperation.Power,
						X,
						Y),
					new BinaryOperationTreeNode(BinaryOperatorOperation.Addition,
						new BinaryOperationTreeNode(BinaryOperatorOperation.Division,
							new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
								Y,
								dX),
							X),
						new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication,
							dY,
							new UnaryOperationTreeNode(UnaryOperation.NaturalLogarithm,
								X)))) },
			
			// (u % v)' = 1 
			{ BinaryOperatorOperation.Remainder,
				new NumberTreeNode(1) },
		};

		TreeNode ReplaceDifferentialArguments(TreeNode node, params TreeNode[] children)
		{
			if (node is FunctionParameterTreeNode fpTreeNode)
			{
				for (int i = 0; i < children.Length; ++i)
				{
					if (fpTreeNode.Index == i)
					{
						return children[i].Clone(node.Parent);
					}
				}
			}

			switch(node)
			{
				case UnaryOperationTreeNode uTreeNode:
					uTreeNode.Child = ReplaceDifferentialArguments(uTreeNode.Child, children);
					break;

				case BinaryOperationTreeNode bTreeNode:
					bTreeNode.LeftChild = ReplaceDifferentialArguments(bTreeNode.LeftChild, children);
					bTreeNode.RightChild = ReplaceDifferentialArguments(bTreeNode.RightChild, children);
					break;

				default:  break;
			}

			return node;
		}

		TreeNode MakeDifferential(TreeNode treeNode, int index)
		{
			switch (treeNode)
			{
				case NumberTreeNode lTreeNode:
					return new NumberTreeNode(0, lTreeNode.Parent);

				case VariableIndexTreeNode iTreeNode:
					return new NumberTreeNode(0, iTreeNode.Parent);

				case FunctionIndexTreeNode fiTreeNode:
					throw new Exception("Remote functions are not supported");

				case FunctionParameterTreeNode fpTreeNode:
					if (fpTreeNode.Index == index)
						return new NumberTreeNode(1, fpTreeNode.Parent);
					else
						return new NumberTreeNode(0, fpTreeNode.Parent);

				case UnaryOperationTreeNode uTreeNode:
					return MakeUnaryDifferential(uTreeNode, index);

				case BinaryOperationTreeNode bTreeNode:
					return MakeBinaryDifferential(bTreeNode, index);

				default:
					throw new Exception("Invalid tree node type");
			}
		}

		private TreeNode MakeUnaryDifferential(UnaryOperationTreeNode uTreeNode, int index)
		{
			TreeNode childDx = MakeDifferential(uTreeNode.Child, index);
			TreeNode differential = UnaryDifferential[uTreeNode.Operation].Clone();
			ReplaceDifferentialArguments(differential, uTreeNode.Child, childDx);
			
			return differential;
		}

		private TreeNode MakeBinaryDifferential(BinaryOperationTreeNode bTreeNode, int index)
		{
			TreeNode leftChildDx = MakeDifferential(bTreeNode.LeftChild, index);
			TreeNode rightChildDx = MakeDifferential(bTreeNode.RightChild, index);
			TreeNode differential = BinaryDifferential[bTreeNode.Operation].Clone();
			ReplaceDifferentialArguments(differential, bTreeNode.LeftChild, leftChildDx, bTreeNode.RightChild, rightChildDx);

			return differential;
		}

		public FinishedFunction FindDifferential(FinishedFunction func, int index)
		{
			if (index < 0 || index >= func.ParameterCount) throw new ArgumentException(String.Format("There is no parameter with index {0}", index));

			TreeNode topNode = func.TopNode;
			TreeNode differentialTopNode = MakeDifferential(topNode, index);

			return new FinishedFunction(differentialTopNode, func.VariableTable, func.FunctionTable, func.ParameterCount);
		}
	}
}
