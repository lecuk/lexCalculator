using System;
using System.Collections.Generic;
using lexCalculator.Types;

namespace lexCalculator.Linking
{
	// It rebuilds trees in a way that unfinished tree nodes and variable nodes are replaced with
	public class ShittyLinker : ILinker
	{
		TreeNode InsertFunction(FunctionTreeNode fTree, string functionName, FinishedFunction function)
		{
			// so, what do we do here. We search for functions with our name and "insert" copy of function tree in or original tree.
			// Also, we replace all parameters with trees specified in original tree
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

				case FunctionTreeNode fTree:
				{
					for (int i = 0; i < fTree.Parameters.Length; ++i)
					{
						fTree.Parameters[i] = LinkTree(fTree.Parameters[i], context, parameterNames);
					}

					if (!context.FunctionTable.ContainsKey(fTree.Name))
					{
						throw new Exception(String.Format("Function \"{0}\" is not defined", fTree.Name));
					}

					return InsertFunction(fTree, fTree.Name, context.FunctionTable[fTree.Name]);
				}

				case VariableTreeNode vTree:
				{
					for (int i = 0; i < parameterNames.Length; ++i)
					{
						if (vTree.Name == parameterNames[i]) return new FunctionParameterTreeNode(i);
					}

					if (context.VariableTable.Indexes.ContainsKey(vTree.Name))
					{
						return new IndexTreeNode(context.VariableTable.Indexes[vTree.Name]);
					}

					throw new Exception(String.Format("Variable \"{0}\" is not defined", vTree.Name));
				}

				default: break;
			}
			return tree;
		}

		TreeNode ReplaceParameterWithTreeNode(TreeNode tree, int index, TreeNode replacement)
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

				case FunctionTreeNode fTree:
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
			
			return new FinishedFunction(LinkTree(treeClone, context, parameterNames), context.VariableTable, parameterNames.Length);
		}
	}
}
