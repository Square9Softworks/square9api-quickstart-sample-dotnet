using System;
using System.Runtime.Serialization;

namespace Square9.QuickstartSample.Square9Api.Exceptions
{
    /// <summary>
    /// A Token Expired API exception has occured during the operation.
    /// </summary>
    [Serializable]
    internal class TokenExpiredException : Exception
    {
        public TokenExpiredException()
        {
        }

        public TokenExpiredException(string message) : base(message)
        {
        }

        public TokenExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TokenExpiredException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}