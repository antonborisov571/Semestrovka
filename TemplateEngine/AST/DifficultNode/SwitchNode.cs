using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.DifficultNode;

class SwitchNode : IExpressionNode
{
    IExpressionNode Model { get; }
    Dictionary<IExpressionNode, IExpressionNode> Cases { get; }
    IExpressionNode Default { get; }
    public VariableStorage Variables { get; }

    public SwitchNode(IExpressionNode model, Dictionary<IExpressionNode, IExpressionNode> cases, IExpressionNode @default)
    {
        Model = model;
        Cases = cases;
        Default = @default;
    }

    public object Eval()
    {
        var model = Model.Eval();
        foreach (var (key, value) in Cases)
        {
            if ((dynamic)model.Equals((dynamic)key.Eval()))
            {
                return value.Eval();
            }
        }
        return Default.Eval();
    }
}
