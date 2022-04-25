using ScriptRun.Expression.Base;
using ScriptRun.Expression.Base.NoTerminal;
using ScriptRun.Expression.Collection;
using ScriptRun.Scan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace ScriptRunTest
{
    [TestClass]
    public class ScriptExpressionParcerTest 
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
        [DataRow("MyFunction ( x = 1 ) ;")]
        [DataRow("MyFunction( x = 1 ) ;")]
        [DataRow("MyFunction(x = 1 ) ;")]
        [DataRow("MyFunction(x= 1 ) ;")]
        [DataRow("MyFunction(x=1 ) ;")]
        [DataRow("MyFunction(x=1) ;")]
        [DataRow("MyFunction(x=1);")]
        [DataRow(" MyFunction(x=1); ")]
        public void SingleFunctionCallWithSingleParameterExpression(string script)
        {
            using (Stream stream = CreateStream(script))
            {
                var scriptParcer = new ExpressionParcer(new Scanner(stream));
                ExpressionBase result = scriptParcer.NextExpression();
                Assert.IsNotNull(result);
                Assert.IsTrue(result is FunctionExpression);

                if (result is FunctionExpression func)
                {
                    Assert.IsTrue(func.Count() == 1);
                    Assert.IsTrue(func.Name == "MyFunction");
                    Assert.IsTrue((double)func.Get("x") == 1);
                }
            }
        }

        [TestMethod]
        [DataRow("MyFunction(x=1,y=2,z = 3 ) ; MyFunction2(x=4,y=5 ) ;")]
        [DataRow(" MyFunction ( x = 1, y = 2, z = 3 ) ; MyFunction2(x = 4,y =5);")]
        [DataRow("MyFunction ( x = 1, y = 2, z = 3 ) ; MyFunction2(x = 4, y =5 );")]
        [DataRow("MyFunction( x = 1, y= 2, z = 3 ) ;MyFunction2( x = 4, y = 5);")]
        [DataRow("MyFunction(x = 1, y=2, z =  3) ; MyFunction2(x = 4,y = 5);")]
        [DataRow("MyFunction(x= 1, y= 2, z = 3 ) ; MyFunction2( x = 4, y = 5);")]
        [DataRow("MyFunction(x=1,y=2,z =  3) ;MyFunction2( x = 4, y = 5 );")]
        public void MultiFunctionCallWitMultiParametersExpression(string script)
        {
            using (Stream stream = CreateStream(script))
            {
                var scriptParcer = new ExpressionParcer(new Scanner(stream));
                ExpressionBase result = scriptParcer.NextExpression();
                Assert.IsNotNull(result);
                Assert.IsTrue(result is FunctionExpression);

                if (result is FunctionExpression func)
                {
                    Assert.IsTrue(func.Count() == 3);
                    Assert.IsTrue(func.Name == "MyFunction");
                    Assert.IsTrue((double)func.Get("x") == 1);
                    Assert.IsTrue((double)func.Get("y") == 2);
                    Assert.IsTrue((double)func.Get("z") == 3);
                }

                ExpressionBase result2 = scriptParcer.NextExpression();
                Assert.IsNotNull(result2);
                Assert.IsTrue(result2 is FunctionExpression);

                if (result2 is FunctionExpression func2)
                {
                    Assert.IsTrue(func2.Count() == 2);
                    Assert.IsTrue(func2.Name == "MyFunction2");
                    Assert.IsTrue((double)func2.Get("x") == 4);
                    Assert.IsTrue((double)func2.Get("y") == 5);
                }
            }
        }

        [TestMethod]
        [DataRow("MyFunction(x=1) ; MyFunction2(y=5 ) ;")]
        [DataRow(" MyFunction ( x = 1) ; MyFunction2(y =5);")]
        [DataRow("MyFunction ( x = 1 ) ; MyFunction2(y =5 );")]
        [DataRow("MyFunction( x =1 ) ;MyFunction2( y = 5);")]
        [DataRow("MyFunction(x =1) ; MyFunction2(y = 5);")]
        [DataRow("MyFunction(x= 1) ; MyFunction2(  y = 5);")]
        [DataRow("MyFunction(x=1) ;MyFunction2( y = 5 );")]
        public void MultiFunctionCallWithSingleParameterExpression(string script)
        {
            using (Stream stream = CreateStream(script))
            {
                var scriptParcer = new ExpressionParcer(new Scanner(stream));
                ExpressionBase result = scriptParcer.NextExpression();
                Assert.IsNotNull(result);
                Assert.IsTrue(result is FunctionExpression);

                if (result is FunctionExpression func)
                {
                    Assert.IsTrue(func.Count() == 1);
                    Assert.IsTrue(func.Name == "MyFunction");
                    Assert.IsTrue((double)func.Get("x") == 1); ;
                }

                ExpressionBase result2 = scriptParcer.NextExpression();
                Assert.IsNotNull(result2);
                Assert.IsTrue(result2 is FunctionExpression);

                if (result2 is FunctionExpression func2)
                {
                    Assert.IsTrue(func2.Count() == 1);
                    Assert.IsTrue(func2.Name == "MyFunction2");
                    Assert.IsTrue((double)func2.Get("y") == 5);
                }
            }
        }

        [TestMethod]
        [DataRow("MyFunc(x = 12 + 13);", 25)]
        [DataRow("MyFunc(x = 12 - 13);", -1)]
        [DataRow("MyFunc(x = 12 / 6);", 2)]
        [DataRow("MyFunc(x = 2 * 3);", 6)]
        public void SingleFunctionCallWithParameterExpression(string script, double xVar)
        {
            using (Stream stream = CreateStream(script))
            {
                var scriptParcer = new ExpressionParcer(new Scanner(stream));
                ExpressionBase result = scriptParcer.NextExpression();
                Assert.IsNotNull(result);
                Assert.IsTrue(result is FunctionExpression);

                if (result is FunctionExpression func)
                {
                    Assert.IsTrue(func.Name == "MyFunc");
                    Assert.IsTrue(func.Count() == 1);
                    Assert.IsTrue((double)func.Get("x") == xVar);
                }
            }
        }

        [TestMethod]
        [DataRow("MyFunc(x=10, y='test');", "test")]
        public void SingleFunctionCallWithTextParameterExpression(string script, string xStr)
        {
            using (Stream stream = CreateStream(script))
            {
                var scriptParcer = new ExpressionParcer(new Scanner(stream));
                ExpressionBase result = scriptParcer.NextExpression();
                Assert.IsNotNull(result);
                Assert.IsTrue(result is FunctionExpression);

                if (result is FunctionExpression func)
                {
                    Assert.IsTrue(func.Name == "MyFunc");
                    Assert.IsTrue(func.Count() == 2);
                    Assert.IsTrue(func.Get("y").ToString() == xStr);
                }
            }
        }


        [TestMethod]
        [DataRow("x = 13;", 13)]
        [DataRow("x = 12 + 13;", 25)]
        [DataRow("x = 20 / 5;", 4)]
        [DataRow("x = 20 - 12;", 8)]
        [DataRow("x = 20 - 5 + 1;", 16)]
        [DataRow("x = 20 - 10/ 2;", 15)]
        [DataRow("x = 20 - 10 /2 - 10;", 5)]
        [DataRow("x = 20 - 10 /2 + 10*2;", 35)] 
        public void AssignValueWithArithmeticExpression(string script, double result)
        {
            using (Stream stream = CreateStream(script))
            {
                var scriptParcer = new ExpressionParcer(new Scanner(stream));
                ExpressionBase expression = scriptParcer.NextExpression();
                Assert.IsNotNull(expression);
                Assert.IsTrue(expression is AssignValueExpression);

                if (expression is AssignValueExpression variable)
                {
                    Assert.IsTrue(variable.GetName() == "x");
                    double val = (double) variable.GetValue();
                    Assert.IsTrue(val == result);
                }
            }
        }
    }
}
