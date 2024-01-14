using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST;

class AssignNode : IExpressionNode
{
    public IExpressionNode Model { get; set; }
    public string Name { get; set; }
    public VariableStorage Variables { get; }

    public AssignNode(IExpressionNode model, string name, VariableStorage variables)
    {
        Model = model;
        Name = name;
        Variables = variables;
    }

    public object Eval()
    { 
        Variables?.SetVariable(Name, Model.Eval());
        return null!;
    }
}
