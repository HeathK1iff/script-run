using ScriptRun.Scan;
using System.Runtime.Serialization;

namespace ScriptRun.Exception
{
    public class ParceSyntaxErrorExeption : System.Exception
    {
        public ParceSyntaxErrorExeption() { }

        protected ParceSyntaxErrorExeption(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ParceSyntaxErrorExeption(string message) : base(message) { }

        public ParceSyntaxErrorExeption(string message, System.Exception innerException) : base(message, innerException) { }

        public ParceSyntaxErrorExeption(IToken token) : base($"Syntax error near '{token.Value}' token") { }

    }

}

