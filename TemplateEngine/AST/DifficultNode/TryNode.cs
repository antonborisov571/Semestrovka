using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.DifficultNode;

class TryNode : IExpressionNode
{
    IExpressionNode Try { get; }
    IExpressionNode Catch { get; }
    IExpressionNode Finally { get; }
    public VariableStorage Variables { get; }

    public TryNode(IExpressionNode @try, IExpressionNode @catch, IExpressionNode @finally)
    {
        Try = @try;
        Catch = @catch;
        Finally = @finally;
    }

    public object Eval()
    {
        var sb = new StringBuilder();
        try
        {
            sb.Append(Try.Eval().ToString());
        }
        catch
        {
            return Catch.Eval();
        }
        finally
        {
            sb.Append(Finally.Eval().ToString());
        }
        return sb.ToString();
    }
}
