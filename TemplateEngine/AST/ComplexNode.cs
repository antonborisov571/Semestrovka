using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST;

class ComplexNode : IExpressionNode
{
    List<IExpressionNode> Nodes { get; }
    public VariableStorage Variables { get; }

    public ComplexNode(VariableStorage variables)
    {
        Nodes = new List<IExpressionNode>();
        Variables = variables;
    }

    public void AddNode(IExpressionNode node)
    {
        Nodes.Add(node);
    }

    public object Eval()
    {
        var sb = new StringBuilder();
        foreach (var node in Nodes)
        {
            sb.Append(node.Eval());
        }
        return sb.ToString();
    }
}
