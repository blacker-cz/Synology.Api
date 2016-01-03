using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models
{
    [DataContract]
    public class ErrorInfo
    {
        [DataMember(Name = "code")]
        public int Code { get; set; }
    }
}