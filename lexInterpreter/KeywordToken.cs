using lexCalculator.Types;
using System;

namespace lexInterpreter
{
	class KeywordToken : Token
	{
		public enum Type
		{
			Input,
			Output,
			If,
			Else,
			While,
			Import,
			Exit,
			Newline
		}

		public readonly Type KeywordType;
		public readonly string Keyword;

		public KeywordToken(Type keywordType, string keyword)
		{
			this.KeywordType = keywordType;
			this.Keyword = keyword;
		}

		public override string ToString()
		{
			return String.Format("~{0}", Keyword);
		}
	}
}
