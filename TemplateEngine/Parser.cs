using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateEngine.AST;
using TemplateEngine.AST.DifficultNode;
using TemplateEngine.AST.ModelOperationNode;
using TemplateEngine.AST.BinOperationNode.BoolNode;
using TemplateEngine.AST.BinOperationNode.CalcNode;
using TemplateEngine.AST.UnaryOperationNode;
using System.Globalization;
using System.Reflection;

namespace TemplateEngine;

class Parser
{
    ModelNode Model { get; }
    int Pos { get; set; } = 0;
    List<Token> Tokens { get; }
    Stack<TokenType> ForStack { get; } = new Stack<TokenType>();
    Stack<TokenType> WhileStack { get; } = new Stack<TokenType>();
    Stack<TokenType> IfStack { get; } = new Stack<TokenType>();
    Stack<TokenType> TryStack { get; } = new Stack<TokenType>();
    Stack<TokenType> SwitchStack { get; } = new Stack<TokenType>();

    public Parser(List<Token> tokens, object model = default!)
    {
        Tokens = tokens;
        Model = new ModelNode(model);
    }

    public IExpressionNode Parse(VariableStorage storage = default!)
    {
        var newStorage = new VariableStorage(storage);
        var complexNode = new ComplexNode(storage);
        while (Pos < Tokens.Count)
        {
            if (MatchAndNext(TokenType.ExprStart))
            {
                if (Match(TokenType.EndFor))
                {
                    if (ForStack.Count == 0)
                    {
                        throw new Exception($"На позиции {Tokens[Pos].Pos} получена ключевое слово \"endfor\", но \"for\" нет");
                    }
                    return complexNode;
                }
                else if (Match(TokenType.EndWhile))
                {
                    if (WhileStack.Count == 0)
                    {
                        throw new Exception($"На позиции {Tokens[Pos].Pos} получена ключевое слово \"endwhile\", но \"while\" нет");
                    }
                    return complexNode;
                }
                else if (Match(TokenType.EndIf))
                {
                    if (IfStack.Count == 0)
                    {
                        throw new Exception($"На позиции {Tokens[Pos].Pos} получена ключевое слово \"endif\", но \"if\" нет");
                    }
                    return complexNode;
                }
                else if (Match(TokenType.EndTry))
                {
                    if (TryStack.Count == 0)
                    {
                        throw new Exception($"На позиции {Tokens[Pos].Pos} получена ключевое слово \"endtry\", но \"try\" нет");
                    }
                    return complexNode;
                }
                else if (Match(TokenType.Catch))
                {
                    if (TryStack.Count == 0)
                    {
                        throw new Exception($"На позиции {Tokens[Pos].Pos} получена ключевое слово \"catch\", но \"try\" нет");
                    }
                    return complexNode;
                }
                else if (Match(TokenType.Finally))
                {
                    if (TryStack.Count == 0)
                    {
                        throw new Exception($"На позиции {Tokens[Pos].Pos} получена ключевое слово \"finally\", но \"try\" нет");
                    }
                    return complexNode;
                }
                else if (Match(TokenType.Case))
                {
                    if (SwitchStack.Count == 0)
                    {
                        throw new Exception($"На позиции {Tokens[Pos].Pos} получена ключевое слово \"case\", но \"switch\" нет");
                    }
                    return complexNode;
                }
                else if (Match(TokenType.Default))
                {
                    if (SwitchStack.Count == 0)
                    {
                        throw new Exception($"На позиции {Tokens[Pos].Pos} получена ключевое слово \"default\", но \"switch\" нет");
                    }
                    return complexNode;
                }
                else if (Match(TokenType.EndSwitch))
                {
                    if (SwitchStack.Count == 0)
                    {
                        throw new Exception($"На позиции {Tokens[Pos].Pos} получена ключевое слово \"endswitch\", но \"switch\" нет");
                    }
                    return complexNode;
                }
                else if (Match(TokenType.Else))
                {
                    if (IfStack.Count == 0)
                    {
                        throw new Exception($"На позиции {Tokens[Pos].Pos} получено \"else\", но нет \"if\"");
                    }
                    return complexNode;
                }

                complexNode.AddNode(ParseExpression(newStorage));
                Require(TokenType.ExprEnd);
            }
            complexNode.AddNode(new TextNode(Tokens[Pos]));
            Pos++;
        }
        return complexNode;
    }

