using System;

namespace Domain.Core.Exceptions
{
    public class NameAlreadyExistsException : Exception
    {
        public NameAlreadyExistsException()
        {
        }

        public NameAlreadyExistsException(string? message) : base(message)
        {
        }

        public NameAlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}