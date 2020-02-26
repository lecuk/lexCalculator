using System.Collections.Generic;

namespace lexCalculator.Types
{
	public interface IReadOnlyTable<T>
	{
		T this[int i] { get; }
		T this[string name] { get; }
		int GetIndex(string name);
	}
}