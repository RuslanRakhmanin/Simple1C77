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
        readonly AST Left;
        readonly Token Operation;
        readonly AST Right;

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
        Token Token;
        AST Expression;

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
        Token Token;
        string Value;

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
        Token Token;
        string Value;

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
        Token Token, Operation;
        AST Expression;

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
        Token Operation;
        AST Right, Left;

        public Assign(AST left, Token op, AST right)
        {
            Left = left;
            Operation = op;
            Right = right;
        }
    }

    public class IfAST : AST
    {
        Token Token;
        List<(AST, Compound)> IfBlocks;
        Compound ElseCompound;

        public IfAST(Token token, List<(AST, Compound)> ifBlocks, Compound elseCompound)
        {
            Token = token;
            IfBlocks = ifBlocks;
            ElseCompound = elseCompound;
        }
    }

    public class Type : AST
    {
        Token Token;
        string Value;

        public Type(Token token)
        {
            Token = token;
            Value = token.type;
        }
    }


    public class Var : AST
    {
        Token Token;
        string Value;
        
        public Var(Token token)
        {
            Token = token;
            Value = Token.value;
        }   
    }


    public class VarDecl : AST
    {
        Var VarNode;
        Type TypeNode;
        public VarDecl(Var varNode, Type typeNode)
        {
            VarNode = varNode;
            TypeNode = typeNode;
        }
    }

    public class Ternary : AST
    {
        Token Token;
        AST Condition;
        AST TrueStatement;
        AST FalseStatement;
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
        Token Token;
        AST Condition;
        Compound Compound;

        public While(Token token, AST condition, Compound compound)
        {
            Token = token;
            Condition = condition;
            Compound = compound;
        }
    }


    public class For : AST
    {
        Token Token;
        Assign AssignmentStatement;
        AST EndNumberExpression;
        Compound Compound;

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
        Token Token;
        string Value;

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
        Token Token;

        public Continue(Token token)
        {
            Token = token;
        }
    }


    public class Break : AST
    {
        Token Token;

        public Break(Token token)
        {
            Token = token;
        }
    }


    public class Compound : AST
    {
        List<AST> Children;
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
        List<AST> Declarations;
        Compound CompoundStatement;
        public Block(List<AST> declarations, Compound compoundStatement)
        {
            Declarations = declarations;
            CompoundStatement = compoundStatement;
        }
    }

    public class ProgramAST : AST
    {
        string Name;
        Block Block;

        public ProgramAST(string name, Block block)
        {
            Block = block;
            Name = name;
        }
    }


    public class Param : AST
    {
        // #TODO Add default initialisation compatibility
        Var VarNode;
        Type TypeNode;
        public Param(Var varNode, Type typeNode)
        {
            VarNode = varNode;
            TypeNode = typeNode;
        }
    }

    public class ProcedureDecl : AST
    {
        string ProcedureName;
        List<Param> ParamNodes;
        Compound CompoundNode;
        public ProcedureDecl(string procedureName, List<Param> paramNodes, Compound compoundNode)
        {
            ProcedureName = procedureName;
            ParamNodes = paramNodes;
            CompoundNode = compoundNode;
        }
    }

    public class FunctionDecl : AST
    {
        string FunctionName;
        List<Param> ParamNodes;
        Compound CompoundNode;
        public FunctionDecl(string functionName, List<Param> paramNodes, Compound compoundNode)
        {
            FunctionName = functionName;
            ParamNodes = paramNodes;
            CompoundNode = compoundNode;
        }
    }


    public class ProcedureCall : AST
    {
        string ProcedureName;
        List<AST> ParamExpressionNodes;
        public ProcedureCall(string procedureName, List<AST> paramExpressionNodes)
        {
            ProcedureName = procedureName;
            ParamExpressionNodes = paramExpressionNodes;
        }
    }

    public class FunctionCall : AST
    {
        string FunctionName;
        List<AST> ParamExpressionNodes;
        public FunctionCall(string functionName, List<AST> paramExpressionNodes)
        {
            FunctionName = functionName;
            ParamExpressionNodes = paramExpressionNodes;
        }
    }



}
