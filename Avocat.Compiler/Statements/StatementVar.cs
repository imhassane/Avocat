using Avocat.Compiler.Expressions;
using Avocat.Compiler.Tokenizer;

namespace Avocat.Statements
{
    public class StatementVar : Statement
    {
        /// <summary>
        /// Variable type if defined
        /// </summary>
        public EType Type { get; private set; }

        public StatementVar(Token token, Expression expression, EType type = EType.NULL) : base (token, expression)
        {
            if (type != EType.NULL) Type = type;
        }

        public override string ToString()
        {
            return $"Var<{Token.Value}, {Expression}>";
        }
    }
}