    IExpressionNode ParseIf(VariableStorage storage)
    {
        IfStack.Push(TokenType.If);
        var condition = ParseFormula(storage);
        Require(TokenType.ExprEnd);
        var newStorage = new VariableStorage(storage);
        var then = Parse(newStorage);
        Require(TokenType.Else);
        Require(TokenType.ExprEnd);
        var @else = Parse(newStorage);
        Require(TokenType.EndIf);
        IfStack.Pop();
        return new IfNode(condition, then, @else);
    }

    IExpressionNode ParseFor(VariableStorage storage)
    {
        ForStack.Push(TokenType.For);
        var newVar = Require(TokenType.Variable).Value;
        Require(TokenType.In);
        var newStorage = new VariableStorage(storage);
        var iterationModel = ParseVariable(newStorage);
        Require(TokenType.ExprEnd);
        var body = Parse(newStorage);
        Require(TokenType.EndFor);
        ForStack.Pop();
        return new ForNode(newVar, iterationModel, body, newStorage);
    }

    IExpressionNode ParseWhile(VariableStorage storage)
    {
        WhileStack.Push(TokenType.While);
        var condition = ParseFormula(storage);
        Require(TokenType.ExprEnd);
        var newStorage = new VariableStorage(storage);
        var body = Parse(newStorage);
        Require(TokenType.EndWhile);
        WhileStack.Pop();
        return new WhileNode(condition, body, newStorage);
    }

    IExpressionNode ParseTry(VariableStorage storage)
    {
        TryStack.Push(TokenType.Try);
        Require(TokenType.ExprEnd);
        var newStorage = new VariableStorage(storage);
        var @try = Parse(newStorage);
        Require(TokenType.Catch);
        Require(TokenType.ExprEnd);
        var @catch = Parse(newStorage);
        Require(TokenType.Finally);
        Require(TokenType.ExprEnd);
        var @finally = Parse(newStorage);
        Require(TokenType.EndTry);
        TryStack.Pop();
        return new TryNode(@try, @catch, @finally);
    }


    IExpressionNode ParseSwitch(VariableStorage storage)
    {
        SwitchStack.Push(TokenType.Switch);
        var model = ParseFormula(storage);
        var cases = new Dictionary<IExpressionNode, IExpressionNode>();
        Require(TokenType.ExprEnd);
        var newStorage = new VariableStorage(storage);
        Require(TokenType.Text);
        Require(TokenType.ExprStart);
        do
        {
            Require(TokenType.Case);
            var checkCase = ParseFormula(storage);
            Require(TokenType.ExprEnd);
            var @case = Parse(newStorage);
            cases.Add(checkCase, @case);

        }
        while (Match(TokenType.Case));
        Require(TokenType.Default);
        Require(TokenType.ExprEnd);
        var @default = Parse(newStorage);
        Require(TokenType.EndSwitch);
        SwitchStack.Pop();
        return new SwitchNode(model, cases, @default);
    }

    IExpressionNode ParseExpression(VariableStorage storage)
    {
        if (Match(TokenType.Variable) && Tokens[Pos + 1].Type == TokenType.Assign)
        {
            return ParseAssignment(storage);
        }

        if (Match(TokenType.String, TokenType.Variable, TokenType.Number, TokenType.LBrac))
        {
            return ParseFormula(storage);
        }

        if (MatchAndNext(TokenType.For))
        {
            return ParseFor(storage);
        }

        if (MatchAndNext(TokenType.If))
        {
            return ParseIf(storage);
        }

        if (MatchAndNext(TokenType.While))
        {
            return ParseWhile(storage);
        }

        if (MatchAndNext(TokenType.Try))
        {
            return ParseTry(storage);
        }

        if (MatchAndNext(TokenType.Switch))
        {
            return ParseSwitch(storage);
        }

        throw new Exception($"Неизвестный оператор: позиция {Tokens[Pos].Pos}, " +
            $"значение {Tokens[Pos].Value}, тип {Tokens[Pos].Type}");
    }

    IExpressionNode ParseFormula(VariableStorage storage)
    {
        var leftOperand = ParseOr(storage);

        return leftOperand;
    }

    IExpressionNode ParseOr(VariableStorage storage)
    {
        var result = ParseAnd(storage);
        while (true)
        {
            if (MatchAndNext(TokenType.Or))
            {
                result = new OrNode(result, ParseAnd(storage));
                continue;
            }
            break;
        }
        return result;
    }

    IExpressionNode ParseAnd(VariableStorage storage)
    {
        var result = ParseEqual(storage);
        while (true)
        {
            if (MatchAndNext(TokenType.And))
            {
                result = new AndNode(result, ParseEqual(storage));
                continue;
            }
            break;
        }
        return result;
    }

