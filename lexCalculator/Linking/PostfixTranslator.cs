using lexCalculator.Types;
using System;
using System.IO;
using lexCalculator.Types.TreeNodes;

namespace lexCalculator.Linking
{
	// Converts function represented in tree form to postfix form
	// Example: (a + b) * c
	/*
	 *    [*]
	 *    / \
	 *  [+]  c
	 *  / \
	 * a   b
	 * 
	 * Converts to: a b + c *
	 * 
	 */

	public class PostfixTranslator : ITranslator<PostfixFunction>
	{
		// this calculator can't work with remote functions, so it inserts function trees directly into code
		void ConvertAndReplaceParameters(TreeNode node, IReadOnlyTable<FinishedFunction> functionTable, MemoryStream stream, TreeNode[] parameters)
		{
			// why rewrite old code?
			DefaultLinker linker = new DefaultLinker();
			node = linker.ReplaceParametersWithTreeNodes(node, parameters);
		}

		void ConvertRecursion(TreeNode node, IReadOnlyTable<FinishedFunction> functionTable, MemoryStream stream)
		{
			switch (node)
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
					stream.WriteByte((byte)PostfixFunction.PostfixCommand.PushLiteral);
					stream.Write(BitConverter.GetBytes(lNode.Value), 0, sizeof(double));
				}
				break;

				case FunctionParameterTreeNode fpNode:
				{
					stream.WriteByte((byte)PostfixFunction.PostfixCommand.PushParameter);
					stream.Write(BitConverter.GetBytes(fpNode.Index), 0, sizeof(int));
				}
				break;

				case VariableIndexTreeNode iNode:
				{
					stream.WriteByte((byte)PostfixFunction.PostfixCommand.PushVariable);
					stream.Write(BitConverter.GetBytes(iNode.Index), 0, sizeof(int));
				}
				break;

				case FunctionIndexTreeNode fiNode:
				{
					TreeNode nodeToInsert = functionTable[fiNode.Index].TopNode.Clone();
					ConvertAndReplaceParameters(nodeToInsert, functionTable, stream, fiNode.Parameters);

					ConvertRecursion(nodeToInsert, functionTable, stream);
				}
				break;

				case UnaryOperationTreeNode uNode:
				{
					ConvertRecursion(uNode.Child, functionTable, stream);

					stream.WriteByte((byte)PostfixFunction.PostfixCommand.CalculateUnary);
					stream.Write(BitConverter.GetBytes(uNode.Operation.Id), 0, sizeof(int));
				}
				break;

				case BinaryOperationTreeNode bNode:
				{
					// push in normal order, pop in reverse!
					ConvertRecursion(bNode.LeftChild, functionTable, stream);
					ConvertRecursion(bNode.RightChild, functionTable, stream);

					stream.WriteByte((byte)PostfixFunction.PostfixCommand.CalculateBinary);
					stream.Write(BitConverter.GetBytes(bNode.Operation.Id), 0, sizeof(int));
				}
				break;

				case TernaryOperationTreeNode tNode:
				{
					// push in normal order, pop in reverse!
					ConvertRecursion(tNode.LeftChild, functionTable, stream);
					ConvertRecursion(tNode.MiddleChild, functionTable, stream);
					ConvertRecursion(tNode.RightChild, functionTable, stream);

					stream.WriteByte((byte)PostfixFunction.PostfixCommand.CalculateTernary);
					stream.Write(BitConverter.GetBytes(tNode.Operation.Id), 0, sizeof(int));
				}
				break;
			}
		}

		public PostfixFunction Convert(FinishedFunction function)
		{
			MemoryStream stream = new MemoryStream();

			ConvertRecursion(function.TopNode, function.FunctionTable, stream);

			stream.WriteByte((byte)PostfixFunction.PostfixCommand.End);

			return new PostfixFunction(stream.GetBuffer(), function);
		}
	}
}
