using System;
using System.Collections.Generic;
using lexCalculator.Types;
using lexCalculator.Types.TreeNodes;
using lexCalculator.Types.Operations;

namespace lexCalculator.Calculation
{
	// Tree calculator is calculating result from tree recursively. It is easier to understand and surprisingly fast enough but is not memory-efficient.
	public class TreeCalculator : ICalculator<FinishedFunction>
	{
		private double CalculateNode(TreeNode topNode, IReadOnlyTable<double> variableTable, IReadOnlyTable<FinishedFunction> functionTable, double[] parameters)
		{
			switch (topNode)
			{
				case UndefinedVariableTreeNode vNode:
				{
					throw new Exception(String.Format("Variable \"{0}\" is not defined", vNode.Name));
				}

				case UndefinedFunctionTreeNode fNode:
				{
					throw new Exception(String.Format("Function \"{0}\" is not defined", fNode.Name));
				}

				case NumberTreeNode lNode:
				{
					return lNode.Value;
				}

				case FunctionParameterTreeNode fpNode:
				{
					return parameters[fpNode.Index];
				}

				case FunctionIndexTreeNode iNode:
				{
					double[] localParameters = new double[iNode.Parameters.Length];

					for (int p = 0; p < iNode.Parameters.Length; ++p)
					{
						localParameters[p] = CalculateNode(iNode.Parameters[p], variableTable, functionTable, parameters);
					}

					// note that we call that function with its own local parameters
					return CalculateNode(functionTable[iNode.Index].TopNode.Clone(), variableTable, functionTable, localParameters);
				}

				case VariableIndexTreeNode iNode:
				{
					return variableTable[iNode.Index];
				}

				case UnaryOperationTreeNode uNode:
				{
					double operand = CalculateNode(uNode.Child, variableTable, functionTable, parameters);

					return uNode.Operation.Function(operand);
				}

				case BinaryOperationTreeNode bNode:
				{
					double leftOperand = CalculateNode(bNode.LeftChild, variableTable, functionTable, parameters);
					double rightOperand = CalculateNode(bNode.RightChild, variableTable, functionTable, parameters);

					return bNode.Operation.Function(leftOperand, rightOperand);
				}

				case TernaryOperationTreeNode tNode:
				{
					double leftOperand = CalculateNode(tNode.LeftChild, variableTable, functionTable, parameters);
					double middleOperand = CalculateNode(tNode.MiddleChild, variableTable, functionTable, parameters);
					double rightOperand = CalculateNode(tNode.RightChild, variableTable, functionTable, parameters);

					return tNode.Operation.Function(leftOperand, middleOperand, rightOperand);
				}

				default: throw new Exception("Unknown tree node");
			}
		}

		public double Calculate(FinishedFunction function, params double[] parameters)
		{
			return CalculateNode(function.TopNode, function.VariableTable, function.FunctionTable, parameters);
		}

		private double[] CalculateNodeMultiple(TreeNode topNode, IReadOnlyTable<double> variableTable, IReadOnlyTable<FinishedFunction> functionTable, double[][] parameters, Queue<double[]> freeValueBuffers)
		{
			int iterations = parameters.GetLength(0);
			double[] result = (freeValueBuffers.Count > 0) ? freeValueBuffers.Dequeue() : new double[iterations];

			switch (topNode)
			{
				case UndefinedVariableTreeNode vNode:
				{
					throw new Exception(String.Format("Variable \"{0}\" is not defined", vNode.Name));
				}

				case UndefinedFunctionTreeNode fNode:
				{
					throw new Exception(String.Format("Function \"{0}\" is not defined", fNode.Name));
				}

				case NumberTreeNode lNode:
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
						result[i] = parameters[i][fpNode.Index];
					}
					return result;
				}

				case VariableIndexTreeNode iNode:
				{
					for (int i = 0; i < iterations; ++i)
					{
						result[i] = variableTable[iNode.Index];
					}
					return result;
				}

				case FunctionIndexTreeNode iNode:
				{
					freeValueBuffers.Enqueue(result); // we don't need it

					double[][] localParameters = new double[iNode.Parameters.Length][];
					
					for (int p = 0; p < iNode.Parameters.Length; ++p)
					{
						localParameters[p] = CalculateNodeMultiple(iNode.Parameters[p], variableTable, functionTable, parameters, freeValueBuffers);
					}

					// note that we call that function with its own local parameters
					return CalculateNodeMultiple(functionTable[iNode.Index].TopNode, variableTable, functionTable, localParameters, freeValueBuffers);
				}

				case UnaryOperationTreeNode uNode:
				{
					double[] operands = CalculateNodeMultiple(uNode.Child, variableTable, functionTable, parameters, freeValueBuffers);

					for (int i = 0; i < result.Length; ++i)
					{
						result[i] = uNode.Operation.Function(operands[i]);
					}
					freeValueBuffers.Enqueue(operands);

					return result;
				}

				case BinaryOperationTreeNode bNode:
				{					
					double[] leftOperands = CalculateNodeMultiple(bNode.LeftChild, variableTable, functionTable, parameters, freeValueBuffers);
					double[] rightOperands = CalculateNodeMultiple(bNode.RightChild, variableTable, functionTable, parameters, freeValueBuffers);

					for (int i = 0; i < result.Length; ++i)
					{
						result[i] = bNode.Operation.Function(leftOperands[i], rightOperands[i]);
					}
					freeValueBuffers.Enqueue(leftOperands);
					freeValueBuffers.Enqueue(rightOperands);

					return result;
				}
				
				case TernaryOperationTreeNode tNode:
				{
					double[] leftOperands = CalculateNodeMultiple(tNode.LeftChild, variableTable, functionTable, parameters, freeValueBuffers);
					double[] middleOperands = CalculateNodeMultiple(tNode.MiddleChild, variableTable, functionTable, parameters, freeValueBuffers);
					double[] rightOperands = CalculateNodeMultiple(tNode.RightChild, variableTable, functionTable, parameters, freeValueBuffers);

					for (int i = 0; i < result.Length; ++i)
					{
						result[i] = tNode.Operation.Function(leftOperands[i], middleOperands[i], rightOperands[i]);
					}
					freeValueBuffers.Enqueue(leftOperands);
					freeValueBuffers.Enqueue(middleOperands);
					freeValueBuffers.Enqueue(rightOperands);

					return result;
				}

				default: throw new Exception("Unknown tree node");
			}
		}

		public double[] CalculateMultiple(FinishedFunction function, double[][] parameters)
		{
			// a small memory & time optimization: it allows multiple usage of number buffers after performing calculations on them
			Queue<double[]> freeValueBuffers = new Queue<double[]>();

			return CalculateNodeMultiple(function.TopNode, function.VariableTable, function.FunctionTable, parameters, freeValueBuffers);
		}
	}
}
