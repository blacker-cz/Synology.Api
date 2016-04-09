using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class Info
    {
        [DataMember(Name = "version")]
        public int Version { get; private set; }

        [DataMember(Name = "is_manager")]
        public bool IsManager { get; private set; }

        [DataMember(Name = "version_string")]
        public string VersionString { get; private set; }
    }
}
