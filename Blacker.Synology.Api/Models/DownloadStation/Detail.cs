using System;
using System.Runtime.Serialization;
using Blacker.Synology.Api.Helpers;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class Detail
    {
        [DataMember(Name = "destination")]
        public string Destination { get; private set; }

        [DataMember(Name = "uri")]
        public string Uri { get; private set; }

        [DataMember(Name = "create_time")]
        private string _created;

        public DateTime Created => DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(_created ?? String.Empty)).DateTime;

        [DataMember(Name = "priority")]
        private string _priority;

        public Priority Priority
        {
            get { return EnumHelper.GetValueFromEnumMember<Priority>(_priority); }
        }

        [DataMember(Name = "total_peers")]
        public int TotalPeers { get; private set; }

        [DataMember(Name = "connected_seeders")]
        public int ConnectedSeeders { get; private set; }

        [DataMember(Name = "connected_leechers")]
        public int ConnectedLeechers { get; private set; }
    }
}