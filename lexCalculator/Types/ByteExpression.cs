using System.IO;

namespace lexCalculator.Types
{
	public class ByteExpression
	{
		public enum CodeCommand
		{
			End, PushLiteral, PushVariable, PushParameter, CalculateUnary, CalculateBinary
		}

		private readonly byte[] Code;

		public MemoryStream GetStream()
		{
			return new MemoryStream(Code, false);
		}

		public ByteExpression(byte[] code)
		{
			Code = (byte[])code.Clone();
		}
	}
}
