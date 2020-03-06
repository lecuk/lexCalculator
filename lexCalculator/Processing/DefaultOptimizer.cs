using System;
using lexCalculator.Types;
using lexCalculator.Types.TreeNodes;
using lexCalculator.Types.Operations;

namespace lexCalculator.Processing
{
	public class DefaultOptimizer : IOptimizer
	{
		// Number of cycles which optimizer does when optimizing the tree 
		// it is useful because result of some optimizations can be optimized further by already passed optimizations
		// 1 is okay, i don't think number of iterations should be > 3
		public int Iterations { get; set; }

		public DefaultOptimizer(int iterations = 1)
		{
			Iterations = iterations;
		}

		bool IsConstantValueOf(TreeNode node, double value, IReadOnlyTable<double> variableTable)
		{
			return (IsConstant(node, variableTable, out double constantValue) && constantValue == value);
		}

		bool IsZero(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			return IsConstantValueOf(node, 0.0, variableTable);
		}

		bool IsOne(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			return IsConstantValueOf(node, 1.0, variableTable);
		}

		bool IsNaN(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			return IsConstantValueOf(node, Double.NaN, variableTable);
		}

		bool IsConstant(TreeNode node, IReadOnlyTable<double> variableTable, out double constantValue)
		{
			if (node is NumberTreeNode lNode)
			{
				constantValue = lNode.Value;
				return true;
			}
			else if (node is VariableIndexTreeNode viChild && variableTable != null)
			{
				constantValue = variableTable[viChild.Index];
				return true;
			}
			else
			{
				constantValue = Double.NaN;
				return false;
			}
		}

		void OptimizeRecursively(ref TreeNode node, IReadOnlyTable<double> variableTable, Func<TreeNode, IReadOnlyTable<double>, TreeNode> checkFunction)
		{
			switch (node)
			{
				case UnaryOperationTreeNode uNode:
				{
					TreeNode child = uNode.Child;
					OptimizeRecursively(ref child, variableTable, checkFunction);
					uNode.Child = child;
				}
				break;

				case BinaryOperationTreeNode bNode:
					TreeNode leftChild = bNode.LeftChild;
					TreeNode rightChild = bNode.RightChild;
					OptimizeRecursively(ref leftChild, variableTable, checkFunction);
					OptimizeRecursively(ref rightChild, variableTable, checkFunction);
					bNode.LeftChild = leftChild;
					bNode.RightChild = rightChild;
					break;

				case FunctionIndexTreeNode fiNode:
					for (int i = 0; i < fiNode.Parameters.Length; ++i)
					{
						TreeNode child = fiNode.Parameters[i];
						OptimizeRecursively(ref child, variableTable, checkFunction);
						fiNode.Parameters[i] = child;
					}
					break;

				default: break;
			}

			checkFunction(node, variableTable);
		}

		TreeNode CalculateConstant(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			if (node is UnaryOperationTreeNode uNode)
			{
				if (IsConstant(uNode.Child, variableTable, out double constantOperand))
				{
					double preCalculatedValue = uNode.Operation.Function(constantOperand);
					return new NumberTreeNode(preCalculatedValue, node.Parent);
				}
			}

			if (node is BinaryOperationTreeNode bNode)
			{
				if (IsConstant(bNode.LeftChild, variableTable, out double constantLeftOperand)
				&&  IsConstant(bNode.RightChild, variableTable, out double constantRightOperand))
				{
					double preCalculatedValue = bNode.Operation.Function(constantLeftOperand, constantRightOperand);
					return new NumberTreeNode(preCalculatedValue, node.Parent);
				}
			}

			return node;
		}

		TreeNode RemoveMultiplicationByZero(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			return (node is BinaryOperationTreeNode bTreeNode && bTreeNode.Operation == BinaryOperatorOperation.Multiplication &&
				(IsZero(bTreeNode.LeftChild, variableTable) || IsZero(bTreeNode.RightChild, variableTable)))
				? new NumberTreeNode(0, node.Parent)
				: node;
		}

