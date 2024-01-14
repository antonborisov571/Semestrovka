using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEngine.AST;

class VariableStorage
{
    public VariableStorage? Child { get; }
    public Dictionary<string, object> Variables = new Dictionary<string, object>();

    public VariableStorage(VariableStorage? child)
    {
        Child = child;
    }

    public void SetVariable(string name, object value)
    {
        var variableStorage = GetVariableStorage(name);
        if (variableStorage != null)
        {
            variableStorage.Variables[name] = value;
        }
        else
        {
            Variables[name] = value;
        }
    }

    public VariableStorage? GetVariableStorage(string name)
    {
        var root = this;
        while (root != null)
        {
            if (root.Variables.ContainsKey(name))
            {
                return root;
            }

            root = root.Child;
        }
        return null;
    }

    public object? GetVariable(string name)
    {
        var variableStorage = GetVariableStorage(name);
        if (variableStorage != null)
            return variableStorage.Variables[name];

        throw new Exception($"Переменная {name} не существует или не доступна");
    }
}
