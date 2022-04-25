using ScriptRun.Expression.Arithmetic.Calculator;
using ScriptRun.Expression.Base;
using ScriptRun.Expression.Base.NoTerminal;
using ScriptRun.Expression.Utils;

namespace ScriptRun.Expression.Arithmetic
{

    public abstract class ArithmeticBaseExpression : NoTerminalExpressionBase, IGetValue
    {
        protected ArithmeticBaseExpression(ExpressionBase left, ExpressionBase right) : base(left, right) { }

        public abstract ArithmeticOperation GetOperation();

        public void Calculate(IArithmeticCalculator context)
        {
            context.Push(Left, GetOperation());

            if (Right is ArithmeticBaseExpression valRight)
            {
                valRight.Calculate(context);
                return;
            }

            context.Push(Right);
        }

        public object GetValue()
        {
            var context = new ArithmeticCalculator();
            Calculate(context);
            return context.Calculate();
        }
    }

}

