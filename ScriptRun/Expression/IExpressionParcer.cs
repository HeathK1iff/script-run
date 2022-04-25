using System.Collections.Generic;

namespace ScriptRun.Expression.Base
{

    public delegate object GetVariableValue(string name);

    public delegate object CallFunctionValue(string name, Dictionary<string, object> parameters);

    public interface IExpressionParcer
    {
        public ExpressionBase NextExpression();
        public void SetGetVariableHandler(GetVariableValue valueGetHandler);
        public void SetCallFunctionHandler(CallFunctionValue callFunctionHandler);
    }

}

