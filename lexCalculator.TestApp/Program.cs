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
		static Stopwatch watch = new Stopwatch();
		static Random rand = new Random();

		static ITokenizer tokenizer = new ShittyTokenizer();
		static IExpressionConstructor constructor = new ShittyExpressionConstructor();
		static ILinker linker = new ShittyLinker();
		static IConvertor<PostfixFunction> convertor = new ShittyConvertor();
		static ICalculator<PostfixFunction> postfixCalculator = new PostfixCalculator();
		static ICalculator<FinishedFunction> treeCalculator = new TreeCalculator();

		static void PrintLibrarySummary(CalculationContext context)
		{
			Console.WriteLine("Functions: ");
			foreach (KeyValuePair<string, FinishedFunction> pair in context.FunctionTable)
			{
				Console.WriteLine("  {0}({1} argument{3}) = {2}", pair.Key, pair.Value.ParameterCount, pair.Value.TopNode,
					(pair.Value.ParameterCount != 1) ? "s" : String.Empty);
			}

			Console.WriteLine("Variables: ");
			foreach (KeyValuePair<string, int> pair in context.VariableTable.Indexes)
			{
				Console.WriteLine("  {0} = #[{1}] = {2}", pair.Key, pair.Value, context.VariableTable[pair.Value].ToString("G", System.Globalization.CultureInfo.InvariantCulture));
			}
		}

		static void PrintLibrary(CalculationContext context)
		{
			ExpressionVisualizer visualizer = new ExpressionVisualizer();

			Console.WriteLine("Variables: ");
			foreach (KeyValuePair<string, int> pair in context.VariableTable.Indexes)
			{
				Console.WriteLine("  {0} = {1}", pair.Key, context.VariableTable[pair.Value]);
			}
			Console.WriteLine();
			Console.WriteLine("Functions: ");
			foreach (KeyValuePair<string, FinishedFunction> pair in context.FunctionTable)
			{
				Console.WriteLine(String.Format("  Function \"{0}\"", pair.Key));
				Console.WriteLine(pair.Value.TopNode);
				Console.WriteLine();
				visualizer.VisualizeAsTree(pair.Value.TopNode, "");
				Console.WriteLine();
			}
		}

		static void DefineFunctionInput(FunctionTreeNode fDefinition, string expressionInput, CalculationContext context)
		{
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
			TreeNode tree = constructor.Construct(tokens);
			FinishedFunction function = linker.BuildFunction(tree, context, parameterNames);
			context.AssignFunction(fDefinition.Name, function);
		}

		static void DefineVariableInput(VariableTreeNode vDefinition, string expressionInput, CalculationContext context)
		{
			Token[] tokens = tokenizer.Tokenize(expressionInput);
			TreeNode tree = constructor.Construct(tokens);
			FinishedFunction function = linker.BuildFunction(tree, context, new string[0]);
			double value = treeCalculator.Calculate(function);

			context.VariableTable.AssignVariable(vDefinition.Name, value);
		}

		static void CalculateInput(string expressionInput, CalculationContext context)
		{
			Token[] tokens = tokenizer.Tokenize(expressionInput);
			TreeNode tree = constructor.Construct(tokens);
			FinishedFunction function = linker.BuildFunction(tree, context, new string[0]);
			double value = treeCalculator.Calculate(function);
			
			Console.WriteLine(String.Format(" = {0}", value.ToString("G", System.Globalization.CultureInfo.InvariantCulture)));
		}

		static void TestCalculator<T>(ICalculator<T> calculator, T function, VariableTable table, int parameters, int iterations, int tests, bool printResults)
		{
			long elapsed = 0;
			for (int t = 0; t < tests; ++t)
			{
				double[,] argsMatrix = new double[iterations, parameters];
				for (int i = 0; i < iterations; i++)
				{
					for (int p = 0; p < parameters; p++)
					{
						argsMatrix[i, p] = Math.Floor(rand.NextDouble() * 100 - 50.0) / 10;
					}
				}
				watch.Restart();
				double[] results = calculator.CalculateMultiple(function, argsMatrix);
				watch.Stop();
				if (printResults)
				{
					for (int i = 0; i < iterations; i++)
					{
						Console.Write("f(");
						if (parameters > 0) Console.Write("{0}", argsMatrix[i, 0].ToString("G5", System.Globalization.CultureInfo.InvariantCulture));
						for (int p = 1; p < parameters; p++)
						{
							Console.Write(", {0}", argsMatrix[i, p].ToString("G5", System.Globalization.CultureInfo.InvariantCulture));
						}
						Console.WriteLine(") = {0}", results[i].ToString("G", System.Globalization.CultureInfo.InvariantCulture));
					}
				}
				elapsed += watch.Elapsed.Ticks;
			}
			Console.WriteLine(String.Format("{0} calculations were done in {1}ms ({2} tests), median time for 1 execution: {3}ms",
				iterations, 
				(elapsed / 10000.0).ToString("G4", System.Globalization.CultureInfo.InvariantCulture), 
				tests, 
				(elapsed / 10000.0 / tests / iterations).ToString("G6", System.Globalization.CultureInfo.InvariantCulture)));
		}

		static void Main(string[] args)
		{
			CalculationContext userContext = new CalculationContext();
			userContext.AssignContext(StandardLibrary.GetContext());

			userContext.AssignFunction("length2d", linker.BuildFunction(constructor.Construct(tokenizer.Tokenize(
				"sqrt(x^2 + y^2)")), userContext, new string[] {
				"x", "y" }));
			userContext.AssignFunction("length3d", linker.BuildFunction(constructor.Construct(tokenizer.Tokenize(
				"sqrt(x^2 + y^2 + z^2)")), userContext, new string[] {
				"x", "y", "z" }));
			userContext.AssignFunction("distance2d", linker.BuildFunction(constructor.Construct(tokenizer.Tokenize(
				"length2d(x2 - x1, y2 - y1)")), userContext, new string[] {
				"x1", "y1", "x2", "y2" }));
			userContext.AssignFunction("distance3d", linker.BuildFunction(constructor.Construct(tokenizer.Tokenize(
				"length3d(x2 - x1, y2 - y1, z2 - z1)")), userContext, new string[] {
				"x1", "y1", "z1", "x2", "y2", "z2" }));

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
						if (String.IsNullOrWhiteSpace(funcName))
						{
							PrintLibrarySummary(userContext);
						}
						else if (funcName == "~")
						{
							PrintLibrary(userContext);
						}
						else visualizer.VisualizeAsTree(userContext.FunctionTable[funcName].TopNode, "");
						continue;
					}

					if (input.StartsWith("&"))
					{
						string funcName = input.Substring(1).TrimEnd(' ');
						FinishedFunction function = userContext.FunctionTable[funcName];
						PostfixFunction postfixFunction = convertor.Convert(function);
						Console.WriteLine("Testing functionality: ");
						TestCalculator(postfixCalculator, postfixFunction, userContext.VariableTable, function.ParameterCount, 10, 1, true);
						TestCalculator(treeCalculator, function, userContext.VariableTable, function.ParameterCount, 10, 1, true);
						Console.WriteLine("Testing speed: ");
						TestCalculator(postfixCalculator, postfixFunction, userContext.VariableTable, function.ParameterCount, 100000, 100, false);
						TestCalculator(treeCalculator, function, userContext.VariableTable, function.ParameterCount, 100000, 100, false);
						continue;
					}

					string expressionString = input;
					TreeNode firstHalfTree = null;
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
