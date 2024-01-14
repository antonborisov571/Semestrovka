using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORM.ORMPostgres;

[AttributeUsage(AttributeTargets.Class)]
public class Table : Attribute
{
    public string Name { get; set; }

    public Table(string name)
    {
        Name = name;
    }
}

[AttributeUsage(AttributeTargets.Property)]
public class Column : Attribute
{
    public string Name { get; set; }

    public Column(string name)
    {
        Name = name;
    }
}
