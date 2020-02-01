using lexCalculator.Types;
using System.Collections.Generic;
using System.IO;
using System;

namespace lexCalculator.Calculation
{
	public class ShittyCalculator : ICalculator<ByteExpression>
	{
		double UnarySwitch(UnaryOperation operation, double operand)
		{
			switch (operation)
			{
				case UnaryOperation.Negative:
					return -operand;

				case UnaryOperation.Sign:
					return (operand > 0) ? 1.0 : (operand < 0) ? -1.0 : 0;

				case UnaryOperation.Sine:
					return Math.Sin(operand);

				case UnaryOperation.Cosine:
					return Math.Cos(operand);

				case UnaryOperation.Tangent:
					return Math.Tan(operand);

				case UnaryOperation.Cotangent:
					return 1.0 / Math.Tan(operand);

				case UnaryOperation.Secant:
					return 1.0 / Math.Cos(operand);

				case UnaryOperation.Cosecant:
					return 1.0 / Math.Sin(operand);

				case UnaryOperation.ArcSine:
					return Math.Asin(operand);

				case UnaryOperation.ArcCosine:
					return Math.Acos(operand);

				case UnaryOperation.ArcTangent:
					return Math.Atan(operand);

				case UnaryOperation.ArcCotangent:
					return Math.Atan(1.0 / operand);

				case UnaryOperation.ArcSecant:
					return Math.Acos(1.0 / operand);

				case UnaryOperation.ArcCosecant:
					return Math.Asin(1.0 / operand);

				case UnaryOperation.SineHyperbolic:
					return Math.Sinh(operand);

				case UnaryOperation.CosineHyperbolic:
					return Math.Cosh(operand);

				case UnaryOperation.TangentHyperbolic:
					return Math.Tanh(operand);

				case UnaryOperation.CotangentHyperbolic:
					return 1.0 / Math.Tanh(operand);

				case UnaryOperation.SecantHyperbolic:
					return 1.0 / Math.Cosh(operand);

				case UnaryOperation.CosecantHyperbolic:
					return 1.0 / Math.Sinh(operand);

				case UnaryOperation.Exponent:
					return Math.Exp(operand);

				case UnaryOperation.NaturalLogarithm:
					return Math.Log(operand);

				case UnaryOperation.SquareRoot:
					return Math.Sqrt(operand);

				case UnaryOperation.CubeRoot:
					throw new NotImplementedException();

				case UnaryOperation.Floor:
					return Math.Floor(operand);

				case UnaryOperation.Ceil:
					return Math.Ceiling(operand);

				case UnaryOperation.AbsoluteValue:
					return Math.Abs(operand);

				case UnaryOperation.Factorial:
					throw new NotImplementedException();

				default:
					return Double.NaN;
			}
		}

		double BinarySwitch(BinaryOperation operation, double leftOperand, double rightOperand)
		{
			switch (operation)
			{
				case BinaryOperation.Addition:
					return leftOperand + rightOperand;

				case BinaryOperation.Substraction:
					return leftOperand - rightOperand;

				case BinaryOperation.Multiplication:
					return leftOperand * rightOperand;

				case BinaryOperation.Division:
					return leftOperand / rightOperand;

				case BinaryOperation.Power:
					return Math.Pow(leftOperand, rightOperand);

				case BinaryOperation.Remainder:
					return Math.IEEERemainder(leftOperand, rightOperand);

				case BinaryOperation.Logarithm:
					throw new NotImplementedException();

				case BinaryOperation.NRoot:
					throw new NotImplementedException();

				default:
					return Double.NaN;
			}
		}

