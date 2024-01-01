namespace Avocat.Statements
{
    public class StatementVar : Statement
    {
        public StatementVar(Tokenizer.Token token, Expressions.Expression expression) : base (token, expression)
        { }

        public override string ToString()
        {
            return $"Var<{Token.Value}, {Expression}>";
        }
    }
}
