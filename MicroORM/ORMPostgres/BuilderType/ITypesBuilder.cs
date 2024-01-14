using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace ORM.ORMPostgres.BuilderType;

public interface ITypesBuilder
{
    TypeBuilder GetTypeBuilder(string name);
    void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType);
}
