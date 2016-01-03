using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blacker.Synology.Api.Models;
using NUnit.Framework;

namespace Blacker.Synology.Api.Tests.Client
{
    [TestFixture]
    class RestClientTests
    {
        private Api.Client.RestClient _restClient;

        [SetUp]
        public void SetUp()
        {
            _restClient = new Api.Client.RestClient(new Uri("http://diskstation:5000/webapi"));
        }

        [TearDown]
        public void TearDown()
        {
            _restClient.Dispose();
        }

        [Test, Explicit]
        public async Task GetAsyncTest()
        {
            var response = await _restClient.GetAsync<ResponseWrapper<IDictionary<string, ApiInfo>>>(@"query.cgi", new Dictionary<string, object>()
                                                                      {
                                                                          {"api", "SYNO.API.Info"},
                                                                          {"version", "1"},
                                                                          {"method", "query"},
                                                                          {"query", "SYNO.API.Info"}
                                                                      });

            Assert.That(response, Is.Not.Null);
            Assert.That(response.Data, Is.Not.Empty);
        }
    }
}
