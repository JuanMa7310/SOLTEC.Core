#nullable enable

using Newtonsoft.Json;
using NUnit.Framework;
using SOLTEC.Core.Connections;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SOLTEC.Core.IntegrationTests.NuNit
{
    [TestFixture]
    /// <summary>
    /// Integration tests for the HttpCore class using mocked HttpClient responses.
    /// </summary>
    public class HttpCoreIntegrationTests
    {
        private class FakeHttpMessageHandler(HttpResponseMessage fakeResponse) : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task.FromResult(fakeResponse);
            }
        }

        private static HttpCore CreateHttpCore(HttpResponseMessage fakeResponse) => new TestableHttpCore(fakeResponse);

        private class TestableHttpCore(HttpResponseMessage fakeResponse) : HttpCore
        {
            protected override HttpClient CreateConfiguredHttpClient(Dictionary<string, string>? headers)
            {
                var _handler = new FakeHttpMessageHandler(fakeResponse);
                var _client = new HttpClient(_handler);
                return _client;
            }
        }

        public class DummyDto
        {
            public string? Name { get; set; }
        }

        [Test]
        /// <summary>
        /// Tests that a GET request returns a deserialized DTO.
        /// Sends a JSON string with a Name field and expects it to be parsed into a DummyDto.
        /// </summary>
        public async Task GetAsync_ShouldDeserializeObject()
        {
            var _expected = new DummyDto { Name = "Juan" };
            var _json = JsonConvert.SerializeObject(_expected);

            var _response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_json)
            };

            var _client = CreateHttpCore(_response);
            var _result = await _client.GetAsync<DummyDto>("http://fake");

            Assert.That(_result!.Name, Is.EqualTo("Juan"));
        }

        [Test]
        /// <summary>
        /// Tests that a GET request returns a list of deserialized DTOs.
        /// Sends a JSON array and expects a List&lt;DummyDto&gt; with matching data.
        /// </summary>
        public async Task GetAsyncList_ShouldDeserializeList()
        {
            var _expected = new List<DummyDto> { new() { Name = "Ana" } };
            var _json = JsonConvert.SerializeObject(_expected);

            var _response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_json)
            };

            var _client = CreateHttpCore(_response);
            var _result = await _client.GetAsyncList<DummyDto>("http://fake");

            Assert.That(_result, Is.Not.Null);
            Assert.That(_result!.Count, Is.EqualTo(1));
            Assert.That(_result[0].Name, Is.EqualTo("Ana"));
        }

        [Test]
        /// <summary>
        /// Tests that a POST request with no body does not throw when the status is OK.
        /// </summary>
        public async Task PostAsync_ShouldNotThrow()
        {
            var _response = new HttpResponseMessage(HttpStatusCode.OK);
            var _client = CreateHttpCore(_response);

            async Task _action() => await _client.PostAsync("http://fake");
            await _action();

            Assert.That(async () => await _action(), Throws.Nothing);
        }

        [Test]
        /// <summary>
        /// Tests that a POST request with a DTO does not throw when the status is OK.
        /// </summary>
        public async Task PostAsync_WithRequest_ShouldNotThrow()
        {
            var _response = new HttpResponseMessage(HttpStatusCode.OK);
            var _client = CreateHttpCore(_response);

            async Task _action() => await _client.PostAsync("http://fake", new DummyDto());
            await _action();

            Assert.That(async () => await _action(), Throws.Nothing);
        }

        [Test]
        /// <summary>
        /// Tests that a POST request returns a deserialized response.
        /// Sends JSON response and expects it to be parsed into DummyDto.
        /// </summary>
        public async Task PostAsync_WithResult_ShouldReturnData()
        {
            var _expected = new DummyDto { Name = "Luis" };
            var _json = JsonConvert.SerializeObject(_expected);

            var _response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_json)
            };

            var _client = CreateHttpCore(_response);
            var _result = await _client.PostAsync<DummyDto>("http://fake");

            Assert.That(_result!.Name, Is.EqualTo("Luis"));
        }

        [Test]
        /// <summary>
        /// Tests that a PUT request with no body does not throw when the status is OK.
        /// </summary>
        public async Task PutAsync_ShouldNotThrow()
        {
            var _response = new HttpResponseMessage(HttpStatusCode.OK);
            var _client = CreateHttpCore(_response);

            async Task _action() => await _client.PutAsync("http://fake");
            await _action();

            Assert.That(async () => await _action(), Throws.Nothing);
        }

        [Test]
        /// <summary>
        /// Tests that a PUT request with a DTO does not throw when the status is OK.
        /// </summary>
        public async Task PutAsync_WithRequest_ShouldNotThrow()
        {
            var _response = new HttpResponseMessage(HttpStatusCode.OK);
            var _client = CreateHttpCore(_response);

            async Task _action() => await _client.PutAsync("http://fake", new DummyDto());
            await _action();

            Assert.That(async () => await _action(), Throws.Nothing);
        }

        [Test]
        /// <summary>
        /// Tests that a PUT request returns a deserialized response.
        /// Sends JSON and expects a DummyDto with correct values.
        /// </summary>
        public async Task PutAsync_WithResult_ShouldReturnData()
        {
            var _expected = new DummyDto { Name = "Mario" };
            var _json = JsonConvert.SerializeObject(_expected);

            var _response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_json)
            };

            var _client = CreateHttpCore(_response);
            var _result = await _client.PutAsync<DummyDto>("http://fake");

            Assert.That(_result!.Name, Is.EqualTo("Mario"));
        }

        [Test]
        /// <summary>
        /// Tests that a DELETE request returns a deserialized response.
        /// Sends JSON and expects a DummyDto with correct Name.
        /// </summary>
        public async Task DeleteAsync_ShouldReturnData()
        {
            var _expected = new DummyDto { Name = "Carlos" };
            var _json = JsonConvert.SerializeObject(_expected);

            var _response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_json)
            };

            var _client = CreateHttpCore(_response);
            var _result = await _client.DeleteAsync<DummyDto>("http://fake");

            Assert.That(_result!.Name, Is.EqualTo("Carlos"));
        }
    }
}
