using System.Collections.Generic;
using System.Threading.Tasks;
using Blacker.Synology.Api.Client;
using Blacker.Synology.Api.Models;
using Blacker.Synology.Api.Services;
using Moq;
using NUnit.Framework;

namespace Blacker.Synology.Api.Tests.Services
{
    [TestFixture]
    class ApiInfoServiceTests
    {
        private Mock<IClient> _clientMock;
        private ApiInfoService _service;

        [SetUp]
        public void SetUp()
        {
            _clientMock = new Mock<IClient>();
            _service = new ApiInfoService(_clientMock.Object);
        }

        [Test]
        public async Task ListAvailableApisTest()
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync<IDictionary<string, ApiInfo>>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .ReturnsAsync(new Dictionary<string, ApiInfo>() {{"Test", new ApiInfo()}});

            var result = await _service.ListAvailableApis();

            _clientMock.Verify((client => client.GetAsync<IDictionary<string, ApiInfo>>("query.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);

            Assert.That(parameters.ContainsKey("api"));
            Assert.That(parameters["api"], Is.EqualTo("SYNO.API.Info"));

            Assert.That(parameters.ContainsKey("version"));
            Assert.That(parameters["version"], Is.EqualTo(1));

            Assert.That(parameters.ContainsKey("method"));
            Assert.That(parameters["method"], Is.EqualTo("query"));

            Assert.That(parameters.ContainsKey("query"));
            Assert.That(parameters["query"], Is.EqualTo("ALL"));
        }

        [TestCase(new string[] {}, "ALL")]
        [TestCase(new[] { "" }, "ALL")]
        [TestCase(new[] { "", " ", "  " }, "ALL")]
        [TestCase(new[] { "SYNO.API.Info" }, "SYNO.API.Info")]
        [TestCase(new[] { "SYNO.API.Info", "SYNO.API.Auth" }, "SYNO.API.Info,SYNO.API.Auth")]
        public async Task ListAvailableApisWithParamsTest(IEnumerable<string> requestedApi, string expected)
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync<IDictionary<string, ApiInfo>>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .ReturnsAsync(new Dictionary<string, ApiInfo>() {{"Test", new ApiInfo()}});

            var result = await _service.ListAvailableApis(requestedApi);

            _clientMock.Verify((client => client.GetAsync<IDictionary<string, ApiInfo>>("query.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Not.Empty);

            Assert.That(parameters.ContainsKey("api"));
            Assert.That(parameters["api"], Is.EqualTo("SYNO.API.Info"));

            Assert.That(parameters.ContainsKey("version"));
            Assert.That(parameters["version"], Is.EqualTo(1));

            Assert.That(parameters.ContainsKey("method"));
            Assert.That(parameters["method"], Is.EqualTo("query"));

            Assert.That(parameters.ContainsKey("query"));
            Assert.That(parameters["query"], Is.EqualTo(expected));
        }
    }
}