		void UnarySwitches(UnaryOperation operation, double[] operand, double[] result)
		{
			switch (operation)
			{
				case UnaryOperation.Negative:
					for (int i = 0; i < result.Length; ++i) result[i] = -operand[i];
					break;

				case UnaryOperation.Sign:
					for (int i = 0; i < result.Length; ++i) result[i] = (operand[i] > 0) ? 1.0 : (operand[i] < 0) ? -1.0 : 0;
					return;

				case UnaryOperation.Sine:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Sin(operand[i]);
					return;

				case UnaryOperation.Cosine:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Cos(operand[i]);
					return;

				case UnaryOperation.Tangent:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Tan(operand[i]);
					return;

				case UnaryOperation.Cotangent:
					for (int i = 0; i < result.Length; ++i) result[i] = 1.0 / Math.Tan(operand[i]);
					return;

				case UnaryOperation.Secant:
					for (int i = 0; i < result.Length; ++i) result[i] = 1.0 / Math.Cos(operand[i]);
					return;

				case UnaryOperation.Cosecant:
					for (int i = 0; i < result.Length; ++i) result[i] = 1.0 / Math.Sin(operand[i]);
					return;

				case UnaryOperation.ArcSine:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Asin(operand[i]);
					return;

				case UnaryOperation.ArcCosine:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Acos(operand[i]);
					return;

				case UnaryOperation.ArcTangent:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Atan(operand[i]);
					return;

				case UnaryOperation.ArcCotangent:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Atan(1.0 / operand[i]);
					return;

				case UnaryOperation.ArcSecant:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Acos(1.0 / operand[i]);
					return;

				case UnaryOperation.ArcCosecant:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Asin(1.0 / operand[i]);
					return;

				case UnaryOperation.SineHyperbolic:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Sinh(operand[i]);
					return;

				case UnaryOperation.CosineHyperbolic:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Cosh(operand[i]);
					return;

				case UnaryOperation.TangentHyperbolic:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Tanh(operand[i]);
					return;

				case UnaryOperation.CotangentHyperbolic:
					for (int i = 0; i < result.Length; ++i) result[i] = 1.0 / Math.Tanh(operand[i]);
					return;

				case UnaryOperation.SecantHyperbolic:
					for (int i = 0; i < result.Length; ++i) result[i] = 1.0 / Math.Cosh(operand[i]);
					return;

				case UnaryOperation.CosecantHyperbolic:
					for (int i = 0; i < result.Length; ++i) result[i] = 1.0 / Math.Sinh(operand[i]);
					return;

				case UnaryOperation.Exponent:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Exp(operand[i]);
					return;

				case UnaryOperation.NaturalLogarithm:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Log(operand[i]);
					return;

				case UnaryOperation.SquareRoot:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Sqrt(operand[i]);
					return;

				case UnaryOperation.CubeRoot:
					throw new NotImplementedException();

				case UnaryOperation.Floor:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Floor(operand[i]);
					return;

				case UnaryOperation.Ceil:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Ceiling(operand[i]);
					return;

				case UnaryOperation.AbsoluteValue:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Abs(operand[i]);
					return;

				case UnaryOperation.Factorial:
					throw new NotImplementedException();

				default:
					for (int i = 0; i < result.Length; ++i) result[i] = Double.NaN;
					return;
			}
		}

		void BinarySwitches(BinaryOperation operation, double[] leftOperand, double[] rightOperand, double[] result)
		{
			switch (operation)
			{
				case BinaryOperation.Addition:
					for (int i = 0; i < result.Length; ++i) result[i] = leftOperand[i] + rightOperand[i];
					return;

				case BinaryOperation.Substraction:
					for (int i = 0; i < result.Length; ++i) result[i] = leftOperand[i] - rightOperand[i];
					return;

				case BinaryOperation.Multiplication:
					for (int i = 0; i < result.Length; ++i) result[i] = leftOperand[i] * rightOperand[i];
					return;

				case BinaryOperation.Division:
					for (int i = 0; i < result.Length; ++i) result[i] = leftOperand[i] / rightOperand[i];
					return;

				case BinaryOperation.Power:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.Pow(leftOperand[i], rightOperand[i]);
					return;

				case BinaryOperation.Remainder:
					for (int i = 0; i < result.Length; ++i) result[i] = Math.IEEERemainder(leftOperand[i], rightOperand[i]);
					return;

				case BinaryOperation.Logarithm:
					throw new NotImplementedException();

				case BinaryOperation.NRoot:
					throw new NotImplementedException();

				default:
					for (int i = 0; i < result.Length; ++i) result[i] = Double.NaN;
					return;
			}
		}

