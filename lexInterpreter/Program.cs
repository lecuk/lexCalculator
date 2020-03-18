using System;
using System.IO;
using lexCalculator.Calculation;
using lexCalculator.Linking;
using lexCalculator.Parsing;
using lexCalculator.Types;
using lexCalculator.Types.TreeNodes;

namespace lexInterpreter
{
	class Program
	{
		static ILexer tokenizer = new DefaultLexer();
		static IParser constructor = new DefaultParser();
		static ILinker linker = new DefaultLinker();
		static ITranslator<PostfixFunction> convertor = new PostfixTranslator();
		static ICalculator<PostfixFunction> postfixCalculator = new PostfixCalculator();
		static ICalculator<FinishedFunction> treeCalculator = new TreeCalculator();

		static void DefineFunctionInput(UndefinedFunctionTreeNode fDefinition, string expressionInput, CalculationContext context)
		{
			string[] parameterNames = new string[fDefinition.Parameters.Length];
			for (int i = 0; i < parameterNames.Length; ++i)
			{
				if (fDefinition.Parameters[i] is UndefinedVariableTreeNode vTreeNode)
				{
					parameterNames[i] = vTreeNode.Name;
				}
				else throw new Exception("Invalid function definition");
			}
			Token[] tokens = tokenizer.Tokenize(expressionInput);
			TreeNode tree = constructor.Construct(tokens);
			FinishedFunction function = linker.BuildFunction(tree, context, parameterNames);
			context.FunctionTable.AssignItem(fDefinition.Name, function);
			Console.WriteLine(String.Format("def {0} = {1}", fDefinition, tree));
		}

		static void DefineVariableInput(UndefinedVariableTreeNode vDefinition, string expressionInput, CalculationContext context)
		{
			Token[] tokens = tokenizer.Tokenize(expressionInput);
			TreeNode tree = constructor.Construct(tokens);
			FinishedFunction function = linker.BuildFunction(tree, context, new string[0]);
			double value = treeCalculator.Calculate(function);

			context.VariableTable.AssignItem(vDefinition.Name, value);
			Console.WriteLine(String.Format("var {0} = {1} = {2}", vDefinition.Name, tree, value.ToString("G", System.Globalization.CultureInfo.InvariantCulture)));
		}

		static void CalculateInput(string expressionInput, CalculationContext context)
		{
			Token[] tokens = tokenizer.Tokenize(expressionInput);
			TreeNode tree = constructor.Construct(tokens);
			FinishedFunction function = linker.BuildFunction(tree, context, new string[0]);
			double value = treeCalculator.Calculate(function);

			Console.WriteLine(String.Format("{0} = {1}", expressionInput, value.ToString("G", System.Globalization.CultureInfo.InvariantCulture)));
		}

		static bool CheckLibraryImportString(string directoryPath, string line, CalculationContext context)
		{
			if (!line.StartsWith("~")) return false;

			string libraryPath = line.Substring(1);
			if (libraryPath == "standard")
			{
				context.AssignContext(StandardLibrary.GenerateStandardContext());
			}
			else RunFile(String.Format("{0}/{1}", directoryPath, libraryPath), context, false, true);

			return true;
		}
		
		static int RunFile(string path, CalculationContext context, bool allowCalculations, bool allowDefinitions)
		{
			string[] lines = File.ReadAllLines(path);
			for (int i = 0; i < lines.Length; ++i)
			{
				string fullLine = lines[i];
				string line = fullLine;
				try
				{
					int commentIndex = fullLine.IndexOf("//");
					if (commentIndex > 0)
					{
						line = fullLine.Substring(0, commentIndex);
					}

					if (String.IsNullOrWhiteSpace(line)) continue;

					if (CheckLibraryImportString(Directory.GetParent(path).FullName, line, context)) continue;

					string expressionString = line;
					TreeNode firstHalfTree = null;
					int equalsPos = line.IndexOf('=');
					if (equalsPos > 0)
					{
						if (!allowDefinitions) throw new Exception("Definitions are not allowed");

						expressionString = fullLine.Substring(equalsPos + 1);
						string identifierString = line.Substring(0, equalsPos);

						Token[] firstHalfTokens = tokenizer.Tokenize(identifierString);
						firstHalfTree = constructor.Construct(firstHalfTokens);

						switch (firstHalfTree)
						{
							case UndefinedFunctionTreeNode fDefinition:
								DefineFunctionInput(fDefinition, expressionString, context);
								break;

							case UndefinedVariableTreeNode vDefinition:
								DefineVariableInput(vDefinition, expressionString, context);
								break;

							default: throw new Exception("Unknown syntax");
						}
					}
					else
					{
						if (!allowCalculations) throw new Exception("Calculations are not allowed");

						CalculateInput(line, context);
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error during file \"{0}\" parsing: {1}", path, ex.Message);
					Console.WriteLine("Line {0}: {1}", i, fullLine);
					return 3;
				}
			}

			return 0;
		}

		static int MainWithoutWaiting(string[] args)
		{
			string path;
			if (args.Length == 0)
			{
				Console.Write("Enter file name to calculate: ");
				path = Console.ReadLine();
			}
			else if (args.Length == 1)
			{
				path = args[0];
			}
			else
			{
				Console.WriteLine("Usage: {0} [path]", System.AppDomain.CurrentDomain.FriendlyName);
				return 1;
			}

			if (!File.Exists(path))
			{
				Console.WriteLine("File \"{0}\" does not exist.", path);
				return 2;
			}

			return RunFile(path, new CalculationContext(), true, true);
		}

		static int Main(string[] args)
		{
			bool startupFromConsole = (args.Length > 0);

			int result = MainWithoutWaiting(args);
			if (!startupFromConsole)
			{
				Console.WriteLine("Press any key to exit...");
				Console.ReadKey();
			}

			return result;
		}
	}
}
