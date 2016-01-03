using System;
using System.Runtime.Serialization;
using Blacker.Synology.Api.Models;

namespace Blacker.Synology.Api.Client
{
    [Serializable]
    public class ClientException : Exception
    {
        public ErrorInfo Error { get; set; }

        public ClientException()
        {
        }

        public ClientException(string message) : base(message)
        {
        }

        public ClientException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ClientException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
