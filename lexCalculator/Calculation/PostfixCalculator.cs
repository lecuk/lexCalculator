using lexCalculator.Types;

using System;
using System.Collections.Generic;
using System.IO;

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
						UnaryOperation operation = (UnaryOperation)BitConverter.ToInt32(buffer, 0);
						resultStack.Push(OperationImplementations.UnaryFunctions[operation](operand));
					}
					break;

					case PostfixFunction.PostfixCommand.CalculateBinary:
					{
						// pop in reverse order!
						double rightOperand = resultStack.Pop();
						double leftOperand = resultStack.Pop();

						codeStream.Read(buffer, 0, sizeof(int));
						BinaryOperation operation = (BinaryOperation)BitConverter.ToInt32(buffer, 0);
						resultStack.Push(OperationImplementations.BinaryFunctions[operation](leftOperand, rightOperand));
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
						UnaryOperation operation = (UnaryOperation)BitConverter.ToInt32(buffer, 0);

						OperationImplementations.UnaryArrayFunctions[operation](operands, values);

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
						BinaryOperation operation = (BinaryOperation)BitConverter.ToInt32(buffer, 0);

						OperationImplementations.BinaryArrayFunctions[operation](leftOperands, rightOperands, values);

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
