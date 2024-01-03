using Avocat.Compiler.Expressions;
using Avocat.Compiler.Tokenizer;

namespace Avocat.Statements
{
    public class StatementEOF : Statement
    {
        public StatementEOF(Token token, ExpressionEOF expression) : base(token, expression)
        { }

        public override string ToString()
        {
            return "EOF";
        }
    }
}
