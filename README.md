# lexCalculator

lexCalculator is a C# DLL for parsing and evaluating math expressions. 
It supports:
  * Number literals (both decimal & scientific notation)
  * Basic arithmetic operations
  * Unary sign operators & factorial
  * Parentnesis
  * Variables & constants
  * Standard functions & user-defined functions
  
Grammar: https://github.com/lexakogut/lexCalculator/blob/master/lexCalculator/grammar.txt

How it works:
  1. Parser divides raw string input into tokens
  2. Constructor creates an "abstract syntax tree" from these tokens
  3. Linker searches for user functions and variables to define them and finish syntax tree
  4. Optimizer makes tree shorter and it deletes unnecessary operations (not implemented yet)
  5. Convertor converts tree to some data structure which can be read by calculator designed for that structure
  6. Calculator reads data and fastly performs calculations
  
Why do we need to convert our syntax tree to byte code, you may ask? Because it will be calculated faster (i am planning to make a graph calculator using this library, so there will be a lot of cauculations)

Sources:
  - https://youtu.be/eF9qWbuQLuw
  - https://en.wikipedia.org/wiki/Abstract_syntax_tree
  - https://en.wikipedia.org/wiki/Shunting-yard_algorithm