    IExpressionNode ParseEqual(VariableStorage storage)
    {
        var result = ParseComparers(storage);
        while (true)
        {
            if (MatchAndNext(TokenType.Equal))
            {
                result = new EqualNode(result, ParseComparers(storage));
                continue;
            }
            if (MatchAndNext(TokenType.NotEqual))
            {
                result = new NotEqualNode(result, ParseComparers(storage));
                continue;
            }
            break;
        }
        return result;
    }

    IExpressionNode ParseComparers(VariableStorage storage)
    {
        var result = ParsePlusAndMinus(storage);
        while (true)
        {
            if (MatchAndNext(TokenType.Bigger))
            {
                result = new BiggerNode(result, ParsePlusAndMinus(storage));
                continue;
            }
            if (MatchAndNext(TokenType.NotBigger))
            {
                result = new NotBiggerNode(result, ParsePlusAndMinus(storage));
                continue;
            }
            if (MatchAndNext(TokenType.Less))
            {
                result = new LessNode(result, ParsePlusAndMinus(storage));
                continue;
            }
            if (MatchAndNext(TokenType.NotLess))
            {
                result = new NotLessNode(result, ParsePlusAndMinus(storage));
                continue;
            }
            break;
        }
        return result;
    }

    IExpressionNode ParsePlusAndMinus(VariableStorage storage)
    {
        var result = ParseMultiplyAndDivide(storage);
        while (true)
        {
            if (MatchAndNext(TokenType.Plus))
            {
                result = new PlusNode(result, ParseMultiplyAndDivide(storage));
                continue;
            }
            if (MatchAndNext(TokenType.Minus))
            {
                result = new MinusNode(result, ParseMultiplyAndDivide(storage));
                continue;
            }
            break;
        }
        return result;
    }

    IExpressionNode ParseMultiplyAndDivide(VariableStorage storage)
    {
        var result = ParseUnary(storage);
        while (true)
        {
            if (MatchAndNext(TokenType.Multiply))
            {
                result = new MultiplyNode(result, ParseUnary(storage));
                continue;
            }
            if (MatchAndNext(TokenType.Divide))
            {
                result = new DivideNode(result, ParseUnary(storage));
                continue;
            }
            break;
        }
        return result;
    }

    IExpressionNode ParseUnary(VariableStorage storage)
    {
        if (MatchAndNext(TokenType.Minus))
        {
            return new InvertNode(ParseBrackets(storage));
        }
        return ParseBrackets(storage);
    }

    IExpressionNode ParseBrackets(VariableStorage storage)
    {
        if (MatchAndNext(TokenType.LBrac))
        {
            var formula = ParseFormula(storage);
            Require(TokenType.RBrac);
            return formula;
        }
        else if (Match(TokenType.Variable))
        {
            return ParseVariable(storage);
        }
        else if (Match(TokenType.Number))
        {
            return ParseNumber(storage);
        }
        else if (Match(TokenType.String))
        {
            return ParseString(storage);
        }
        throw new Exception($"Что-то неправильно");
    }

    IExpressionNode ParseString(VariableStorage storage)
    {
        var str = Require(TokenType.String);
        var model = new ModelNode(str.Value);
        
        if (MatchAndNext(TokenType.Accessor))
        {
            return ParseAccessor(storage, model);
        }
        if (MatchAndNext(TokenType.IndexStart))
        {
            return ParseIndexer(storage, model);
        }

        return model;
    }

    IExpressionNode ParseNumber(VariableStorage storage)
    {
        if (double.TryParse(
            Require(TokenType.Number).Value, 
            NumberStyles.AllowDecimalPoint, 
            CultureInfo.InvariantCulture, 
            out double number))
        {
            return new ModelNode(number);
        }
        throw new InvalidCastException($"Неправильное число на позиции {Tokens[Pos - 1].Pos}: {Tokens[Pos - 1].Value}");
    }

    IExpressionNode ParseVariable(VariableStorage storage)
    {
        var model = new ModelNode(Require(TokenType.Variable).Value, storage);
        ModelNode result;

        if (model.Name == "Model")
        {
            result = Model;
        }
        else
        {
            result = new ModelNode(model.Name, storage);
        }

        if (MatchAndNext(TokenType.IndexStart))
        {
            return ParseIndexer(storage, result);
        }
        else if (MatchAndNext(TokenType.Accessor))
        {
            if (Match(TokenType.Variable) && Tokens[Pos + 1].Type == TokenType.LBrac)
            {
                return ParseMethod(storage, result, Require(TokenType.Variable).Value);
            }
            else if (Match(TokenType.Variable))
            {
                return ParseAccessor(storage, result);
            }
        }

        return result;
    }

