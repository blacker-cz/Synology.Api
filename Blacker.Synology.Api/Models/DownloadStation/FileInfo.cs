using System;
using System.Runtime.Serialization;
using Blacker.Synology.Api.Helpers;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class FileInfo
    {
        [DataMember(Name = "filename")]
        public string Name { get; private set; }

        [DataMember(Name = "size")]
        private string _size;

        public long Size => Convert.ToInt64(_size);

        [DataMember(Name = "size_downloaded")]
        private string _sizeDownloaded;

        public long SizeDownloaded => Convert.ToInt64(_sizeDownloaded);

        [DataMember(Name = "priority")]
        private string _priority;

        public Priority Status => EnumHelper.GetValueFromEnumMember<Priority>(_priority);
    }
}