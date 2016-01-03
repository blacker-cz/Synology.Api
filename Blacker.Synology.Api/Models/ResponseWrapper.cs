using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models
{
    [DataContract]
    internal class ResponseWrapper<T> where T : class
    {
        [DataMember(Name = "error")]
        public ErrorInfo Error { get; set; }

        [DataMember(Name = "data")]
        public T Data { get; set; }

        [DataMember(Name = "success")]
        public bool Success { get; set; }
    }
}
