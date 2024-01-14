using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.BinOperationNode;

abstract class BinOperationNode : IExpressionNode
{
    public IExpressionNode Left { get; }
    public IExpressionNode Right { get; }
    public VariableStorage Variables { get; }
    public BinOperationNode(IExpressionNode left, IExpressionNode right)
    {
        Left = left;
        Right = right;
    }

    public abstract object Eval();
}
