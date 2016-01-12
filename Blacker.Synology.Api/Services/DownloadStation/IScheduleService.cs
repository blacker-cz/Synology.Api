using System.Threading.Tasks;
using Blacker.Synology.Api.Models.DownloadStation;

namespace Blacker.Synology.Api.Services.DownloadStation
{
    public interface IScheduleService
    {
        Task<ScheduleConfig> GetConfig();
        Task SetConfig(ScheduleConfig config);
    }
}