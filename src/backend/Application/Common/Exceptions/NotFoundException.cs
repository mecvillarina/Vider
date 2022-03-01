﻿using System;
using System.Runtime.Serialization;

namespace Application.Common.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException()
        {
        }

        public NotFoundException(string message)
                : base(message)
        {
        }

        public NotFoundException(string message, Exception innerException)
                : base(message, innerException)
        {
        }

        public NotFoundException(string name, object key)
                : base($"Entity \"{name}\" ({key}) was not found.")
        {
        }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}