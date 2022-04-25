namespace ScriptRun.Scan
{
    public enum TokenType
    {
        Undefined, Id, Number, Text, BeginBracket, Comma, Plus, Minus, Divide, Multiply,
        EndBracket, Assign, Semicolon, BeginBlock, EndBlock, ReservedWord
    }

    public interface IToken
    {
        public TokenType Type { get; }
        public string Value { get; }
    }
}
