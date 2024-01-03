using Avocat.Compiler.Expressions;
using Avocat.Compiler.Tokenizer;

namespace Avocat.Statements
{
    public abstract class Statement
    {
        public Token Token { get; private set; }

        public Expression Expression { get; private set; }

        public Statement(Token token, Expression expression)
        {
            Token = token;
            Expression = expression;
        }

        public abstract override string ToString();
    }
}
