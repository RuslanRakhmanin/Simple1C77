using System;
using System.Collections.Generic;
using System.Text;

namespace Simple1C77
{
    public class GenerateASTForDOT : NodeVisiter, INodeVisiter
    {
        int _ncount;
        string _dotHeader;
        StringBuilder _dotBody;
        string _dotFooter;
        Dictionary<AST, int> _nodeNumbers;

        public GenerateASTForDOT()
        {
            _ncount = 1;
            _dotHeader = @"
digraph astgraph {
    node[shape = circle, fontsize = 12, fontname = ""Courier"", height = .1];
    ranksep = .3;
    edge[arrowsize = .5]
            ";
            _dotFooter = "}";
            _dotBody = new StringBuilder();
            _nodeNumbers = new Dictionary<AST, int>();
        }

        void INodeVisiter.VisitAssign(Assign node)
        {
            string s;
            s = String.Format("  node{0} [label=\"{1}\"]", _ncount, node.Operation.value);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            Visit(node.Left);
            Visit(node.Right);

            s = String.Format("  node{0} -> node{1}", 
                _nodeNumbers[node], _nodeNumbers[node.Left]);
            _dotBody.AppendLine(s);

            s = String.Format("  node{0} -> node{1}",
                _nodeNumbers[node], _nodeNumbers[node.Right]);
            _dotBody.AppendLine(s);

        }

        void INodeVisiter.VisitBinOp(BinOp node)
        {
            string s;
            s = String.Format("  node{0} [label=\"{1}\"]", _ncount, node.Operation.value);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            Visit(node.Left);
            Visit(node.Right);

            s = String.Format("  node{0} -> node{1}",
                _nodeNumbers[node], _nodeNumbers[node.Left]);
            _dotBody.AppendLine(s);

            s = String.Format("  node{0} -> node{1}",
                _nodeNumbers[node], _nodeNumbers[node.Right]);
            _dotBody.AppendLine(s);
        }

        void INodeVisiter.VisitBlock(Block node)
        {
            string s;
            s = String.Format("  node{0} [label=\"Block\"]", _ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            foreach (AST childNode in node.Declarations)
            {
                Visit(childNode);
                s = String.Format("  node{0} -> node{1}",
                    _nodeNumbers[node], _nodeNumbers[childNode]);
                _dotBody.AppendLine(s);
            }

            Visit(node.CompoundStatement);
            s = String.Format("  node{0} -> node{1}",
                _nodeNumbers[node], _nodeNumbers[node.CompoundStatement]);
            _dotBody.AppendLine(s);


        }

        void INodeVisiter.VisitBreak(AST node)
        {
            string s;
            s = String.Format("  node{0} [label=\"Break\"]", _ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;
        }

        void INodeVisiter.VisitCompound(Compound node)
        {
            string s;
            s = String.Format("  node{0} [label=\"Compound\"]", _ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            foreach (AST childNode in node.Children)
            {
                Visit(childNode);
                s = String.Format("  node{0} -> node{1}",
                    _nodeNumbers[node], _nodeNumbers[childNode]);
                _dotBody.AppendLine(s);
            }
        }

        void INodeVisiter.VisitContinue(AST node)
        {
            string s;
            s = String.Format("  node{0} [label=\"Continue\"]", _ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;
        }

        void INodeVisiter.VisitFor(For node)
        {
            string s;
            s = String.Format("  node{0} [label=\"For\"]", _ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            Visit(node.AssignmentStatement);
            s = String.Format("  node{0} -> node{1} [label=\"From\"]",
                _nodeNumbers[node], _nodeNumbers[node.AssignmentStatement]);
            _dotBody.AppendLine(s);

            Visit(node.EndNumberExpression);
            s = String.Format("  node{0} -> node{1} [label=\"Till\"]",
                _nodeNumbers[node], _nodeNumbers[node.EndNumberExpression]);
            _dotBody.AppendLine(s);

            Visit(node.Compound);
            s = String.Format("  node{0} -> node{1} [label=\"Compound\"]",
                _nodeNumbers[node], _nodeNumbers[node.Compound]);
            _dotBody.AppendLine(s);
        }

        void INodeVisiter.VisitFunctionCall(FunctionCall node)
        {
            string s;

            s = String.Format("  node{0} [label=\"FunctionCall:{1}\"]",
                                _ncount, node.FunctionName);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            foreach(AST paramNode in node.ParamExpressionNodes)
            {
                Visit(paramNode);
                s = String.Format("  node{0} -> node{1}",
                    _nodeNumbers[node], _nodeNumbers[paramNode]);
                _dotBody.AppendLine(s);
            }
        }

        void INodeVisiter.VisitFunctionDecl(FunctionDecl node)
        {
            string s;

            s = String.Format("  node{0} [label=\"FunctionDecl:{1}\"]",
                                _ncount, node.FunctionName);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            foreach (AST paramNode in node.ParamNodes)
            {
                Visit(paramNode);
                s = String.Format("  node{0} -> node{1}",
                    _nodeNumbers[node], _nodeNumbers[paramNode]);
                _dotBody.AppendLine(s);
            }

            Visit(node.CompoundNode);
            s = String.Format("  node{0} -> node{1}",
                _nodeNumbers[node], _nodeNumbers[node.CompoundNode]);
            _dotBody.AppendLine(s);
        }

        void INodeVisiter.VisitIfAST(IfAST node)
        {
            string s;
            s = String.Format("  node{0} [label=\"If\"]", _ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            foreach((AST condition, Compound ifCompound) in node.IfBlocks)
            {
                Visit(condition);
                s = String.Format("  node{0} -> node{1} [label=\"Condition\"]",
                    _nodeNumbers[node], _nodeNumbers[condition]);
                _dotBody.AppendLine(s);
                Visit(ifCompound);
                s = String.Format("  node{0} -> node{1} [label=\"If compound\"]",
                    _nodeNumbers[node], _nodeNumbers[ifCompound]);
                _dotBody.AppendLine(s);
            }

            Visit(node.ElseCompound);
            s = String.Format("  node{0} -> node{1} [label=\"Else compound\"]",
                _nodeNumbers[node], _nodeNumbers[node.ElseCompound]);
            _dotBody.AppendLine(s);
        }

        void INodeVisiter.VisitNoneType(AST node)
        {
            //No visual for this
        }

        void INodeVisiter.VisitNoOp(AST node)
        {
            string s;
            s = String.Format("  node{0} [label=\"NoOp\"]", _ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;
        }

        void INodeVisiter.VisitNum(Num node)
        {
            string s;
            s = String.Format("  node{0} [label=\"{1}\"]", _ncount, node.Value);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;
        }

        void INodeVisiter.VisitParam(Param node)
        {
            string s;
            s = String.Format("  node{0} [label=\"Param\"]", _ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            Visit(node.VarNode);
            s = String.Format("  node{0} -> node{1}",
                _nodeNumbers[node], _nodeNumbers[node.VarNode]);
            _dotBody.AppendLine(s);
        }

        void INodeVisiter.VisitProcedureCall(ProcedureCall node)
        {
            string s;

            s = String.Format("  node{0} [label=\"ProcedureCall:{1}\"]",
                                _ncount, node.ProcedureName);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            foreach (AST paramNode in node.ParamExpressionNodes)
            {
                Visit(paramNode);
                s = String.Format("  node{0} -> node{1}",
                    _nodeNumbers[node], _nodeNumbers[paramNode]);
                _dotBody.AppendLine(s);
            }
        }

        void INodeVisiter.VisitProcedureDecl(ProcedureDecl node)
        {
            string s;

            s = String.Format("  node{0} [label=\"ProcedureDecl:{1}\"]",
                                _ncount, node.ProcedureName);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            foreach (AST paramNode in node.ParamNodes)
            {
                Visit(paramNode);
                s = String.Format("  node{0} -> node{1}",
                    _nodeNumbers[node], _nodeNumbers[paramNode]);
                _dotBody.AppendLine(s);
            }

            Visit(node.CompoundNode);
            s = String.Format("  node{0} -> node{1}",
                _nodeNumbers[node], _nodeNumbers[node.CompoundNode]);
            _dotBody.AppendLine(s);
        }

        void INodeVisiter.VisitProgramAST(ProgramAST node)
        {
            string s;
            s = String.Format("  node{0} [label=\"Program\"]", _ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            Visit(node.Block);
            s = String.Format("  node{0} -> node{1}",
                _nodeNumbers[node], _nodeNumbers[node.Block]);
            _dotBody.AppendLine(s);
        }

        void INodeVisiter.VisitReturn(Return node)
        {
            string s;

            s = String.Format("  node{} [label=\"Return:\"]",_ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            Visit(node.Expression);
            s = String.Format("  node{0} -> node{1}",
                _nodeNumbers[node], _nodeNumbers[node.Expression]);
            _dotBody.AppendLine(s);
        }

        void INodeVisiter.VisitStringData(StringData node)
        {
            string s;
            s = String.Format("  node{0} [label=\"{1}\"]", _ncount, node.Value);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;
        }

        void INodeVisiter.VisitTernary(Ternary node)
        {
            string s;
            s = String.Format("  node{0} [label=\"Ternary\"]", _ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            Visit(node.Condition);
            s = String.Format("  node{0} -> node{1} [label=\"Condition\"]",
                _nodeNumbers[node], _nodeNumbers[node.Condition]);
            _dotBody.AppendLine(s);

            Visit(node.TrueStatement);
            s = String.Format("  node{0} -> node{1} [label=\"True\"]",
                _nodeNumbers[node], _nodeNumbers[node.TrueStatement]);
            _dotBody.AppendLine(s);

            Visit(node.FalseStatement);
            s = String.Format("  node{0} -> node{1} [label=\"False\"]",
                _nodeNumbers[node], _nodeNumbers[node.FalseStatement]);
            _dotBody.AppendLine(s);
        }

        void INodeVisiter.VisitType(Type node)
        {
            string s;
            s = String.Format("  node{0} [label=\"{1}\"]", _ncount, node.Value);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;
        }

        void INodeVisiter.VisitUnaryOp(UnaryOp node)
        {
            string s;
            s = String.Format("  node{0} [label=\"{1}\"]", _ncount, node.Operation.value);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            Visit(node.Expression);
            s = String.Format("  node{0} -> node{1}",
                _nodeNumbers[node], _nodeNumbers[node.Expression]);
            _dotBody.AppendLine(s);

        }

        void INodeVisiter.VisitVar(Var node)
        {
            string s;
            s = String.Format("  node{0} [label=\"{1}\"]", _ncount, node.Value);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;
        }

        void INodeVisiter.VisitVarDecl(VarDecl node)
        {
            string s;
            s = String.Format("  node{0} [label=\"VarDecl\"]", _ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            Visit(node.VarNode);
            s = String.Format("  node{0} -> node{1}",
                _nodeNumbers[node], _nodeNumbers[node.VarNode]);
            _dotBody.AppendLine(s);

        }

        void INodeVisiter.VisitWhile(While node)
        {
            string s;
            s = String.Format("  node{0} [label=\"While\"]", _ncount);
            _dotBody.AppendLine(s);
            _nodeNumbers.Add(node, _ncount);
            _ncount += 1;

            Visit(node.Condition);
            s = String.Format("  node{0} -> node{1} [label=\"Condition\"]",
                _nodeNumbers[node], _nodeNumbers[node.Condition]);
            _dotBody.AppendLine(s);

            Visit(node.Compound);
            s = String.Format("  node{0} -> node{1} [label=\"Compound\"]",
                _nodeNumbers[node], _nodeNumbers[node.Compound]);
            _dotBody.AppendLine(s);

        }

        string GenerateDOT(AST ast)
        {
            Visit(ast);
            string result;

            result = String.Concat(_dotHeader, _dotBody.ToString(), _dotFooter);

            return result;
        }

    }
}
