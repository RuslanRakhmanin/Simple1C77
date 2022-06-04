using System;
using System.Collections.Generic;
using System.Text;

namespace Simple1C77
{

    public class Parser
    {
        Lexer _lexer;
        Token CurrentToken;
        Token NextToken;

        public Parser(Lexer lexer)
        {
            _lexer = lexer;
            CurrentToken = _lexer.GetNextToken();
            NextToken = _lexer.GetNextToken();
        }

        public void Error()
        {
            throw new ArgumentException("Invalid syntax");
        }


        //Compare the current token type with the passed token
        //type and if they match then "eat" the current token
        //and assign the next token to the self.current_token,
        //        otherwise raise an exception.
        public void Eat(string tokenType)
        {
            if (CurrentToken.type == tokenType)
            {
                CurrentToken = NextToken;
                NextToken = _lexer.GetNextToken();
            }
            else
                CurrentToken.RaiseError(String.Format("Expected: {0}.", tokenType));
        }

        // Program : Block
        public AST Program()
        {
            return new ProgramAST("1s 7.7", Block());
        }

        // block : declarations compound_statement
        public Block Block()
        {
            return new Block(Declarations(), CompoundStatement());
        }

        //declarations  : (Перем (ID COMMA)+ (Экспорт)? SEMI)*
        //              | ((Процедура ID LPAREN(formal_parameters)? RPAREN block КонецПроцедуры SEMI)
        //              |   | (Функция ID LPAREN(formal_parameters)? RPAREN block КонецФункции SEMI))*
        //              | empty
        public List<AST> Declarations()
        {
            List<AST> declarations = new List<AST>();

            while (CurrentToken.type == Const.Perem)
            {
                Var varNode;
                Token noTypeToken;
                Const.ReservedTokens.TryGetValue(Const.NoType, out noTypeToken);
                Type noTypeType = new Type(noTypeToken);

                Eat(Const.Perem);
                varNode = new Var(CurrentToken); // first ID
                declarations.Add(new VarDecl(varNode, noTypeType));
                Eat(Const.Id);
                while(CurrentToken.type == Const.Comma)
                {
                    Eat(Const.Comma);
                    varNode = new Var(CurrentToken);
                    declarations.Add(new VarDecl(varNode, noTypeType));
                    Eat(Const.Id);
                }
                if(CurrentToken.type == Const.Export)
                {
                    Eat(Const.Export);
                    //#TODO Save variable in the global scope
                }
                Eat(Const.Semi);
            }
            while(CurrentToken.type == Const.Procedure 
                || CurrentToken.type == Const.Function)
            {
                if (CurrentToken.type == Const.Procedure)
                {
                    List<Param> parameters;
                    Eat(Const.Procedure);
                    string procName = CurrentToken.value;
                    Eat(Const.Id);
                    Eat(Const.LParen);
                    if (CurrentToken.value == Const.Id)
                        parameters = FormalParameters();
                    else
                        parameters = new List<Param>();
                    Compound compoundNode = CompoundStatement();
                    Eat(Const.ProcedureEnd);
                    Eat(Const.Semi);
                    declarations.Add(new ProcedureDecl(procName, parameters, compoundNode));
                } else if (CurrentToken.type == Const.Function)
                {
                    List<Param> parameters;
                    Eat(Const.Function);
                    string procName = CurrentToken.value;
                    Eat(Const.Id);
                    Eat(Const.LParen);
                    if (CurrentToken.value == Const.Id)
                        parameters = FormalParameters();
                    else
                        parameters = new List<Param>();
                    Compound compoundNode = CompoundStatement();
                    Eat(Const.FunctionEnd);
                    Eat(Const.Semi);
                    declarations.Add(new FunctionDecl(procName, parameters, compoundNode));
                }
            }

            return declarations;
        }

