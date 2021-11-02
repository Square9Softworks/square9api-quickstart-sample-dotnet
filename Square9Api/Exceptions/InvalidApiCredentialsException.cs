using System;
using System.Runtime.Serialization;

namespace Square9.QuickstartSample.Square9Api.Exceptions
{
    /// <summary>
    /// Credentials provided for a Square 9 API request were not accepted as valid for the requested operation.
    /// </summary>
    [Serializable]
    internal class InvalidApiCredentialsException : Exception
    {
        public InvalidApiCredentialsException()
        {
        }

        public InvalidApiCredentialsException(string message) : base(message)
        {
        }

        public InvalidApiCredentialsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidApiCredentialsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}