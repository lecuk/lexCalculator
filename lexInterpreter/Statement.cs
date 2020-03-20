using lexCalculator.Calculation;
using lexCalculator.Types;
using lexCalculator.Types.Operations;
using System;
using System.Globalization;

namespace lexInterpreter
{
	abstract class Statement
	{
		public ExecutionContext Context { get; protected set; }

		public abstract void Prepare();
		public abstract void Execute();

		public Statement(ExecutionContext context)
		{
			Context = context;
		}
	}

	class EmptyStatement : Statement
	{
		public override void Prepare()
		{
		}

		public override void Execute()
		{
		}

		public EmptyStatement(ExecutionContext context) : base(context) { }
	}

	class ExitStatement : Statement
	{
		public class ExitException : Exception { }

		public override void Prepare()
		{
		}

		public override void Execute()
		{
			throw new ExitException();
		}

		public ExitStatement(ExecutionContext context) : base(context) {  }
	}

	class FunctionDefinitionStatement : Statement
	{
		public FunctionDefinitionStatement(ExecutionContext context, string functionName, string[] parameterNames, TreeNode functionDefinition)
			: base (context)
		{
			FunctionName = functionName;
			ParameterNames = parameterNames;
			FunctionDefinition = functionDefinition;
		}

		public string FunctionName { get; set; }
		public string[] ParameterNames { get; set; }
		public TreeNode FunctionDefinition { get; set; }

		private FinishedFunction DefinedFunction { get; set; }

		public override void Prepare()
		{
			DefinedFunction = Context.Linker.BuildFunction(FunctionDefinition, Context.CalculationContext, ParameterNames);
			Context.CalculationContext.FunctionTable.AssignItem(FunctionName, DefinedFunction);
		}

		public override void Execute()
		{
		}
	}

	class VariableAssignmentStatement : Statement
	{
		public string VariableName { get; set; }
		public BinaryOperatorOperation OperationToMake { get; set; }
		public TreeNode VariableDefinition { get; set; }

		private FinishedFunction DefinedExpression;

		public VariableAssignmentStatement(ExecutionContext context, string variableName, BinaryOperatorOperation operationToMake, TreeNode variableDefinition)
			: base(context)
		{
			VariableName = variableName;
			OperationToMake = operationToMake;
			VariableDefinition = variableDefinition;
		}

		public VariableAssignmentStatement(ExecutionContext context, string variableName, TreeNode variableDefinition)
			: this (context, variableName, null, variableDefinition) {  }

		public override void Prepare()
		{
			DefinedExpression = Context.Linker.BuildFunction(VariableDefinition, Context.CalculationContext, new string[0]);
			Context.CalculationContext.VariableTable.AssignItem(VariableName, Double.NaN);
		}

		public override void Execute()
		{
			double value = Context.Calculator.Calculate(DefinedExpression);
			if (OperationToMake != null)
			{
				double variableValue = Context.CalculationContext.VariableTable[VariableName];
				value = OperationToMake.Function(variableValue, value);
			}
			Context.CalculationContext.VariableTable.AssignItem(VariableName, value);
		}

		public bool IsDirect => (OperationToMake == null);
	}

	class ConditionalStatement : Statement
	{
		public TreeNode Condition { get; set; }
		public Statement StatementIfTrue { get; set; }
		public Statement StatementIfFalse { get; set; }

		private FinishedFunction DefinedCondition { get; set; }

		public ConditionalStatement(ExecutionContext context, TreeNode condition, Statement statementIfTrue, Statement statementIfFalse = null)
			: base(context)
		{
			Condition = condition;
			StatementIfTrue = statementIfTrue;
			StatementIfFalse = statementIfFalse;
		}

		public override void Prepare()
		{
			DefinedCondition = Context.Linker.BuildFunction(Condition, Context.CalculationContext, new string[0]);
			StatementIfTrue.Prepare();
			StatementIfFalse?.Prepare();
		}

		public override void Execute()
		{
			double value = Context.Calculator.Calculate(DefinedCondition);
			if (value != 0 && !Double.IsNaN(value))
			{
				StatementIfTrue.Execute();
			}
			else if (StatementIfFalse != null)
			{
				StatementIfFalse.Execute();
			}
		}
	}

	class LoopStatement : Statement
	{
		public TreeNode Condition { get; set; }
		public Statement StatementWhileTrue { get; set; }

		private FinishedFunction DefinedCondition { get; set; }
		
		public LoopStatement(ExecutionContext context, TreeNode condition, Statement statementWhileTrue)
			: base(context)
		{
			Condition = condition;
			StatementWhileTrue = statementWhileTrue;
		}

		public override void Prepare()
		{
			DefinedCondition = Context.Linker.BuildFunction(Condition, Context.CalculationContext, new string[0]);
			StatementWhileTrue.Prepare();
		}

		public override void Execute()
		{
			while (MoreMath.IsTrue(Context.Calculator.Calculate(DefinedCondition)))
			{
				StatementWhileTrue.Execute();
			}
		}
	}

	class MultiStatement : Statement
	{
		public Statement[] Statements { get; set; }

		public MultiStatement(ExecutionContext context, Statement[] statements)
			: base(context)
		{
			Statements = statements;
		}

		public override void Prepare()
		{
			for (int i = 0; i < Statements.Length; ++i)
			{
				Statements[i].Prepare();
			}
		}

		public override void Execute()
		{
			for (int i = 0; i < Statements.Length; ++i)
			{
				Statements[i].Execute();
			}
		}
	}

	class InputStatement : Statement
	{
		public string VariableName { get; set; }

		public InputStatement(ExecutionContext context, string variableName)
			: base(context)
		{
			VariableName = variableName;
		}

		public override void Prepare()
		{
		}

		public override void Execute()
		{
			double value;
			while (true)
			{
				Console.ForegroundColor = ConsoleColor.Green;
				Console.Write("> ");
				string input = Console.ReadLine();
				if (Double.TryParse(input, 
					NumberStyles.AllowExponent | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, 
					CultureInfo.InvariantCulture, 
					out value)) break;

				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine("Invalid input");
			}
			Console.ResetColor();

			Context.CalculationContext.VariableTable.AssignItem(VariableName, value);
		}
	}

	class OutputStatement : Statement
	{
		public TreeNode Expression { get; set; }

		private FinishedFunction DefinedExpression;

		public OutputStatement(ExecutionContext context, TreeNode expression)
			: base(context)
		{
			Expression = expression;
		}

		public override void Prepare()
		{
			DefinedExpression = Context.Linker.BuildFunction(Expression, Context.CalculationContext, new string[0]);
		}

		public override void Execute()
		{
			double value = Context.Calculator.Calculate(DefinedExpression);
			Console.Write("{0}", value.ToString("G6", System.Globalization.CultureInfo.InvariantCulture));
		}
	}

	class OutputStringStatement : Statement
	{
		public string Text { get; set; }

		public OutputStringStatement(ExecutionContext context, string text)
			: base(context)
		{
			Text = text;
		}

		public override void Prepare()
		{
		}

		public override void Execute()
		{
			Console.Write(Text);
		}
	}

	class OutputNewLineStatement : Statement
	{
		public OutputNewLineStatement(ExecutionContext context)
			: base(context) {  }

		public override void Prepare()
		{
		}

		public override void Execute()
		{
			Console.WriteLine();
		}
	}

	class ImportStatement : Statement
	{
		public string Path { get; set; }

		public ImportStatement(ExecutionContext context, string path)
			: base(context)
		{
			Path = path;
		}

		public override void Prepare()
		{
		}

		public override void Execute()
		{
			throw new NotImplementedException();
		}
	}
}