    IExpressionNode ParseIndexer(VariableStorage storage, IExpressionNode model)
    {
        var indexer = model;
        do
        {
            IExpressionNode index;
            if (Match(TokenType.Variable, TokenType.Number))
            {
                index = ParseFormula(storage);
            }
            else
            {
                throw new Exception($"Неправильная индексация на позиции {Tokens[Pos].Pos}: {Tokens[Pos].Value}");
            }
            indexer = new IndexerNode(indexer, index);
            Require(TokenType.IndexEnd);
        }
        while (MatchAndNext(TokenType.IndexStart));

        if (MatchAndNext(TokenType.Accessor))
        {
            if (Match(TokenType.Variable) && Tokens[Pos + 1].Type == TokenType.LBrac)
            {
                return ParseMethod(storage, indexer, Require(TokenType.Variable).Value);
            }
            else if (Match(TokenType.Variable))
            {
                return ParseAccessor(storage, indexer);
            }
        }
        return indexer;
    }

    IExpressionNode ParseAccessor(VariableStorage storage, IExpressionNode model)
    {
        var property = model;
        do
        {
            property = new AccessorNode(property, Require(TokenType.Variable).Value);
        }
        while (MatchAndNext(TokenType.Accessor) && Tokens[Pos + 1].Type != TokenType.LBrac);
        
        if (MatchAndNext(TokenType.IndexStart))
        {
            return ParseIndexer(storage, property);
        }
        else if (Match(TokenType.Variable))
        {
            return ParseMethod(storage, property, Require(TokenType.Variable).Value);
        }
        return property;
    }

    IExpressionNode ParseMethod(VariableStorage storage, IExpressionNode modelSome, string name)
    {
        Require(TokenType.LBrac);
        var model = modelSome;
        var parameters = new List<IExpressionNode>();
        parameters = ParseParameters(storage);
        Require(TokenType.RBrac);

        model = new MethodNode(model, name, parameters);

        if (MatchAndNext(TokenType.IndexStart))
        {
            return ParseIndexer(storage, model);
        }
        else if (MatchAndNext(TokenType.Accessor))
        {
            if (Match(TokenType.Variable) && Tokens[Pos + 1].Type == TokenType.LBrac) 
            {
                return ParseMethod(storage, model, Require(TokenType.Variable).Value);
            }
            else if (Match(TokenType.Variable))  
            {
                return ParseAccessor(storage, model);
            }
        }
        return model;
    }

    List<IExpressionNode> ParseParameters(VariableStorage storage)
    {
        var parameters = new List<IExpressionNode>();
        while (Match(TokenType.Variable, TokenType.String, TokenType.Number))
        {
            if (Match(TokenType.Variable))
            {
                parameters.Add(
                new ModelNode(Require(TokenType.Variable).Value,
                storage));
            }
            else if (Match(TokenType.Number))
            {
                parameters.Add(ParseNumber(storage));
            }
            else if (Match(TokenType.String))
            {
                parameters.Add(ParseString(storage));
            }

            if (MatchAndNext(TokenType.Comma))
            {
                continue;
            }
            break;
        }
        return parameters;
    }

    IExpressionNode ParseAssignment(VariableStorage storage)
    {
        var variable = Require(TokenType.Variable);
        if (variable.Value == "Model")
        {
            throw new Exception($"Исходную модель нельзя изменять. Позиция: {Tokens[Pos - 1].Pos}");
        }
        Require(TokenType.Assign);
        var right = ParseFormula(storage);
        return new AssignNode(right, variable.Value, storage);
    }

    Token GetNext() => Tokens[Pos++];

    bool Match(params TokenType[] tokens) => tokens.Contains(Tokens[Pos].Type);

    bool MatchAndNext(params TokenType[] tokens)
    {
        if (!Match(tokens)) 
            return false;
        Pos++;
        return true;
    }

    Token Require(params TokenType[] tokens) =>
        Match(tokens)
        ? GetNext()
        : throw new Exception($"На позиции {Tokens[Pos].Pos} ожидалось нечто другое:" +
            $"\"{string.Join("\" \"", tokens)}\", а не \"{Tokens[Pos].Value}\"");
}
