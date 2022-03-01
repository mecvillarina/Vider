using System;
using System.Runtime.Serialization;

namespace Application.Common.Exceptions
{
    [Serializable]
    public class XRPLServerErrorException : Exception
    {
        public XRPLServerErrorException(string message)
                : base(message)
        {
        }

        protected XRPLServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
