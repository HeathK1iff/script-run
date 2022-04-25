/*
 * Copyright 2022 heathk1iff@outlook.com
 * Licensed under the Apache License, Version 2.0
*/

using ScriptRun.Expression.Base;
using ScriptRun.Expression.Collection;
using ScriptRun.Expression.Utils;
using System.Collections.Generic;

namespace ScriptRun
{
    public delegate object OnCallFunction(string name, Dictionary<string, object> param);

    public delegate void OnSetVariableValue(string name, object value);

    public delegate object OnGetVariableValue(string name);

    public class ScriptRunner
    {
        IExpressionParcer _parcer;
        public event OnCallFunction OnCallFunction;
        public event OnSetVariableValue OnSetVariableValue;
        public event OnGetVariableValue OnGetVariableValue;

        public ScriptRunner(IExpressionParcer parcer)
        {
            _parcer = parcer;
        }

        public void ExecuteScript()
        {
            ExpressionBase expression = null;
            _parcer.SetGetVariableHandler((name) => OnGetVariableValue?.Invoke(name));
            _parcer.SetCallFunctionHandler((name, parameters) => OnCallFunction?.Invoke(name, parameters));


            while ((expression = _parcer.NextExpression()) != null)
            {
                if (expression is FunctionExpression func)
                {
                    func.GetValue();
                    continue;
                }

                if ((expression is IGetValue val) && ((expression is IGetName valName)))
                {
                    OnSetVariableValue?.Invoke(valName.GetName(), val.GetValue());
                }

            }
        }

    }
}
