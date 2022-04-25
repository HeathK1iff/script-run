using ScriptRun.Expression.Arithmetic.Calculator;
using ScriptRun.Expression.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScriptRun.Expression.Arithmetic
{
    public class MultiplyValueExpression : ArithmeticBaseExpression
    {
        public MultiplyValueExpression(ExpressionBase left, ExpressionBase right) : base(left, right) {}

        public override ArithmeticOperation GetOperation()
        {
            return ArithmeticOperation.mul;
        }
    }
}
