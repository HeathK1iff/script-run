namespace ScriptRun.Expression.Base.NoTerminal
{
    public abstract class NoTerminalExpressionBase: ExpressionBase
    {
        public NoTerminalExpressionBase(ExpressionBase left, ExpressionBase right)
        {
            Left = left;
            Right = right;
        }

        public ExpressionBase Left { get; }
        public ExpressionBase Right { get; }
    }
}
