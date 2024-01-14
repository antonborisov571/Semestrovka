using ORM.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ORM.ORMPostgres.QueryProvider;

internal class MyORMQueryProvider 
{
    public string Execute(Expression expression, string tableName)
    {
        var builder = new QueryBuilder(tableName);
        var sql = builder.Compile(expression);
        return sql;
    }
}
