using Avocat.Compiler.Expressions;
using Avocat.Compiler.Tokenizer;

namespace Avocat.Statements
{
    public class StatementExit : Statement
    {
        public StatementExit(Token token, Expression expression) : base(token, expression)
        { }

        public override string ToString()
        {
            return $"Exit<{Expression}>";
        }
    }
}
