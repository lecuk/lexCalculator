using lexCalculator.Types;
using System;

namespace lexInterpreter
{
	class StringToken : Token
	{
		public string Text;

		public StringToken(string text)
		{
			this.Text = text;
		}

		public override string ToString()
		{
			return String.Format("\"{0}\"", Text);
		}
	}
}
