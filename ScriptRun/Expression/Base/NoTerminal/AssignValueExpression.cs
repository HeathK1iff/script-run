using ScriptRun.Expression.Base.Terminal;
using ScriptRun.Expression.Utils;

namespace ScriptRun.Expression.Base.NoTerminal
{
    public class AssignValueExpression : NoTerminalExpressionBase, IGetValue, IGetName
    {
        public AssignValueExpression(ExpressionBase left, ExpressionBase right) : base(left, right) {}
        public string GetName()
        {
            if (Left is VariableExpression variable)
            {
                return variable.Name;
            }
            return string.Empty;
        }

        public object GetValue()
        {
            if (Right is IGetValue val)
            {
                return val.GetValue();
            }

            return null;
        }
    }

}

