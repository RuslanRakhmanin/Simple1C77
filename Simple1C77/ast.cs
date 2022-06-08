using System;
using System.Collections.Generic;
using System.Text;

namespace Simple1C77
{
    public class AST
    {
        public void RaiseError(string description = "")
        {
            throw new ArgumentException(string.Format("{0}: {1}", this.GetType().Name, description));
        }
    }

    public class BinOp: AST
    {
        public readonly AST Left;
        public readonly Token Operation;
        public readonly AST Right;

        public BinOp(AST left, Token op, AST right)
        {
            Left = left;
            Operation = op;
            Right = right;
        }

        public override string ToString()
        {
            return String.Format("{0}({1})", "BinOp", Operation.ToString());
        }

    }

    public class Return : AST
    {
        public readonly Token Token;
        public readonly AST Expression;

        public Return(Token token, AST expression)
        {
            Token = token;
            Expression = expression;
        }

        public override string ToString()
        {
            return String.Format("{0}({1})", "Return", Expression.ToString());
        }

    }

    public class Num : AST
    {
        public readonly Token Token;
        public readonly string Value;

        public Num(Token token)
        {
            Token = token;
            Value = token.value;
        }

        public override string ToString()
        {
            return String.Format("{0}({1})", "Num", Value);
        }

    }

    public class StringData : AST
    {
        public readonly Token Token;
        public readonly string Value;

        public StringData(Token token)
        {
            Token = token;
            Value = token.value;
        }

        public override string ToString()
        {
            return String.Format("{0}({1})", "StringData", Value);
        }

    }

    public class UnaryOp : AST
    {
        public readonly Token Token;
        public readonly Token Operation;
        public readonly AST Expression;

        public UnaryOp(Token op, AST expr)
        {
            Token = op;
            Operation = op;
            Expression = expr;
        }

        public override string ToString()
        {
            return String.Format("{0}({1})", "UnaryOp", Expression.ToString());
        }

    }

    public class Assign : AST
    {
        public readonly Token Operation;
        public readonly AST Left;
        public readonly AST Right;

        public Assign(AST left, Token op, AST right)
        {
            Left = left;
            Operation = op;
            Right = right;
        }

    }

    public class IfAST : AST
    {
        public readonly Token Token;
        public readonly List<(AST, Compound)> IfBlocks;
        public readonly Compound ElseCompound;

        public IfAST(Token token, List<(AST, Compound)> ifBlocks, Compound elseCompound)
        {
            Token = token;
            IfBlocks = ifBlocks;
            ElseCompound = elseCompound;
        }
    }

    public class Type : AST
    {
        public readonly Token Token;
        public readonly string Value;

        public Type(Token token)
        {
            Token = token;
            Value = token.type;
        }
    }


    public class Var : AST
    {
        public readonly Token Token;
        public readonly string Value;
        
        public Var(Token token)
        {
            Token = token;
            Value = Token.value;
        }   
    }


    public class VarDecl : AST
    {
        public readonly Var VarNode;
        public readonly Type TypeNode;
        public VarDecl(Var varNode, Type typeNode)
        {
            VarNode = varNode;
            TypeNode = typeNode;
        }
    }

    public class Ternary : AST
    {
        public readonly Token Token;
        public readonly AST Condition;
        public readonly AST TrueStatement;
        public readonly AST FalseStatement;
        public Ternary(Token token, AST condition, AST trueStatement, AST falseStatement)
        {
            Token = token;
            Condition = condition;
            TrueStatement = trueStatement;
            FalseStatement = falseStatement;
        }
    }

    public class While : AST
    {
        public readonly Token Token;
        public readonly AST Condition;
        public readonly Compound Compound;

        public While(Token token, AST condition, Compound compound)
        {
            Token = token;
            Condition = condition;
            Compound = compound;
        }
    }


    public class For : AST
    {
        public readonly Token Token;
        public readonly Assign AssignmentStatement;
        public readonly AST EndNumberExpression;
        public readonly Compound Compound;

        public For(Token token, Assign assignmentStatement, AST endNumberExpression, Compound compound)
        {
            Token = token;
            AssignmentStatement = assignmentStatement;
            EndNumberExpression = endNumberExpression;
            Compound = compound;

        }
    }


    public class Comment : AST
    {
        public readonly Token Token;
        public readonly string Value;

        public Comment(Token token)
        {
            Token = token;
            Value = token.value;
        }
    }


    public class NoOp : AST
    {
    }

    public class Continue : AST
    {
        public readonly Token Token;

        public Continue(Token token)
        {
            Token = token;
        }
    }


    public class Break : AST
    {
        public readonly Token Token;

        public Break(Token token)
        {
            Token = token;
        }
    }


    public class Compound : AST
    {
        public readonly List<AST> Children;

        public Compound(List<AST> children)
        {
            Children = new List<AST>();
            foreach (var statement in children)
            {
                Children.Add(statement);
            }
        }

    }

    public class Block : AST
    {
        public readonly List<AST> Declarations;
        public readonly Compound CompoundStatement;
        public Block(List<AST> declarations, Compound compoundStatement)
        {
            Declarations = declarations;
            CompoundStatement = compoundStatement;
        }
    }

    public class ProgramAST : AST
    {
        public readonly string Name;
        public readonly Block Block;

        public ProgramAST(string name, Block block)
        {
            Block = block;
            Name = name;
        }
    }


    public class Param : AST
    {
        // #TODO Add default initialisation compatibility
        public readonly Var VarNode;
        public readonly Type TypeNode;
        public Param(Var varNode, Type typeNode)
        {
            VarNode = varNode;
            TypeNode = typeNode;
        }
    }

    public class ProcedureDecl : AST
    {
        public readonly string ProcedureName;
        public readonly List<Param> ParamNodes;
        public readonly Block BlockNode;
        public ProcedureDecl(string procedureName, List<Param> paramNodes, Block blockNode)
        {
            ProcedureName = procedureName;
            ParamNodes = paramNodes;
            BlockNode = blockNode;
        }
    }

    public class FunctionDecl : AST
    {
        public readonly string FunctionName;
        public readonly List<Param> ParamNodes;
        public readonly Block BlockNode;
        public FunctionDecl(string functionName, List<Param> paramNodes, Block blockNode)
        {
            FunctionName = functionName;
            ParamNodes = paramNodes;
            BlockNode = blockNode;
        }
    }


    public class ProcedureCall : AST
    {
        public readonly string ProcedureName;
        public readonly List<AST> ParamExpressionNodes;
        public ProcedureCall(string procedureName, List<AST> paramExpressionNodes)
        {
            this.ProcedureName = procedureName;
            this.ParamExpressionNodes = paramExpressionNodes;
        }

    }

    public class FunctionCall : AST
    {
        public readonly string FunctionName;
        public readonly List<AST> ParamExpressionNodes;
        public FunctionCall(string functionName, List<AST> paramExpressionNodes)
        {
            FunctionName = functionName;
            ParamExpressionNodes = paramExpressionNodes;
        }

    }



}
