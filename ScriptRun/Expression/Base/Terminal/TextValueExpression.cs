using ScriptRun.Expression.Utils;

namespace ScriptRun.Expression.Base.Terminal
{
    public class TextValueExpression : ExpressionBase, IGetValue
    {
        string _value;
        public TextValueExpression(string value)
        {
            _value = value.Trim('\'');
        }

        public object GetValue()
        {
            return _value;
        }
    }
}
