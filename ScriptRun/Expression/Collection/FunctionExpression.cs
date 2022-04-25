using ScriptRun.Expression.Base;
using ScriptRun.Expression.Utils;
using System.Collections.Generic;
using System.Linq;

namespace ScriptRun.Expression.Collection
{
    public class FunctionExpression : ExpressionBase, IGetValue
    {
        ExpressionBase _parameters;
        CallFunctionValue _callFunctionHandler;

        public FunctionExpression(string name, ExpressionBase parameters, CallFunctionValue callFunctionHandler)
        {
            Name = name;
            _parameters = parameters;
            _callFunctionHandler = callFunctionHandler;
        }

        public string Name { get; }

        public int Count()
        {
            if (_parameters is ICollectionExpression parameters)
            {
                return parameters.Count();
            }

            return 0;
        }

        public object Get(string name)
        {
            if (_parameters is ICollectionExpression parameters)
            {
                if (parameters.Get(name) is IGetValue value)
                {
                    return value.GetValue();
                };
            }

            return null;
        }
        public object Get(int index)
        {
            if (_parameters is ICollectionExpression parameters)
            {
                if (parameters.Get(index) is IGetValue value)
                {
                    return value.GetValue();
                };
            }

            return null;
        }

        public object GetValue()
        {
            Dictionary<string, object> dictVars = null;
            if (_parameters is ICollectionExpression parameters)
            {
                dictVars = new Dictionary<string, object>(parameters.GetKeys()
                    .Select(f =>
                    {
                        if (parameters.Get(f) is IGetValue val)
                        {
                            return new KeyValuePair<string, object>(f, val.GetValue());
                        }
                        return new KeyValuePair<string, object>(f, null);
                    }
                    ));
            }
            return _callFunctionHandler?.Invoke(Name, dictVars);
        }

    }

}

