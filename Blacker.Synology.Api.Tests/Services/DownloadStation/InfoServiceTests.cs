using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blacker.Synology.Api.Client;
using Blacker.Synology.Api.Models;
using Blacker.Synology.Api.Models.DownloadStation;
using Blacker.Synology.Api.Services.DownloadStation;
using Moq;
using NUnit.Framework;

namespace Blacker.Synology.Api.Tests.Services.DownloadStation
{
    [TestFixture]
    class InfoServiceTests
    {
        private Mock<IClient> _clientMock;
        private InfoService _service;
        private readonly AuthInfo _authInfo = new AuthInfo() {SessionId = "testingsession"};

        [SetUp]
        public void SetUp()
        {
            _clientMock = new Mock<IClient>();
            _service = new InfoService(_clientMock.Object, _authInfo);
        }

        [Test]
        public async Task GetInfoTest()
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync<Info>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .ReturnsAsync(new Info());

            var result = await _service.GetInfo();

            _clientMock.Verify((client => client.GetAsync<Info>("DownloadStation/info.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            Assert.That(result, Is.Not.Null);

            var expected = new Dictionary<string, object>()
                           {
                               {"api", "SYNO.DownloadStation.Info"},
                               {"version", 1},
                               {"method", "getinfo"},
                               {"sid", _authInfo.SessionId}
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task GetConfigTest()
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync<Config>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .ReturnsAsync(new Config() {DefaultDestination = "test"});

            var result = await _service.GetConfig();

            _clientMock.Verify((client => client.GetAsync<Config>("DownloadStation/info.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.DefaultDestination, Is.EqualTo("test"));

            var expected = new Dictionary<string, object>()
                           {
                               {"api", "SYNO.DownloadStation.Info"},
                               {"version", 1},
                               {"method", "getconfig"},
                               {"sid", _authInfo.SessionId}
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task SetServerConfigOneChangeTest()
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .Returns(Task.FromResult(default(object)));

            await _service.SetServerConfig(new Config() {DefaultDestination = "test"});

            _clientMock.Verify((client => client.GetAsync("DownloadStation/info.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            var expected = new Dictionary<string, object>()
                           {
                               {"api", "SYNO.DownloadStation.Info"},
                               {"version", 1},
                               {"method", "setserverconfig"},
                               {"sid", _authInfo.SessionId},
                               {"default_destination", "test"}
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task SetServerConfigAllChangedTest()
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .Returns(Task.FromResult(default(object)));

            await _service.SetServerConfig(new Config()
                                           {
                                               BtMaxUpload = 1,
                                               BtMaxDownload = 1,
                                               EMuleEnabled = true,
                                               EMuleMaxDownload = 1,
                                               EMuleMaxUpload = 1,
                                               FtpMaxDownload = 1,
                                               HttpMaxDownload = 1,
                                               NzbMaxDownload = 1,
                                               UnzipServiceEnabled = true,
                                               DefaultDestination = "test",
                                               EMuleDefaultDestination = "test"
                                           });

            _clientMock.Verify((client => client.GetAsync("DownloadStation/info.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            var expected = new Dictionary<string, object>()
                           {
                               {"api", "SYNO.DownloadStation.Info"},
                               {"version", 1},
                               {"method", "setserverconfig"},
                               {"sid", _authInfo.SessionId},
                               {"bt_max_upload", 1},
                               {"bt_max_download", 1},
                               {"emule_enabled", true},
                               {"emule_max_download", 1},
                               {"emule_max_upload", 1},
                               {"ftp_max_download", 1},
                               {"http_max_download", 1},
                               {"nzb_max_download", 1},
                               {"unzip_service_enabled", true},
                               {"default_destination", "test"},
                               {"emule_default_destination", "test"}
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task SetServerConfigThrowsOnEmptyConfigTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentException>(() => _service.SetServerConfig(new Config()));
        }

        [Test]
        public async Task SetServerConfigThrowsOnNullConfigTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentNullException>(() => _service.SetServerConfig(null));
        }

    }
}
