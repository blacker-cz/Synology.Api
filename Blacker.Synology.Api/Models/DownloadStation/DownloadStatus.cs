using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public enum DownloadStatus
    {
        [EnumMember(Value = "waiting")]
        Waiting,
        [EnumMember(Value = "downloading")]
        Downloading,
        [EnumMember(Value = "paused")]
        Paused,
        [EnumMember(Value = "finishing")]
        Finishing,
        [EnumMember(Value = "finished")]
        Finished,
        [EnumMember(Value = "hash_checking")]
        HashChecking,
        [EnumMember(Value = "seeding")]
        Seeding,
        [EnumMember(Value = "filehosting_waiting")]
        FilehostingWaiting,
        [EnumMember(Value = "extracting")]
        Extracting,
        [EnumMember(Value = "error")]
        Error
    }
}