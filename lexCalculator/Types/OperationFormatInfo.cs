using System;

namespace lexCalculator.Types
{
	public struct OperationFormatInfo
	{
		public OperationFormatInfo(string functionName, string shortName, string specialFormat = null, bool childrenMayNeedBrackets = true) : this()
		{
			FunctionName = functionName ?? throw new ArgumentNullException(nameof(functionName));
			ShortName = shortName ?? throw new ArgumentNullException(nameof(shortName));
			SpecialFormat = specialFormat;
			ChildrenMayNeedBrackets = (specialFormat != null) ? childrenMayNeedBrackets : false;
		}

		public string FunctionName { get; set; }
		public string ShortName { get; set; }
		public string SpecialFormat { get; set; }
		public bool	ChildrenMayNeedBrackets { get; set; }

		public bool HasSpecialFormat
		{
			get
			{
				return SpecialFormat != null;
			}
		}
	}
}
