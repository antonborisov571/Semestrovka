using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.UnaryOperationNode;

abstract class UnaryOperationNode : IExpressionNode
{
    public IExpressionNode Operand { get; }
    public VariableStorage Variables { get; }

    protected UnaryOperationNode(IExpressionNode operand)
    {
        Operand = operand;
    }

    public abstract object Eval();
}
