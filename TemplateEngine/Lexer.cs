using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine;

class Lexer
{
    private int Pos { get; set; } = 0;
    private string Input { get; }
    public List<Token> Tokens { get; set; } = new List<Token>();
    private Dictionary<string, TokenType> TokensType = new Dictionary<string, TokenType>() 
    {
        ["endwhile"] = TokenType.EndWhile,
        ["while"] = TokenType.While,
        ["endfor"] = TokenType.EndFor,
        ["for"] = TokenType.For,
        ["in"] = TokenType.In,
        ["if"] = TokenType.If,
        ["endif"] = TokenType.EndIf,
        ["else"] = TokenType.Else,
        ["try"] = TokenType.Try,
        ["catch"] = TokenType.Catch,
        ["finally"] = TokenType.Finally,
        ["endtry"] = TokenType.EndTry,
        ["switch"] = TokenType.Switch,
        ["case"] = TokenType.Case,
        ["default"] = TokenType.Default,
        ["endswitch"] = TokenType.EndSwitch,

        ["=="] = TokenType.Equal,
        ["!="] = TokenType.NotEqual,
        [">="] = TokenType.NotLess,
        ["<="] = TokenType.NotBigger,
        [">"] = TokenType.Bigger,
        ["<"] = TokenType.Less,
        ["&&"] = TokenType.And,
        ["||"] = TokenType.Or,

        ["="] = TokenType.Assign,

        ["."] = TokenType.Accessor,
        [","] = TokenType.Comma,
        ["["] = TokenType.IndexStart,
        ["]"] = TokenType.IndexEnd,
        ["("] = TokenType.LBrac,
        [")"] = TokenType.RBrac,

        ["+"] = TokenType.Plus,
        ["-"] = TokenType.Minus,
        ["*"] = TokenType.Multiply,
        ["/"] = TokenType.Divide,

        [" "] = TokenType.Space,
        ["\n"] = TokenType.Space,
        ["\r"] = TokenType.Space,
        ["\t"] = TokenType.Space,
    };

    public Lexer(string input)
    {
        Input = input;
    }

    public List<Token> Tokenize()
    {
        while (!IsEnd())
        {
            if (CheckNext("{{"))
                Tokens.AddRange(TokenizeExpr());

            Tokens.Add(TokenizeText());
        }
        Tokens = Tokens.Where(x => x.Type != TokenType.Space).ToList();
        return Tokens;
    }

    IEnumerable<Token> TokenizeExpr()
    {
        var tokens = new List<Token> { new Token(TokenType.ExprStart, "{{", Pos) };
        Pos += 2;
        while (!IsEnd() && !CheckNext("}}")) 
        {
            var isTokenFound = false;
            foreach (var (key, value) in TokensType)
            {
                if (CheckNext(key))
                {
                    tokens.Add(new Token(value, key, Pos));
                    Pos += key.Length;
                    isTokenFound = true;
                }
            }
            if (!isTokenFound)
            {
                if (char.IsLetter(Get(0)))
                {
                    tokens.Add(TokenizeVariable());
                }
                else if (char.IsDigit(Get(0)))
                {
                    tokens.Add(TokenizeNumber());
                }
                else if (CheckNext("\""))
                {
                    tokens.Add(TokenizeString());
                }
                else
                {
                    throw new Exception($"Неверный символ на позиции {Pos}, \"{Input[Pos]}\"");
                }
            }
        }

        if (IsEnd() && !CheckNext("}}"))
        {
            throw new Exception($"На позиции {Pos} ожидались закрывающие скобки");
        }

        tokens.Add(new Token(TokenType.ExprEnd, "}}", Pos));
        Pos += 2;
        return tokens;
    }

    Token TokenizeString()
    {
        Pos++;
        var startPos = Pos;
        var str = new StringBuilder();
        while (!IsEnd() && !CheckNext("\""))
        {
            if (CheckNext("\\"))
            {
                Pos++;
                str.Append(Get(0) switch
                {
                    '\n' => 'n',
                    '\t' => 't',
                    '\r' => 'r',
                    '\\' => '\\',
                    '\"' => '\"',
                    _ => string.Empty
                });
                Pos++;
            }
            str.Append(Get(0));
            Pos++;
        }

        if (IsEnd() && !CheckNext("\""))
        {
            throw new Exception($"На позиции {Pos} ожидалась закрывающая ковычка");
        }

        Pos++;
        return new Token(TokenType.String, str.ToString(), startPos);
    }

    Token TokenizeNumber()
    {
        var startPos = Pos;
        var number = new StringBuilder();
        var isDot = false;
        while (char.IsDigit(Get(0)) || Get(0) == '.')
        {
            if (Get(0) == '.' && !isDot) 
            {
                number.Append(Get(0));
                Pos++;
                isDot = true;
            }
            else if (char.IsDigit(Get(0)))
            {
                number.Append(Get(0));
                Pos++;
            }
            else
            {
                throw new Exception($"На позиции {Pos} лишняя точка в числе");
            }
        }   

        if (number[^1] == '.')
        {
            throw new Exception($"На позиции {Pos} не должна быть точка, продолжите число");
        }

        return new Token(TokenType.Number, number.ToString(), startPos);
    }

    Token TokenizeVariable()
    {
        var startPos = Pos;
        var variable = new StringBuilder();
        while (char.IsLetterOrDigit(Get(0)) || Get(0) == '_') 
        {
            variable.Append(Get(0));
            Pos++;
        }
        
        return new Token(TokenType.Variable, variable.ToString(), startPos);
    }

    Token TokenizeText()
    {
        var startPos = Pos;
        var text = new StringBuilder();
        while (!IsEnd() && !CheckNext("{{"))
        {
            text.Append(Get(0));
            Pos++;
        }
        return new Token(TokenType.Text, text.ToString(), startPos);
    }

    bool IsEnd() => Get(0) == '\0';

    char Get(int index) 
    {
        var position = Pos + index;
        if (position >= Input.Length) 
            return '\0';

        return Input[position];
    }

    bool CheckNext(string str)
    {
        for (var i = 0; i < str.Length; i++)
            if (str[i] != Get(i))
                return false;

        return true;
    }
}
