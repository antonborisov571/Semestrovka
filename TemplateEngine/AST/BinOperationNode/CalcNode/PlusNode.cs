using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.BinOperationNode.CalcNode;

class PlusNode : BinOperationNode
{
    public PlusNode(IExpressionNode left, IExpressionNode right) : base(left, right)
    {
    }

    public override object Eval() =>
        (dynamic)Left.Eval() + (dynamic)Right.Eval();
}
