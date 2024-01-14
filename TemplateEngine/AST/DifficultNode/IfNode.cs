using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.DifficultNode;

class IfNode : IExpressionNode
{
    IExpressionNode Condition { get; }
    IExpressionNode Then { get; }
    IExpressionNode Else { get; }
    public VariableStorage Variables { get; }

    public IfNode(IExpressionNode condition, IExpressionNode then, IExpressionNode @else)
    {
        Condition = condition;
        Then = then;
        Else = @else;
    }

    public object Eval()
    {
        if ((dynamic)Condition.Eval())
        {
            return Then.Eval();
        }
        else
        {
            return Else.Eval();
        }
    }
}
