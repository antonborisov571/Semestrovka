using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.ModelOperationNode;

class IndexerNode : IExpressionNode
{
    IExpressionNode Model { get; }
    IExpressionNode Index { get; }
    public VariableStorage Variables { get; }

    public IndexerNode(IExpressionNode model, IExpressionNode index)
    {
        Model = model;
        Index = index;
    }

    public object Eval()
    {
        var model = Model.Eval();
        var index = Index.Eval();
        foreach (var pi in model.GetType().GetProperties())
        {
            if (pi.GetIndexParameters().Length > 0)
            {
                return pi.GetValue(model, new object[] { Convert.ChangeType(index, pi.GetIndexParameters()[0].ParameterType) })!;
            }
        }
        throw new Exception($"У переменной {model} нет индексатора");
    }
}
