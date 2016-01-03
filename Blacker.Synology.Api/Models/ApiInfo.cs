using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models
{
    [DataContract]
    public class ApiInfo
    {
        [DataMember(Name = "maxVersion")]
        public int MaxVersion { get; set; }

        [DataMember(Name = "minVersion")]
        public int MinVersion { get; set; }

        [DataMember(Name = "path")]
        public string Path { get; set; }
    }
}
