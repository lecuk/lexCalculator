using lexCalculator.Types;
using System;
using System.Collections.Generic;
using System.IO;

namespace lexCalculator.Linking
{
	public class ShittyConvertor : IConvertor<PostfixFunction>
	{
		void ConvertRecursion(TreeNode node, MemoryStream stream)
		{
			switch (node)
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
					stream.WriteByte((byte)PostfixFunction.CodeCommand.PushLiteral);
					stream.Write(BitConverter.GetBytes(lNode.Value), 0, sizeof(double));
				}
				break;

				case FunctionParameterTreeNode fpNode:
				{
					stream.WriteByte((byte)PostfixFunction.CodeCommand.PushParameter);
					stream.Write(BitConverter.GetBytes(fpNode.Index), 0, sizeof(int));
				}
				break;

				case IndexTreeNode iNode:
				{
					stream.WriteByte((byte)PostfixFunction.CodeCommand.PushVariable);
					stream.Write(BitConverter.GetBytes(iNode.Index), 0, sizeof(int));
				}
				break;

				case UnaryOperationTreeNode uNode:
				{
					ConvertRecursion(uNode.Child, stream);

					stream.WriteByte((byte)PostfixFunction.CodeCommand.CalculateUnary);
					stream.Write(BitConverter.GetBytes((int)uNode.Operation), 0, sizeof(int));
				}
				break;

				case BinaryOperationTreeNode bNode:
				{
					// push in normal order, pop in reverse!
					ConvertRecursion(bNode.LeftChild, stream);
					ConvertRecursion(bNode.RightChild, stream);

					stream.WriteByte((byte)PostfixFunction.CodeCommand.CalculateBinary);
					stream.Write(BitConverter.GetBytes((int)bNode.Operation), 0, sizeof(int));
				}
				break;
			}
		}

		public PostfixFunction Convert(FinishedFunction function)
		{
			MemoryStream stream = new MemoryStream();

			ConvertRecursion(function.TopNode, stream);

			stream.WriteByte((byte)PostfixFunction.CodeCommand.End);

			return new PostfixFunction(stream.GetBuffer(), function);
		}
	}
}
