using ScriptRun.Exception;
using ScriptRun.Expression.Arithmetic;
using ScriptRun.Expression.Base.NoTerminal;
using ScriptRun.Expression.Base.Terminal;
using ScriptRun.Expression.Collection;
using ScriptRun.Scan;
using ScriptRun.Scan.Iterator;

namespace ScriptRun.Expression.Base
{
    public class ExpressionParcer : IExpressionParcer
    {
        ITokenIterator _tokenIterator;
        GetVariableValue _valueGetHandler;
        CallFunctionValue _callFunctionHandler;

        public ExpressionParcer(Scanner scanner)
        {
            _tokenIterator = new TokenIterator(scanner);
        }

        ExpressionBase NumberValueExpression(ITokenIterator tokenIterator)
        {
            var result = new NumberValueExpression(double.Parse(tokenIterator.Token.Value));

            if (tokenIterator.NextToken().Token is null)
            {
                if (tokenIterator.Previous.Type != TokenType.Semicolon)
                    throw new ParceSyntaxErrorExeption(tokenIterator.Previous);
                
                return null;
            }

            switch (tokenIterator.Token.Type)
            {
                case TokenType.Plus: return new SumValueExpression(result, AdditionExpression(tokenIterator));
                case TokenType.Minus: return new SubtractionValueExpression(result, SubstractExpression(tokenIterator));
                case TokenType.Divide: return new DivideValueExpression(result, DivideExpression(tokenIterator));
                case TokenType.Multiply: return new MultiplyValueExpression(result, MultiplyExpression(tokenIterator));
                case TokenType.Comma: return result;
                case TokenType.Semicolon: return result;
                case TokenType.EndBracket: return result;
            }

            throw new ParceSyntaxErrorExeption(tokenIterator.Token);
        }

        ExpressionBase TextValueExpression(ITokenIterator tokenIterator)
        {
            var result = new TextValueExpression(tokenIterator.Token.Value);

            if (tokenIterator.NextToken().Token is null)
            {
                if (tokenIterator.Previous.Type != TokenType.Semicolon)
                    throw new ParceSyntaxErrorExeption(tokenIterator.Previous);

                return null;
            }

            switch (tokenIterator.Token.Type)
            {
                case TokenType.Comma: return result;
                case TokenType.Semicolon: return result;
                case TokenType.EndBracket: return result;
            }

            throw new ParceSyntaxErrorExeption(tokenIterator.Token);
        }


        ExpressionBase FunctionParametersExpression(ITokenIterator tokenIterator)
        {
            var parameters = new CollectionExpression();

            IToken token = null;

            while (((token = tokenIterator.NextToken().Token) != null) &&
                (token.Type != TokenType.Semicolon) && (token.Type != TokenType.EndBracket))
            {
                switch (token.Type)
                {
                    case TokenType.Id: parameters.Add(IdentExpression(tokenIterator)); break;
                    case TokenType.Comma: continue;
                    case TokenType.EndBracket: continue;
                    case TokenType.Semicolon: break;
                    default: throw new ParceSyntaxErrorExeption(tokenIterator.Token);
                }

                if (tokenIterator.Token.Type == TokenType.EndBracket)
                    break;
            }

            return parameters;
        }


        ExpressionBase AdditionExpression(ITokenIterator tokenIterator)
        {
            if (tokenIterator.NextToken().Token is null)
            {
                if (tokenIterator.Previous.Type != TokenType.Semicolon) 
                    throw new ParceSyntaxErrorExeption(tokenIterator.Previous);

                return null;
            }

            switch (tokenIterator.Token.Type)
            {
                case TokenType.Number: return NumberValueExpression(tokenIterator);
                case TokenType.Id: return IdentExpression(tokenIterator);
                default: throw new ParceSyntaxErrorExeption(tokenIterator.Token);
            }
        }

        ExpressionBase SubstractExpression(ITokenIterator tokenIterator)
        {
            if (tokenIterator.NextToken().Token is null)
            {
                if (tokenIterator.Previous.Type != TokenType.Semicolon)
                    throw new ParceSyntaxErrorExeption(tokenIterator.Previous);

                return null;
            }

            switch (tokenIterator.Token.Type)
            {
                case TokenType.Number: return NumberValueExpression(tokenIterator);
                case TokenType.Id: return IdentExpression(tokenIterator);
                default: throw new ParceSyntaxErrorExeption(tokenIterator.Token);
            }
        }

        ExpressionBase DivideExpression(ITokenIterator tokenIterator)
        {
            if (tokenIterator.NextToken().Token is null)
            {
                if (tokenIterator.Previous.Type != TokenType.Semicolon)
                    throw new ParceSyntaxErrorExeption(tokenIterator.Previous);

                return null;
            }

            switch (tokenIterator.Token.Type)
            {
                case TokenType.Number: return NumberValueExpression(tokenIterator);
                case TokenType.Id: return IdentExpression(tokenIterator);
                default: throw new ParceSyntaxErrorExeption(tokenIterator.Token);
            }
        }

