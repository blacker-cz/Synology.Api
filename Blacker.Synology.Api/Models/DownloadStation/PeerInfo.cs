using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class PeerInfo
    {
        [DataMember(Name = "address")]
        public string Address { get; private set; }

        [DataMember(Name = "agent")]
        public string Agent { get; private set; }

        [DataMember(Name = "progress")]
        public float Progress { get; private set; }

        [DataMember(Name = "speed_download")]
        public int SpeedDownload { get; private set; }

        [DataMember(Name = "speed_upload")]
        public int SpeedUpload { get; private set; }
    }
}