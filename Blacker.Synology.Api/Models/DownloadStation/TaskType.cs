using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public enum TaskType
    {
        [EnumMember(Value = "bt")]
        BT,
        [EnumMember(Value = "nzb")]
        NZB,
        [EnumMember(Value = "http")]
        HTTP,
        [EnumMember(Value = "ftp")]
        FTP,
        [EnumMember(Value = "emule")]
        eMule
    }
}