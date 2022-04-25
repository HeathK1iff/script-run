using ScriptRun.Scan;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ScriptRunTest
{
    [TestClass]
    public class ScannerTest
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
        [DataRow("\rh", TokenType.Id)]
        [DataRow("h\r", TokenType.Id)]
        [DataRow("h\n", TokenType.Id)]
        [DataRow("\nh", TokenType.Id)]
        [DataRow(">X4<", TokenType.Undefined)]
        [DataRow("hss33", TokenType.Id)]
        [DataRow("=", TokenType.Assign)]
        [DataRow("(", TokenType.BeginBracket)]
        [DataRow(")", TokenType.EndBracket)]
        [DataRow(",", TokenType.Comma)]
        [DataRow("33233", TokenType.Number)]
        [DataRow(";", TokenType.Semicolon)]
        [DataRow("{", TokenType.BeginBlock)]
        [DataRow("}", TokenType.EndBlock)]
        [DataRow("'test'", TokenType.Text)]
        [DataRow("*", TokenType.Multiply)]

        public void ReservedSymbolsTest(string code, TokenType type)
        {
            MemoryStream stream = new MemoryStream();
            using (var sw = new StreamWriter(stream, null, -1, true))
            {
                sw.Write(code);
                sw.Flush();
            }
            var scanner = new Scanner(stream);

            Assert.IsTrue(scanner.GetToken().Type == type);
        }

        [TestMethod]
        [DataRow("function ( x= 1, y = 2 );", "1", "2")]
        [DataRow("function ( x= -11, y = 24 );", "-11", "24")]
        [DataRow("function ( x= 11, y = -24 );", "11", "-24")]
        [DataRow("function(x=-1,y=2);", "-1", "2")]
        [DataRow("function(x= -11,y=24);", "-11", "24")]
        [DataRow("function ( x= 11, y = -24 );", "11", "-24")]

        public void FunctionWithSimpleParameters(string expression, string tokenValue1, string tokenValue2)
        {
            var scanner = new Scanner(CreateStream(expression));

            Assert.IsTrue(scanner.GetToken().Type == TokenType.Id);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.BeginBracket);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Id);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Assign);

            IToken valueToken1 = scanner.GetToken();
            Assert.IsTrue(valueToken1.Type == TokenType.Number);
            Assert.IsTrue(valueToken1.Value == tokenValue1);

            Assert.IsTrue(scanner.GetToken().Type == TokenType.Comma);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Id);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Assign);

            IToken valueToken2 = scanner.GetToken();
            Assert.IsTrue(valueToken2.Type == TokenType.Number);
            Assert.IsTrue(valueToken2.Value == tokenValue2);

            Assert.IsTrue(scanner.GetToken().Type == TokenType.EndBracket);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Semicolon);
            Assert.IsTrue(scanner.GetToken() == null);
        }

        [TestMethod]
        [DataRow("x=1 + 4/2 - 5  *1;")]
        [DataRow("x=1 + 4/ 2 - 5 * 1 ;")]
        [DataRow("x=1 + 4 /2 - 5*1;")]
        [DataRow("x=1 +4 /2 - 5 *1;")]
        [DataRow("x=1 + 4/ 2- 5* 1;")]

        public void AssingNumberWithArithmeticExpression(string expression)
        {
            var scanner = new Scanner(CreateStream(expression));

            Assert.IsTrue(scanner.GetToken().Type == TokenType.Id);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Assign);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Number);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Plus);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Number);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Divide);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Number);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Minus);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Number);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Multiply);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Number);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Semicolon);
            Assert.IsTrue(scanner.GetToken() == null);
        }

        [TestMethod]
        [DataRow("x=1;", "1")]
        [DataRow("x =1;", "1")]
        [DataRow("x = 1;", "1")]
        [DataRow("x= 1;", "1")]
        [DataRow("x=-42;", "-42")]
        [DataRow("x=1.5;", "1.5")]
        [DataRow("x=-1.8 ;", "-1.8")]

        public void AssingNumber(string expression, string tokenValue)
        {

            var scanner = new Scanner(CreateStream(expression));

            Assert.IsTrue(scanner.GetToken().Type == TokenType.Id);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Assign);

            IToken token = scanner.GetToken();
            Assert.IsTrue(token.Type == TokenType.Number);
            Assert.IsTrue(token.Value == tokenValue);

            Assert.IsTrue(scanner.GetToken().Type == TokenType.Semicolon);
            Assert.IsTrue(scanner.GetToken() == null);
        }

        [TestMethod]
        [DataRow("x='x/*31&$s+-^s*)(#';", "'x/*31&$s+-^s*)(#'")]
        [DataRow("x ='x/*31&$s+-^s*)(#';", "'x/*31&$s+-^s*)(#'")]
        [DataRow("x = 'x/*31&$s+-^s*)(#';", "'x/*31&$s+-^s*)(#'")]
        [DataRow("x= 'x/*31&$s+-^s*)(#';", "'x/*31&$s+-^s*)(#'")]
        [DataRow("x= 'x/*31&$s+-^s*)(#';", "'x/*31&$s+-^s*)(#'")]
        public void AssingTextTest(string expression, string tokenValue)
        {
            var scanner = new Scanner(CreateStream(expression));

            Assert.IsTrue(scanner.GetToken().Type == TokenType.Id);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Assign);

            IToken token = scanner.GetToken();
            Assert.IsTrue(token.Type == TokenType.Text);
            Assert.IsTrue(token.Value == tokenValue);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Semicolon);
            Assert.IsTrue(scanner.GetToken() == null);
        }

        [TestMethod]
        [DataRow("func(x=100, y='x/*31&$s+-^s*)(#');", "'x/*31&$s+-^s*)(#'")]
        public void AssingFuncTextTest(string expression, string tokenValue)
        {
            var scanner = new Scanner(CreateStream(expression));

            Assert.IsTrue(scanner.GetToken().Type == TokenType.Id);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.BeginBracket);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Id);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Assign);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Number);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Comma);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Id);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Assign);

            IToken token = scanner.GetToken();
            Assert.IsTrue(token.Type == TokenType.Text);
            Assert.IsTrue(token.Value == tokenValue);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.EndBracket);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Semicolon);
            Assert.IsTrue(scanner.GetToken() == null);
        }


        [TestMethod]
        [DataRow("{x='test';y=100;}")]
        [DataRow(" { x = 'test'; y = 100; } ")]
        [DataRow("{x= 'test'; y= 100;}")]
        [DataRow("{x='test' ; y = 100;}")]
        [DataRow("{ x ='test';y = 100;}")]
        public void ExpressionBlockTest(string expression)
        {
            var scanner = new Scanner(CreateStream(expression));

            Assert.IsTrue(scanner.GetToken().Type == TokenType.BeginBlock);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Id);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Assign);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Text);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Semicolon);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Id);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Assign);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Number);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.Semicolon);
            Assert.IsTrue(scanner.GetToken().Type == TokenType.EndBlock);
            Assert.IsTrue(scanner.GetToken() == null);
        }
    }
}
