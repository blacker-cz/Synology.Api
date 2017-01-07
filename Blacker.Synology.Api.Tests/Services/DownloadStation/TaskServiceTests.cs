using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blacker.Synology.Api.Client;
using Blacker.Synology.Api.Models;
using Blacker.Synology.Api.Models.DownloadStation;
using Blacker.Synology.Api.Services;
using Blacker.Synology.Api.Services.DownloadStation;
using Moq;
using NUnit.Framework;

namespace Blacker.Synology.Api.Tests.Services.DownloadStation
{
    [TestFixture]
    class TaskServiceTests
    {
        private Mock<IClient> _clientMock;
        private TaskService _service;
        private readonly AuthInfo _authInfo = new AuthInfo() { SessionId = "testingsession" };

        [SetUp]
        public void SetUp()
        {
            _clientMock = new Mock<IClient>();
            _service = new TaskService(_clientMock.Object, _authInfo);
        }

        [Test]
        public void DeserializeTest()
        {
            const string data = @"
{
   ""total"":12,
   ""offset"":10,
   ""tasks"":[
      {
         ""id"":""dbid_001"",
         ""type"":""bt"",
         ""username"":""admin"",
         ""title"":""TOP 100 MIX"",
         ""size"":""9427312332"",
         ""status"":""downloading"",
         ""status_extra"":null,
         ""additional"":{
            ""detail"":{
               ""connected_leechers"":0,
               ""connected_seeders"":0,
               ""create_time"":""1341210005"",
               ""destination"":""Download"",
               ""priority"":""auto"",
               ""total_peers"":0,
               ""uri"":""http://mp3.com/mix.torrent""
            },
            ""file"":[
               {
                  ""filename"":""mix001.mp3"",
                  ""priority"":""normal"",
                  ""size"":""41835"",
                  ""size_downloaded"":""0""
               },
               {
                  ""filename"":""mix002.mp3"",
                  ""priority"":""normal"",
                  ""size"":""31689"",
                  ""size_downloaded"":""0""
               }
            ]
         }
      },
      {
         ""id"":""dbid_002"",
         ""type"":""http"",
         ""username"":""user1"",
         ""title"":""short clip"",
         ""size"":""112092412"",
         ""status"":""finished"",
         ""status_extra"":null,
         ""additional"":{
            ""detail"":{
               ""connected_leechers"":0,
               ""connected_seeders"":0,
               ""create_time"":""1356214565"",
               ""destination"":""Download"",
               ""priority"":""auto"",
               ""total_peers"":0,
               ""uri"":""http://mymovies.com/mv.avi""
            }
         }
      }
   ]
}";

            var serializer = new JsonSerializer();

            TaskInfoList result;

            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(data)))
            {
                result = serializer.Deserialize<TaskInfoList>(stream);
            }

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Tasks, Is.Not.Null);
            Assert.That(result.Tasks.Count(), Is.EqualTo(2));
        }

        [TestCase(0, 1, AdditionalInfoFlags.None, null)]
        [TestCase(0, -1, AdditionalInfoFlags.Detail, "detail")]
        [TestCase(0, -1, AdditionalInfoFlags.Detail | AdditionalInfoFlags.None, "detail")]
        [TestCase(1, 10, AdditionalInfoFlags.Detail | AdditionalInfoFlags.File, "detail,file")]
        [TestCase(10, 0, AdditionalInfoFlags.Detail | AdditionalInfoFlags.Transfer | AdditionalInfoFlags.File | AdditionalInfoFlags.Tracker | AdditionalInfoFlags.Peer,
            "detail,transfer,file,tracker,peer")]
        public async Task ListTaskTest(int offset, int limit, AdditionalInfoFlags flags, string flagsExpected)
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync<TaskInfoList>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .ReturnsAsync(new TaskInfoList());

            var result = await _service.ListTasks(offset, limit, flags);

            _clientMock.Verify((client => client.GetAsync<TaskInfoList>("DownloadStation/task.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            Assert.That(result, Is.Not.Null);

            var expected = new Dictionary<string, object>()
                           {
                               {"api", "SYNO.DownloadStation.Task"},
                               {"version", 1},
                               {"method", "list"},
                               {"offset", offset},
                               {"limit", limit},
                               {"_sid", _authInfo.SessionId}
                           };

            if (flagsExpected != null)
            {
                expected["additional"] = flagsExpected;
            }

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task GetInfoThrowsOnNullIdsTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentNullException>(() => _service.GetInfo(null));
        }

        [Test]
        public async Task GetInfoThrowsOnEmptyIdsTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentException>(() => _service.GetInfo(Enumerable.Empty<string>()));
        }

        [TestCase(new [] {"1"}, AdditionalInfoFlags.None, "1", null)]
        [TestCase(new [] {"1", "2"}, AdditionalInfoFlags.None, "1,2", null)]
        [TestCase(new [] {"1", "2"}, AdditionalInfoFlags.Detail, "1,2", "detail")]
        [TestCase(new [] {"1", "2"}, AdditionalInfoFlags.Detail | AdditionalInfoFlags.File, "1,2", "detail,file")]
        public async Task GetInfoTest(IEnumerable<string> ids, AdditionalInfoFlags flags, string idsExpected, string flagsExpected)
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync<TaskInfoList>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .ReturnsAsync(new TaskInfoList());

            var result = await _service.GetInfo(ids, flags);

            _clientMock.Verify((client => client.GetAsync<TaskInfoList>("DownloadStation/task.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            Assert.That(result, Is.Not.Null);

            var expected = new Dictionary<string, object>()
                           {
                               {"api", "SYNO.DownloadStation.Task"},
                               {"version", 1},
                               {"method", "getinfo"},
                               {"id", idsExpected},
                               {"_sid", _authInfo.SessionId}
                           };

            if (flagsExpected != null)
            {
                expected["additional"] = flagsExpected;
            }

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task CreateTaskThrowsOnNullUrisTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentNullException>(() => _service.CreateTask(null));
        }

        [Test]
        public async Task CreateTaskThrowsOnEmptyUrisTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentException>(() => _service.CreateTask(Enumerable.Empty<string>()));
        }

        [TestCase(new [] {"http://test"}, "http://test")]
        [TestCase(new [] {"http://test", "ftp://test2"}, "http://test,ftp://test2")]
        public async Task CreateTaskUsingUrisTest(IEnumerable<string> uris, string expectedUris)
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.PostAsync(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .Returns(Task.FromResult(default(object)));

            await _service.CreateTask(uris, new CreateTaskConfig()
                                            {
                                                Destination = "destination"
                                            });

            _clientMock.Verify((client => client.PostAsync("DownloadStation/task.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            var expected = new Dictionary<string, object>()
                           {
                                 {"api", "SYNO.DownloadStation.Task"},
                                 {"version", 3},
                                 {"method", "create"},
                                 {"uri", expectedUris},
                                 {"_sid", _authInfo.SessionId},
                                 {"destination", "destination"},
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task DeleteTaskThrowsOnNullIdsTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentNullException>(() => _service.DeleteTask(null, false));
        }

        [Test]
        public async Task DeleteTaskThrowsOnEmptyIdsTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentException>(() => _service.DeleteTask(Enumerable.Empty<string>(), false));
        }

        [TestCase(new [] {"1"}, true, "1")]
        [TestCase(new [] {"1", "2"}, false, "1,2")]
        public async Task DeleteTaskTest(IEnumerable<string> ids, bool force, string idsExpected)
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync<IEnumerable<TaskActionResult>>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .ReturnsAsync(Enumerable.Empty<TaskActionResult>());

            var result = await _service.DeleteTask(ids, force);

            _clientMock.Verify((client => client.GetAsync<IEnumerable<TaskActionResult>>("DownloadStation/task.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            Assert.That(result, Is.Not.Null);

            var expected = new Dictionary<string, object>()
                           {
                                 {"api", "SYNO.DownloadStation.Task"},
                                 {"version", 1},
                                 {"method", "delete"},
                                 {"id", idsExpected},
                                 {"force_complete", force},
                                 {"_sid", _authInfo.SessionId}
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task PauseTaskThrowsOnNullIdsTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentNullException>(() => _service.PauseTask(null));
        }

        [Test]
        public async Task PauseTaskThrowsOnEmptyIdsTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentException>(() => _service.PauseTask(Enumerable.Empty<string>()));
        }

        [TestCase(new [] {"1"}, "1")]
        [TestCase(new [] {"1", "2"}, "1,2")]
        public async Task PauseTaskTest(IEnumerable<string> ids, string idsExpected)
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync<IEnumerable<TaskActionResult>>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .ReturnsAsync(Enumerable.Empty<TaskActionResult>());

            var result = await _service.PauseTask(ids);

            _clientMock.Verify((client => client.GetAsync<IEnumerable<TaskActionResult>>("DownloadStation/task.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            Assert.That(result, Is.Not.Null);

            var expected = new Dictionary<string, object>()
                           {
                                 {"api", "SYNO.DownloadStation.Task"},
                                 {"version", 1},
                                 {"method", "pause"},
                                 {"id", idsExpected},
                                 {"_sid", _authInfo.SessionId}
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task ResumeTaskThrowsOnNullIdsTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentNullException>(() => _service.ResumeTask(null));
        }

        [Test]
        public async Task ResumeTaskThrowsOnEmptyIdsTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentException>(() => _service.ResumeTask(Enumerable.Empty<string>()));
        }

        [TestCase(new [] {"1"}, "1")]
        [TestCase(new [] {"1", "2"}, "1,2")]
        public async Task ResumeTaskTest(IEnumerable<string> ids, string idsExpected)
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync<IEnumerable<TaskActionResult>>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .ReturnsAsync(Enumerable.Empty<TaskActionResult>());

            var result = await _service.ResumeTask(ids);

            _clientMock.Verify((client => client.GetAsync<IEnumerable<TaskActionResult>>("DownloadStation/task.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            Assert.That(result, Is.Not.Null);

            var expected = new Dictionary<string, object>()
                           {
                                 {"api", "SYNO.DownloadStation.Task"},
                                 {"version", 1},
                                 {"method", "resume"},
                                 {"id", idsExpected},
                                 {"_sid", _authInfo.SessionId}
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }

        [Test]
        public async Task EditTaskThrowsOnNullIdsTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentNullException>(() => _service.EditTask(null, "destination"));
        }

        [Test]
        public async Task EditTaskThrowsOnEmptyIdsTest()
        {
            await AssertHelpers.ThrowsAsync<ArgumentException>(() => _service.EditTask(Enumerable.Empty<string>(), "destination"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("    ")]
        public async Task EditTaskThrowsOnNullOrWhitespaceTest(string destination)
        {
            await AssertHelpers.ThrowsAsync<ArgumentException>(() => _service.EditTask(Enumerable.Empty<string>(), destination));
        }

        [TestCase(new [] {"1"}, "destination1", "1")]
        [TestCase(new [] {"1", "2"}, "destination2", "1,2")]
        public async Task EditTaskTest(IEnumerable<string> ids, string destination, string idsExpected)
        {
            IDictionary<string, object> parameters = null;

            _clientMock.Setup((client => client.GetAsync<IEnumerable<TaskActionResult>>(It.IsAny<string>(), It.IsAny<IDictionary<string, object>>())))
                .Callback<string, IDictionary<string, object>>((s, objects) => { parameters = objects; })
                .ReturnsAsync(Enumerable.Empty<TaskActionResult>());

            var result = await _service.EditTask(ids, destination);

            _clientMock.Verify((client => client.GetAsync<IEnumerable<TaskActionResult>>("DownloadStation/task.cgi", It.IsAny<IDictionary<string, object>>())), Times.Once);

            Assert.That(result, Is.Not.Null);

            var expected = new Dictionary<string, object>()
                           {
                                 {"api", "SYNO.DownloadStation.Task"},
                                 {"version", 2},
                                 {"method", "edit"},
                                 {"id", idsExpected},
                                 {"destination", destination},
                                 {"_sid", _authInfo.SessionId}
                           };

            CollectionAssert.AreEquivalent(expected, parameters);
        }
    }
}
