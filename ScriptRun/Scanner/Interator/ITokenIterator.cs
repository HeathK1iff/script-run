namespace ScriptRun.Scan.Iterator
{
    public interface ITokenIterator
    {
        public IToken Token { get; }

        public IToken Previous { get; }

        public ITokenIterator NextToken();
    }

}

