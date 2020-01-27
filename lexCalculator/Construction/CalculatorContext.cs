using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Construction
{
	public class CalculationContext
	{
		readonly Dictionary<string, double> variableTable;
		readonly Dictionary<string, Function> functionTable;

		public void AssignVariable(string name, double value)
		{
			if (variableTable.ContainsKey(name))
			{
				variableTable[name] = value;
			}
			else
			{
				variableTable.Add(name, value);
			}
		}

		public double GetVariableValue(string name)
		{
			if (!variableTable.ContainsKey(name)) throw new ArgumentException("No such variable in the table");
			return variableTable[name];
		}

		public CalculationContext()
		{
			variableTable = new Dictionary<string, double>();
		}
	}
}
