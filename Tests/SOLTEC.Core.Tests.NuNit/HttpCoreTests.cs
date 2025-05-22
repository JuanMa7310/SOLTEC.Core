using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SOLTEC.Core.Connections;
using SOLTEC.Core.Connections.Exceptions;
using SOLTEC.Core.DTOS;
using SOLTEC.Core.Enums;
using System.Net;
using System.Text;

namespace SOLTEC.Core.Tests.NuNit;

/// <summary>
/// Unit tests for the HttpCore class using NUnit.
/// </summary>
public class HttpCoreTests
{
    private static HttpClient CreateMockHttpClient(HttpStatusCode statusCode, string content)
    {
        var _handlerMock = new Mock<HttpMessageHandler>();
        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            });

        return new HttpClient(_handlerMock.Object);
    }
    private sealed class HttpCoreTestable(HttpClient client) : HttpCore
    {
        protected override HttpClient CreateConfiguredHttpClient(Dictionary<string, string>? headers)
            => client;

        public override async Task<IList<T>?> GetAsyncList<T>(string uri, Dictionary<string, string>? headerParameters = null)
        {
            using var _client = CreateConfiguredHttpClient(headerParameters);
            var _response = await _client.GetAsync(uri);
            await ValidateStatusResponse(_response);
            var _result = await _response.Content.ReadAsStringAsync();
            // Detect whether the return type is a collection, not whether T is a collection.
            // Therefore, validation should be based on typeof(IList<T>), not T
            if (_result.TrimStart().StartsWith('{'))
            {
                ValidateResult(_result);

                return JsonConvert.DeserializeObject<IList<T>>(_result);
            }
            return JsonConvert.DeserializeObject<IList<T>>(_result);
        }
    }
    public sealed class HttpCoreForTest : HttpCore
    {
        public static Task CallValidateStatusResponse(HttpResponseMessage response)
        {
            return ValidateStatusResponse(response);
        }
    }

    [Test]
    /// <summary>
    /// Tests that GetAsync<T> returns a deserialized object.
    /// </summary>
    public async Task GetAsync_ReturnsDeserializedObject()
    {
        var dto = new ProblemDetailsDto { Title = "Test", Status = 200 };
        var json = JsonConvert.SerializeObject(dto);
        var client = CreateMockHttpClient(HttpStatusCode.OK, json);
        var httpCore = new HttpCoreTestable(client);
        var result = await httpCore.GetAsync<ProblemDetailsDto>("http://test");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Title, Is.EqualTo("Test"));
    }
    [Test]
    /// <summary>
    /// Tests that PostAsync<T, TResult> returns the deserialized response object.
    /// </summary>
    public async Task PostAsync_ReturnsDeserializedResponse()
    {
        var expected = new ProblemDetailsDto { Title = "Posted", Status = 200 };
        var json = JsonConvert.SerializeObject(expected);
        var client = CreateMockHttpClient(HttpStatusCode.OK, json);
        var httpCore = new HttpCoreTestable(client);
        var result = await httpCore.PostAsync<ProblemDetailsDto, ProblemDetailsDto>("http://test", new ProblemDetailsDto());

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Title, Is.EqualTo("Posted"));
    }
    [Test]
    /// <summary>
    /// Tests that ValidateResult throws HttpCoreException when status is a defined error.
    /// </summary>
    public async Task ValidateResult_ThrowsHttpCoreException_OnProblemDetails()
    {
        var problem = new ProblemDetailsDto
        {
            Status = 400,
            Title = "Custom Title",
            Type = "Custom Type",
            Detail = "Detailed message"
        };
        var json = JsonConvert.SerializeObject(problem);
        var client = CreateMockHttpClient(HttpStatusCode.OK, json);
        var httpCore = new HttpCoreTestable(client);

        try
        {
            var _ = await httpCore.GetAsync<ProblemDetailsDto>("http://mock");

            Assert.Fail("Expected HttpCoreException was not thrown.");
        }
        catch (HttpCoreException ex)
        {
            Assert.Multiple(() =>
            {
                Assert.That(ex.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
                Assert.That(ex.Key, Is.EqualTo("Custom Title"));
                Assert.That(ex.Reason, Is.EqualTo("Custom Type"));
                Assert.That(ex.ErrorMessage, Is.EqualTo("Detailed message"));
                Assert.That(ex.ErrorType, Is.EqualTo(HttpCoreErrorEnum.BadRequest));
            });
        }
    }
    [Test]
    /// <summary>
    /// Validates ErrorType is correctly assigned in thrown exception from HTTP status.
    /// </summary>
    public async Task GetAsync_ThrowsHttpCoreException_WithProperErrorType()
    {
        var client = CreateMockHttpClient(HttpStatusCode.Forbidden, "Access Denied");
        var httpCore = new HttpCoreTestable(client);

        try
        {
            var _ = await httpCore.GetAsync<ProblemDetailsDto>("http://fail");

            Assert.Fail("Expected HttpCoreException was not thrown.");
        }
        catch (HttpCoreException ex)
        {
            Assert.Multiple(() =>
            {
                Assert.That(ex.HttpStatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
                Assert.That(ex.ErrorType, Is.EqualTo(HttpCoreErrorEnum.Forbidden));
            });
        }
    }
    [Test]
    /// <summary>
    /// Validates that invalid JSON in ValidateResult triggers a HttpCoreException of type InternalServerError.
    /// </summary>
    public async Task ValidateResult_ThrowsHttpCoreException_OnJsonDeserializationError()
    {
        var malformedJson = "Not a JSON string at all";
        var client = CreateMockHttpClient(HttpStatusCode.OK, malformedJson);
        var httpCore = new HttpCoreTestable(client);

        try
        {
            var _ = await httpCore.GetAsync<ProblemDetailsDto>("http://mock");

            Assert.Fail("Expected HttpCoreException was not thrown.");
        }
        catch (HttpCoreException ex)
        {
            Assert.Multiple(() =>
            {
                Assert.That(ex.HttpStatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
                Assert.That(ex.ErrorType, Is.EqualTo(HttpCoreErrorEnum.InternalServerError));
                Assert.That(ex.Key, Is.EqualTo("JsonDeserializationError"));
            });
        }
    }
    [Test]
    /// <summary>
    /// Verifies that GetAsyncList returns a list of deserialized objects from JSON.
    /// </summary>
    public async Task GetAsyncList_ShouldReturnExpectedList()
    {
        var _data = new List<string> { "Alpha", "Beta" };
        var _json = JsonConvert.SerializeObject(_data);
        var _client = CreateMockHttpClient(HttpStatusCode.OK, _json);
        var _httpCore = new HttpCoreTestable(_client);
        var _result = await _httpCore.GetAsyncList<string>("http://api/items");

        Assert.That(_result, Is.Not.Null);
        Assert.That(_result, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(_result[0], Is.EqualTo("Alpha"));
            Assert.That(_result[1], Is.EqualTo("Beta"));
        });
    }
    [Test]
    /// <summary>
    /// Ensures that PutAsync returns a deserialized result from a mocked HTTP response.
    /// </summary>
    public async Task PutAsync_ShouldReturnTypedResponse()
    {
        var _requestData = new ProblemDetailsDto { Title = "Request", Status = 100 };
        var _expected = new ProblemDetailsDto { Title = "Updated", Status = 200 };
        var _json = JsonConvert.SerializeObject(_expected);
        var _client = CreateMockHttpClient(HttpStatusCode.OK, _json);
        var _httpCore = new HttpCoreTestable(_client);
        var _result = await _httpCore.PutAsync<ProblemDetailsDto, ProblemDetailsDto>("http://api/update", _requestData);

        Assert.That(_result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(_result.Title, Is.EqualTo("Updated"));
            Assert.That(_result.Status, Is.EqualTo(200));
        });
    }
    [Test]
    /// <summary>
    /// Tests that DeleteAsync returns the deserialized object from a successful response.
    /// </summary>
    public async Task DeleteAsync_ShouldReturnDeserializedResult()
    {
        var _expected = new ProblemDetailsDto { Title = "Removed", Status = 204 };
        var _json = JsonConvert.SerializeObject(_expected);
        var _client = CreateMockHttpClient(HttpStatusCode.OK, _json);
        var _httpCore = new HttpCoreTestable(_client);
        var _result = await _httpCore.DeleteAsync<ProblemDetailsDto>("http://api/remove");

        Assert.That(_result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(_result.Title, Is.EqualTo("Removed"));
            Assert.That(_result.Status, Is.EqualTo(204));
        });
    }
    [Test]
    /// <summary>
    /// Validates that an HttpCoreException is thrown for a 400 BadRequest with detailed content.
    /// </summary>
    public void ValidateStatusResponse_WithBadRequest_ShouldThrowWithContent()
    {
        var _response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Invalid input")
        };
        var _ex = Assert.ThrowsAsync<HttpCoreException>(async () => await HttpCoreForTest.CallValidateStatusResponse(_response));

        Assert.Multiple(() =>
        {
            Assert.That(_ex!.HttpStatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(_ex.ErrorMessage, Is.EqualTo("Invalid input"));
            Assert.That(_ex.ErrorType.ToString(), Is.EqualTo("BadRequest"));
        });
    }
    [Test]
    /// <summary>
    /// Validates that an HttpCoreException is thrown for a 418 status code not in the enum.
    /// </summary>
    public void ValidateStatusResponse_WithUnknownStatus_ShouldThrowWithFallback()
    {
        var _response = new HttpResponseMessage((HttpStatusCode)418)
        {
            ReasonPhrase = "I'm a teapot",
            Content = new StringContent("")
        };
        var _ex = Assert.ThrowsAsync<HttpCoreException>(async () => await HttpCoreForTest.CallValidateStatusResponse(_response));

        Assert.Multiple(() =>
        {
            Assert.That(_ex!.HttpStatusCode, Is.EqualTo((HttpStatusCode)418));
            Assert.That(_ex.ErrorMessage, Is.EqualTo("I'm a teapot"));
            Assert.That(_ex.ErrorType, Is.Null);
        });
    }
    [Test]
    /// <summary>
    /// Validates that no exception is thrown for a successful response (200 OK).
    /// </summary>
    public async Task ValidateStatusResponse_WithSuccessStatus_ShouldNotThrow()
    {
        var _response = new HttpResponseMessage(HttpStatusCode.OK);

        await HttpCoreForTest.CallValidateStatusResponse(_response);

        Assert.Pass();
    }
    [Test]
    /// <summary>
    /// Verifies that GetAsyncList returns a correctly deserialized list of items when the response is valid.
    /// </summary>
    public async Task GetAsyncList_ReturnsDeserializedList()
    {
        var list = new List<string> { "Item1", "Item2" };
        var json = JsonConvert.SerializeObject(list);
        var client = CreateMockHttpClient(HttpStatusCode.OK, json);
        var httpCore = new HttpCoreTestable(client);
        var result = await httpCore.GetAsyncList<string>("http://test");

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(2));
        Assert.Multiple(() =>
        {
            Assert.That(result[0], Is.EqualTo("Item1"));
            Assert.That(result[1], Is.EqualTo("Item2"));
        });
    }
    [Test]
    /// <summary>
    /// Tests that PutAsync returns a deserialized response object when posting valid data.
    /// </summary>
    public async Task PutAsync_ReturnsDeserializedResponse()
    {
        var requestData = new ProblemDetailsDto { Title = "Request", Status = 201 };
        var expectedResponse = new ProblemDetailsDto { Title = "Response", Status = 200 };
        var json = JsonConvert.SerializeObject(expectedResponse);
        var client = CreateMockHttpClient(HttpStatusCode.OK, json);
        var httpCore = new HttpCoreTestable(client);
        var result = await httpCore.PutAsync<ProblemDetailsDto, ProblemDetailsDto>("http://put-endpoint", requestData);

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Title, Is.EqualTo("Response"));
            Assert.That(result.Status, Is.EqualTo(200));
        });
    }
    [Test]
    /// <summary>
    /// Verifies that DeleteAsync returns a deserialized object from the response.
    /// </summary>
    public async Task DeleteAsync_ReturnsDeserializedObject()
    {
        var expected = new ProblemDetailsDto { Title = "Deleted", Status = 204 };
        var json = JsonConvert.SerializeObject(expected);
        var client = CreateMockHttpClient(HttpStatusCode.OK, json);
        var httpCore = new HttpCoreTestable(client);
        var result = await httpCore.DeleteAsync<ProblemDetailsDto>("http://delete-endpoint");

        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Title, Is.EqualTo("Deleted"));
            Assert.That(result.Status, Is.EqualTo(204));
        });
    }
}
