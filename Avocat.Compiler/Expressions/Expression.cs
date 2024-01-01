using Avocat.Tokenizer;

namespace Avocat.Expressions
{
    /// <summary>
    /// Represents an expression
    /// </summary>
    public class Expression
    {
        /// <summary>
        /// Token of the expression
        /// </summary>
        public Token Token { get; private set; }

        public Expression(Token token)
        {
            Token = token;
        }

        public override string ToString()
        {
            return $"Expr<{Token.Value}>";
        }
    }
}
