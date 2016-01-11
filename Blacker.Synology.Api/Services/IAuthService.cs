using System.Threading.Tasks;
using Blacker.Synology.Api.Models;

namespace Blacker.Synology.Api.Services
{
    public interface IAuthService
    {
        Task<AuthInfo> Authenticate(string account, string password, string sessionName, string optCode = null);
        Task Logout(string sessionName);
    }
}