        // formal_parameters : ID (COMMA ID)*
        public List<Param> FormalParameters()
        {
            // #TODO Add default initialisation compatibility
            Var varNode;
            Token noTypeToken;
            Const.ReservedTokens.TryGetValue(Const.NoType, out noTypeToken);
            Type noTypeType = new Type(noTypeToken);
            List<Param> declarations = new List<Param>();

            varNode = new Var(CurrentToken);
            declarations.Add(new Param(varNode, noTypeType));
            Eat(Const.Id);
            while (CurrentToken.type == Const.Comma)
            {
                Eat(Const.Comma);
                varNode = new Var(CurrentToken);
                declarations.Add(new Param(varNode, noTypeType));
                Eat(Const.Id);
            }
            return declarations;
        }

        public Type TypeSpecification()
        {
            // #TODO not in use right now.
            // Need to add type specification to variable declaration sections 
            if (CurrentToken.type == Const.Integer)
                Eat(Const.Integer);
            else if (CurrentToken.type == Const.Real)
                Eat(Const.Real);
            else if (CurrentToken.type == Const.String)
                Eat(Const.String);
            else
                CurrentToken.RaiseError("Unknown type specification: " + CurrentToken.type);
            return new Type(CurrentToken);
        }

        public Compound CompoundStatement()
        {
            return new Compound(StatementList());
        }

        public Compound EmptyCompoundStatement()
        {
            return new Compound(new List<AST>() { Empty() });
        }

        //statement_list : statement
        //               | statement SEMI statement_list
        public List<AST> StatementList()
        {
            List<AST> result = new List<AST>();
            result.Add(Statement());
            while (CurrentToken.type == Const.Semi)
            {
                Eat(Const.Semi);
                result.Add(Statement());
            }

            return result;
        }

        //statement : assignment_statement
        //          | if statement
        //          | while statement
        //          | for statement
        //          | continue
        //          | break
        //          | procedure call
        //          | return_statement
        //          | empty
        public AST Statement()
        {
            AST node;
            if (CurrentToken.type == Const.Id)
                if (NextToken.type == Const.LParen)
                    node = ProcedureCall();
                else
                    node = AssignmentStatement();
            else if (CurrentToken.type == Const.IfConst)
                node = IfStatement();
            else if (CurrentToken.type == Const.While)
                node = WhileStatement();
            else if (CurrentToken.type == Const.For)
                node = ForStatement();
            else if (CurrentToken.type == Const.Return)
                node = ReturnStatement();
            else if (CurrentToken.type == Const.Comment)
                node = Comment();
            else if (CurrentToken.type == Const.Continue)
            {
                node = new Continue(CurrentToken);
                Eat(Const.Continue);
            }
            else if (CurrentToken.type == Const.Break)
            {
                node = new Break(CurrentToken);
                Eat(Const.Break);
            }
            else
                node = Empty();

            return node;
        }

        //parameters : expr (COMMA expr)*
        public List<AST> Parameters()
        {
            List<AST> ParamNodes = new List<AST>();
            ParamNodes.Add(Expression());
            while(CurrentToken.type == Const.Comma)
            {
                Eat(Const.Comma);
                ParamNodes.Add(Expression());
            }
            return ParamNodes;
        }

        //ProcedureCall : name LParent (parameters)? RParent
        public ProcedureCall ProcedureCall()
        {
            string procedureName = CurrentToken.value;
            Eat(Const.Id);
            Eat(Const.LParen);
            List<AST> ParamNodes = Parameters();
            Eat(Const.RParen);

            return new ProcedureCall(procedureName, ParamNodes);
        }

        //FunctionCall : name LParent (parameters)? RParent
        public FunctionCall FunctionCall()
        {
            string functionName = CurrentToken.value;
            Eat(Const.Id);
            Eat(Const.LParen);
            List<AST> ParamNodes = Parameters();
            Eat(Const.RParen);

            return new FunctionCall(functionName, ParamNodes);
        }

        //assignment_statement : variable EQUAL expr
        public Assign AssignmentStatement()
        {
            Var left = Variable();
            Token EqualToken = CurrentToken;
            Eat(Const.Equal);
            AST right = Expression();
            return new Assign(left, EqualToken, right);
        }

