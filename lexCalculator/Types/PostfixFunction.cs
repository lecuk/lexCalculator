using lexCalculator.Calculation;
using System.IO;

namespace lexCalculator.Types
{
	public class PostfixFunction
	{
		public enum PostfixCommand
		{
			End, PushLiteral, PushVariable, PushParameter, CalculateUnary, CalculateBinary, CalculateTernary
		}

		private readonly byte[] Code;
		public readonly FinishedFunction OriginalFunction;

		public MemoryStream GetStream()
		{
			return new MemoryStream(Code, false);
		}

		public PostfixFunction(byte[] code, FinishedFunction originalFunction)
		{
			Code = (byte[])code.Clone();
			OriginalFunction = originalFunction;
		}
	}
}
