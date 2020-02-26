using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Types
{
	public class Table<T> : IReadOnlyTable<T>
	{
		public const int DEFAULT_TABLE_SIZE = 1024;

		private readonly T[] items;
		private int curIndex;

		private readonly Dictionary<string, int> indexes;
		public IEnumerable<string> AllItemNames => indexes.Keys;

		public T this[int index]
		{
			get
			{
				return items[index];
			}
		}

		public T this[string name]
		{
			get
			{
				if (!IsIdentifierDefined(name)) throw new Exception("No such item");

				return items[indexes[name]];
			}
		}
		
		public bool IsIdentifierDefined(string name)
		{
			return indexes.ContainsKey(name);
		}

		public int GetIndex(string name)
		{
			if (!IsIdentifierDefined(name)) throw new Exception("No such item");

			return indexes[name];
		}

		public void RenameItem(string name, string newName)
		{
			int index = indexes[name];
			indexes.Remove(name);
			indexes.Add(newName, index);
		}

		public int AssignNewItem(string name, T item)
		{
			indexes.Add(name, curIndex);
			items[curIndex] = item;
			return curIndex++;
		}

		public int AssignItem(string name, T item)
		{
			if (!indexes.ContainsKey(name))
			{
				return AssignNewItem(name, item);
			}
			else
			{
				int index = indexes[name];
				items[index] = item;
				return index;
			}
		}

		public Table(int tableSize = DEFAULT_TABLE_SIZE)
		{
			items = new T[tableSize];
			curIndex = 0;
			indexes = new Dictionary<string, int>();
		}
	}
}
