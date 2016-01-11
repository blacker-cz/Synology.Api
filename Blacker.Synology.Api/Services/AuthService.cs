using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blacker.Synology.Api.Client;
using Blacker.Synology.Api.Models;

namespace Blacker.Synology.Api.Services
{
    class AuthService : IAuthService
    {
        private readonly IClient _client;

        public AuthService(IClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            _client = client;
        }

        public Task<AuthInfo> Authenticate(string account, string password, string sessionName, string optCode = null)
        {
            if (String.IsNullOrWhiteSpace(account))
                throw new ArgumentException("Argument is null or whitespace", nameof(account));
            if (String.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Argument is null or whitespace", nameof(password));
            if (String.IsNullOrWhiteSpace(sessionName))
                throw new ArgumentException("Argument is null or whitespace", nameof(sessionName));

            var parameters = new Dictionary<string, object>()
                             {
                                 {"api", "SYNO.API.Auth"},
                                 {"version", 2},
                                 {"method", "login"},
                                 {"account", account},
                                 {"passwd", password},
                                 {"session", sessionName},
                                 {"format", "sid"}
                             };

            if (!String.IsNullOrWhiteSpace(optCode))
            {
                parameters["opt_code"] = optCode;
            }

            return _client.GetAsync<AuthInfo>(@"auth.cgi", parameters);
        }

        public Task Logout(string sessionName)
        {
            if (String.IsNullOrWhiteSpace(sessionName))
                throw new ArgumentException("Argument is null or whitespace", nameof(sessionName));

            var parameters = new Dictionary<string, object>()
                             {
                                 {"api", "SYNO.API.Auth"},
                                 {"version", 2},
                                 {"method", "logout"},
                                 {"session", sessionName}
                             };

            return _client.GetAsync(@"auth.cgi", parameters);
        }
    }
}
