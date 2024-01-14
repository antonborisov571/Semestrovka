using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.BinOperationNode.CalcNode;

class MinusNode : BinOperationNode
{
    public MinusNode(IExpressionNode left, IExpressionNode right) : base(left, right)
    {
    }

    public override object Eval() =>
        (dynamic)Left.Eval() - (dynamic)Right.Eval();
}
