using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Types
{
	public class VariableTable
	{
		public const int DEFAULT_TABLE_SIZE = 1024; // i think 1024 unique variables is enough

		private readonly Dictionary<string, int> indexes;
		public IReadOnlyDictionary<string, int> Indexes => indexes;
		private readonly double[] values;
		private int curIndex;

		public double this[int i]
		{
			get
			{
				return values[i];
			}
		}

		public void RenameVariable(string name, string newName)
		{
			int index = indexes[name];
			indexes.Remove(name);
			indexes.Add(newName, index);
		}

		public int AssignNewVariable(string name, double value)
		{
			indexes.Add(name, curIndex);
			values[curIndex] = value;
			return curIndex++;
		}

		public int AssignVariable(string name, double value)
		{
			if (!indexes.ContainsKey(name))
			{
				return AssignNewVariable(name, value);
			}
			else
			{
				int index = indexes[name];
				values[index] = value;
				return index;
			}
		}

		public VariableTable(int tableSize = DEFAULT_TABLE_SIZE)
		{
			values = new double[tableSize];
			curIndex = 0;
			indexes = new Dictionary<string, int>();
		}
	}
}
