using System;

namespace MovieBookingSystem.Application.Common.Exceptions
{
    public class ApplicationException : Exception
    {
        public ApplicationException(string message)
            : base(message)
        {
        }

        public ApplicationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
