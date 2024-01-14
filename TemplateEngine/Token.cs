using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine;

class Token
{
    public TokenType Type { get; }
    public string Value { get; }
    public int Pos { get; }

    public Token(TokenType type, string value, int pos)
    {
        Type = type;
        Value = value;
        Pos = pos;
    }

    public override string ToString()
    {
        return $"{Type}, \"{Value}\", {Pos}";
    }
}