        //ternary_operator: QUESTION LPAREN logical_expr
        //                                 COMMA statement COMMA statement
        //                            RPAREN
        public Ternary TernaryOperator()
        {
            Token token = CurrentToken;
            Eat(Const.Question);
            Eat(Const.LParen);
            AST condition = LogicalExpression();
            Eat(Const.Comma);
            AST trueStatement = Expression();
            Eat(Const.Comma);
            AST falseStatement = Expression();
            Eat(Const.RParen);

            return new Ternary(token, condition, trueStatement, falseStatement);
        }


        public AST Empty()
        {
            return new NoOp();
        }

        //if_statement: ЕСЛИ logical_expr ТОГДА statement_list
        //                (ИНАЧЕЕСЛИ logical_expr ТОГДА statement_list)*
        //                (ИНАЧЕ statement_list)?
        //            КОНЕЦЕСЛИ
        public IfAST IfStatement()
        {
            Token token = CurrentToken;
            Compound ifCompound, elseCompound;
            AST condition;
            List<(AST, Compound)> ifblock = new List<(AST, Compound)>();

            Eat(Const.IfConst);
            condition = LogicalExpression();
            Eat(Const.Then);
            ifCompound = CompoundStatement();
            ifblock.Add((condition, ifCompound));

            while(CurrentToken.type == Const.ElseIfConst)
            {
                Eat(Const.ElseIfConst);
                condition = LogicalExpression();
                Eat(Const.Then);
                ifCompound = CompoundStatement();
                ifblock.Add((condition, ifCompound));
            }

            if (CurrentToken.type == Const.ElseConst)
            {
                Eat(Const.ElseConst);
                elseCompound = CompoundStatement();
            }
            else
                elseCompound = EmptyCompoundStatement();
            
            Eat(Const.EndIf);

            return new IfAST(token, ifblock, elseCompound);
        }

        
        //while_statement: ПОКА logical_expr ЦИКЛ
        //                    statement_list
        //                 КОНЕЦЦИКЛА
        public While WhileStatement()
        {
            Token token = CurrentToken;
            Compound compound;
            AST condition;

            Eat(Const.While);
            condition = LogicalExpression();
            Eat(Const.Loop);
            compound = CompoundStatement();
            Eat(Const.LoopEnd);

            return new While(token, condition, compound);
        }

        //for_statement: ДЛЯ assignment_statement ПО expr ЦИКЛ
        //                     statement_list
        //               КОНЕЦЦИКЛА
        public AST ForStatement()
        {
            Token token = CurrentToken;
            Assign assignmentStatement;
            Compound compound;
            AST endNumber;

            Eat(Const.For);
            assignmentStatement = AssignmentStatement();
            Eat(Const.To);
            endNumber = Expression();
            Eat(Const.Loop);
            compound = CompoundStatement();
            Eat(Const.LoopEnd);

            return new For(token, assignmentStatement, endNumber, compound);
        }

        // return_statement : RETURN expr
        public Return ReturnStatement()
        {
            Token token = CurrentToken;
            AST right;

            Eat(Const.Return);
            right = Expression();

            return new Return(token, right);
        }


        public Comment Comment()
        {
            return new Comment(CurrentToken);
        }

        //variable : ID
        public Var Variable()
        {
            Var node = new Var(CurrentToken);
            Eat(Const.Id);
            return node;
        }


        //logical_expr : logical_factor((LOGICAL_OR | LOGICAL_AND) logical_factor)*
        public AST LogicalExpression()
        {
            AST node;
            Token token;

            node = LogicalFactor();
            while(CurrentToken.type == Const.LogicalAnd 
                || CurrentToken.type == Const.LogicalOr)
            {
                token = CurrentToken;
                Eat(token.type);
                node = new BinOp(node, token, LogicalFactor());
            }

            return node;
        }

        //logical_factor : LPAREN logical_expr RPAREN
        //                | НЕ logical_factor
        //                | expr(> | < | >= | <= | = | <>) expr
        public AST LogicalFactor()
        {
            AST node;
            Token token;
            token = CurrentToken;
            if (CurrentToken.type == Const.LParen)
            {
                Eat(Const.LParen);
                node = LogicalExpression();
                Eat(Const.RParen);
                return node;
            }
            else if (CurrentToken.type == Const.LogicalNot)
            {
                Eat(Const.LogicalNot);
                node = new UnaryOp(token, LogicalFactor());
                return node;
            }
            else
            {
                node = Expression();
                token = CurrentToken;
                if (token.type == Const.Greater
                    || token.type == Const.GreaterEqual
                    || token.type == Const.Less
                    || token.type == Const.LessEqual
                    || token.type == Const.Equal
                    || token.type == Const.NotEqual)
                {
                    Eat(token.type);
                    node = new BinOp(node, token, Expression());
                }
                return node;
            }
        }

