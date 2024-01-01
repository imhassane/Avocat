namespace Avocat.Statements
{
    public class StatementEOF : Statement
    {
        public StatementEOF(Tokenizer.Token token, Expressions.ExpressionEOF expression) : base(token, expression)
        { }

        public override string ToString()
        {
            return "EOF";
        }
    }
}
