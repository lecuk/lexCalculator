using System;
using System.Collections.Generic;
using lexCalculator.Parsing;
using lexCalculator.Types;
using lexCalculator.Linking;
using lexCalculator.Calculation;
using System.Diagnostics;

namespace lexCalculator.TestApp
{
	class Program
	{
		static void PrintLibrary(ExpressionContext context)
		{
			ExpressionVisualizer visualizer = new ExpressionVisualizer();

			Console.WriteLine("Variables: ");
			foreach (KeyValuePair<string, int> pair in context.VariableTable.Indexes)
			{
				Console.WriteLine("  {0} = {1}", pair.Key, context.VariableTable[pair.Value]);
			}
			Console.WriteLine();
			Console.WriteLine("Functions: ");
			foreach (KeyValuePair<string, Function> pair in context.FunctionTable)
			{
				Console.WriteLine(String.Format("  Function \"{0}\"", pair.Key));
				visualizer.VisualizeAsNormalEquation(pair.Value.TopNode);
				Console.WriteLine();
				visualizer.VisualizeAsTree(pair.Value.TopNode, "");
				Console.WriteLine();
			}
		}

		static void DefineFunctionInput(FunctionTreeNode fDefinition, string expressionInput, ExpressionContext context)
		{
			ITokenizer tokenizer = new ShittyTokenizer();
			IExpressionConstructor constructor = new ShittyExpressionConstructor();
			IFunctionBuilder linker = new ShittyLinker();

			string[] parameterNames = new string[fDefinition.Parameters.Length];
			for (int i = 0; i < parameterNames.Length; ++i)
			{
				if (fDefinition.Parameters[i] is VariableTreeNode vTreeNode)
				{
					parameterNames[i] = vTreeNode.Name;
				}
				else throw new Exception("Invalid function definition");
			}
			Token[] tokens = tokenizer.Tokenize(expressionInput);
			ExpressionTreeNode tree = constructor.Construct(tokens);
			Function function = linker.BuildFunction(tree, context, parameterNames);
			context.AssignFunction(fDefinition.Name, function);
		}

		static void DefineVariableInput(VariableTreeNode vDefinition, string expressionInput, ExpressionContext context)
		{
			ITokenizer tokenizer = new ShittyTokenizer();
			IExpressionConstructor constructor = new ShittyExpressionConstructor();
			IFunctionBuilder linker = new ShittyLinker();
			IConvertor<ByteExpression> convertor = new ShittyConvertor();
			ICalculator<ByteExpression> calculator = new ShittyCalculator();

			Token[] tokens = tokenizer.Tokenize(expressionInput);
			ExpressionTreeNode tree = constructor.Construct(tokens);
			Function function = linker.BuildFunction(tree, context, new string[0]);
			ByteExpression expression = convertor.Convert(function);
			double value = calculator.Calculate(expression, context.VariableTable);

			context.VariableTable.AssignVariable(vDefinition.Name, value);
		}

		static void CalculateInput(string expressionInput, ExpressionContext context)
		{
			ITokenizer tokenizer = new ShittyTokenizer();
			IExpressionConstructor constructor = new ShittyExpressionConstructor();
			IFunctionBuilder linker = new ShittyLinker();
			IConvertor<ByteExpression> convertor = new ShittyConvertor();
			ICalculator<ByteExpression> calculator = new ShittyCalculator();

			Token[] tokens = tokenizer.Tokenize(expressionInput);
			ExpressionTreeNode tree = constructor.Construct(tokens);
			Function function = linker.BuildFunction(tree, context, new string[0]);
			ByteExpression expression = convertor.Convert(function);
			double value = calculator.Calculate(expression, context.VariableTable);
			
			Console.WriteLine(String.Format(" = {0}", value.ToString("0.####", System.Globalization.CultureInfo.InvariantCulture)));
		}

		static void Main(string[] args)
		{
			ITokenizer tokenizer = new ShittyTokenizer();
			IExpressionConstructor constructor = new ShittyExpressionConstructor();
			IFunctionBuilder linker = new ShittyLinker();
			IConvertor<ByteExpression> convertor = new ShittyConvertor();
			ICalculator<ByteExpression> calculator = new ShittyCalculator();

			ExpressionContext userContext = new ExpressionContext();
			userContext.AssignContext(StandardLibrary.GetContext());
			userContext.AssignFunction("userfunc", new Function(
				new BinaryOperationTreeNode(BinaryOperation.Addition, new FunctionParameterTreeNode(0),
				new BinaryOperationTreeNode(BinaryOperation.Multiplication, new FunctionParameterTreeNode(1), new FunctionParameterTreeNode(2))),
				3));
			userContext.VariableTable.AssignVariable("myVar1", 666.666);
			userContext.VariableTable.AssignVariable("myVar2", 69);
			userContext.VariableTable.AssignVariable("myVar3", 420);

			ExpressionVisualizer visualizer = new ExpressionVisualizer();

			while (true)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("> ");
				string input = Console.ReadLine();
				Console.ResetColor();

				try
				{
					if (input.StartsWith("~"))
					{
						string funcName = input.Substring(1).TrimEnd(' ');
						visualizer.VisualizeAsTree(userContext.FunctionTable[funcName].TopNode, "");
						continue;
					}

					if (input.StartsWith("&"))
					{
						int calculations = 1000000;
						string funcName = input.Substring(1).TrimEnd(' ');
						Function function = userContext.FunctionTable[funcName];
						if (function.ParameterCount != 1) throw new Exception("Parameter count should be 1");
						ByteExpression expr = convertor.Convert(function);
						Stopwatch watch = new Stopwatch();
						double[] results = new double[calculations];
						watch.Start();
						for (int i = 0; i < calculations; i++)
						{
							results[i] = calculator.Calculate(expr, userContext.VariableTable, i);
						}
						watch.Stop();
						Console.WriteLine(String.Format("{0} calculations were done in {1}ms (1st way)", calculations, watch.ElapsedTicks / 10000.0));
						watch.Restart();
						var argsMatrix = new double[calculations, 1];
						for (int i = 0; i < calculations; i++)
						{
							argsMatrix[i, 0] = i;
						}
						results = calculator.CalculateMultiple(expr, userContext.VariableTable, argsMatrix);
						watch.Stop();
						Console.WriteLine(String.Format("{0} calculations were done in {1}ms (2nd way)", calculations, watch.ElapsedTicks / 10000.0));
						continue;
					}

					string expressionString = input;
					ExpressionTreeNode firstHalfTree = null;
					int equalsPos = input.IndexOf('=');
					if (equalsPos > 0)
					{
						expressionString = input.Substring(equalsPos + 1);
						string identifierString = input.Substring(0, equalsPos);

						Token[] firstHalfTokens = tokenizer.Tokenize(identifierString);
						firstHalfTree = constructor.Construct(firstHalfTokens);

						switch (firstHalfTree)
						{
							case FunctionTreeNode fDefinition:
								DefineFunctionInput(fDefinition, expressionString, userContext);
								break;

							case VariableTreeNode vDefinition:
								DefineVariableInput(vDefinition, expressionString, userContext);
								break;

							default: throw new Exception("Unknown syntax");
						}
					}
					else
					{
						CalculateInput(input, userContext);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(String.Format("Error: {0}", ex.Message));
				}
				Console.WriteLine();
			}
		}
	}
}
