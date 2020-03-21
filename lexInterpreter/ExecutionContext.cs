using lexCalculator.Calculation;
using lexCalculator.Linking;
using lexCalculator.Parsing;
using lexCalculator.Types;
using lexCalculator.Processing;

namespace lexInterpreter
{
	class ExecutionContext
	{
		public CalculationContext CalculationContext { get; set; }
		public ILexer Lexer { get; set; }
		public IParser Parser { get; set; }
		public ILinker Linker { get; set; }
		public ICalculator<FinishedFunction> Calculator { get; set; }
		public IDifferentiator Differentiator { get; set; }
		public IOptimizer Optimizer { get; set; }

		public ExecutionContext(CalculationContext calculationContext)
		{
			CalculationContext = calculationContext;
			Lexer = new ExpressionLexer();
			Parser = new DefaultParser();
			Linker = new DefaultLinker(true, false);
			Calculator = new TreeCalculator();
			Differentiator = new DefaultDifferentiator();
			Optimizer = new DefaultOptimizer(2);
		}
	}
}
