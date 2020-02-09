using System;
using System.Collections.Generic;

namespace lexCalculator.Types
{
	public class CalculationContext
	{
		public readonly VariableTable VariableTable;

		readonly Dictionary<string, FinishedFunction> functionTable;
		public IReadOnlyDictionary<string, FinishedFunction> FunctionTable => functionTable;

		public void AssignFunction(string name, FinishedFunction function)
		{
			if (functionTable.ContainsKey(name))
			{
				functionTable[name] = function;
			}
			else
			{
				functionTable.Add(name, function);
			}
		}

		public void RenameFunction(string name, string newName)
		{
			if (!functionTable.ContainsKey(name)) throw new ArgumentException(String.Format("Function \"{0}\" is not defined", name));
			if (functionTable.ContainsKey(newName)) throw new ArgumentException(String.Format("Function \"{0}\" is already defined", newName));

			FinishedFunction function = functionTable[name];
			functionTable.Remove(name);
			AssignFunction(newName, function);
		}

		public void AssignContext(CalculationContext anotherContext)
		{
			foreach (KeyValuePair<string, int> pair in anotherContext.VariableTable.Indexes)
			{
				VariableTable.AssignVariable(pair.Key, anotherContext.VariableTable[pair.Value]);
			}

			foreach (KeyValuePair<string, FinishedFunction> pair in anotherContext.FunctionTable)
			{
				AssignFunction(pair.Key, pair.Value);
			}
		}

		public CalculationContext()
		{
			VariableTable = new VariableTable();
			functionTable = new Dictionary<string, FinishedFunction>();
		}
	}
}
