using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class TaskActionResult
    {
        [DataMember(Name = "error")]
        public int Error { get; private set; }

        [DataMember(Name = "id")]
        public string TaskId { get; private set; }
    }
}
