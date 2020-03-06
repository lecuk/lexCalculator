using System;
using System.Collections.Generic;
using System.IO;

using lexCalculator.Types;
using lexCalculator.Types.Operations;

namespace lexCalculator.Calculation
{
	public class PostfixCalculator : ICalculator<PostfixFunction>
	{
		public double Calculate(PostfixFunction function, double[] parameters)
		{
			Stack<double> resultStack = new Stack<double>();
			MemoryStream codeStream = function.GetStream();
			byte[] buffer = new byte[sizeof(double)];
			
			while (true)
			{
				int command = codeStream.ReadByte();
				if (command == -1) throw new Exception("Can't execute next command");

				switch ((PostfixFunction.PostfixCommand)command)
				{
					case PostfixFunction.PostfixCommand.End: return resultStack.Pop();

					case PostfixFunction.PostfixCommand.PushLiteral:
					{
						codeStream.Read(buffer, 0, sizeof(double));
						double literal = BitConverter.ToDouble(buffer, 0);
						resultStack.Push(literal);
					}
					break;

					case PostfixFunction.PostfixCommand.PushVariable:
					{
						codeStream.Read(buffer, 0, sizeof(int));
						int index = BitConverter.ToInt32(buffer, 0);
						resultStack.Push(function.OriginalFunction.VariableTable[index]);
					}
					break;

					case PostfixFunction.PostfixCommand.PushParameter:
					{
						codeStream.Read(buffer, 0, sizeof(int));
						int index = BitConverter.ToInt32(buffer, 0);
						resultStack.Push(parameters[index]);
					}
					break;

					case PostfixFunction.PostfixCommand.CalculateUnary:
					{
						double operand = resultStack.Pop();

						codeStream.Read(buffer, 0, sizeof(int));
						int id = BitConverter.ToInt32(buffer, 0);
						UnaryOperation operation = (UnaryOperation)Operation.AllOperations[id];
						resultStack.Push(operation.Function(operand));
					}
					break;

					case PostfixFunction.PostfixCommand.CalculateBinary:
					{
						// pop in reverse order!
						double rightOperand = resultStack.Pop();
						double leftOperand = resultStack.Pop();

						codeStream.Read(buffer, 0, sizeof(int));
						int id = BitConverter.ToInt32(buffer, 0);
						BinaryOperation operation = (BinaryOperation)Operation.AllOperations[id];
						resultStack.Push(operation.Function(leftOperand, rightOperand));
					}
					break;

					default: throw new Exception("Unknown command");
				}
			}
		}

		double[] CalculateMultipleWithBuffer(MemoryStream codeStream, IReadOnlyTable<double> table, double[][] parameters)
		{
			int iterations = parameters.GetLength(0);
			Stack<double[]> resultStack = new Stack<double[]>();
			byte[] buffer = new byte[sizeof(double)];

			// a small memory & time optimization: it allows multiple usage of number buffers after performing calculations on them
			Queue<double[]> freeValueBuffers = new Queue<double[]>();

			while (true)
			{
				int command = codeStream.ReadByte();
				if (command == -1) throw new Exception("Can't execute next command");

				double[] values = (freeValueBuffers.Count > 0) ? freeValueBuffers.Dequeue() : new double[iterations];

				if ((PostfixFunction.PostfixCommand)command == PostfixFunction.PostfixCommand.End) return resultStack.Pop();

				switch ((PostfixFunction.PostfixCommand)command)
				{
					case PostfixFunction.PostfixCommand.PushLiteral:
					{
						codeStream.Read(buffer, 0, sizeof(double));
						double literal = BitConverter.ToDouble(buffer, 0);

						for (int i = 0; i < iterations; ++i)
						{
							values[i] = literal;
						}
					}
					break;

					case PostfixFunction.PostfixCommand.PushVariable:
					{
						codeStream.Read(buffer, 0, sizeof(int));
						int index = BitConverter.ToInt32(buffer, 0);

						for (int i = 0; i < iterations; ++i)
						{
							values[i] = table[index];
						}
					}
					break;

					case PostfixFunction.PostfixCommand.PushParameter:
					{
						codeStream.Read(buffer, 0, sizeof(int));
						int index = BitConverter.ToInt32(buffer, 0);

						for (int i = 0; i < iterations; ++i)
						{
							values[i] = parameters[i][index];
						}
					}
					break;

					case PostfixFunction.PostfixCommand.CalculateUnary:
					{
						double[] operands = resultStack.Pop();

						codeStream.Read(buffer, 0, sizeof(int));
						int id = BitConverter.ToInt32(buffer, 0);
						UnaryOperation operation = (UnaryOperation)Operation.AllOperations[id];

						for (int i = 0; i < values.Length; ++i)
						{
							values[i] = operation.Function(operands[i]);
						}

						// add free buffer to the queue
						freeValueBuffers.Enqueue(operands);
					}
					break;

					case PostfixFunction.PostfixCommand.CalculateBinary:
					{
						// pop in reverse order!
						double[] rightOperands = resultStack.Pop();
						double[] leftOperands = resultStack.Pop();

						codeStream.Read(buffer, 0, sizeof(int));
						int id = BitConverter.ToInt32(buffer, 0);
						BinaryOperation operation = (BinaryOperation)Operation.AllOperations[id];

						for (int i = 0; i < values.Length; ++i)
						{
							values[i] = operation.Function(leftOperands[i], rightOperands[i]);
						}

						// add free buffers to the queue
						freeValueBuffers.Enqueue(rightOperands);
						freeValueBuffers.Enqueue(leftOperands);
					}
					break;
				}

				resultStack.Push(values);
			}
		}

		public double[] CalculateMultiple(PostfixFunction expression, double[][] parameters)
		{
			return CalculateMultipleWithBuffer(expression.GetStream(), expression.OriginalFunction.VariableTable, parameters);
		}
	}
}
