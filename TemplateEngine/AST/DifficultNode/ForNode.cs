using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.DifficultNode;

class ForNode : IExpressionNode
{
    string NewVar { get; }
    IExpressionNode IterationModel { get; }
    IExpressionNode Body { get; }
    public VariableStorage Variables { get; }

    public ForNode(string newVar, IExpressionNode model, IExpressionNode body, VariableStorage variables)
    {
        NewVar = newVar;
        IterationModel = model;
        Body = body;
        Variables = variables;
    }

    public object Eval()
    {
        var model = IterationModel.Eval();
        var sb = new StringBuilder();
        foreach (var item in (dynamic)model)
        {
            Variables.SetVariable(NewVar, item);
            sb.Append(Body.Eval().ToString());
        }
        return sb.ToString();
    }
}
