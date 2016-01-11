using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blacker.Synology.Api.Client;
using Blacker.Synology.Api.Models;

namespace Blacker.Synology.Api.Services.DownloadStation
{
    class InfoService : IInfoService
    {
        private readonly IClient _client;
        private readonly AuthInfo _authInfo;

        public InfoService(IClient client, AuthInfo authInfo)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            if (authInfo == null)
                throw new ArgumentNullException(nameof(authInfo));

            _client = client;
            _authInfo = authInfo;
        }

        public Task<Models.DownloadStation.Info> GetInfo()
        {
            return _client.GetAsync<Models.DownloadStation.Info>(@"DownloadStation/info.cgi", new Dictionary<string, object>()
                                                                                              {
                                                                                                  {"api", "SYNO.DownloadStation.Info"},
                                                                                                  {"version", 1},
                                                                                                  {"method", "getinfo"},
                                                                                                  {"sid", _authInfo.SessionId}
                                                                                              });
        }

        public Task<Models.DownloadStation.Config> GetConfig()
        {
            return _client.GetAsync<Models.DownloadStation.Config>(@"DownloadStation/info.cgi", new Dictionary<string, object>()
                                                                                              {
                                                                                                  {"api", "SYNO.DownloadStation.Info"},
                                                                                                  {"version", 1},
                                                                                                  {"method", "getconfig"},
                                                                                                  {"sid", _authInfo.SessionId}
                                                                                              });
        }

        public Task SetServerConfig(Models.DownloadStation.Config config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var properties = config.GetChangedProperties();

            if (properties.Count == 0)
                throw new ArgumentException("No config was set to be changed.", nameof(config));

            properties["api"] = "SYNO.DownloadStation.Info";
            properties["version"] = 1;
            properties["method"] = "setserverconfig";
            properties["sid"] = _authInfo.SessionId;

            return _client.GetAsync(@"DownloadStation/info.cgi", properties);
        }
    }
}
