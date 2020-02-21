using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using lexCalculator.Types;

namespace lexCalculator.Calculation
{
	// Tree calculator is calculating result from tree recursively. It is easier to understand and surprisingly fast enough but is not memory-efficient.
	public class TreeCalculator : ICalculator<FinishedFunction>
	{
		private double CalculateNode(TreeNode topNode, IReadOnlyTable<double> variableTable, IReadOnlyTable<FinishedFunction> functionTable, double[] parameters)
		{
			switch (topNode)
			{
				case UnknownVariableTreeNode vNode:
				{
					throw new Exception(String.Format("Variable \"{0}\" is not defined", vNode.Name));
				}

				case UnknownFunctionTreeNode fNode:
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

				case FunctionIndexTreeNode iNode:
				{
					double[] localParameters = new double[iNode.Parameters.Length];

					for (int p = 0; p < iNode.Parameters.Length; ++p)
					{
						localParameters[p] = CalculateNode(iNode.Parameters[p], variableTable, functionTable, parameters);
					}

					// note that we call that function with its own local parameters
					return CalculateNode(functionTable[iNode.Index].TopNode, variableTable, functionTable, localParameters);
				}

				case VariableIndexTreeNode iNode:
				{
					return variableTable[iNode.Index];
				}

				case UnaryOperationTreeNode uNode:
				{
					double operand = CalculateNode(uNode.Child, variableTable, functionTable, parameters);

					return OperationImplementations.UnaryFunctions[uNode.Operation](operand);
				}

				case BinaryOperationTreeNode bNode:
				{
					double leftOperand = CalculateNode(bNode.LeftChild, variableTable, functionTable, parameters);
					double rightOperand = CalculateNode(bNode.RightChild, variableTable, functionTable, parameters);

					return OperationImplementations.BinaryFunctions[bNode.Operation](leftOperand, rightOperand);
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
				case UnknownVariableTreeNode vNode:
				{
					throw new Exception(String.Format("Variable \"{0}\" is not defined", vNode.Name));
				}

				case UnknownFunctionTreeNode fNode:
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

					OperationImplementations.UnaryArrayFunctions[uNode.Operation](operands, result);
					freeValueBuffers.Enqueue(operands);

					return result;
				}

				case BinaryOperationTreeNode bNode:
				{					
					double[] leftOperands = CalculateNodeMultiple(bNode.LeftChild, variableTable, functionTable, parameters, freeValueBuffers);
					double[] rightOperands = CalculateNodeMultiple(bNode.RightChild, variableTable, functionTable, parameters, freeValueBuffers);

					OperationImplementations.BinaryArrayFunctions[bNode.Operation](leftOperands, rightOperands, result);
					freeValueBuffers.Enqueue(leftOperands);
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
