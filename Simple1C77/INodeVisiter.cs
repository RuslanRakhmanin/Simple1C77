using System;
using System.Collections.Generic;
using System.Text;

namespace Simple1C77
{
    internal interface INodeVisiter
    {
        public void VisitProgramAST(ProgramAST node);

        public void VisitBlock(Block node);

        public void VisitIfAST(IfAST node);

        public void VisitWhile(While node);

        public void VisitFor(For node);

        public void VisitTernary(Ternary node);

        public void VisitVarDecl(VarDecl node);

        public void VisitProcedureDecl(ProcedureDecl node);

        public void VisitFunctionDecl(FunctionDecl node);

        public void VisitParam(Param node);

        public void VisitType(Type node);

        public void VisitNum(Num node);

        public void VisitStringData(StringData node);

        public void VisitBinOp(BinOp node);

        public void VisitUnaryOp(UnaryOp node);

        public void VisitCompound(Compound node);

        public void VisitProcedureCall(ProcedureCall node);

        public void VisitFunctionCall(FunctionCall node);

        public void VisitReturn(Return node);

        public void VisitAssign(Assign node);

        public void VisitVar(Var node);

        public void VisitBreak(AST node);

        public void VisitContinue(AST node);

        public void VisitNoOp(AST node);

        public void VisitNoneType(AST node);


    }
}
