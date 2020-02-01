using System;
using System.Collections.Generic;

namespace lexCalculator.Types
{
	public class ExpressionContext
	{
		public readonly VariableIndexTable VariableTable;

		readonly Dictionary<string, Function> functionTable;
		public IReadOnlyDictionary<string, Function> FunctionTable => functionTable;

		public void AssignFunction(string name, Function function)
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

			Function function = functionTable[name];
			functionTable.Remove(name);
			AssignFunction(newName, function);
		}

		public void AssignContext(ExpressionContext anotherContext)
		{
			foreach (KeyValuePair<string, int> pair in anotherContext.VariableTable.Indexes)
			{
				VariableTable.AssignVariable(pair.Key, anotherContext.VariableTable[pair.Value]);
			}

			foreach (KeyValuePair<string, Function> pair in anotherContext.FunctionTable)
			{
				AssignFunction(pair.Key, pair.Value);
			}
		}

		public ExpressionContext()
		{
			VariableTable = new VariableIndexTable();
			functionTable = new Dictionary<string, Function>();
		}
	}
}
