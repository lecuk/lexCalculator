using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexCalculator.Types;

namespace lexCalculator.Calculation
{
	// Tree calculator is calculating result from tree recursively. It is easier to understand but is much slower.
	class TreeCalculator : ICalculator<FinishedFunction>
	{
		private double CalculateNode(TreeNode topNode, IReadOnlyVariableTable variableTable, double[] parameters)
		{
			switch (topNode)
			{
				case VariableTreeNode vNode:
				{
					throw new Exception(String.Format("Variable \"{0}\" is not defined", vNode.Name));
				}

				case FunctionTreeNode fNode:
				{
					throw new Exception(String.Format("Function \"{0}\" is not defined", fNode.Name));
				}

				case LiteralTreeNode lNode:
				{
					return lNode.Value;
				}

				case FunctionParameterTreeNode fpNode:
				{
					return parameters[fpNode.Index];
				}

				case IndexTreeNode iNode:
				{
					return variableTable[iNode.Index];
				}

				case UnaryOperationTreeNode uNode:
				{
					double operand = CalculateNode(uNode.Child, variableTable, parameters);

					return BasicOperations.UnaryFunctions[uNode.Operation](operand);
				}

				case BinaryOperationTreeNode bNode:
				{
					double leftOperand = CalculateNode(bNode.LeftChild, variableTable, parameters);
					double rightOperand = CalculateNode(bNode.LeftChild, variableTable, parameters);

					return BasicOperations.BinaryFunctions[bNode.Operation](leftOperand, rightOperand);
				}

				default: throw new Exception("Unknown tree node");
			}
		}

		public double Calculate(FinishedFunction function, params double[] parameters)
		{
			return CalculateNode(function.TopNode, function.VariableTable, parameters);
		}

		private double[] CalculateNodeMultiple(TreeNode topNode, IReadOnlyVariableTable variableTable, double[,] parameters, Queue<double[]> freeValueBuffers)
		{
			int iterations = parameters.GetLength(0);
			double[] result = (freeValueBuffers.Count > 0) ? freeValueBuffers.Dequeue() : new double[iterations];

			switch (topNode)
			{
				case VariableTreeNode vNode:
				{
					throw new Exception(String.Format("Variable \"{0}\" is not defined", vNode.Name));
				}

				case FunctionTreeNode fNode:
				{
					throw new Exception(String.Format("Function \"{0}\" is not defined", fNode.Name));
				}

				case LiteralTreeNode lNode:
				{
					for (int i = 0; i < iterations; ++i)
					{
						result[i] = lNode.Value;
					}
					return result;
				}

				case FunctionParameterTreeNode fpNode:
				{
					for (int i = 0; i < iterations; ++i)
					{
						result[i] = parameters[i, fpNode.Index];
					}
					return result;
				}

				case IndexTreeNode iNode:
				{
					for (int i = 0; i < iterations; ++i)
					{
						result[i] = variableTable[iNode.Index];
					}
					return result;
				}

				case UnaryOperationTreeNode uNode:
				{
					double[] operands = CalculateNodeMultiple(uNode.Child, variableTable, parameters, freeValueBuffers);

					BasicOperations.UnaryArrayFunctions[uNode.Operation](operands, result);
					freeValueBuffers.Enqueue(operands);

					return result;
				}

				case BinaryOperationTreeNode bNode:
				{					
					double[] leftOperands = CalculateNodeMultiple(bNode.LeftChild, variableTable, parameters, freeValueBuffers);
					double[] rightOperands = CalculateNodeMultiple(bNode.RightChild, variableTable, parameters, freeValueBuffers);

					BasicOperations.BinaryArrayFunctions[bNode.Operation](leftOperands, rightOperands, result);
					freeValueBuffers.Enqueue(leftOperands);
					freeValueBuffers.Enqueue(rightOperands);

					return result;
				}

				default: throw new Exception("Unknown tree node");
			}
		}

		public double[] CalculateMultiple(FinishedFunction function, double[,] parameters)
		{
			// a small memory & time optimization: it allows multiple usage of number buffers after performing calculations on them
			Queue<double[]> freeValueBuffers = new Queue<double[]>();

			return CalculateNodeMultiple(function.TopNode, function.VariableTable, parameters, freeValueBuffers);
		}
	}
}
