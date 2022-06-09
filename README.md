# Simple1C77
A simple interpreter of [1C:Enterprise 7.7](https://en.wikipedia.org/wiki/1C:Enterprise#Versions_7.%D1%85_(7.0,_7.5,_7.7)) source code.

Created with C# dotNet.

Based on [Let’s Build A Simple Interpreter](https://github.com/rspivak/lsbasi) by [Ruslan Spivak](https://ruslanspivak.com/)

## General Information
This is a training pet-project. The main goal is to improve C# programming skills.

### Vocabulary
- Lexer. Process a pure text to a stream of lexemes (tokens)
- AST - Abstract Syntax Tree is a tree that represents the abstract syntactic structure of a language construct where each interior node and the root node represents an operator, and the children of the node represent the operands of that operator.
- Parser converts from a stream of lexemes to a structure called AST.
- Translator generates a source code from AST.

## Project Status
In a progress.

### Done
- Lexer class.
- AST classes.
- Passer class.
- NodeVisiter class and INodeVisiter interface.
- GenerateASTForDOT class as implementation of INodeVisiter interface. Go through AST and generate a .dot file for Graphviz. This class can convert inner AST to a graphical representation.

### To do
- Translator class. Can convert AST to a Python code.
- Interpreter.

## Acknowledgements
Many thanks to Ruslan Spivak for the tutorial.
