namespace Avocat.Statements
{
    public abstract class Statement
    {
        public Tokenizer.Token Token { get; private set; }

        public Expressions.Expression Expression { get; private set; }

        public Statement(Tokenizer.Token token, Expressions.Expression expression)
        {
            Token = token;
            Expression = expression;
        }

        public abstract override string ToString();
    }
}
