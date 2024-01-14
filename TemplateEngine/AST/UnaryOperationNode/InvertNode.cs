using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.UnaryOperationNode;

class InvertNode : UnaryOperationNode
{
    public InvertNode(IExpressionNode operand) : base(operand)
    {
    }

    public override object Eval() =>
        -(dynamic)Operand.Eval();
}
