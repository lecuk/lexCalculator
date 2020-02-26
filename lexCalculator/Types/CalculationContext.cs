using System;
using System.Collections.Generic;

namespace lexCalculator.Types
{
	public class CalculationContext
	{
		public readonly Table<double> VariableTable;
		public readonly Table<FinishedFunction> FunctionTable;
		
		public void AssignContext(CalculationContext anotherContext)
		{
			foreach (string key in anotherContext.VariableTable.AllItemNames)
			{
				VariableTable.AssignNewItem(key, anotherContext.VariableTable[key]);
			}

			foreach (string key in anotherContext.FunctionTable.AllItemNames)
			{
				FunctionTable.AssignNewItem(key, anotherContext.FunctionTable[key]);
			}
		}

		public CalculationContext()
		{
			VariableTable = new Table<double>();
			FunctionTable = new Table<FinishedFunction>();
		}
	}
}
