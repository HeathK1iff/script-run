namespace ScriptRun.Scan.Iterator
{
    public class TokenIterator : ITokenIterator
    {
        Scanner _scanner;
        Token _current;
        Token _previous;
        public TokenIterator(Scanner scanner)
        {
            _scanner = scanner;
            _previous = null;
        }

        public IToken Token => _current;

        public IToken Previous => _previous;

        public ITokenIterator NextToken()
        {
            if (_current != null)
                _previous = new Token(_current);
            _current = _scanner.GetToken();
            return this;
        }
    }

}

