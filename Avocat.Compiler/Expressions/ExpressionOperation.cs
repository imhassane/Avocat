namespace Avocat.Expressions
{
    public class ExpressionOperation : Expression
    {
        public Expression Left { get; set; }
        public Expression Right { get; set; }

        public ExpressionOperation(Tokenizer.Token token) : base(token)
        { }

        public override string ToString()
        {
            return $"Oper<{Left} {Token} {Right}>";
        }
    }
}
