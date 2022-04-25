using ScriptRun;
using ScriptRun.Expression.Base;
using ScriptRun.Scan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;

namespace ScriptRunTest
{
    [TestClass]
    public class ScriptRunnerTest
    {

        private Stream CreateStream(string script)
        {
            MemoryStream stream = new MemoryStream();
            using (var sw = new StreamWriter(stream, null, -1, true))
            {
                sw.Write(script);
                sw.Flush();
            }
            return stream;
        }

        [TestMethod]
        [DataRow("x=myFunc(x1=1, y2=2);", 3)]
        [DataRow("x=5 + myFunc(x1=1, y2=2);", 8)]
        [DataRow("x=myFunc(x1=1, y2=2) + 5;", 8)]
        [DataRow("a=2; b=4; x=myFunc(x1=a, y2=b) + 4;", 10)]
        public void ExecuteFunction(string script, double result)
        {
            using (Stream stream = CreateStream(script))
            {
                var vars = new Dictionary<string, object>();
                var runner = new ScriptRunner(new ExpressionParcer(new Scanner(stream)));
                runner.OnCallFunction += (name, parameters) =>
                {
                    if (name == "myFunc")
                    {
                        return (double)parameters["x1"] + (double)parameters["y2"];
                    }
                    return 0d;
                };

                runner.OnGetVariableValue += (name) =>
                {
                    if (vars.TryGetValue(name, out object val))
                        return val;
                    return null;
                };

                runner.OnSetVariableValue += (name, value) =>
                {
                    vars[name] = value;
                };

                runner.ExecuteScript();

                Assert.IsTrue((double)vars["x"] == result);
            }
        }


        [TestMethod]
        [DataRow("a=10; x=a;", 10)]
        [DataRow("a=10; x=a+3;", 13)]
        [DataRow("a=10; x=3+a;", 13)]
        [DataRow("a=10; b=12; x=a+b;", 22)]
        [DataRow("a=10; b=12; x=a+b;", 22)]
        [DataRow("a=10; b=12; c=7; x=a+b-c;", 15)]
        [DataRow("a=10; b=12; c=2; x=a+b/c;", 16)]
        [DataRow("a=12; b=2; c=10; x=a/b+c;", 16)]
        [DataRow("a=12; b=2; c=3; x=a/b/c;", 2)]
        [DataRow("a=2; b=2; c=2; x=a*b*c;", 8)]

        public void CalcArithmeticOperation(string script, double result)
        {
            using (Stream stream = CreateStream(script))
            {
                var vars = new Dictionary<string, object>();
                var runner = new ScriptRunner(new ExpressionParcer(new Scanner(stream)));
                runner.OnGetVariableValue += (name) =>
                {
                    if (vars.TryGetValue(name, out object val))
                        return val;
                    return null;
                };

                runner.OnSetVariableValue += (name, value) =>
                {
                    vars[name] = value;
                };

                runner.ExecuteScript();

                Assert.IsTrue((double)vars["x"] == result);
            }
        }

    }
}
