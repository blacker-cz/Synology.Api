using System;
using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class TransferInfo
    {
        [DataMember(Name = "size_downloaded")]
        private string _sizeDownloaded;

        public long SizeDownloaded => Convert.ToInt64(_sizeDownloaded);

        [DataMember(Name = "size_uploaded")]
        private string _sizeUploaded;

        public long SizeUploaded => Convert.ToInt64(_sizeUploaded);

        [DataMember(Name = "speed_download")]
        public int SpeedDownload { get; private set; }

        [DataMember(Name = "speed_upload")]
        public int SpeedUpload { get; private set; }
    }
}