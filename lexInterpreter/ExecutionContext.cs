using lexCalculator.Calculation;
using lexCalculator.Linking;
using lexCalculator.Parsing;
using lexCalculator.Types;

namespace lexInterpreter
{
	class ExecutionContext
	{
		public CalculationContext CalculationContext { get; set; }
		public ILexer Lexer { get; set; }
		public IParser Parser { get; set; }
		public ILinker Linker { get; set; }
		public ICalculator<FinishedFunction> Calculator { get; set; }
		
		public ExecutionContext(CalculationContext calculationContext, ILexer lexer, IParser parser, ILinker linker, ICalculator<FinishedFunction> calculator)
		{
			CalculationContext = calculationContext;
			Lexer = lexer;
			Parser = parser;
			Linker = linker;
			Calculator = calculator;
		}
	}
}
