using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST;

interface IExpressionNode
{
    VariableStorage Variables { get; }
    object Eval();
}
