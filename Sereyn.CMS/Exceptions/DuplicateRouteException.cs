using System;

namespace Sereyn.CMS.Exceptions
{
    public class DuplicateRouteException : Exception
    {
        public DuplicateRouteException(string message) : base(message)
        {
        }

        public DuplicateRouteException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
