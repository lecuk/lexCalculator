using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexCalculator.Calculation;
using lexCalculator.Types;

namespace lexCalculator.Processing
{
	public class MyOptimizer : IOptimizer
	{
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

		bool IsConstant(TreeNode node, IReadOnlyTable<double> variableTable, out double constantValue)
		{
			if (node is LiteralTreeNode lNode)
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

		void CheckChildrenRecursively(TreeNode node, IReadOnlyTable<double> variableTable, Func<TreeNode, IReadOnlyTable<double>, TreeNode> checkFunction)
		{
			switch (node)
			{
				case UnaryOperationTreeNode uNode:
					uNode.Child = checkFunction(uNode.Child, variableTable);
					break;

				case BinaryOperationTreeNode bNode:
					bNode.LeftChild = checkFunction(bNode.LeftChild, variableTable);
					bNode.RightChild = checkFunction(bNode.RightChild, variableTable);
					break;

				case FunctionIndexTreeNode fiNode:
					for (int i = 0; i < fiNode.Parameters.Length; ++i)
					{
						fiNode.Parameters[i] = checkFunction(fiNode.Parameters[i], variableTable);
					}
					break;

				default: break;
			}
		}

		TreeNode CalculateConstant(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			CheckChildrenRecursively(node, variableTable, CalculateConstant);
			
			if (node is UnaryOperationTreeNode uNode)
			{
				if (IsConstant(uNode.Child, variableTable, out double constantOperand))
				{
					double preCalculatedValue = OperationImplementations.UnaryFunctions[uNode.Operation](constantOperand);
					return new LiteralTreeNode(preCalculatedValue, node.Parent);
				}
			}

			if (node is BinaryOperationTreeNode bNode)
			{
				if (IsConstant(bNode.LeftChild, variableTable, out double constantLeftOperand)
				&&  IsConstant(bNode.RightChild, variableTable, out double constantRightOperand))
				{
					double preCalculatedValue = OperationImplementations.BinaryFunctions[bNode.Operation](constantLeftOperand, constantRightOperand);
					return new LiteralTreeNode(preCalculatedValue, node.Parent);
				}
			}

			return node;
		}

		TreeNode RemoveMultiplicationByZero(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			CheckChildrenRecursively(node, variableTable, RemoveMultiplicationByZero);

			return (node is BinaryOperationTreeNode bTreeNode && bTreeNode.Operation == BinaryOperation.Multiplication &&
				(IsZero(bTreeNode.LeftChild, variableTable) || IsZero(bTreeNode.RightChild, variableTable)))
				? new LiteralTreeNode(0, node.Parent)
				: node;
		}

		TreeNode RemoveMultiplicationByOne(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			CheckChildrenRecursively(node, variableTable, RemoveMultiplicationByOne);

			if (node is BinaryOperationTreeNode bTreeNode && bTreeNode.Operation == BinaryOperation.Multiplication)
			{
				return IsOne(bTreeNode.LeftChild, variableTable) ? bTreeNode.RightChild.Clone(bTreeNode.Parent)
					: IsOne(bTreeNode.RightChild, variableTable) ? bTreeNode.LeftChild.Clone(bTreeNode.Parent)
					: bTreeNode;
			}

			return node;
		}

		TreeNode RemoveZeroAdditionOrSubstraction(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			CheckChildrenRecursively(node, variableTable, RemoveZeroAdditionOrSubstraction);

			if (node is BinaryOperationTreeNode bTreeNode
				&& (bTreeNode.Operation == BinaryOperation.Addition
				||  bTreeNode.Operation == BinaryOperation.Substraction))
			{
				return IsZero(bTreeNode.LeftChild, variableTable) ? bTreeNode.RightChild.Clone(bTreeNode.Parent)
					: IsZero(bTreeNode.RightChild, variableTable) ? bTreeNode.LeftChild.Clone(bTreeNode.Parent)
					: bTreeNode;
			}

			return node;
		}

		TreeNode TrySwapConstantOperands(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			CheckChildrenRecursively(node, variableTable, TrySwapConstantOperands);
			
			return node;
		}

		TreeNode ReplaceSquareAndCubePowWithSqrAndCube(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			CheckChildrenRecursively(node, variableTable, ReplaceSquareAndCubePowWithSqrAndCube);

			if (node is BinaryOperationTreeNode bTreeNode
				&& bTreeNode.Operation == BinaryOperation.Power)
			{
				if (IsConstantValueOf(bTreeNode.RightChild, 2.0, variableTable))
				{
					return new UnaryOperationTreeNode(UnaryOperation.Square, bTreeNode.LeftChild, bTreeNode.Parent);
				}

				if (IsConstantValueOf(bTreeNode.RightChild, 3.0, variableTable))
				{
					return new UnaryOperationTreeNode(UnaryOperation.Cube, bTreeNode.LeftChild, bTreeNode.Parent);
				}
			}

			return node;
		}

		TreeNode RemoveZeroPower(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			CheckChildrenRecursively(node, variableTable, RemoveZeroPower);

			return (node is BinaryOperationTreeNode bTreeNode && bTreeNode.Operation == BinaryOperation.Power &&
				(IsZero(bTreeNode.RightChild, variableTable)))
				? new LiteralTreeNode(1.0, node.Parent)
				: node;
		}

		TreeNode RemoveFirstPower(TreeNode node, IReadOnlyTable<double> variableTable)
		{
			CheckChildrenRecursively(node, variableTable, RemoveFirstPower);

			return (node is BinaryOperationTreeNode bTreeNode && bTreeNode.Operation == BinaryOperation.Power &&
				(IsOne(bTreeNode.RightChild, variableTable)))
				? bTreeNode.LeftChild.Clone(bTreeNode.Parent)
				: node;
		}

		public FinishedFunction Optimize(FinishedFunction unoptimized)
		{
			throw new NotImplementedException();
		}

		public FinishedFunction OptimizeWithTable(FinishedFunction unoptimized)
		{
			FinishedFunction optimized = new FinishedFunction(
				unoptimized.TopNode.Clone(), 
				unoptimized.VariableTable, 
				unoptimized.FunctionTable, 
				unoptimized.ParameterCount);

			optimized.TopNode = CalculateConstant(optimized.TopNode, optimized.VariableTable);
			optimized.TopNode = RemoveMultiplicationByZero(optimized.TopNode, optimized.VariableTable);
			optimized.TopNode = RemoveMultiplicationByOne(optimized.TopNode, optimized.VariableTable);
			optimized.TopNode = RemoveZeroAdditionOrSubstraction(optimized.TopNode, optimized.VariableTable);
			optimized.TopNode = ReplaceSquareAndCubePowWithSqrAndCube(optimized.TopNode, optimized.VariableTable);
			optimized.TopNode = RemoveZeroPower(optimized.TopNode, optimized.VariableTable);
			optimized.TopNode = RemoveFirstPower(optimized.TopNode, optimized.VariableTable);

			return optimized;
		}
	}
}
