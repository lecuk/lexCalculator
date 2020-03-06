using System;
using System.Collections.Generic;
using lexCalculator.Types.Operations;

namespace lexCalculator.Types
{
	public abstract class Operation
	{
		public enum ArgumentType
		{
			Unary,
			Binary,
			List,
			Variable
		}

		private static int lastId = 0;

		public readonly int Id;
		public readonly ArgumentType Type;
		public readonly string FunctionName;
		public readonly string SpecialFormat;
		public readonly bool ChildrenInSpecialFormatMayNeedBrackets;

		public bool HasSpecialFormat
		{
			get
			{
				return (SpecialFormat != null);
			}
		}
		
		protected Operation(ArgumentType type, string functionName, string specialFormat, bool childrenInSpecialFormatMayNeedBrackets)
		{
			Type = type;
			FunctionName = functionName ?? throw new ArgumentNullException(nameof(functionName));
			SpecialFormat = specialFormat;
			ChildrenInSpecialFormatMayNeedBrackets = (specialFormat != null) ? childrenInSpecialFormatMayNeedBrackets : false;

			Id = lastId;
			++lastId;
		}

		public bool Equals(Operation operation)
		{
			return (Id == operation.Id);
		}
	}
}
