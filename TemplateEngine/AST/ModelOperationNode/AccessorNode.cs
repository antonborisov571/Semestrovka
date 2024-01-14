using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.ModelOperationNode;

class AccessorNode : IExpressionNode
{
    IExpressionNode Model { get; }
    string Name { get; }
    public VariableStorage Variables { get; }

    public AccessorNode(IExpressionNode model, string name)
    {
        Model = model;
        Name = name;
    }

    public dynamic Eval()
    {
        var model = Model.Eval();
        var property = model.GetType().GetProperty(Name);
        if (property == null)
        {
            throw new Exception($"У переменной {model.GetType().Name} не сущесвует свойства {Name}");
        }

        var value = property.GetValue(model);
        return (dynamic)value;
    }
}
