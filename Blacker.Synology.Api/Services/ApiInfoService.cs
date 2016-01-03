using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blacker.Synology.Api.Client;
using Blacker.Synology.Api.Models;

namespace Blacker.Synology.Api.Services
{
    class ApiInfoService : IApiInfoService
    {
        private readonly IClient _client;

        public ApiInfoService(IClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            _client = client;
        }

        public Task<IDictionary<string, ApiInfo>> ListAvailableApis()
        {
            return ListAvailableApis(@"ALL");
        }

        public Task<IDictionary<string, ApiInfo>> ListAvailableApis(IEnumerable<string> apis)
        {
            if (apis == null)
                throw new ArgumentNullException(nameof(apis));

            var requestedApis = String.Join(",", apis.Where(api => !String.IsNullOrWhiteSpace(api)));

            return ListAvailableApis(!String.IsNullOrEmpty(requestedApis) ? requestedApis : @"ALL");
        }

        private Task<IDictionary<string, ApiInfo>> ListAvailableApis(string requestedApi)
        {
            return _client.GetAsync<IDictionary<string, ApiInfo>>(@"query.cgi", new Dictionary<string, object>()
                                                                                {
                                                                                    {"api", "SYNO.API.Info"},
                                                                                    {"version", 1},
                                                                                    {"method", "query"},
                                                                                    {"query", requestedApi}
                                                                                });
        }
    }
}
