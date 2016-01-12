using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class ScheduleConfig : ResponseConfigBase
    {
        [DataMember(Name = "enabled")]
        private bool? _enabled;
        [DataMember(Name = "emule_enabled")]
        private bool? _eMuleEnabled;

        public bool Enabled
        {
            get { return _enabled ?? false; }
            set { _enabled = value; }
        }

        public bool EMuleEnabled
        {
            get { return _eMuleEnabled ?? false; }
            set { _eMuleEnabled = value; }
        }
    }
}
