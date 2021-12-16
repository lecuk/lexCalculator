# lexCalculator

lexCalculator is a C# DLL for parsing and evaluating math expressions. 
It supports:
  * Number literals (both decimal & scientific notation)
  * Basic arithmetic operations
  * Unary sign operators & factorial
  * Parentnesis
  * Variables & constants
  * Standard functions & user-defined functions
  
Example program:
```
using System;
using lexCalculator.Parsing;
using lexCalculator.Types;
using lexCalculator.Linking;
using lexCalculator.Calculation;

class Program
{
    static void Main()
    {
        ILexer lexer = new DefaultLexer();
        IParser parser = new DefaultParser();
        ILinker linker = new DefaultLinker();
        ICalculator<FinishedFunction> calculator = new TreeCalculator();
   
        CalculationContext userContext = new CalculationContext();
  
        // if you need standard mathematical functions, use this
        // userContext.AssignContext(StandardLibrary.GenerateStandardContext());
    
        string expression = "2 + 2";
        Token[] tokens = lexer.Tokenize(expression);
        TreeNode tree = parser.Construct(tokens);
        FinishedFunction expressionFunction = linker.BuildFunction(tree, userContext, new string[0]);
        double value = calculator.Calculate(expressionFunction);
   
        Console.WriteLine(" = {0}", value);
    }
}
```

Example of function definition and usage
```
// func(x, y) = x^2 + 1/ln(y)
string functionDefinitionExpression = "x^2 + 1/ln(y)";
TreeNode functionTree = parser.Construct(lexer.Tokenize(functionDefinitionExpression));
FinishedFunction function = linker.BuildFunction(functionDefinitionExpression, userContext, new string[] { "x", "y" });
userContext.FunctionTable.AssignNewItem("func", function);
...   
string expression = "func(1, 2) + func(2, 1)";
TreeNode tree = parser.Construct(lexer.Tokenize(expression));
FinishedFunction expressionFunction = linker.BuildFunction(tree, userContext, new string[0]);
double value = calculator.Calculate(expressionFunction);
```
  
Grammar: https://github.com/lexakogut/lexCalculator/blob/master/lexCalculator/grammar.txt

How it works:
  1. Lexer divides raw string input into tokens
  2. Parser creates an "abstract syntax tree" from these tokens
  3. Linker searches for user functions and variables to define them and finish syntax tree
  4. Optimizer makes tree shorter and it deletes unnecessary operations
  5. Translator converts tree to some data structure which can be read by calculator designed for that structure (optional)
  6. Calculator reads tree or some custom data structure and finally performs calculations

Some useful info:
  - https://youtu.be/eF9qWbuQLuw
  - https://en.wikipedia.org/wiki/Abstract_syntax_tree
  - https://en.wikipedia.org/wiki/Shunting-yard_algorithm
