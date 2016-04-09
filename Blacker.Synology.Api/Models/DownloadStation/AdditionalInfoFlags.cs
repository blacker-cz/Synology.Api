using System;

namespace Blacker.Synology.Api.Models.DownloadStation
{
    [Flags]
    public enum AdditionalInfoFlags
    {
        None = 0,
        Detail = 1 << 0,
        Transfer = 1 << 1,
        File = 1 << 2,
        Tracker = 1 << 3,
        Peer = 1 << 4
    }
}