		TreeNode RemoveMultiplicationByOne(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			if (node is BinaryOperationTreeNode bTreeNode && bTreeNode.Operation == BinaryOperatorOperation.Multiplication)
			{
				return IsOne(bTreeNode.LeftChild, variableTable) ? bTreeNode.RightChild.Clone(node.Parent)
					: IsOne(bTreeNode.RightChild, variableTable) ? bTreeNode.LeftChild.Clone(node.Parent)
					: bTreeNode;
			}

			return node;
		}

		TreeNode RemoveMultiplicationByMinusOne(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			if (node is BinaryOperationTreeNode bTreeNode && bTreeNode.Operation == BinaryOperatorOperation.Multiplication)
			{
				return IsConstantValueOf(bTreeNode.LeftChild, -1, variableTable) 
					? new UnaryOperationTreeNode(UnaryOperation.Negative, bTreeNode.RightChild, node.Parent)
					: IsConstantValueOf(bTreeNode.RightChild, -1, variableTable) 
					? new UnaryOperationTreeNode(UnaryOperation.Negative, bTreeNode.LeftChild, node.Parent)
					: node;
			}

			return node;
		}

		TreeNode RemoveZeroAdditionOrSubstraction(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			if (node is BinaryOperationTreeNode bTreeNode
				&& (bTreeNode.Operation == BinaryOperatorOperation.Addition
				||  bTreeNode.Operation == BinaryOperatorOperation.Substraction))
			{
				return IsZero(bTreeNode.LeftChild, variableTable) ? bTreeNode.RightChild.Clone(node.Parent)
					: IsZero(bTreeNode.RightChild, variableTable) ? bTreeNode.LeftChild.Clone(node.Parent)
					: node;
			}

			return node;
		}

		TreeNode TrySwapConstantOperands(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			return node;
		}

		TreeNode ReplaceSquareAndCubePowWithSqrAndCube(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			if (node is BinaryOperationTreeNode bTreeNode
				&& bTreeNode.Operation == BinaryOperatorOperation.Power)
			{
				if (IsConstantValueOf(bTreeNode.RightChild, 2.0, variableTable))
				{
					return new UnaryOperationTreeNode(UnaryOperation.Square, bTreeNode.LeftChild, node.Parent);
				}

				if (IsConstantValueOf(bTreeNode.RightChild, 3.0, variableTable))
				{
					return new UnaryOperationTreeNode(UnaryOperation.Cube, bTreeNode.LeftChild, node.Parent);
				}
			}

			return node;
		}

		TreeNode RemoveZeroPower(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			return (node is BinaryOperationTreeNode bTreeNode && bTreeNode.Operation == BinaryOperatorOperation.Power &&
				(IsZero(bTreeNode.RightChild, variableTable)))
				? new NumberTreeNode(1.0, node.Parent)
				: node;
		}

		TreeNode RemoveFirstPower(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			return (node is BinaryOperationTreeNode bTreeNode && bTreeNode.Operation == BinaryOperatorOperation.Power &&
				(IsOne(bTreeNode.RightChild, variableTable)))
				? bTreeNode.LeftChild.Clone(bTreeNode.Parent)
				: node;
		}

		TreeNode RemoveZeroDivision(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			return (node is BinaryOperationTreeNode bTreeNode && bTreeNode.Operation == BinaryOperatorOperation.Division &&
				(IsZero(bTreeNode.LeftChild, variableTable)))
				? new NumberTreeNode(0, node.Parent)
				: node;
		}

		TreeNode ReplaceDivisionByZeroWithNAN(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			return (node is BinaryOperationTreeNode bTreeNode && bTreeNode.Operation == BinaryOperatorOperation.Division &&
				(IsZero(bTreeNode.RightChild, variableTable)))
				? new NumberTreeNode(Double.NaN, node.Parent)
				: node;
		}

