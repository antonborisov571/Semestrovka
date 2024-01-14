using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.ModelOperationNode;

class ModelNode : IExpressionNode
{
    object Model { get; set; }
    public string Name { get; set; }
    public VariableStorage Variables { get; }

    public ModelNode(string name, VariableStorage variables)
    {
        Name = name;
        Variables = variables;
    }

    public ModelNode(object model)
    {
        Model = model;
    }

    public object Eval() =>
        Variables == null
        ? Model
        : Variables.GetVariable(Name)!;
}
