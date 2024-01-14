using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST.ModelOperationNode;

class MethodNode : IExpressionNode
{

    IExpressionNode Model { get; }
    string Name { get; }
    List<IExpressionNode> Parameters { get; }
    public VariableStorage Variables { get; }

    public MethodNode(IExpressionNode model, string name, List<IExpressionNode> parameters = default!)
    {
        Model = model;
        Parameters = parameters;
        Name = name;
    }

    public dynamic Eval()
    {
        var model = Model.Eval();
        var methods = model.GetType().GetMethods();
        var parameters = new object[Parameters.Count];
        dynamic invoke = string.Empty;
        var isFound = false;
        var countMethodsException = 0;
        foreach (var method in methods)
        {
            try
            {
                if (method != null
                    && method.GetParameters().Length == Parameters.Count
                    && method.Name == Name)
                {
                    for (var i = 0; i < Parameters.Count; i++)
                    {
                        parameters[i] = Convert.ChangeType(Parameters[i].Eval(), method.GetParameters()[i].ParameterType);
                    }
                    invoke = method.Invoke(model, parameters)!;
                    isFound = true;
                }
                else
                {
                    throw new Exception($"Метода {Name} не существует или не совпадает количество параметров");
                }
            }
            catch (Exception ex)
            {
                countMethodsException++;
                if (isFound && countMethodsException == methods.Length)
                {
                    throw new Exception($"Метод {Name} найден, но возможно параметры переданы неправильно");
                }
                else if (countMethodsException == methods.Length)
                {
                    throw new Exception(ex.Message, ex);
                }
            }
        }
        return invoke;
    }
}