		TreeNode ReplaceNANOperationsWithNAN(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			return (node is BinaryOperationTreeNode bTreeNode
				&& (IsNaN(bTreeNode.LeftChild, variableTable) || IsNaN(bTreeNode.RightChild, variableTable)))
				? new NumberTreeNode(Double.NaN, node.Parent)
				: node;
		}

		TreeNode ReplaceELogWithNaturalLog(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			return (node is BinaryOperationTreeNode bTreeNode && (bTreeNode.Operation == BinaryOperation.Logarithm)
				&& IsConstantValueOf(bTreeNode.LeftChild, Math.E, variableTable))
				? new UnaryOperationTreeNode(UnaryOperation.NaturalLogarithm, bTreeNode.RightChild, node.Parent)
				: node;
		}

		TreeNode AddEqualTrees(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			if (node is BinaryOperationTreeNode bTreeNode)
			{
				// a*x + b*x = (a + b)*x, a and b are constants
				if (bTreeNode.Operation == BinaryOperatorOperation.Addition || bTreeNode.Operation == BinaryOperatorOperation.Substraction)
				{
					// now the insane part begins
					double a = Double.NaN, b = Double.NaN;
					TreeNode lChild = bTreeNode.LeftChild, rChild = bTreeNode.RightChild;
					TreeNode commonMultiplier = null;

					// x + x = 2x
					if (lChild == rChild)
					{
						a = 1;
						b = 1;
						commonMultiplier = lChild;
					}

					if (lChild is BinaryOperationTreeNode lbChild)
					{
						if (IsConstant(lbChild.LeftChild, variableTable, out a))
						{
							commonMultiplier = lbChild.RightChild;
						}
						else if (IsConstant(lbChild.RightChild, variableTable, out a))
						{
							commonMultiplier = lbChild.LeftChild;
						}
					}
					
					return new BinaryOperationTreeNode(BinaryOperatorOperation.Multiplication, commonMultiplier, new NumberTreeNode(a + b), node.Parent);
				}
			}

			return node;
		}

		public FinishedFunction Optimize(FinishedFunction unoptimized)
		{
			throw new NotImplementedException();
		}

		public FinishedFunction OptimizeWithTable(FinishedFunction unoptimized)
		{
			TreeNode optimizedTree = unoptimized.TopNode.Clone();
			IReadOnlyTable<double> variableTable = unoptimized.VariableTable;
			IReadOnlyTable<FinishedFunction> functionTable = unoptimized.FunctionTable;

			for (int i = 0; i < Iterations; ++i)
			{
				OptimizeRecursively(ref optimizedTree, variableTable, CalculateConstant);
				OptimizeRecursively(ref optimizedTree, variableTable, RemoveMultiplicationByZero);
				OptimizeRecursively(ref optimizedTree, variableTable, RemoveMultiplicationByOne);
				OptimizeRecursively(ref optimizedTree, variableTable, RemoveMultiplicationByMinusOne);
				OptimizeRecursively(ref optimizedTree, variableTable, RemoveZeroAdditionOrSubstraction);
				OptimizeRecursively(ref optimizedTree, variableTable, ReplaceSquareAndCubePowWithSqrAndCube);
				OptimizeRecursively(ref optimizedTree, variableTable, RemoveZeroPower);
				OptimizeRecursively(ref optimizedTree, variableTable, RemoveFirstPower);
				//optimized.TopNode = OptimizeRecursively(optimized.TopNode, optimized.VariableTable, AddEqualTrees);
				OptimizeRecursively(ref optimizedTree, variableTable, ReplaceDivisionByZeroWithNAN);
				OptimizeRecursively(ref optimizedTree, variableTable, RemoveZeroDivision);
				OptimizeRecursively(ref optimizedTree, variableTable, ReplaceNANOperationsWithNAN);
				OptimizeRecursively(ref optimizedTree, variableTable, ReplaceELogWithNaturalLog);
			}
			return new FinishedFunction(optimizedTree, variableTable, functionTable);
		}
	}
}
