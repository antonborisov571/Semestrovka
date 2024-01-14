using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace ORM.ORMPostgres.QueryProvider;


public class QueryBuilder : ExpressionVisitor
{
    public readonly string _tableName = string.Empty;
    private Expression selectList;
    private Expression whereExpression;

    public QueryBuilder(string tableName)
    {
        _tableName = tableName;
    }

    protected override Expression VisitMethodCall(MethodCallExpression node)
    {
        if (node.Method.IsGenericMethod)
        {
            var genericMethod = node.Method.GetGenericMethodDefinition();
            if (genericMethod == QueryableMethods.Select)
            {
                VisitSelect(node);
            }
            else if (genericMethod == QueryableMethods.Where)
            {
                VisitWhere(node);
            }
        }
        return base.VisitMethodCall(node);
    }

    private void VisitWhere(MethodCallExpression node)
    {
        whereExpression = ((UnaryExpression)node.Arguments[1]).Operand;
    }

    private void VisitSelect(MethodCallExpression node)
    {
        selectList = ((UnaryExpression)node.Arguments[1]).Operand;
    }
    public string Compile(Expression expression)
    {
        Visit(expression);
        var whereVisitor = new WhereVisitor(_tableName);
        whereVisitor.Visit(whereExpression);
        var selectVisitor = new SelectVisitor(_tableName);
        selectVisitor.Visit(selectList);
        var whereClause = whereVisitor.Result;
        var selectListClause = selectVisitor.Result;
        var sql = string.Empty;

        if (string.IsNullOrEmpty(selectListClause))
        {
            sql = $"select * from {_tableName} where {whereClause};";
        }
        else
        {
            sql = $"select {selectListClause} from {_tableName} where {whereClause};";
        }

        return sql;
    }
}

internal class StringExpression : Expression
{
    public string String { get; }
    public StringExpression(string @string, ExpressionType nodeType, Type type)
    {
        String = @string;
        NodeType = nodeType;
        Type = type;
    }
    public override ExpressionType NodeType { get; }
    public override Type Type { get; }
}

internal class WhereVisitor : ExpressionVisitor
{
    private readonly string _tableName = string.Empty;

    public WhereVisitor(string tableName)
    {
        _tableName = tableName;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        var @operator = node.NodeType switch
        {
            ExpressionType.GreaterThan => ">",
            ExpressionType.Equal => "=",
            ExpressionType.OrElse => "or",
            ExpressionType.AndAlso => "and",
            ExpressionType.LessThan => "<",
        };

        var left = ToString(node.Left);
        var right = ToString(node.Right);
        Result = $"{left} {@operator} {right}";
        return base.VisitBinary(node);
    }

    public string Result { get; set; }

    private string ToString(Expression exp)
    {
        if (exp is ConstantExpression cs)
            return cs.Value.ToString();

        return $"{_tableName}.{((MemberExpression)exp).Member.Name}";
    }
}

internal class SelectVisitor : ExpressionVisitor
{
    private readonly string _tableName = string.Empty;

    public SelectVisitor(string tableName)
    {
        _tableName = tableName;
    }
    protected override Expression VisitMemberInit(MemberInitExpression node)
    {
        var nodes = node.Bindings.Cast<MemberAssignment>().Select(x => ToString(x.Expression));
        Result = string.Join(", ", nodes);
        return base.VisitMemberInit(node);
    }

    public string Result { get; set; }

    private string ToString(Expression exp)
    {
        if (exp is ConstantExpression cs)
            return cs.Value.ToString();

        return $"{_tableName}.{((MemberExpression)exp).Member.Name}";
    }
}