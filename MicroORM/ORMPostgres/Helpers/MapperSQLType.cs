using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.ORMPostgres.Helpers;

static class MapperSQLType
{
    public static string MapSQLType(this Type type)
    {
        if (type.IsAssignableFrom(typeof(int)))
            return "integer";
        else if (type.IsAssignableFrom(typeof(long)))
            return "bigint";
        else if (type.IsAssignableFrom(typeof(decimal)))
            return "decimal";
        else if (type.IsAssignableFrom(typeof(float)))
            return "real";
        else if (type.IsAssignableFrom(typeof(double)))
            return "double precision";
        else if (type.IsAssignableFrom(typeof(string)))
            return "varchar";
        else if (type.IsAssignableFrom(typeof(DateTime)))
            return "date";
        else if (type.IsAssignableFrom(typeof(bool)))
            return "boolean";
        return "text";
    }
}
