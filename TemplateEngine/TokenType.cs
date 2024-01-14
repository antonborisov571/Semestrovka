using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine;

enum TokenType
{
    Plus,
    Minus,
    Multiply,
    Divide,

    Less,
    NotLess,
    Bigger,
    NotBigger,
    Equal,
    NotEqual,
    And,
    Or,

    Assign,

    Space,

    Variable,
    String,
    Number,
    Text,

    If,
    Else,
    EndIf,
    For,
    In,
    While,
    EndFor,
    EndWhile,

    Try,
    Catch,
    Finally,
    EndTry,

    Switch,
    Case,
    Default,
    EndSwitch,

    ExprStart,
    ExprEnd,
    IndexStart,
    IndexEnd,
    Accessor,
    LBrac,
    RBrac,
    Comma,

}
