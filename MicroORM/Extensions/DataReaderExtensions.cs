using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.Extensions;

public static class DataReaderExtensions
{
    public static bool TryGetOrdinal(this NpgsqlDataReader reader, string name, out int ordinal)
    {
        try
        {
            ordinal = reader.GetOrdinal(name);
            return true;
        }
        catch 
        { 
            ordinal = -1;
            return false;
        }
    }
}