		public double Calculate(ByteExpression expression, VariableIndexTable table, double[] parameters)
		{
			Stack<double> resultStack = new Stack<double>();
			MemoryStream codeStream = expression.GetStream();
			byte[] buffer = new byte[sizeof(double)];

			while (true)
			{
				int command = codeStream.ReadByte();
				if (command == -1) throw new Exception("Can't execute next command");

				switch ((ByteExpression.CodeCommand)command)
				{
					case ByteExpression.CodeCommand.End: return resultStack.Pop();

					case ByteExpression.CodeCommand.PushLiteral:
					{
						codeStream.Read(buffer, 0, sizeof(double));
						double literal = BitConverter.ToDouble(buffer, 0);
						resultStack.Push(literal);
					}
					break;

					case ByteExpression.CodeCommand.PushVariable:
					{
						codeStream.Read(buffer, 0, sizeof(int));
						int index = BitConverter.ToInt32(buffer, 0);
						resultStack.Push(table[index]);
					}
					break;

					case ByteExpression.CodeCommand.PushParameter:
					{
						codeStream.Read(buffer, 0, sizeof(int));
						int index = BitConverter.ToInt32(buffer, 0);
						resultStack.Push(parameters[index]);
					}
					break;

					case ByteExpression.CodeCommand.CalculateUnary:
					{
						double operand = resultStack.Pop();

						codeStream.Read(buffer, 0, sizeof(int));
						int operation = BitConverter.ToInt32(buffer, 0);
						resultStack.Push(UnarySwitch((UnaryOperation)operation, operand));
					}
					break;

					case ByteExpression.CodeCommand.CalculateBinary:
					{
						// pop in reverse order!
						double rightOperand = resultStack.Pop();
						double leftOperand = resultStack.Pop();

						codeStream.Read(buffer, 0, sizeof(int));
						int operation = BitConverter.ToInt32(buffer, 0);
						resultStack.Push(BinarySwitch((BinaryOperation)operation, leftOperand, rightOperand));
					}
					break;
				}
			}
		}

		void CalculateMultipleWithBuffer(MemoryStream codeStream, VariableIndexTable table, double[,] parameters, Stack<double[]> resultStack)
		{
			int parameterCount = parameters.GetLength(0);
			byte[] buffer = new byte[sizeof(double)];

			// a small memory & time optimization: it allows multiple usage of number buffers after performing calculations on them
			Queue<double[]> freeBuffers = new Queue<double[]>();

			while (true)
			{
				int command = codeStream.ReadByte();
				if (command == -1) throw new Exception("Can't execute next command");
				double[] values = (freeBuffers.Count > 0) ? freeBuffers.Dequeue() : new double[parameterCount];

				switch ((ByteExpression.CodeCommand)command)
				{
					case ByteExpression.CodeCommand.End: return;

					case ByteExpression.CodeCommand.PushLiteral:
					{
						codeStream.Read(buffer, 0, sizeof(double));
						double literal = BitConverter.ToDouble(buffer, 0);

						for (int i = 0; i < parameterCount; ++i)
						{
							values[i] = literal;
						}
						resultStack.Push(values);
					}
					break;

					case ByteExpression.CodeCommand.PushVariable:
					{
						codeStream.Read(buffer, 0, sizeof(int));
						int index = BitConverter.ToInt32(buffer, 0);
						
						for (int i = 0; i < parameterCount; ++i)
						{
							values[i] = table[index];
						}
						resultStack.Push(values);
					}
					break;

					case ByteExpression.CodeCommand.PushParameter:
					{
						codeStream.Read(buffer, 0, sizeof(int));
						int index = BitConverter.ToInt32(buffer, 0);
						
						for (int i = 0; i < parameterCount; ++i)
						{
							values[i] = parameters[i, index];
						}
						resultStack.Push(values);
					}
					break;

					case ByteExpression.CodeCommand.CalculateUnary:
					{
						double[] operands = resultStack.Pop();

						codeStream.Read(buffer, 0, sizeof(int));
						int operation = BitConverter.ToInt32(buffer, 0);

						UnarySwitches((UnaryOperation)operation, operands, values);
						resultStack.Push(values);

						// add free buffer to the queue
						freeBuffers.Enqueue(operands);
					}
					break;

					case ByteExpression.CodeCommand.CalculateBinary:
					{
						// pop in reverse order!
						double[] rightOperands = resultStack.Pop();
						double[] leftOperands = resultStack.Pop();

						codeStream.Read(buffer, 0, sizeof(int));
						int operation = BitConverter.ToInt32(buffer, 0);

						BinarySwitches((BinaryOperation)operation, leftOperands, rightOperands, values);
						resultStack.Push(values);

						// add free buffers to the queue
						freeBuffers.Enqueue(rightOperands);
						freeBuffers.Enqueue(leftOperands);
					}
					break;
				}
			}
		}

		public double[] CalculateMultiple(ByteExpression expression, VariableIndexTable table, double[,] parameters)
		{
			Stack<double[]> resultStack = new Stack<double[]>();
			CalculateMultipleWithBuffer(expression.GetStream(), table, parameters, resultStack);
			return resultStack.Pop();
		}
	}
}
