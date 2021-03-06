using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Simple1C77
{
    public class NodeVisiter
    {
        public NodeVisiter Visit(AST node)
        {
            string methodName = "Visit" + node.GetType().Name;
            //{Void Simple1C77.INodeVisiter.VisitProgramAST(Simple1C77.ProgramAST)}
            //Type type = typeof(this);
            
            System.Type type = this.GetType();
            MethodInfo methodInfo = type.GetInterface("INodeVisiter").GetMethod(methodName);
            if (methodInfo == null)
            {
                genericVisit(node);
                return this;
            }
            else
                return (NodeVisiter) methodInfo.Invoke(this , new object[] { node });

        }

        public void genericVisit(AST node)
        {
            throw new ArgumentException(String.Format("No visit_{0} method", this.GetType().Name), "node");
        }

    }
}
