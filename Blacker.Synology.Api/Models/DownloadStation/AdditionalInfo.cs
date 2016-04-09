using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class AdditionalInfo
    {
        [DataMember(Name = "detail")]
        public Detail Detail { get; private set; }

        [DataMember(Name = "transfer")]
        public TransferInfo Transfer { get; private set; }

        [DataMember(Name = "file")]
        public IEnumerable<FileInfo> Files { get; private set; }

        [DataMember(Name = "tracker")]
        public IEnumerable<TrackerInfo> Trackers { get; private set; }

        [DataMember(Name = "peer")]
        public IEnumerable<PeerInfo> Peers { get; private set; }
    }
}