using ScriptRun.Expression.Base;
using ScriptRun.Expression.Utils;
using System;
using System.Collections.Generic;

namespace ScriptRun.Expression.Arithmetic.Calculator
{
    public class ArithmeticCalculator : IArithmeticCalculator
    {
        Stack<double> _stack = new Stack<double>();
        ArithmeticOperation _lastOperation;

        
        public double Calculate()
        {
            Func<double, double> leftSumFunc = null;
            leftSumFunc = (val) =>
            {
                double result = val;
                if (_stack.TryPop(out double stackResult))
                    result += leftSumFunc(stackResult);

                return result;
            };

            return leftSumFunc(_stack.Pop());
        }

        public void Push(ExpressionBase expression, ArithmeticOperation operation = ArithmeticOperation.none)
        {
            double val = 0;

            if (expression is IGetValue valExp)
            {
                val = (double) valExp.GetValue();
            }
            else
                throw new InvalidCastException();

            switch (_lastOperation)
            {
                case ArithmeticOperation.div:
                    val = _stack.Pop() / val;
                    break;
                case ArithmeticOperation.mul:
                    val = _stack.Pop() * val;
                    break;
                case ArithmeticOperation.sub:
                    val = -1 * val;
                    break;
            }

            _stack.Push(val);
            _lastOperation = operation;
        }
    }

}

