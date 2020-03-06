using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lexCalculator.Types.Operations
{
	public class VariableArgumentOperation : Operation
	{
		public VariableArgumentOperation(string functionName, string specialFormat, bool childrenInSpecialFormatMayNeedBrackets) 
			: base(ArgumentType.Variable, functionName, specialFormat, childrenInSpecialFormatMayNeedBrackets)
		{  }
	}
}
