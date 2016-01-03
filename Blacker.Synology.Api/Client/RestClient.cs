using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blacker.Synology.Api.Client
{
    class RestClient : IClient
    {
        private readonly Uri _baseUri;
        private readonly ISerializer _serializer;
        private readonly HttpClient _httpClient;

        internal RestClient(Uri baseUri) : this(baseUri, new JsonSerializer())
        {
        }

        internal RestClient(Uri baseUri, ISerializer serializer)
        {
            if (baseUri == null)
                throw new ArgumentNullException(nameof(baseUri));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            _baseUri = baseUri;
            _serializer = serializer;
            _httpClient = new HttpClient()
                          {
                              BaseAddress = baseUri
                          };
        }

        public virtual async Task<T> GetAsync<T>(string path, IDictionary<string, object> parameters) where T : class
        {
            if (Disposed)
                throw new ObjectDisposedException(null);

            using (var stream = await _httpClient.GetStreamAsync(UriHelper.BuildUri(_baseUri, path, parameters)).ConfigureAwait(false))
            {
                return _serializer.Deserialize<T>(stream);
            }
        }

        #region IDisposable implementation

        protected bool Disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (Disposed)
                return;

            if (disposing)
            {
                _httpClient?.Dispose();

                Disposed = true;
            }
        }

        #endregion
    }
}
