using System;
using System.Collections.Generic;
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
		static StatementLexer lexer = new StatementLexer();
		static StatementParser parser = new StatementParser();
		static ExecutionContext context = new ExecutionContext(StandardLibrary.GenerateStandardContext());
		static StatementVisualizer visualizer = new StatementVisualizer();

		// C:\Users\Asus\source\repos\lexCalculator\lexInterpreter\example2.txt
		static int RunFile(string path, CalculationContext context, bool allowCalculations, bool allowDefinitions)
		{
			string[] lines = File.ReadAllLines(path);
			List<Token[]> tokenLines = new List<Token[]>();
			for (int i = 0; i < lines.Length; ++i)
			{
				string fullLine = lines[i];
				string line = fullLine;
				try
				{
					if (String.IsNullOrWhiteSpace(line)) continue;

					int commentIndex = fullLine.IndexOf("//");
					if (commentIndex > 0)
					{
						line = fullLine.Substring(0, commentIndex);
					}
					else if (commentIndex == 0) continue;

					tokenLines.Add(lexer.Tokenize(line));
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error during file \"{0}\" lexing: {1}", path, ex.Message);
					Console.WriteLine("Line {0}: {1}", i, fullLine);
					return 3;
				}
			}

			/* PRINT TOKENS FOR DEBUG */
			foreach (var tokenLine in tokenLines)
			{
				Console.Write("[ ");
				foreach (var token in tokenLine)
				{
					Console.Write("{0} ", token);
				}
				Console.WriteLine("]");
			}

			Statement mainStatement;

			try
			{
				mainStatement = parser.ParseLines(tokenLines.ToArray());
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error during file \"{0}\" parsing: {1}", path, ex.Message);
				return 4;
			}

			/* PRINT TREE FOR DEBUG */
			visualizer.Visualize(mainStatement);
			
			try
			{
				mainStatement.Prepare();
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error during file \"{0}\" preparing: {1}", path, ex.Message);
				return 5;
			}

			try
			{
				mainStatement.Execute();
			}
			catch (ExitStatement.ExitException)
			{
				Console.WriteLine("Program finished.");
				return 0;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error during file \"{0}\" executing: {1}", path, ex.Message);
				return 6;
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