        ExpressionBase MultiplyExpression(ITokenIterator tokenIterator)
        {
            if (tokenIterator.NextToken().Token is null)
            {
                if (tokenIterator.Previous.Type != TokenType.Semicolon)
                    throw new ParceSyntaxErrorExeption(tokenIterator.Previous);

                return null;
            }

            switch (tokenIterator.Token.Type)
            {
                case TokenType.Number: return NumberValueExpression(tokenIterator);
                case TokenType.Id: return IdentExpression(tokenIterator);
                default: throw new ParceSyntaxErrorExeption(tokenIterator.Token);
            }
        }

        ExpressionBase AssignExpression(ITokenIterator tokenIterator)
        {

            if (tokenIterator.NextToken().Token is null)
            {
                if (tokenIterator.Previous.Type != TokenType.Semicolon)
                    throw new ParceSyntaxErrorExeption(tokenIterator.Previous);

                return null;
            }

            switch (tokenIterator.Token.Type)
            {
                case TokenType.Number: return NumberValueExpression(tokenIterator);
                case TokenType.Id: return IdentExpression(tokenIterator);
                case TokenType.Text: return TextValueExpression(tokenIterator);
                default: throw new ParceSyntaxErrorExeption(tokenIterator.Token);
            }
        }

        ExpressionBase BeginBracketBeginBracket(ITokenIterator tokenIterator, string identName)
        {
            ExpressionBase parameters = FunctionParametersExpression(tokenIterator);
            
            if (tokenIterator.NextToken().Token is null)
            {
                if (tokenIterator.Previous.Type != TokenType.Semicolon)
                    throw new ParceSyntaxErrorExeption(tokenIterator.Previous);

                return null;
            }

            switch (tokenIterator.Token.Type)
            {
                case TokenType.Plus: return new SumValueExpression(new FunctionExpression(identName, parameters, _callFunctionHandler), AdditionExpression(tokenIterator));
                case TokenType.Minus: return new SubtractionValueExpression(new FunctionExpression(identName, parameters, _callFunctionHandler), SubstractExpression(tokenIterator));
                case TokenType.Divide: return new DivideValueExpression(new FunctionExpression(identName, parameters, _callFunctionHandler), DivideExpression(tokenIterator));
                case TokenType.Multiply: return new MultiplyValueExpression(new FunctionExpression(identName, parameters, _callFunctionHandler), MultiplyExpression(tokenIterator));
                default: return new FunctionExpression(identName, parameters, _callFunctionHandler);
            }
        }

        ExpressionBase IdentExpression(ITokenIterator tokenIterator)
        {
            var identName = tokenIterator.Token.Value;
            var type = tokenIterator.Token.Type;

            if (tokenIterator.NextToken().Token is null)
            {
                if (tokenIterator.Previous.Type != TokenType.Semicolon)
                    throw new ParceSyntaxErrorExeption(tokenIterator.Previous);
                
                return null;
            }


            switch (tokenIterator.Token.Type)
            {
                case TokenType.BeginBracket: return BeginBracketBeginBracket(tokenIterator, identName);
                case TokenType.Assign: return new AssignValueExpression(new VariableExpression(identName, _valueGetHandler), AssignExpression(tokenIterator));
                case TokenType.Plus: return new SumValueExpression(new VariableExpression(identName, _valueGetHandler), AdditionExpression(tokenIterator));
                case TokenType.Minus: return new SubtractionValueExpression(new VariableExpression(identName, _valueGetHandler), SubstractExpression(tokenIterator));
                case TokenType.Divide: return new DivideValueExpression(new VariableExpression(identName, _valueGetHandler), DivideExpression(tokenIterator));
                case TokenType.Multiply: return new MultiplyValueExpression(new VariableExpression(identName, _valueGetHandler), MultiplyExpression(tokenIterator));
                case TokenType.Semicolon: return new VariableExpression(identName, _valueGetHandler);
                case TokenType.Comma: return new VariableExpression(identName, _valueGetHandler);
                case TokenType.EndBracket: return new VariableExpression(identName, _valueGetHandler);
                default: throw new ParceSyntaxErrorExeption(tokenIterator.Token);
            }
        }

        public ExpressionBase NextExpression()
        {
            ITokenIterator tokenIterator = null;
            while ((tokenIterator = _tokenIterator.NextToken()) != null)
            {
                if (tokenIterator.Token == null)
                    return null;

                switch (tokenIterator.Token.Type)
                {
                    case TokenType.Id: return IdentExpression(tokenIterator);
                    default: throw new ParceSyntaxErrorExeption(tokenIterator.Token);
                };
            }
            return null;
        }

        public void SetGetVariableHandler(GetVariableValue valueGetHandler)
        {
            _valueGetHandler = valueGetHandler;
        }

        public void SetCallFunctionHandler(CallFunctionValue callFunctionHandler)
        {
            _callFunctionHandler = callFunctionHandler;
        }
    }

}

