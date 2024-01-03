using Avocat.Exceptions;
using System.Collections.Generic;
using System.Text;

namespace Avocat.Compiler.Tokenizer
{
    /// <summary>
    /// Class that handles the tokenization of the source code
    /// </summary>
    public class Tokenizer
    {
        #region Properties
        /// <summary>
        /// Index
        /// </summary>
        private uint Index = 0;

        private readonly string Source = string.Empty;

        /// <summary>
        /// Current line in the code
        /// </summary>
        private ushort Line = 1;

        /// <summary>
        /// Current position in the code
        /// </summary>
        private ushort Position = 1;
        #endregion

        #region Constructor
        public Tokenizer(string source)
        {
            Source = source + "\n";
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get tokens from code source
        /// </summary>
        /// <param name="source">Code source</param>
        /// <returns>The tokens</returns>
        public IEnumerable<Token> GetTokens()
        {
            var buffer = new StringBuilder();

            while (Peek().HasValue)
            {
                buffer.Clear();

                if (char.IsLetter(Peek().Value))
                {
                    buffer.Append(Consume());
                    ConsumePosition();

                    while (Peek().HasValue && char.IsLetterOrDigit(Peek().Value))
                    {
                        buffer.Append(Consume());
                        ConsumePosition();
                    }

                    var content = buffer.ToString();

                    if (Keywords.EXIT.Equals(content))
                    {
                        yield return new Token(ETokenType.EXIT, line: PeekLine(), position: PeekPosition());
                    }
                    else if (Keywords.VAR.Equals(content))
                    {
                        yield return new Token(ETokenType.VAR, line: PeekLine(), position: PeekPosition());
                    }
                    else if (Keywords.TYPE_CHAR.Equals(content))
                    {
                        yield return new Token(ETokenType.TYPE_CHAR, line: PeekLine(), position: PeekPosition());
                    }
                    else if (Keywords.TYPE_FLOAT.Equals(content))
                    {
                        yield return new Token(ETokenType.TYPE_FLOAT, line: PeekLine(), position: PeekPosition());
                    }
                    else if (Keywords.TYPE_INTEGER.Equals(content))
                    {
                        yield return new Token(ETokenType.TYPE_INTEGER, line: PeekLine(), position: PeekPosition());
                    }
                    else if (Keywords.TYPE_STRING.Equals(content))
                    {
                        yield return new Token(ETokenType.TYPE_STRING, line: PeekLine(), position: PeekPosition());
                    }
                    else
                    {
                        yield return new Token(ETokenType.IDENTIFIER, content, PeekLine(), PeekPosition());
                    }
                }

                else if (char.IsDigit(Peek().Value))
                {
                    buffer.Append(Consume());
                    ConsumePosition();

                    while (Peek().HasValue && (char.IsDigit(Peek().Value) || Peek().Value == '.'))
                    {
                        buffer.Append(Consume());
                        ConsumePosition();
                    }

                    var content = buffer.ToString();
                    ETokenType type = ETokenType.INTEGER;
                    if (content.Contains(".")) type = ETokenType.FLOAT;

                    yield return new Token(type, content, PeekLine(), PeekPosition());
                }

                else if (Peek().Value == Keywords.OPEN_PARAM)
                {
                    ConsumePosition();
                    Consume();
                    yield return new Token(ETokenType.OPEN_PARENT, line: PeekLine(), position: PeekPosition());
                }

                else if (Peek().Value == Keywords.CLOSE_PARAM)
                {
                    ConsumePosition();
                    Consume();
                    yield return new Token(ETokenType.CLOSE_PARENT, line: PeekLine(), position: PeekPosition());
                }

                else if (Peek().Value == Keywords.EQUAL)
                {
                    ConsumePosition();
                    Consume();
                    yield return new Token(ETokenType.EQUAL, line: PeekLine(), position: PeekPosition());
                }

                else if (Peek().Value == Keywords.QUOTE)
                {
                    ConsumePosition();

                    // Consuming the opening quote
                    Consume();

                    bool isEscaped = false;
                    while (Peek().HasValue && (isEscaped || Peek().Value != Keywords.QUOTE))
                    {
                        if (Peek().Value == '\n')
                            throw new InvalidSyntaxException($"'\"' attendu. (ligne: {PeekLine()}, position: {PeekPosition()})");

                        if (Peek().Value == '\\')
                            isEscaped = true;
                        else
                            isEscaped = false;

                        buffer.Append(Consume());
                        ConsumePosition();
                    }

                    // Handling the case where there is no closing quote.
                    // There should always be a value returned by the peek function if a string is corrects
                    if (!Peek().HasValue)
                        throw new InvalidSyntaxException($"'\"' attendu. (ligne: {PeekLine()}, position: {PeekPosition()})");

                    // Consuming the closing quote.
                    Consume();
                    ConsumePosition();

                    yield return new Token(ETokenType.STRING, buffer.ToString(), PeekLine(), PeekPosition());
                }

                else if (Peek().Value == Keywords.PLUS)
                {
                    Consume();
                    yield return new Token(ETokenType.PLUS, line: ConsumeLine(), position: PeekPosition());
                }

                else if (Peek().Value == Keywords.MINUS)
                {
                    Consume();
                    yield return new Token(ETokenType.MINUS, line: ConsumeLine(), position: PeekPosition());
                }

                else if (Peek().Value == Keywords.MULTIPLY)
                {
                    Consume();
                    yield return new Token(ETokenType.MULTIPLY, line: ConsumeLine(), position: PeekPosition());
                }

                else if (Peek().Value == Keywords.TWO_POINTS)
                {
                    Consume();
                    yield return new Token(ETokenType.TWO_POINTS, line: PeekLine(), position: ConsumePosition());
                }

                else if (Peek().Value == Keywords.SINGLE_QUOTE)
                {
                    Consume();
                    ConsumePosition();
                    if (Peek().HasValue && Peek(1).HasValue && (char.IsLetterOrDigit(Peek().Value) || char.IsWhiteSpace(Peek().Value)))
                    {
                        var caractere = Peek().Value;

                        // Consume the character
                        Consume();
                        ConsumePosition();

                        // Consume the closing quote
                        Consume();
                        ConsumePosition();
                        yield return new Token(ETokenType.CHAR, caractere.ToString(), line: PeekLine(), position: ConsumePosition());
                    } else
                    {
                        if (!Peek().HasValue)
                            throw new InvalidSyntaxException($"Un caractère est attendu. (ligne: {PeekLine()}, position: {PeekPosition()})");
                        else
                            throw new InvalidSyntaxException($"''' est attendu. (ligne: {PeekLine()}, position: {PeekPosition()})");
                    }
                }

                else if (Peek().Value == Keywords.COMMENT)
                {
                    ConsumeLine();
                    while (Peek().HasValue && Peek().Value != '\n')
                        Consume();
                    // Consume the \n character if it is present
                    if (Peek(1).HasValue) Consume();
                }

                else if (Peek().Value == Keywords.NEW_LINE)
                {
                    Consume();
                    yield return new Token(ETokenType.NEW_LINE, line: ConsumeLine(), position: PeekPosition());
                }

                else if (char.IsWhiteSpace(Peek().Value))
                {
                    ConsumePosition();
                    Consume();
                }

                else
                {
                    throw new InvalidSyntaxException($"(ligne: {PeekLine()}, position: {PeekPosition()})");
                }

            }
        }

        private char? Peek(uint offset = 0)
        {
            if (Index + offset >= Source.Length) return null;
            return Source[(int)(Index + offset)];
        }

        private char Consume()
        {
            return Source[(int)Index++];
        }

        /// <summary>
        /// Returns the current line
        /// </summary>
        private ushort PeekLine()
        {
            return Line;
        }

        /// <summary>
        /// Returns the current position
        /// </summary>
        private ushort PeekPosition()
        {
            return Position;
        }

        /// <summary>
        /// Returns the current line and increase the line
        /// </summary>
        private ushort ConsumeLine()
        {
            Position = 0;
            return Line++;
        }

        /// <summary>
        /// Returns the current position and increase the position
        /// </summary>
        private ushort ConsumePosition()
        {
            return Position++;
        }
        #endregion
    }
}
