using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models
{
    [DataContract]
    public class AuthInfo
    {
        [DataMember(Name = "sid")]
        public string SessionId { get; set; }
    }
}
