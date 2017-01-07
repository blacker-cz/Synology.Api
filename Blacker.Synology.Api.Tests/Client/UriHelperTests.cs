using System;
using System.Collections.Generic;
using Blacker.Synology.Api.Client;
using NUnit.Framework;

namespace Blacker.Synology.Api.Tests.Client
{
    [TestFixture]
    class UriHelperTests
    {
        [Test]
        public void BuildUriThrowsOnNullArgumentTest()
        {
            Assert.DoesNotThrow(() => UriHelper.BuildUri(new Uri("http://localhost/"), "path", new Dictionary<string, object>()));
            Assert.DoesNotThrow(() => UriHelper.BuildUri(new Uri("http://localhost/"), "path", null));

            Assert.Throws<ArgumentNullException>(() => UriHelper.BuildUri(null, "path", new Dictionary<string, object>()));
            Assert.Throws<ArgumentNullException>(() => UriHelper.BuildUri(new Uri("http://localhost/"), null, new Dictionary<string, object>()));
        }

        [TestCaseSource(nameof(BuildUriCreatesValidUriTestCaseData))]
        public void BuildUriCreatesValidUriTest(Uri baseUri, string path, IDictionary<string, object> parameters, string expected)
        {
            Assert.That(UriHelper.BuildUri(baseUri, path, parameters).AbsoluteUri, Is.EqualTo(expected));
        }

        static IEnumerable<TestCaseData> BuildUriCreatesValidUriTestCaseData
        {
            get
            {
                yield return new TestCaseData(new Uri("http://localhost/"), "path", new Dictionary<string, object>(), "http://localhost/path");
                yield return new TestCaseData(new Uri("http://localhost/base"), "path", new Dictionary<string, object>(), "http://localhost/base/path");
                yield return new TestCaseData(new Uri("http://localhost/base"), "/path", new Dictionary<string, object>(), "http://localhost/path");
                yield return new TestCaseData(new Uri("http://localhost/"), "path", new Dictionary<string, object>() {{"query", "value"}}, "http://localhost/path?query=value");
                yield return new TestCaseData(new Uri("http://localhost/"), "path", new Dictionary<string, object>() {{"query", 1}}, "http://localhost/path?query=1");
                yield return new TestCaseData(new Uri("http://localhost/"), "path", new Dictionary<string, object>() {{"query", "val&value"}}, "http://localhost/path?query=val%26value");
                yield return new TestCaseData(new Uri("http://localhost/"), "path", new Dictionary<string, object>() {{"query", "value"}, {"query2", "value2"} }, "http://localhost/path?query=value&query2=value2");
            }
        }
    }
}
