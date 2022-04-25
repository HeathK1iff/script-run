using ScriptRun.Expression.Base;

namespace ScriptRun.Expression.Collection
{
    public interface ICollectionExpression
    {
        public int Count();
        public string[] GetKeys();
        public ExpressionBase Get(string name);
        public ExpressionBase Get(int index);
    }

}

