using System.Runtime.Serialization;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [DataContract]
    public enum ErrorDetail
    {
        [EnumMember(Value = "broken_link")]
        BrokenLink,
        [EnumMember(Value = "destination_not_exist")]
        DestinationNotExist,
        [EnumMember(Value = "destination_denied")]
        DestinationDenied,
        [EnumMember(Value = "disk_full")]
        DiskFull,
        [EnumMember(Value = "quota_reached")]
        QuotaReached,
        [EnumMember(Value = "timeout")]
        Timeout,
        [EnumMember(Value = "exceed_max_file_system_size")]
        ExceedMaxFileSystemSize,
        [EnumMember(Value = "exceed_max_destination_size")]
        ExceedMaxDestinationSize,
        [EnumMember(Value = "exceed_max_temp_size")]
        ExceedMaxTempSize,
        [EnumMember(Value = "encrypted_name_too_long")]
        EncryptedNameTooLong,
        [EnumMember(Value = "name_too_long")]
        NameTooLong,
        [EnumMember(Value = "torrent_duplicate")]
        TorrentDuplicate,
        [EnumMember(Value = "file_not_exist")]
        FileNotExist,
        [EnumMember(Value = "required_premium_account")]
        RequiredPremiumAccount,
        [EnumMember(Value = "not_supported_type")]
        NotSupportedType,
        [EnumMember(Value = "try_it_later")]
        TryItLater,
        [EnumMember(Value = "task_encryption")]
        TaskEncryption,
        [EnumMember(Value = "missing_python")]
        MissingPython,
        [EnumMember(Value = "private_video")]
        PrivateVideo,
        [EnumMember(Value = "ftp_encryption_not_supported_type")]
        FtpEncryptionNotSupportedType,
        [EnumMember(Value = "extract_failed")]
        ExtractFailed,
        [EnumMember(Value = "extract_failed_wrong_password")]
        ExtractFailedWrongPassword,
        [EnumMember(Value = "extract_failed_invalid_archive")]
        ExtractFailedInvalidArchive
    }
}