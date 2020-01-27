using lexCalculator.Construction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Linking
{
	static class StandardFunctions
	{
		public static IReadOnlyDictionary<string, Function> Functions = new Dictionary<string, Function>()
		{
			{ "sin", new Function(new string[] { "x" }, 
					new UnaryOperationTreeNode(UnaryOperation.Sine, new VariableTreeNode("x"))) },

			{ "cos", new Function(new string[] { "x" },
					new UnaryOperationTreeNode(UnaryOperation.Cosine, new VariableTreeNode("x"))) },

			{ "tan", new Function(new string[] { "x" }, 
					new UnaryOperationTreeNode(UnaryOperation.Tangent, new VariableTreeNode("x"))) },

			{ "cot", new Function(new string[] { "x" }, 
					new UnaryOperationTreeNode(UnaryOperation.Cotangent, new VariableTreeNode("x"))) },

			{ "exp", new Function(new string[] { "x" }, 
					new UnaryOperationTreeNode(UnaryOperation.Exponent, new VariableTreeNode("x"))) },

			{ "sqrt", new Function(new string[] { "x" }, 
					new UnaryOperationTreeNode(UnaryOperation.SquareRoot, new VariableTreeNode("x"))) },

			{ "cbrt", new Function(new string[] { "x" }, 
					new UnaryOperationTreeNode(UnaryOperation.CubeRoot, new VariableTreeNode("x"))) },

			{ "floor", new Function(new string[] { "x" }, 
					new UnaryOperationTreeNode(UnaryOperation.Floor, new VariableTreeNode("x"))) },

			{ "ceil", new Function(new string[] { "x" }, 
					new UnaryOperationTreeNode(UnaryOperation.Ceil, new VariableTreeNode("x"))) },

			{ "ln", new Function(new string[] { "x" }, 
					new UnaryOperationTreeNode(UnaryOperation.NaturalLogarithm, new VariableTreeNode("x"))) },

			{ "abs", new Function(new string[] { "x" }, 
					new UnaryOperationTreeNode(UnaryOperation.Absolute, new VariableTreeNode("x"))) },


			{ "log", new Function(new string[] { "p", "x" }, 
					new BinaryOperationTreeNode(BinaryOperation.Logarithm, new VariableTreeNode("p"), new VariableTreeNode("x"))) },

			{ "nrt", new Function(new string[] { "n", "x" }, 
					new BinaryOperationTreeNode(BinaryOperation.NRoot, new VariableTreeNode("n"), new VariableTreeNode("x"))) }
		};
	}
}
