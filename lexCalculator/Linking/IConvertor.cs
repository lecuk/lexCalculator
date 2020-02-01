using lexCalculator.Types;

namespace lexCalculator.Linking
{
	// Converts function to T data structure, which is finally used by calculator to get result
	public interface IConvertor<T>
	{
		T Convert(Function function);
	}
}
