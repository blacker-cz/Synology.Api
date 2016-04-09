using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class TaskInfoList
    {
        [DataMember(Name = "total")]
        public int Total { get; private set; }

        [DataMember(Name = "offset")]
        public int Offset { get; private set; }

        [DataMember(Name = "tasks")]
        public IEnumerable<TaskInfo> Tasks { get; private set; }
    }
}