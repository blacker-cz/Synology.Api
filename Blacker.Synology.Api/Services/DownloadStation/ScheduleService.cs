using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blacker.Synology.Api.Client;
using Blacker.Synology.Api.Models;
using Blacker.Synology.Api.Models.DownloadStation;

namespace Blacker.Synology.Api.Services.DownloadStation
{
    class ScheduleService : IScheduleService
    {
        private readonly IClient _client;
        private readonly AuthInfo _authInfo;

        public ScheduleService(IClient client, AuthInfo authInfo)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (authInfo == null)
                throw new ArgumentNullException(nameof(authInfo));

            _client = client;
            _authInfo = authInfo;
        }

        public Task<ScheduleConfig> GetConfig()
        {
            return _client.GetAsync<ScheduleConfig>(@"DownloadStation/schedule.cgi", new Dictionary<string, object>()
                                                                                              {
                                                                                                  {"api", "SYNO.DownloadStation.Schedule"},
                                                                                                  {"version", 1},
                                                                                                  {"method", "getconfig"},
                                                                                                  {"_sid", _authInfo.SessionId}
                                                                                              });
        }

        public Task SetConfig(ScheduleConfig config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var properties = config.GetChangedProperties();

            if (properties.Count == 0)
                throw new ArgumentException("No config was set to be changed.", nameof(config));

            properties["api"] = "SYNO.DownloadStation.Schedule";
            properties["version"] = 1;
            properties["method"] = "setconfig";
            properties["_sid"] = _authInfo.SessionId;

            return _client.GetAsync(@"DownloadStation/schedule.cgi", properties);
        }
    }
}
