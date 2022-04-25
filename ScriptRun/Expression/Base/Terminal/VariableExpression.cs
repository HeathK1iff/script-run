using ScriptRun.Expression.Utils;

namespace ScriptRun.Expression.Base.Terminal
{
    public class VariableExpression : ExpressionBase, IGetValue
    {
        GetVariableValue _valueGetHandler;
        public VariableExpression(string name, GetVariableValue valueGetHandler)
        {
            Name = name;
            _valueGetHandler = valueGetHandler;
        }

        public string Name { get;}

        public object GetValue()
        {
            return _valueGetHandler?.Invoke(Name) ?? 0d;
        }
    }

}

