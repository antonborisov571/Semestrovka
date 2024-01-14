using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace ORM.ORMPostgres.BuilderType;

internal class TypesBuilder : ITypesBuilder
{

    public void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
    {
        propertyName = propertyName.ToLower();
        FieldBuilder fieldBuilder = tb.DefineField(propertyName, propertyType, FieldAttributes.Private);

        PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
        MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, propertyType, Type.EmptyTypes);
        ILGenerator getIl = getPropMthdBldr.GetILGenerator();

        getIl.Emit(OpCodes.Ldarg_0);
        getIl.Emit(OpCodes.Ldfld, fieldBuilder);
        getIl.Emit(OpCodes.Ret);

        MethodBuilder setPropMthdBldr =
            tb.DefineMethod("set_" + propertyName,
              MethodAttributes.Public |
              MethodAttributes.SpecialName |
              MethodAttributes.HideBySig,
              null, new[] { propertyType });

        ILGenerator setIl = setPropMthdBldr.GetILGenerator();
        Label modifyProperty = setIl.DefineLabel();
        Label exitSet = setIl.DefineLabel();

        setIl.MarkLabel(modifyProperty);
        setIl.Emit(OpCodes.Ldarg_0);
        setIl.Emit(OpCodes.Ldarg_1);
        setIl.Emit(OpCodes.Stfld, fieldBuilder);

        setIl.Emit(OpCodes.Nop);
        setIl.MarkLabel(exitSet);
        setIl.Emit(OpCodes.Ret);

        propertyBuilder.SetGetMethod(getPropMthdBldr);
        propertyBuilder.SetSetMethod(setPropMthdBldr);
    }

    public TypeBuilder GetTypeBuilder(string name)
    {
        var an = new AssemblyName(name.ToLower());
        var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
        TypeBuilder tb = moduleBuilder.DefineType(name,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                null);
        return tb;
    }
}
