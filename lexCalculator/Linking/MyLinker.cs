using System;
using System.Collections.Generic;
using lexCalculator.Types;

namespace lexCalculator.Linking
{
	public class MyLinker : ILinker
	{
		public bool InsertFunctionTreesDirectly { get; set; }
		public bool InsertVariableValuesDirectly { get; set; }

		// so, we "insert" copy of function tree in or original tree.
		// Also, we replace all parameters with trees specified in original tree
		TreeNode InsertFunction(UnknownFunctionTreeNode fTree, FinishedFunction function)
		{
			if (fTree.Parameters.Length != function.ParameterCount)
				throw new Exception(String.Format("Invalid parameter count in \"{0}\" call (expected {1}, actual {2})",
					fTree.Name, function.ParameterCount, fTree.Parameters.Length));

			TreeNode parent = fTree.Parent;
			TreeNode[] parameters = fTree.Parameters;

			TreeNode newTree = function.TopNode.Clone();
			for (int i = 0; i < fTree.Parameters.Length; ++i)
			{
				newTree = ReplaceParameterWithTreeNode(newTree, i, parameters[i]);
			}
			newTree.Parent = parent;

			return newTree;
		}

		TreeNode LinkTree(TreeNode tree, CalculationContext context, string[] parameterNames)
		{
			switch (tree)
			{
				case UnaryOperationTreeNode uTree:
					uTree.Child = LinkTree(uTree.Child, context, parameterNames);
					break;

				case BinaryOperationTreeNode bTree:
					bTree.LeftChild = LinkTree(bTree.LeftChild, context, parameterNames);
					bTree.RightChild = LinkTree(bTree.RightChild, context, parameterNames);
					break;

				case UnknownFunctionTreeNode fTree:
				{
					for (int i = 0; i < fTree.Parameters.Length; ++i)
					{
						fTree.Parameters[i] = LinkTree(fTree.Parameters[i], context, parameterNames);
					}

					if (context.FunctionTable.IsIdentifierDefined(fTree.Name))
					{
						if (InsertFunctionTreesDirectly)
							return InsertFunction(fTree, context.FunctionTable.GetItemWithName(fTree.Name));
						
						return new FunctionIndexTreeNode(context.FunctionTable[fTree.Name], fTree.Parameters, tree.Parent);
					}

					throw new Exception(String.Format("Function \"{0}\" is not defined", fTree.Name));
				}

				case UnknownVariableTreeNode vTree:
				{
					for (int i = 0; i < parameterNames.Length; ++i)
					{
						if (vTree.Name == parameterNames[i]) return new FunctionParameterTreeNode(i);
					}

					if (context.VariableTable.IsIdentifierDefined(vTree.Name))
					{
						if (InsertVariableValuesDirectly) return new LiteralTreeNode(context.VariableTable[vTree.Name], tree.Parent);
						
						return new VariableIndexTreeNode(context.VariableTable[vTree.Name], tree.Parent);
					}

					throw new Exception(String.Format("Variable \"{0}\" is not defined", vTree.Name));
				}

				default: break;
			}
			return tree;
		}

		public TreeNode ReplaceParameterWithTreeNode(TreeNode tree, int index, TreeNode replacement)
		{
			if (tree is FunctionParameterTreeNode iTree)
			{
				if (iTree.Index == index)
				{
					TreeNode parent = tree.Parent;
					tree = replacement;
					tree.Parent = parent;
					return tree;
				}
			}

			// recursively checking children
			switch (tree)
			{
				case UnaryOperationTreeNode uTree:
					uTree.Child = ReplaceParameterWithTreeNode(uTree.Child, index, replacement);
					break;

				case BinaryOperationTreeNode bTree:
					bTree.LeftChild = ReplaceParameterWithTreeNode(bTree.LeftChild, index, replacement);
					bTree.RightChild = ReplaceParameterWithTreeNode(bTree.RightChild, index, replacement);
					break;

				case UnknownFunctionTreeNode fTree:
				{
					for (int i = 0; i < fTree.Parameters.Length; ++i)
					{
						fTree.Parameters[i] = ReplaceParameterWithTreeNode(fTree.Parameters[i], index, replacement);
					}
					break;
				}

				default: break;
			}
			return tree;
		}
		
		public FinishedFunction BuildFunction(TreeNode tree, CalculationContext context, string[] parameterNames)
		{
			TreeNode treeClone = tree.Clone();
			
			return new FinishedFunction(LinkTree(treeClone, context, parameterNames), context.VariableTable, context.FunctionTable, parameterNames.Length);
		}

		public MyLinker(bool insertFunctionTreesDirectly = false, bool insertVariableValuesDirectly = false)
		{
			InsertFunctionTreesDirectly = insertFunctionTreesDirectly;
			InsertVariableValuesDirectly = insertVariableValuesDirectly;
		}
	}
}
