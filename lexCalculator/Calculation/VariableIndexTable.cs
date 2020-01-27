using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Calculation
{
	public class VariableIndexTable
	{
		private readonly Dictionary<string, int> keys;
		public readonly IReadOnlyDictionary<string, int> Keys;
		public readonly double[] values;

		public VariableIndexTable(Dictionary<string, double> keyValueTable)
		{
			values = new double[keyValueTable.Count];
			keys = new Dictionary<string, int>();
			Keys = keys;

			int index = 0;
			foreach (KeyValuePair<string, double> pair in keyValueTable)
			{
				keys.Add(pair.Key, index);
				values[index] = pair.Value;
				++index;
			}
		}
	}
}
