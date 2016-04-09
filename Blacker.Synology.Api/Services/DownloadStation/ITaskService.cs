using System.Collections.Generic;
using System.Threading.Tasks;
using Blacker.Synology.Api.Models.DownloadStation;

namespace Blacker.Synology.Api.Services.DownloadStation
{
    public interface ITaskService
    {
        Task<TaskInfoList> ListTasks(int offset = 0, int limit = -1, AdditionalInfoFlags additional = AdditionalInfoFlags.None);
        Task<IEnumerable<TaskInfo>> GetInfo(IEnumerable<string> ids, AdditionalInfoFlags additional = AdditionalInfoFlags.None);
        Task<IEnumerable<TaskActionResult>> DeleteTask(IEnumerable<string> ids, bool forceComplete);
        Task<IEnumerable<TaskActionResult>> PauseTask(IEnumerable<string> ids);
        Task<IEnumerable<TaskActionResult>> ResumeTask(IEnumerable<string> ids);
        Task<IEnumerable<TaskActionResult>> EditTask(IEnumerable<string> ids, string destination);
    }
}