using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blacker.Synology.Api.Client
{
    class RestClient : IDisposable
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
                throw new ObjectDisposedException(GetType().Name);

            using (var stream = await _httpClient.GetStreamAsync(UriHelper.BuildUri(_baseUri, path, parameters)).ConfigureAwait(false))
            {
                return _serializer.Deserialize<T>(stream);
            }
        }

        public virtual async Task<T> PostAsync<T>(string path, IDictionary<string, object> parameters) where T : class
        {
            if (Disposed)
                throw new ObjectDisposedException(GetType().Name);

            HttpContent content = null;

            try
            {
                if (parameters != null && parameters.Values.Any(v => v is Stream))
                {
                    throw new NotSupportedException();
                }
                else
                {
                    content = new FormUrlEncodedContent(parameters
                        ?.Select(kvp => new KeyValuePair<string, string>(kvp.Key, kvp.Value?.ToString()))
                                                        ?? Enumerable.Empty<KeyValuePair<string, string>>());
                }

                using (var response = await _httpClient.PostAsync(UriHelper.BuildUri(_baseUri, path), content).ConfigureAwait(false))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                    {
                        return _serializer.Deserialize<T>(stream);
                    }
                }
            }
            finally
            {
                content?.Dispose();
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
