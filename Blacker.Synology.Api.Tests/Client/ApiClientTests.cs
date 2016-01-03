using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blacker.Synology.Api.Client;
using Blacker.Synology.Api.Models;
using NUnit.Framework;

namespace Blacker.Synology.Api.Tests.Client
{
    [TestFixture]
    class ApiClientTests
    {
        private ApiClient _apiClient;

        [SetUp]
        public void SetUp()
        {
            _apiClient = new ApiClient(new Uri("http://diskstation:5000/webapi"));
        }

        [TearDown]
        public void TearDown()
        {
            _apiClient.Dispose();
        }

        [Test, Explicit]
        public async Task GetAsyncTest()
        {
            var response = await _apiClient.GetAsync<IDictionary<string, ApiInfo>>(@"query.cgi", new Dictionary<string, object>()
                                                                      {
                                                                          {"api", "SYNO.API.Info"},
                                                                          {"version", "1"},
                                                                          {"method", "query"},
                                                                          {"query", "SYNO.API.Info"}
                                                                      });

            Assert.That(response, Is.Not.Null);
            Assert.That(response, Is.Not.Empty);
        }

        [Test, Explicit]
        public async Task GetAsyncFailureTest()
        {
            await AssertHelpers.ThrowsAsync<ClientException>(() => _apiClient.GetAsync<IDictionary<string, ApiInfo>>(@"query.cgi", new Dictionary<string, object>()
                                                                                                                                   {
                                                                                                                                       {"api", "SYNO.API.Info"},
                                                                                                                                       {"version", "2"}
                                                                                                                                   }));
        }
    }
}
