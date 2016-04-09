using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class TrackerInfo
    {
        [DataMember(Name = "url")]
        public string Url { get; private set; }

        [DataMember(Name = "status")]
        public string Status { get; private set; }

        [DataMember(Name = "update_timer")]
        public int UpdateTimer { get; private set; }

        [DataMember(Name = "seeds")]
        public int Seeds { get; private set; }

        [DataMember(Name = "peers")]
        public int Peers { get; private set; }
    }
}