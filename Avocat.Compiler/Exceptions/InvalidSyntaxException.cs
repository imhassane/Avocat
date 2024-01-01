using System;

namespace Avocat.Exceptions
{
    public class InvalidSyntaxException : Exception
    {
        public InvalidSyntaxException(string message) : base($"Syntaxe invalide. {message}")
        { }
    }
}
