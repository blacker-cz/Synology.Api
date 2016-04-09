using System;
using System.Runtime.Serialization;
using Blacker.Synology.Api.Helpers;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class TaskInfo
    {
        [DataMember(Name = "id")]
        public string Id { get; private set; }

        [DataMember(Name = "type")]
        private string _type;

        public TaskType Type => EnumHelper.GetValueFromEnumMember<TaskType>(_type);

        [DataMember(Name = "username")]
        public string Username { get; private set; }

        [DataMember(Name = "title")]
        public string Title { get; private set; }

        [DataMember(Name = "size")]
        private string _size;

        public long Size => Convert.ToInt64(_size);

        [DataMember(Name = "status")]
        private string _status;

        public DownloadStatus Status => EnumHelper.GetValueFromEnumMember<DownloadStatus>(_status);

        [DataMember(Name = "status_extra")]
        public StatusExtra StatusExtra { get; private set; }

        [DataMember(Name = "additional")]
        public AdditionalInfo Additional { get; private set; }
    }
}