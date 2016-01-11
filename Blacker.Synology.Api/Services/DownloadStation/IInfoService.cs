using System.Threading.Tasks;
using Blacker.Synology.Api.Models.DownloadStation;

namespace Blacker.Synology.Api.Services.DownloadStation
{
    public interface IInfoService
    {
        Task<Config> GetConfig();
        Task<Info> GetInfo();
        Task SetServerConfig(Config config);
    }
}