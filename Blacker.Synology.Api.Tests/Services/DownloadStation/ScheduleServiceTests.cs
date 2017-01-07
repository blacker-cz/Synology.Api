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
    class ScheduleServiceTests
    {
        private Mock<IClient> _clientMock;
        private ScheduleService _service;
        private readonly AuthInfo _authInfo = new AuthInfo() { SessionId = "testingsession" };

        [SetUp]
        public void SetUp()
        {
            _clientMock = new Mock<IClient>();
            _service = new ScheduleService(_clientMock.Object, _authInfo);
        }


        [Test]
        public async Task GetConfigTest()
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync<ScheduleConfig>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .ReturnsAsync(new ScheduleConfig() { Enabled = true });

            var result = await _service.GetConfig();

            _clientMock.Verify((client => client.GetAsync<ScheduleConfig>("DownloadStation/schedule.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Enabled, Is.EqualTo(true));

            var expected = new Dictionary<string, object>()
                           {
                               {"api", "SYNO.DownloadStation.Schedule"},
                               {"version", 1},
                               {"method", "getconfig"},
                               {"_sid", _authInfo.SessionId}
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task SetConfigTest()
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .Returns(Task.FromResult(default(object)));

            await _service.SetConfig(new ScheduleConfig() {Enabled = true, EMuleEnabled = false});

            _clientMock.Verify((client => client.GetAsync("DownloadStation/schedule.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            var expected = new Dictionary<string, object>()
                           {
                               {"api", "SYNO.DownloadStation.Schedule"},
                               {"version", 1},
                               {"method", "setconfig"},
                               {"_sid", _authInfo.SessionId},
                               {"enabled", true},
                               {"emule_enabled", false}
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task SetConfigThrowsOnEmptyConfigTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentException>(() => _service.SetConfig(new ScheduleConfig()));
        }

        [Test]
        public async Task SetConfigThrowsOnNullConfigTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentNullException>(() => _service.SetConfig(null));
        }
    }
}
