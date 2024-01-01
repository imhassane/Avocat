using Avocat.Exceptions;
using Avocat.Expressions;
using Avocat.Statements;
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
                case Tokenizer.ETokenType.VAR:

                    // Consumes the dec keyword
                    Iterator.MoveNext();

                    // Expect the variable name
                    if (Iterator.Current.Type != Tokenizer.ETokenType.IDENTIFIER)
                        throw new InvalidSyntaxException($"Un nom de variable est attendu. {Iterator.Current.FormatPosition()}");

                    var variableName = Iterator.Current;

                    // Consumes the variable name
                    Iterator.MoveNext();

                    // Expect an equal keyword
                    if (Iterator.Current.Type != Tokenizer.ETokenType.EQUAL)
                        throw new InvalidSyntaxException($"'=' attendu. {Iterator.Current.FormatPosition()}");

                    // Consumes the equal keyword
                    Iterator.MoveNext();

                    // Expect an expression
                    var expression = GetExpression();

                    // Expect an end of line or the end of the file
                    if (Iterator.Current.Type != Tokenizer.ETokenType.NEW_LINE)
                        throw new InvalidSyntaxException($"retour à la ligne attendu. {Iterator.Current.FormatPosition()}");

                    stmt = new StatementVar(variableName, expression);
                    break;

                case Tokenizer.ETokenType.EXIT:

                    // Consume the exit keyword
                    Iterator.MoveNext();

                    // Expect an open parenthesis
                    if (Iterator.Current.Type != Tokenizer.ETokenType.OPEN_PARENT)
                        throw new InvalidSyntaxException($"'(' attendu. {Iterator.Current.FormatPosition()}");

                    // Consume the open parenthesis
                    Iterator.MoveNext();

                    // Expect an integer expression
                    var expr = GetExpression();

                    if (!(expr is ExpressionInteger))
                        throw new InvalidSyntaxException($"Un entier attendu. {Iterator.Current.FormatPosition()}");

                    // Expect a closing parenthesis
                    if (Iterator.Current.Type != Tokenizer.ETokenType.CLOSE_PARENT)
                        throw new InvalidSyntaxException($"')' attendu. {Iterator.Current.FormatPosition()}");

                    // Consume the closing parenthesis
                    Iterator.MoveNext();

                    // Expect a new line
                    if (Iterator.Current.Type != Tokenizer.ETokenType.NEW_LINE)
                        throw new InvalidSyntaxException($"retour à la ligne attendu. {Iterator.Current.FormatPosition()}");

                    stmt = new StatementExit(Iterator.Current, expr);
                    break;

                case Tokenizer.ETokenType.NEW_LINE:
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
                    throw new InvalidSyntaxException($"parenthèse fermante sans parenthèse ouvrante. {Iterator.Current.FormatPosition()}");

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
        #endregion
    }
}
