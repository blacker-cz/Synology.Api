using System.Collections.Generic;
using System.Threading.Tasks;
using Blacker.Synology.Api.Models;

namespace Blacker.Synology.Api.Services
{
    public interface IApiInfoService
    {
        Task<IDictionary<string, ApiInfo>> ListAvailableApis();
        Task<IDictionary<string, ApiInfo>> ListAvailableApis(IEnumerable<string> apis);
    }
}