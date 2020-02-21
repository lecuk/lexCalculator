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

		public int this[string key]
		{
			get
			{
				return indexes[key];
			}
		}
		
		public bool IsIdentifierDefined(string name)
		{
			return indexes.ContainsKey(name);
		}

		public bool TryGetItemWithName(string key, out T item)
		{
			if (IsIdentifierDefined(key))
			{
				item = items[indexes[key]];
				return true;
			}
			item = default(T);
			return false;
		}

		public T GetItemWithName(string key)
		{
			return items[indexes[key]];
		}

		public T this[int index]
		{
			get
			{
				return items[index];
			}
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
