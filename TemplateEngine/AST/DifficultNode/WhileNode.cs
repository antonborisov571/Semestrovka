using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.DifficultNode;

class WhileNode : IExpressionNode
{
    IExpressionNode Condition { get; }
    IExpressionNode Body { get; }
    public VariableStorage Variables { get; }

    public WhileNode(IExpressionNode condition, IExpressionNode body, VariableStorage variables)
    {
        Condition = condition;
        Body = body;
        Variables = variables;
    }

    public object Eval()
    {
        var sb = new StringBuilder();
        while ((dynamic)Condition.Eval())
        {
            sb.Append(Body.Eval().ToString());
        }
        return sb.ToString();
    }
}
