using ScriptRun.Expression.Arithmetic.Calculator;
using ScriptRun.Expression.Base;

namespace ScriptRun.Expression.Arithmetic
{
    public class SubtractionValueExpression : ArithmeticBaseExpression
    {
        public SubtractionValueExpression(ExpressionBase left, ExpressionBase right) : base(left, right) { }

        public override ArithmeticOperation GetOperation()
        {
            return ArithmeticOperation.sub;
        }
    }

}

