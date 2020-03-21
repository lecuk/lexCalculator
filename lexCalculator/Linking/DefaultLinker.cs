using System;
using System.Collections.Generic;
using lexCalculator.Types;
using lexCalculator.Types.TreeNodes;

namespace lexCalculator.Linking
{
	public class DefaultLinker : ILinker
	{
		public bool InsertFunctionTreesDirectly { get; set; }
		public bool InsertVariableValuesDirectly { get; set; }

		// so, we "insert" copy of function tree in or original tree.
		// Also, we replace all parameters with trees specified in original tree
		TreeNode InsertFunction(UndefinedFunctionTreeNode fTree, FinishedFunction function)
		{
			if (fTree.Parameters.Length != function.ParameterCount)
				throw new Exception(String.Format("Invalid parameter count in \"{0}\" call (expected {1}, actual {2})",
					fTree.Name, function.ParameterCount, fTree.Parameters.Length));

			TreeNode parent = fTree.Parent;
			TreeNode[] parameters = fTree.Parameters;

			TreeNode newTree = function.TopNode.Clone(parent);
			newTree = ReplaceParametersWithTreeNodes(newTree, parameters);

			return newTree;
		}

		TreeNode LinkUndefinedFunction(UndefinedFunctionTreeNode fTree, CalculationContext context, string[] parameterNames)
		{
			for (int i = 0; i < fTree.Parameters.Length; ++i)
			{
				fTree.Parameters[i] = LinkTree(fTree.Parameters[i], context, parameterNames);
			}

			if (context.FunctionTable.IsIdentifierDefined(fTree.Name))
			{
				// can't use ternary operator >:(
				if (InsertFunctionTreesDirectly)
					return InsertFunction(fTree, context.FunctionTable[fTree.Name]);
				else
					return new FunctionIndexTreeNode(context.FunctionTable.GetIndex(fTree.Name), fTree.Parameters, fTree.Parent);
			}

			throw new Exception(String.Format("Function \"{0}\" is not defined", fTree.Name));
		}

		TreeNode LinkUndefinedVariable(UndefinedVariableTreeNode vTree, CalculationContext context, string[] parameterNames)
		{
			for (int i = 0; i < parameterNames.Length; ++i)
			{
				if (vTree.Name == parameterNames[i]) return new FunctionParameterTreeNode(i);
			}

			if (context.VariableTable.IsIdentifierDefined(vTree.Name))
			{
				// can't optimize to ternary expression, idk why
				if (InsertVariableValuesDirectly)
					return new NumberTreeNode(context.VariableTable[vTree.Name], vTree.Parent);
				else
					return new VariableIndexTreeNode(context.VariableTable.GetIndex(vTree.Name), vTree.Parent);
			}

			throw new Exception(String.Format("Variable \"{0}\" is not defined", vTree.Name));
		}

		TreeNode LinkTree(TreeNode tree, CalculationContext context, string[] parameterNames)
		{
			switch (tree)
			{
				case UnaryOperationTreeNode uTree:
					uTree.Child = LinkTree(uTree.Child, context, parameterNames);
					return uTree;

				case BinaryOperationTreeNode bTree:
					bTree.LeftChild = LinkTree(bTree.LeftChild, context, parameterNames);
					bTree.RightChild = LinkTree(bTree.RightChild, context, parameterNames);
					return bTree;

				case TernaryOperationTreeNode bTree:
					bTree.LeftChild = LinkTree(bTree.LeftChild, context, parameterNames);
					bTree.MiddleChild = LinkTree(bTree.MiddleChild, context, parameterNames);
					bTree.RightChild = LinkTree(bTree.RightChild, context, parameterNames);
					return bTree;

				case UndefinedFunctionTreeNode fTree:
					return LinkUndefinedFunction(fTree, context, parameterNames);

				case UndefinedVariableTreeNode vTree:
					return LinkUndefinedVariable(vTree, context, parameterNames);

				default: break;
			}
			return tree;
		}

		public TreeNode ReplaceParameterTreeNode(FunctionParameterTreeNode pTree, TreeNode[] parameterTrees)
		{
			if (pTree.Index < 0 || pTree.Index >= parameterTrees.Length) throw new Exception("No such parameter in tree");
			TreeNode parent = pTree.Parent;
			TreeNode replacedTree = parameterTrees[pTree.Index];
			replacedTree.Parent = parent;
			return replacedTree;
		}

		public TreeNode ReplaceParametersWithTreeNodes(TreeNode tree, TreeNode[] parameterTrees)
		{
			switch (tree)
			{
				case FunctionParameterTreeNode pTree:
					return ReplaceParameterTreeNode(pTree, parameterTrees);

				case UnaryOperationTreeNode uTree:
					uTree.Child = ReplaceParametersWithTreeNodes(uTree.Child, parameterTrees);
					return uTree;

				case BinaryOperationTreeNode bTree:
					bTree.LeftChild = ReplaceParametersWithTreeNodes(bTree.LeftChild, parameterTrees);
					bTree.RightChild = ReplaceParametersWithTreeNodes(bTree.RightChild, parameterTrees);
					return bTree;

				case TernaryOperationTreeNode bTree:
					bTree.LeftChild = ReplaceParametersWithTreeNodes(bTree.LeftChild, parameterTrees);
					bTree.MiddleChild = ReplaceParametersWithTreeNodes(bTree.MiddleChild, parameterTrees);
					bTree.RightChild = ReplaceParametersWithTreeNodes(bTree.RightChild, parameterTrees);
					return bTree;

				case UndefinedFunctionTreeNode fTree:
				{
					for (int i = 0; i < fTree.Parameters.Length; ++i)
					{
						fTree.Parameters[i] = ReplaceParametersWithTreeNodes(fTree.Parameters[i], parameterTrees);
					}
					return fTree;
				}

				default: return tree;
			}
		}
		
		public FinishedFunction BuildFunction(TreeNode tree, CalculationContext context, string[] parameterNames)
		{
			TreeNode treeClone = tree.Clone();
			
			return new FinishedFunction(LinkTree(treeClone, context, parameterNames), context.VariableTable, context.FunctionTable, parameterNames.Length);
		}

		public DefaultLinker(bool insertFunctionTreesDirectly = false, bool insertVariableValuesDirectly = false)
		{
			InsertFunctionTreesDirectly = insertFunctionTreesDirectly;
			InsertVariableValuesDirectly = insertVariableValuesDirectly;
		}
	}
}