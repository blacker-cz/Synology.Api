using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blacker.Synology.Api.Client;
using Blacker.Synology.Api.Models;
using Blacker.Synology.Api.Models.DownloadStation;

namespace Blacker.Synology.Api.Services.DownloadStation
{
    class TaskService : ITaskService
    {
        private readonly IClient _client;
        private readonly AuthInfo _authInfo;

        public TaskService(IClient client, AuthInfo authInfo)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (authInfo == null)
                throw new ArgumentNullException(nameof(authInfo));

            _client = client;
            _authInfo = authInfo;
        }

        public Task<TaskInfoList> ListTasks(int offset = 0, int limit = -1, AdditionalInfoFlags additional = AdditionalInfoFlags.None)
        {
            var parameters = new Dictionary<string, object>()
                             {
                                 {"api", "SYNO.DownloadStation.Task"},
                                 {"version", 1},
                                 {"method", "list"},
                                 {"offset", offset},
                                 {"limit", limit},
                                 {"_sid", _authInfo.SessionId}
                             };

            if (additional != AdditionalInfoFlags.None)
            {
                parameters["additional"] = additional.ToString().ToLowerInvariant().Replace(" ", "");
            }

            return _client.GetAsync<TaskInfoList>(@"DownloadStation/task.cgi", parameters);
        }

        public Task<TaskInfoList> GetInfo(IEnumerable<string> ids, AdditionalInfoFlags additional = AdditionalInfoFlags.None)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var stringIds = String.Join(",", ids);

            if (String.IsNullOrWhiteSpace(stringIds))
                throw new ArgumentException("Collection is empty.", nameof(ids));

            var parameters = new Dictionary<string, object>()
                             {
                                 {"api", "SYNO.DownloadStation.Task"},
                                 {"version", 1},
                                 {"method", "getinfo"},
                                 {"id", stringIds},
                                 {"_sid", _authInfo.SessionId}
                             };

            if (additional != AdditionalInfoFlags.None)
            {
                parameters["additional"] = additional.ToString().ToLowerInvariant().Replace(" ", "");
            }

            return _client.GetAsync<TaskInfoList>(@"DownloadStation/task.cgi", parameters);
        }

        public Task CreateTask(IEnumerable<string> uris, CreateTaskConfig config = null)
        {
            if (uris == null)
                throw new ArgumentNullException(nameof(uris));

            var stringUris = String.Join(",", uris);

            if (String.IsNullOrWhiteSpace(stringUris))
                throw new ArgumentException("Collection is empty.", nameof(uris));

            var parameters = new Dictionary<string, object>()
                             {
                                 {"api", "SYNO.DownloadStation.Task"},
                                 {"version", 3},
                                 {"method", "create"},
                                 {"uri", stringUris},
                                 {"_sid", _authInfo.SessionId}
                             };

            config?.GetChangedProperties().Where(prop => prop.Key != null).ToList().ForEach(prop => parameters[prop.Key] = prop.Value);

            return _client.PostAsync(@"DownloadStation/task.cgi", parameters);
        }

        public Task<IEnumerable<TaskActionResult>> DeleteTask(IEnumerable<string> ids, bool forceComplete)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var stringIds = String.Join(",", ids);

            if (String.IsNullOrWhiteSpace(stringIds))
                throw new ArgumentException("Collection is empty.", nameof(ids));

            var parameters = new Dictionary<string, object>()
                             {
                                 {"api", "SYNO.DownloadStation.Task"},
                                 {"version", 1},
                                 {"method", "delete"},
                                 {"id", stringIds},
                                 {"force_complete", forceComplete},
                                 {"_sid", _authInfo.SessionId}
                             };

            return _client.GetAsync<IEnumerable<TaskActionResult>>(@"DownloadStation/task.cgi", parameters);
        }

        public Task<IEnumerable<TaskActionResult>> PauseTask(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var stringIds = String.Join(",", ids);

            if (String.IsNullOrWhiteSpace(stringIds))
                throw new ArgumentException("Collection is empty.", nameof(ids));

            var parameters = new Dictionary<string, object>()
                             {
                                 {"api", "SYNO.DownloadStation.Task"},
                                 {"version", 1},
                                 {"method", "pause"},
                                 {"id", stringIds},
                                 {"_sid", _authInfo.SessionId}
                             };

            return _client.GetAsync<IEnumerable<TaskActionResult>>(@"DownloadStation/task.cgi", parameters);
        }

        public Task<IEnumerable<TaskActionResult>> ResumeTask(IEnumerable<string> ids)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            var stringIds = String.Join(",", ids);

            if (String.IsNullOrWhiteSpace(stringIds))
                throw new ArgumentException("Collection is empty.", nameof(ids));

            var parameters = new Dictionary<string, object>()
                             {
                                 {"api", "SYNO.DownloadStation.Task"},
                                 {"version", 1},
                                 {"method", "resume"},
                                 {"id", stringIds},
                                 {"_sid", _authInfo.SessionId}
                             };

            return _client.GetAsync<IEnumerable<TaskActionResult>>(@"DownloadStation/task.cgi", parameters);
        }

        public Task<IEnumerable<TaskActionResult>> EditTask(IEnumerable<string> ids, string destination)
        {
            if (ids == null)
                throw new ArgumentNullException(nameof(ids));

            if (String.IsNullOrWhiteSpace(destination))
                throw new ArgumentException("Argument is null or whitespace", nameof(destination));

            var stringIds = String.Join(",", ids);

            if (String.IsNullOrWhiteSpace(stringIds))
                throw new ArgumentException("Collection is empty.", nameof(ids));

            var parameters = new Dictionary<string, object>()
                             {
                                 {"api", "SYNO.DownloadStation.Task"},
                                 {"version", 2},
                                 {"method", "edit"},
                                 {"id", stringIds},
                                 {"destination", destination},
                                 {"_sid", _authInfo.SessionId}
                             };

            return _client.GetAsync<IEnumerable<TaskActionResult>>(@"DownloadStation/task.cgi", parameters);
        }
    }
}
