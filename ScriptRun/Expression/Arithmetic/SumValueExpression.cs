using ScriptRun.Expression.Arithmetic.Calculator;
using ScriptRun.Expression.Base;

namespace ScriptRun.Expression.Arithmetic
{
    public class SumValueExpression : ArithmeticBaseExpression
    {
        public SumValueExpression(ExpressionBase left, ExpressionBase right) : base(left, right) { }

        public override ArithmeticOperation GetOperation()
        {
            return ArithmeticOperation.sum;
        }
    }

}

