using System;

namespace Avocat.Exceptions
{
    public class InvalidOperationException : Exception
    {
        public InvalidOperationException(string message) : base($"Opération invalide. {message}")
        { }
    }
}
