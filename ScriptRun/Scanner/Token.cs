namespace ScriptRun.Scan
{
    public class Token : IToken
    {
        public Token(IToken token)
        {
            Type = token.Type;
            Value = token.Value;
        }

        public Token(TokenType type, string value)
        {
            Value = value;
            Type = type;
        }

        public TokenType Type { get; }
        public string Value { get; }
    }
}
