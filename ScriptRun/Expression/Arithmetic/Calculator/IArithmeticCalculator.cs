using ScriptRun.Expression.Base;

namespace ScriptRun.Expression.Arithmetic.Calculator
{
    public interface IArithmeticCalculator
    {
        public double Calculate();
        public void Push(ExpressionBase expression, ArithmeticOperation operation = ArithmeticOperation.none);
    }

}

