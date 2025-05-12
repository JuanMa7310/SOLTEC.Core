using Newtonsoft.Json;
using SOLTEC.Core.Connections;
using System.Net;
using Xunit;

namespace SOLTEC.Core.IntegrationTests.xUnit;

/// <summary>
/// Integration tests for the HttpCore class using mocked HttpClient responses in xUnit.
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
            return new HttpClient(_handler);
        }
    }

    public class DummyDto
    {
        public string? Name { get; set; }
    }

    [Fact]
    /// <summary>
    /// Tests GET request returns a deserialized DTO.
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

        Assert.Equal("Juan", _result!.Name);
    }

    [Fact]
    /// <summary>
    /// Tests GET request returns a list of DTOs.
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

        Assert.NotNull(_result);
        Assert.Single(_result!);
        Assert.Equal("Ana", _result[0].Name);
    }

    [Fact]
    /// <summary>
    /// Tests POST request with no body does not throw.
    /// </summary>
    public async Task PostAsync_ShouldNotThrow()
    {
        var _response = new HttpResponseMessage(HttpStatusCode.OK);
        var _client = CreateHttpCore(_response);

        var _ex = await Record.ExceptionAsync(() => _client.PostAsync("http://fake"));
        Assert.Null(_ex);
    }

    [Fact]
    /// <summary>
    /// Tests POST request with DTO does not throw.
    /// </summary>
    public async Task PostAsync_WithRequest_ShouldNotThrow()
    {
        var _response = new HttpResponseMessage(HttpStatusCode.OK);
        var _client = CreateHttpCore(_response);

        var _ex = await Record.ExceptionAsync(() => _client.PostAsync("http://fake", new DummyDto()));
        Assert.Null(_ex);
    }

    [Fact]
    /// <summary>
    /// Tests POST request returns deserialized response.
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

        Assert.Equal("Luis", _result!.Name);
    }

    [Fact]
    /// <summary>
    /// Tests PUT request with no body does not throw.
    /// </summary>
    public async Task PutAsync_ShouldNotThrow()
    {
        var _response = new HttpResponseMessage(HttpStatusCode.OK);
        var _client = CreateHttpCore(_response);

        var _ex = await Record.ExceptionAsync(() => _client.PutAsync("http://fake"));
        Assert.Null(_ex);
    }

    [Fact]
    /// <summary>
    /// Tests PUT request with DTO does not throw.
    /// </summary>
    public async Task PutAsync_WithRequest_ShouldNotThrow()
    {
        var _response = new HttpResponseMessage(HttpStatusCode.OK);
        var _client = CreateHttpCore(_response);

        var _ex = await Record.ExceptionAsync(() => _client.PutAsync("http://fake", new DummyDto()));
        Assert.Null(_ex);
    }

    [Fact]
    /// <summary>
    /// Tests PUT request returns deserialized response.
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

        Assert.Equal("Mario", _result!.Name);
    }

    [Fact]
    /// <summary>
    /// Tests DELETE request returns deserialized response.
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

        Assert.Equal("Carlos", _result!.Name);
    }
}
