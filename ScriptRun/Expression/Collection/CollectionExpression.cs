using ScriptRun.Expression.Base;
using ScriptRun.Expression.Base.NoTerminal;
using ScriptRun.Expression.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ScriptRun.Expression.Collection
{
    public class CollectionExpression : ExpressionBase, ICollectionExpression
    {
        List<ExpressionBase> _params = new List<ExpressionBase>();

        public CollectionExpression() { }


        public void Add(ExpressionBase expression)
        {
            _params.Add(expression);
        }

        public int Count()
        {
            return _params.Count;
        }
        public string[] GetKeys()
        {
            return _params.OfType<IGetName>().Select(f => f.GetName()).ToArray();
        }

        public ExpressionBase Get(string name)
        {
            return _params.Select(f => ((AssignValueExpression)f)).
                                FirstOrDefault(f => f.GetName() == name);
        }

        public ExpressionBase Get(int index)
        {
            return _params[index];
        }
    }

}