        //expr : term((PLUS | MINUS) term)*
        public AST Expression()
        {
            AST node;
            Token token;

            node = Term();
            while(CurrentToken.type == Const.Plus 
                || CurrentToken.type == Const.Minus)
            {
                token = CurrentToken;
                Eat(token.type);
                node = new BinOp(node, token, Term());
            }

            return node;
        }

        //term : factor ((MUL | INTEGER_DIV | FLOAT_DIV) factor)*
        public AST Term()
        {
            AST node;
            Token token;

            node = Factor();
            while(CurrentToken.type == Const.Mul
                || CurrentToken.type == Const.IntegerDiv
                || CurrentToken.type == Const.FloatDiv)
            {
                token = CurrentToken;
                Eat(token.type);
                node = new BinOp(node, token, Factor());
            }

            return node;
        }

        //factor : PLUS factor
        //          | MINUS factor
        //          | INTEGER_CONST
        //          | REAL_CONST
        //          | LPAREN expr RPAREN
        //          | variable
        //          | ternary_operator
        //          | function_call
        public AST Factor()
        {
            AST node;
            Token token;

            token = CurrentToken;
            if (token.type == Const.Plus)
            {
                Eat(token.type);
                node = new UnaryOp(token, Factor());
            }
            else if (token.type == Const.Minus)
            {
                Eat(token.type);
                node = new UnaryOp(token, Factor());
            }
            else if (token.type == Const.IntegerConst)
            {
                Eat(token.type);
                node = new Num(token);
            }
            else if (token.type == Const.RealConst)
            {
                Eat(token.type);
                node = new Num(token);
            }
            else if (token.type == Const.StringConst)
            {
                Eat(token.type);
                node = new StringData(token);
            }
            else if (token.type == Const.LParen)
            {
                Eat(Const.LParen);
                node = Expression();
                Eat(Const.RParen);
            }
            else if (token.type == Const.Question)
            {
                node = TernaryOperator();
            }
            else if (NextToken.type == Const.LParen)
            {
                node = FunctionCall();
            }
            else
                node = Variable();


            return node;
        }


        //block : declarations statement_list

        //declarations : (Перем (ID COMMA)+ (Экспорт)? SEMI)*
        //             | (Процедура ID LPAREN(formal_parameters)? RPAREN block КонецПроцедуры SEMI)*
        //             | (Функция ID LPAREN(formal_parameters)? RPAREN block КонецФункции SEMI)*
        //             | empty

        //formal_parameters : ID(COMMA ID)*

        //statement_list : statement
        //               | statement SEMI statement_list

        //statement : assignment_statement
        //          | if_statement
        //          | while statement
        //          | for statement
        //          | procedure_call
        //          | comment
        //          | return_statement
        //          | empty

        //assignment_statement : variable EQUAL expr

        //if_statement: ЕСЛИ expr ТОГДА statement_list(ИНАЧЕ statement_list)? КОНЕЦЕСЛИ SEMI


        //while_statement: ПОКА logical_expr ЦИКЛ
        //                    statement_list
        //                 КОНЕЦЦИКЛА

        //for_statement: ДЛЯ assignment_statement ПО expr ЦИКЛ
        //                    statement_list
        //               КОНЕЦЦИКЛА

        //empty :

        //expr : term((PLUS | MINUS) term)*

        //term : factor((MUL | INTEGER_DIV | FLOAT_DIV) factor)*

        //factor : PLUS factor
        //       | MINUS factor
        //       | INTEGER_CONST
        //       | REAL_CONST
        //       | LPAREN expr RPAREN
        //       | variable
        //       | function_call

        //variable: ID
        public AST Parse()
        {
            AST node;

            node = Program();

            return node;
        }
    }
}

