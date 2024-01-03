using Avocat.Exceptions;
using Avocat.Expressions;
using Avocat.Statements;
using Avocat.Tokenizer;
using System.Collections.Generic;

namespace Avocat.Parser
{
    /// <summary>
    /// Class that parses the code
    /// </summary>
    public class Parser
    {
        #region Properties
        /// <summary>
        /// Tokens to parse
        /// </summary>
        private readonly IEnumerable<Tokenizer.Token> Tokens;

        /// <summary>
        /// Iterator
        /// </summary>
        private readonly IEnumerator<Tokenizer.Token> Iterator;
        #endregion

        #region Constructor
        public Parser(IEnumerable<Tokenizer.Token> tokens)
        {
            Tokens = tokens;
            Iterator = Tokens.GetEnumerator();
        }
        #endregion

        #region Methods

        public IEnumerable<Statement> Parse()
        {
            while (Iterator.MoveNext())
                yield return GetStatement();
        }

        private Statement GetStatement()
        {
            Statement stmt;

            switch (Iterator.Current.Type)
            {
                case ETokenType.VAR:

                    // Consumes the dec keyword
                    Iterator.MoveNext();

                    // Expect a variable name
                    var variableName = ExpectAndConsumeToken(ETokenType.IDENTIFIER);

                    // Expect an equal keyword
                    ExpectAndConsumeToken(ETokenType.EQUAL);

                    // Expect an expression
                    var expression = GetExpression();

                    // Expect an end of line or the end of the file
                    ExpectToken(ETokenType.NEW_LINE);

                    stmt = new StatementVar(variableName, expression);
                    break;

                case ETokenType.EXIT:

                    // Consume the exit keyword
                    Iterator.MoveNext();

                    // Expect an open parenthesis
                    ExpectAndConsumeToken(ETokenType.OPEN_PARENT);

                    // Expect an integer expression
                    var expr = GetExpression();

                    if (!(expr is ExpressionInteger))
                        throw new InvalidSyntaxException($"Un entier attendu. {expr.Token.FormatPosition()}");

                    // Expect a closing parenthesis
                    ExpectAndConsumeToken(ETokenType.CLOSE_PARENT);

                    // Expect a new line
                    ExpectToken(ETokenType.NEW_LINE);

                    stmt = new StatementExit(Iterator.Current, expr);
                    break;

                case ETokenType.NEW_LINE:
                    stmt = new StatementEOF(Iterator.Current, new ExpressionEOF(Iterator.Current));
                    break;

                default:
                    throw new InvalidSyntaxException(Iterator.Current.FormatPosition());
            }

            return stmt;
        }

        /// <summary>
        /// Parses an expression
        /// </summary>
        /// <returns>An expression</returns>
        private Expression GetExpression()
        {
            Expression expr;
            switch (Iterator.Current.Type)
            {
                case Tokenizer.ETokenType.INTEGER:
                    ExpressionNumber left = new ExpressionInteger(Iterator.Current);

                    // Try to to build an operation expression
                    var operExpr = GetOperationExpression(left);

                    if (operExpr is null)
                        expr = left;
                    else
                        expr = operExpr;

                    break;

                case Tokenizer.ETokenType.FLOAT:
                    left = new ExpressionFloat(Iterator.Current);

                    // Try to to build an operation expression
                    operExpr = GetOperationExpression(left);

                    if (operExpr is null)
                        expr = left;
                    else
                        expr = operExpr;

                    break;

                case Tokenizer.ETokenType.STRING:
                    expr = new ExpressionString(Iterator.Current);

                    // Consumes the string
                    Iterator.MoveNext();

                    break;

                case Tokenizer.ETokenType.OPEN_PARENT:

                    // Consume the parenthesis
                    Iterator.MoveNext();

                    // Get the expression
                    expr = GetExpression();

                    // Expects a closing parenthesis
                    if (Iterator.Current.Type != Tokenizer.ETokenType.CLOSE_PARENT)
                        throw new InvalidSyntaxException($"')' attendu. {Iterator.Current.FormatPosition()}");

                    // Consume the closing parameter
                    Iterator.MoveNext();

                    break;

                case Tokenizer.ETokenType.CLOSE_PARENT:
                    throw new InvalidSyntaxException($"{Iterator.Current.FormatPosition()}");

                default:
                    throw new InvalidSyntaxException(Iterator.Current.FormatPosition());
            }

            return expr;
        }

        private Expression GetOperationExpression(Expression left)
        {
            if (!(left is ExpressionNumber))
                return null;

            // Consuming the expression
            Iterator.MoveNext();

            switch (Iterator.Current.Type)
            {
                case Tokenizer.ETokenType.PLUS:
                case Tokenizer.ETokenType.MINUS:
                case Tokenizer.ETokenType.MULTIPLY:
                    var operationToken = Iterator.Current;
                    
                    // Consuming the token
                    var canMove = Iterator.MoveNext();

                    if (!canMove)
                        throw new InvalidOperationException($"Une opération arithmétique nécessite deux expressions arithmétiques. {Iterator.Current.FormatPosition()}");
                    
                    var right = GetExpression();
                    if (!(right is ExpressionNumber))
                        throw new InvalidOperationException($"Un nombre est attendu. {Iterator.Current.FormatPosition()}");
                    
                    var operationExpression = new ExpressionOperation(operationToken)
                    {
                        // Assigning the left hand part of an operation
                        Left = left,
                        // Assiging the right hand part of an operation
                        Right = right
                    };

                    return operationExpression;

                default:
                    return null;
            }
        }

        /// <summary>
        /// Gets the next token
        /// </summary>
        /// <param name="expected">The expected token</param>
        /// <returns>Next token</returns>
        private Token ExpectToken(ETokenType expected)
        {
            if(Iterator.Current.Type != expected)
            {
                var token = string.Empty;

                switch (expected)
                {
                    case ETokenType.CLOSE_PARENT:
                        token = Keywords.CLOSE_PARAM.ToString();
                        break;
                    case ETokenType.EQUAL:
                        token = Keywords.EQUAL.ToString();
                        break;
                    case ETokenType.SINGLE_QUOTE:
                        token = Keywords.SINGLE_QUOTE.ToString();
                        break;
                    case ETokenType.OPEN_PARENT:
                        token = Keywords.OPEN_PARAM.ToString();
                        break;
                    case ETokenType.NEW_LINE:
                        token = "Un retour à la ligne est";
                        break;
                    default:
                        break;
                }

                throw new InvalidSyntaxException($"{(token.Length > 0 ? token : "Un mot-clé différent est")} attendu. {Iterator.Current.FormatPosition()}");
            }

            return Iterator.Current;
        }

        /// <summary>
        /// Gets and consumes the next token
        /// </summary>
        /// <param name="expected">The expected token</param>
        /// <returns>Next token</returns>
        private Token ExpectAndConsumeToken(ETokenType expected)
        {
            var token = ExpectToken(expected);

            Iterator.MoveNext();

            return token;
        }
        #endregion
    }
}
