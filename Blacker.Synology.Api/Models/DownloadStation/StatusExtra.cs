using System.Runtime.Serialization;
using Blacker.Synology.Api.Helpers;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public class StatusExtra
    {
        [DataMember(Name = "error_detail")]
        private string _errorDetail;

        public ErrorDetail ErrorDetail => EnumHelper.GetValueFromEnumMember<ErrorDetail>(_errorDetail);

        [DataMember(Name = "unzip_progress")]
        public int UnzipProgress { get; private set; }
    }
}