﻿using System;

namespace Avocat.Tokenizer
{
    /// <summary>
    /// Represents a token
    /// </summary>
    public struct Token
    {
        #region Properties
        /// <summary>
        /// Type of the token
        /// </summary>
        public ETokenType Type;

        /// <summary>
        /// Value of the token
        /// </summary>
        public string Value;

        /// <summary>
        /// Position of the token in the code
        /// </summary>
        public ushort Line;

        /// <summary>
        /// Position of the token in the code
        /// </summary>
        public ushort Position;
        #endregion

        #region Constructor
        public Token(ETokenType type, string value = "", ushort line = 0, ushort position = 0)
        {
            Type = type;
            Value = value;
            Line = line;
            Position = position;
        }
        #endregion

        #region Overrides
        public string FormatPosition()
        {
            return $"(ligne: {Line}, position: {Position});";
        }
        public override string ToString()
        {
            if (Value == string.Empty) return $"Token<type={Type}, line={Line}, position={Position}>";
            return $"Token<type={Type}, value={Value}, line={Line}, position={Position}>";
        }
        #endregion
    }

    /// <summary>
    /// Represents the types of tokens
    /// </summary>
    public enum ETokenType
    {
        VAR,
        EXIT,
        EQUAL,
        IDENTIFIER,
        NEW_LINE,
        STRING,
        INTEGER,
        FLOAT,
        PLUS,
        MINUS,
        MULTIPLY,
        OPEN_PARENT,
        CLOSE_PARENT,
        COMMENT
    }
}
