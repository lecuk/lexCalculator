using lexCalculator.Types.TreeNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexInterpreter
{
	class StatementVisualizer
	{
		ExpressionVisualizer expressionVisualizer = new ExpressionVisualizer();

		void VisualizeRecursion(Statement statement, string prefixStr)
		{
			expressionVisualizer.PrintPrefix(prefixStr);

			switch (statement)
			{
				case EmptyStatement emptyStatement:
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.WriteLine("{Empty}");
					Console.ResetColor();
					break;

				case ExitStatement exitStatement:
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("{Exit}");
					Console.ResetColor();
					break;

				case OutputNewLineStatement newlineStatement:
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("{NewLine}");
					Console.ResetColor();
					break;

				case InputStatement inputStatement:
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("{Input}");
					expressionVisualizer.PrintPrefix(prefixStr + (char)ConsoleColor.Green);
					Console.WriteLine(inputStatement.VariableName);
					Console.ResetColor();
					break;

				case OutputStatement outputStatement:
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("{Output}");
					expressionVisualizer.VisualizeAsTreeRecursion(outputStatement.Expression, 
						outputStatement.Context.CalculationContext.VariableTable, 
						outputStatement.Context.CalculationContext.FunctionTable, true, prefixStr + (char)ConsoleColor.Green);
					Console.ResetColor();
					break;

				case OutputStringStatement outputStringStatement:
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("{Output}");
					expressionVisualizer.PrintPrefix(prefixStr + (char)ConsoleColor.Green);
					Console.WriteLine("\"{0}\"", outputStringStatement.Text);
					Console.ResetColor();
					break;

				case ConditionalStatement ifStatement:
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("{If}");
					expressionVisualizer.VisualizeAsTreeRecursion(ifStatement.Condition,
						ifStatement.Context.CalculationContext.VariableTable,
						ifStatement.Context.CalculationContext.FunctionTable, true, prefixStr + (char)ConsoleColor.White);
					expressionVisualizer.PrintPrefix(prefixStr);
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("{Then}");
					VisualizeRecursion(ifStatement.StatementIfTrue, prefixStr + (char)ConsoleColor.White);
					if (ifStatement.StatementIfFalse != null)
					{
						expressionVisualizer.PrintPrefix(prefixStr);
						Console.ForegroundColor = ConsoleColor.White;
						Console.WriteLine("{Else}");
						VisualizeRecursion(ifStatement.StatementIfFalse, prefixStr + (char)ConsoleColor.White);
					}
					Console.ResetColor();
					break;

				case LoopStatement whileStatement:
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("{While}");
					expressionVisualizer.VisualizeAsTreeRecursion(whileStatement.Condition,
						whileStatement.Context.CalculationContext.VariableTable,
						whileStatement.Context.CalculationContext.FunctionTable, true, prefixStr + (char)ConsoleColor.White);
					expressionVisualizer.PrintPrefix(prefixStr);
					Console.ForegroundColor = ConsoleColor.White;
					Console.WriteLine("{Do}");
					VisualizeRecursion(whileStatement.StatementWhileTrue, prefixStr + (char)ConsoleColor.White);
					Console.ResetColor();
					break;

				case MultiStatement multiStatement:
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.WriteLine("{Multi}");
					if (multiStatement.Statements.Length > 0) VisualizeRecursion(multiStatement.Statements[0], prefixStr + (char)ConsoleColor.Gray);
					for (int i = 1; i < multiStatement.Statements.Length; ++i)
					{
						expressionVisualizer.PrintPrefix(prefixStr + (char)ConsoleColor.Gray);
						Console.WriteLine();
						VisualizeRecursion(multiStatement.Statements[i], prefixStr + (char)ConsoleColor.Gray);
					}
					Console.ResetColor();
					break;

				case VariableAssignmentStatement varAssignStatement:
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine("{Assign}");
					expressionVisualizer.PrintPrefix(prefixStr + (char)ConsoleColor.Cyan);
					Console.WriteLine(varAssignStatement.VariableName);
					expressionVisualizer.PrintPrefix(prefixStr);
					Console.ForegroundColor = ConsoleColor.Cyan;
					if (varAssignStatement.IsDirect)
						Console.WriteLine("{=}");
					else
						Console.WriteLine("{{{0}=}}", varAssignStatement.OperationToMake.Operator);
					expressionVisualizer.VisualizeAsTreeRecursion(varAssignStatement.VariableDefinition,
						varAssignStatement.Context.CalculationContext.VariableTable,
						varAssignStatement.Context.CalculationContext.FunctionTable, true, prefixStr + (char)ConsoleColor.Cyan);
					Console.ResetColor();
					break;

				case FunctionDefinitionStatement functionDefinitionStatement:
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine("{Assign}");
					expressionVisualizer.PrintPrefix(prefixStr + (char)ConsoleColor.Cyan);
					Console.Write("{0}(", functionDefinitionStatement.FunctionName);
					if (functionDefinitionStatement.ParameterNames.Length > 0) Console.Write(functionDefinitionStatement.ParameterNames[0]);
					for (int i = 1; i < functionDefinitionStatement.ParameterNames.Length; ++i)
					{
						Console.Write(", {0}", functionDefinitionStatement.ParameterNames[i]);
					}
					Console.WriteLine(")");
					expressionVisualizer.PrintPrefix(prefixStr);
					Console.ForegroundColor = ConsoleColor.Cyan;
					Console.WriteLine("{=}");
					expressionVisualizer.VisualizeAsTreeRecursion(functionDefinitionStatement.FunctionDefinition,
						functionDefinitionStatement.Context.CalculationContext.VariableTable,
						functionDefinitionStatement.Context.CalculationContext.FunctionTable, true, prefixStr + (char)ConsoleColor.Cyan);
					Console.ResetColor();
					break;

				case DerivativeAssignmentStatement functionDerivativeStatement:
					Console.ForegroundColor = ConsoleColor.DarkCyan;
					Console.WriteLine("{Assign}");
					expressionVisualizer.PrintPrefix(prefixStr + (char)ConsoleColor.DarkCyan);
					Console.Write("{0}(", functionDerivativeStatement.FunctionName);
					if (functionDerivativeStatement.ParameterNames.Length > 0) Console.Write(functionDerivativeStatement.ParameterNames[0]);
					for (int i = 1; i < functionDerivativeStatement.ParameterNames.Length; ++i)
					{
						Console.Write(", {0}", functionDerivativeStatement.ParameterNames[i]);
					}
					Console.WriteLine(")");
					expressionVisualizer.PrintPrefix(prefixStr);
					Console.ForegroundColor = ConsoleColor.DarkCyan;
					Console.WriteLine("{DerivativeByParameter}");
					expressionVisualizer.PrintPrefix(prefixStr + (char)ConsoleColor.DarkCyan);
					Console.WriteLine(functionDerivativeStatement.ParameterIndex);
					expressionVisualizer.PrintPrefix(prefixStr);
					Console.ForegroundColor = ConsoleColor.DarkCyan;
					Console.WriteLine("{OfFunction}");
					expressionVisualizer.PrintPrefix(prefixStr + (char)ConsoleColor.DarkCyan);
					Console.WriteLine(functionDerivativeStatement.FunctionDefinition.ToString());
					Console.ResetColor();
					break;

				case OptimizeTreeStatement optimizeStatement:
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine("{Optimize}");
					expressionVisualizer.PrintPrefix(prefixStr + (char)ConsoleColor.DarkYellow);
					Console.WriteLine(optimizeStatement.FunctionName.ToString());
					Console.ResetColor();
					break;

				case DrawTreeStatement drawTreeStatement:
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine("{DrawTree}");
					expressionVisualizer.PrintPrefix(prefixStr + (char)ConsoleColor.DarkYellow);
					Console.WriteLine(drawTreeStatement.FunctionName.ToString());
					Console.ResetColor();
					break;

				default:
					Console.ForegroundColor = ConsoleColor.Gray;
					Console.WriteLine("{?}");
					Console.ResetColor();
					break;
			}
		}

		public void Visualize(Statement statement)
		{
			VisualizeRecursion(statement, String.Empty);
		}
	}
}
