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
    class AuthServiceTests
    {
        private Mock<IClient> _clientMock;
        private AuthService _service;

        [SetUp]
        public void SetUp()
        {
            _clientMock = new Mock<IClient>();
            _service = new AuthService(_clientMock.Object);
        }

        [Test]
        public async Task AuthenticateTest()
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync<AuthInfo>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .ReturnsAsync(new AuthInfo() {SessionId = "test"});

            var result = await _service.Authenticate("testuser", "testpassword", "testsession", "testoptcode");

            _clientMock.Verify((client => client.GetAsync<AuthInfo>("auth.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.SessionId, Is.Not.Null);
            Assert.That(result.SessionId, Is.EqualTo("test"));

            var expected = new Dictionary<string, object>()
                           {
                                 {"api", "SYNO.API.Auth"},
                                 {"version", 2},
                                 {"method", "login"},
                                 {"account", "testuser"},
                                 {"passwd", "testpassword"},
                                 {"session", "testsession"},
                                 {"opt_code", "testoptcode"},
                                 {"format", "sid"}
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task LogoutTest()
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .Returns(Task.FromResult(default(object)));

            await _service.Logout("testsession");

            _clientMock.Verify((client => client.GetAsync("auth.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            var expected = new Dictionary<string, object>()
                           {
                                 {"api", "SYNO.API.Auth"},
                                 {"version", 2},
                                 {"method", "logout"},
                                 {"session", "testsession"}
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }
    }
}
