namespace Avocat.Statements
{
    public class StatementExit : Statement
    {
        public StatementExit(Tokenizer.Token token, Expressions.Expression expression) : base(token, expression)
        { }

        public override string ToString()
        {
            return $"Exit<{Expression}>";
        }
    }
}
