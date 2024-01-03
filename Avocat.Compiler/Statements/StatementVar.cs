using Avocat.Compiler.Expressions;
using Avocat.Compiler.Tokenizer;

namespace Avocat.Statements
{
    public class StatementVar : Statement
    {
        public StatementVar(Token token, Expression expression) : base (token, expression)
        { }

        public override string ToString()
        {
            return $"Var<{Token.Value}, {Expression}>";
        }
    }
}
