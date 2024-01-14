using System.Diagnostics;
using TemplateEngine;
var str = "<li> {{model = \"FDFSfdsfasdfasdf\" }} {{model.Substring(1, 2)}} <li/>";
var tokens = new Lexer(str).Tokenize();
var parse = new Parser(tokens, "Login".ToCharArray()).Parse();
Console.WriteLine(parse.Eval().ToString());
var s = "213".ToList();
/*class Product
{
    public string Str { get; set; }

    public Product(string str)
    {
        Str = str;
    }
    
    public string GetStr()
    {
        return Str;
    }
}*/