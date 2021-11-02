using System;
using System.Runtime.Serialization;

namespace Square9.QuickstartSample.Square9Api.Exceptions
{
    /// <summary>
    /// All licenses are in use, can not get a new license or activate the provided license token.
    /// </summary>
    [Serializable]
    internal class AllLicensesInUseException : Exception
    {
        public AllLicensesInUseException()
        {
        }

        public AllLicensesInUseException(string message) : base(message)
        {
        }

        public AllLicensesInUseException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected AllLicensesInUseException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}