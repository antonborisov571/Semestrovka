using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST;

class TextNode : IExpressionNode
{
    public Token Token { get; set; }
    public VariableStorage Variables { get; }

    public TextNode(Token token)
    {
        Token = token;
    }

    public object Eval() => Token.Value;
}
