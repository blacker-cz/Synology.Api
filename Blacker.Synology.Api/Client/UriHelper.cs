using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Blacker.Synology.Api.Client
{
    class UriHelper
    {
        public static Uri BuildUri(Uri baseUri, string path, IDictionary<string, object> parameters = null)
        {
            if (baseUri == null)
                throw new ArgumentNullException(nameof(baseUri));
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var uriBuilder = new UriBuilder(baseUri);

            if (parameters != null)
            {
                var queryParams = HttpUtility.ParseQueryString("");

                foreach (var parameter in parameters)
                {
                    queryParams.Add(parameter.Key, parameter.Value?.ToString() ?? String.Empty);
                }

                uriBuilder.Query = queryParams.ToString();
            }

            uriBuilder.Path = Path.Combine(uriBuilder.Path, path).Replace('\\', '/');

            return uriBuilder.Uri;
        }
    }
}