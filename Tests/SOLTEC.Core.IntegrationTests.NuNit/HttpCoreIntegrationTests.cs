using NUnit.Framework;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using SOLTEC.Core.Connections;
using SOLTEC.Core;

namespace SOLTEC.Core.IntegrationTests.NuNit
{
    [TestFixture]
    public class HttpCoreIntegrationTests
    {
        private class HttpCoreTestable : HttpCore
        {
            private readonly HttpClient _client;

            public HttpCoreTestable(HttpClient client) => _client = client;

            protected override HttpClient CreateConfiguredHttpClient(System.Collections.Generic.Dictionary<string, string>? headers)
                => _client;
        }

        [Test]
        public async Task GetAsync_ShouldReturnServiceResponseT()
        {
            var expected = new ServiceResponse<string> { Message = "OK", ResponseCode = 200, Result = "data" };
            var json = JsonConvert.SerializeObject(expected);
            var handler = new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
            var client = new HttpClient(handler);
            var core = new HttpCoreTestable(client);

            var result = await core.GetAsync<ServiceResponse<string>>("http://fake-endpoint");

            Assert.That(result, Is.Not.Null);
            Assert.That(result?.Result, Is.EqualTo("data"));
            Assert.That(result?.Message, Is.EqualTo("OK"));
        }
    }
}
