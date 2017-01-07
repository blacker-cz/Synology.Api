using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blacker.Synology.Api.Models;

namespace Blacker.Synology.Api.Client
{
    class ApiClient : RestClient, IClient
    {
        public ApiClient(Uri baseUri) : base(baseUri)
        {
        }

        public ApiClient(Uri baseUri, ISerializer serializer) : base(baseUri, serializer)
        {
        }

        public override async Task<T> GetAsync<T>(string path, IDictionary<string, object> parameters)
        {
            try
            {
                var response = await base.GetAsync<ResponseWrapper<T>>(path, parameters).ConfigureAwait(false);

                if (response.Success)
                {
                    return response.Data;
                }

                throw new ClientException("There was an issue during communication with API. See error info for more details.")
                      {
                          Error = response.Error
                      };
            }
            catch (ClientException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ClientException("There was an issue during communication with API. See inner exception for more details.", e);
            }
        }

        public Task GetAsync(string path, IDictionary<string, object> parameters)
        {
            return GetAsync<object>(path, parameters);
        }

        public override async Task<T> PostAsync<T>(string path, IDictionary<string, object> parameters)
        {
            try
            {
                var response = await base.PostAsync<ResponseWrapper<T>>(path, parameters).ConfigureAwait(false);

                if (response.Success)
                {
                    return response.Data;
                }

                throw new ClientException("There was an issue during communication with API. See error info for more details.")
                {
                    Error = response.Error
                };
            }
            catch (ClientException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ClientException("There was an issue during communication with API. See inner exception for more details.", e);
            }
        }

        public Task PostAsync(string path, IDictionary<string, object> parameters)
        {
            return PostAsync<object>(path, parameters);
        }
    }
}
