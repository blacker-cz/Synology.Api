using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public enum Priority
    {
        [EnumMember(Value = "normal")]
        Normal,
        [EnumMember(Value = "skip")]
        Skip,
        [EnumMember(Value = "low")]
        Low,
        [EnumMember(Value = "high")]
        High,
        [EnumMember(Value = "auto")]
        Auto
    }
}