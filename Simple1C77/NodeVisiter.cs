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
            string methodName = "Visit" + this.GetType().Name;

            //Type type = typeof(this);
            System.Type type = this.GetType();
            MethodInfo methodInfo = type.GetMethod(methodName);
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
