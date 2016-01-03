using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

namespace Blacker.Synology.Api.Client
{
    class UriHelper
    {
        public static Uri BuildUri(Uri baseUri, string path, IDictionary<string, object> parameters)
        {
            if (baseUri == null)
                throw new ArgumentNullException(nameof(baseUri));
            if (path == null)
                throw new ArgumentNullException(nameof(path));
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            var queryParams = HttpUtility.ParseQueryString("");

            foreach (var parameter in parameters)
            {
                queryParams[parameter.Key] = HttpUtility.UrlEncode(parameter.Value?.ToString() ?? String.Empty);
            }

            var uriBuilder = new UriBuilder(baseUri)
                             {
                                 Query = queryParams.ToString()
                             };

            uriBuilder.Path = Path.Combine(uriBuilder.Path, path).Replace('\\', '/');

            return uriBuilder.Uri;
        }
    }
}