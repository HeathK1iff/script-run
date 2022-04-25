using ScriptRun.Expression.Utils;

namespace ScriptRun.Expression.Base.Terminal
{
    public class NumberValueExpression : ExpressionBase, IGetValue
    {
        double _value;
        public NumberValueExpression(double value)
        {
            _value = value;
        }

        public object GetValue()
        {
            return _value;
        }
    }

}

