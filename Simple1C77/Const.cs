using System;
using System.Collections.Generic;
using System.Text;

namespace Simple1C77
{
    public static class Const
    {
        public const string Integer = "INTEGER";
        public const string Real = "REAL";
        public const string String = "STRING";
        public const string Boolean = "BOOLEAN";
        public const string IntegerConst = "INTEGER_CONST";
        public const string RealConst = "REAL_CONST";
        public const string StringConst = "STRING_CONST";
        public const string Plus = "PLUS";
        public const string Minus = "MINUS";
        public const string Mul = "MUL";
        public const string IntegerDiv = "INTEGER_DIV";
        public const string FloatDiv = "FLOAT_DIV";
        public const string Equal = "EQUAL";
        public const string NotEqual = "NOTEQUAL";
        public const string Greater = "GREATER";
        public const string Less = "LESS";
        public const string GreaterEqual = "GREATER_EQUAL";
        public const string LessEqual = "LESS_EQUAL";
        public const string LogicalAnd = "И";
        public const string LogicalOr = "ИЛИ";
        public const string LogicalNot = "НЕ";
        public const string LParen = "LPAREN";
        public const string RParen = "RPAREN";
        public const string Id = "ID";
        //        public const string Assign        = "ASSIGN";
        public const string Semi = "SEMI";
        public const string Dot = "DOT";
        public const string Colon = "COLON";
        public const string Comma = "COMMA";
        public const string EOF = "EOF";
        public const string Export = "ЭКСПОРТ";
        public const string Perem = "ПЕРЕМ";
        public const string Procedure = "ПРОЦЕДУРА";
        public const string ProcedureEnd = "КОНЕЦПРОЦЕДУРЫ";
        public const string Function = "ФУНКЦИЯ";
        public const string Return = "ВОЗВРАТ";
        public const string FunctionEnd = "КОНЕЦФУНКЦИИ";
        public const string NoType = "NOTYPE";
        public const string Comment = "COMMENT";
        public const string IfConst = "ЕСЛИ";
        public const string Then = "ТОГДА";
        public const string ElseConst = "ИНАЧЕ";
        public const string ElseIfConst = "ИНАЧЕЕСЛИ";
        public const string EndIf = "КОНЕЦЕСЛИ";
        public const string While = "ПОКА";
        public const string Loop = "ЦИКЛ";
        public const string LoopEnd = "КОНЕЦЦИКЛА";
        public const string For = "ДЛЯ";
        public const string To = "ПО";
        public const string Continue = "ПРОДОЛЖИТЬ";
        public const string Break = "ПРЕРВАТЬ";
        public const string Question = "?";
        public const char None = Char.MinValue;


        //public static readonly Dictionary<string, string> ReservedKeywords = new Dictionary<string, string> 
        //{
        //      {Export, Export},
        //      {Perem, Perem},
        //      {Procedure, Procedure},
        //      {ProcedureEnd, ProcedureEnd},
        //      {Function, Function},
        //      {FunctionEnd, FunctionEnd},
        //      {Return, Return},
        //      {IfConst, IfConst},
        //      {Then, Then},
        //      {ElseConst, ElseConst},
        //      {ElseIfConst ,ElseIfConst},
        //      {EndIf, EndIf},
        //        //TODO to let use "и" as a variable name
        //      {LogicalAnd, LogicalAnd},
        //      {LogicalOr, LogicalOr},
        //      {LogicalNot, LogicalNot},
        //      {While, While},
        //      {Loop, Loop},
        //      {LoopEnd, LoopEnd},
        //      {Continue, Continue},
        //      {Break, Break},
        //      {For, For},
        //      {To, To},
        //      {Question, Question},
        //      {NoType, NoType}
        //};

        public static readonly List<string> ReservedKeywords = new List<string>()
        {
            Export, Perem, Procedure, ProcedureEnd, Function, FunctionEnd, Return, IfConst, Then, ElseConst, ElseIfConst, EndIf,
            LogicalAnd, LogicalOr, LogicalNot, While, Loop, LoopEnd, Continue, Break, For, To, Question, NoType
        };

        public static readonly Dictionary<string, Token> ReservedTokens = new Dictionary<string, Token>
        {
            {Integer, new Token(Integer,Integer) },
            {Real, new Token(Real,Real) },
            {String, new Token(String,String) },
            {NoType, new Token(NoType,NoType) }
        };
    }
}
