using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using SOLTEC.Core.Connections;
using SOLTEC.Core.Connections.Exceptions;
using SOLTEC.Core.DTOS;
using SOLTEC.Core.Enums;
using System.Net;
using System.Text;

namespace SOLTEC.Core.Tests.xUnit;

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

    [Fact]
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

        Assert.NotNull(result);
        Assert.Equal("Test", result.Title);
    }
    [Fact]
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

        Assert.NotNull(result);
        Assert.Equal("Posted", result.Title);
    }
    [Fact]
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

        var ex = await Assert.ThrowsAsync<HttpCoreException>(() => httpCore.GetAsync<ProblemDetailsDto>("http://mock"));

        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
        Assert.Equal("Custom Title", ex.Key);
        Assert.Equal("Custom Type", ex.Reason);
        Assert.Equal("Detailed message", ex.ErrorMessage);
        Assert.Equal(HttpCoreErrorEnum.BadRequest, ex.ErrorType);
    }
    [Fact]
    /// <summary>
    /// Validates ErrorType is correctly assigned in thrown exception from HTTP status.
    /// </summary>
    public async Task GetAsync_ThrowsHttpCoreException_WithProperErrorType()
    {
        var client = CreateMockHttpClient(HttpStatusCode.Forbidden, "Access Denied");
        var httpCore = new HttpCoreTestable(client);

        var ex = await Assert.ThrowsAsync<HttpCoreException>(() => httpCore.GetAsync<ProblemDetailsDto>("http://fail"));

        Assert.Equal(HttpStatusCode.Forbidden, ex.HttpStatusCode);
        Assert.Equal(HttpCoreErrorEnum.Forbidden, ex.ErrorType);
    }
    [Fact]
    /// <summary>
    /// Validates that invalid JSON in ValidateResult triggers a HttpCoreException of type InternalServerError.
    /// </summary>
    public async Task ValidateResult_ThrowsHttpCoreException_OnJsonDeserializationError()
    {
        var malformedJson = "Not a JSON string at all";
        var client = CreateMockHttpClient(HttpStatusCode.OK, malformedJson);
        var httpCore = new HttpCoreTestable(client);

        var ex = await Assert.ThrowsAsync<HttpCoreException>(() => httpCore.GetAsync<ProblemDetailsDto>("http://mock"));

        Assert.Equal(HttpStatusCode.InternalServerError, ex.HttpStatusCode);
        Assert.Equal(HttpCoreErrorEnum.InternalServerError, ex.ErrorType);
        Assert.Equal("JsonDeserializationError", ex.Key);
    }
    [Fact]
    /// <summary>
    /// Verifies that GetAsyncList returns a list of deserialized objects from JSON.
    /// </summary>
    public async Task GetAsyncList_ShouldReturnExpectedList()
    {
        var data = new List<string> { "Alpha", "Beta" };
        var json = JsonConvert.SerializeObject(data);
        var client = CreateMockHttpClient(HttpStatusCode.OK, json);
        var httpCore = new HttpCoreTestable(client);
        var result = await httpCore.GetAsyncList<string>("http://api/items");

        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
        Assert.Equal("Alpha", result[0]);
        Assert.Equal("Beta", result[1]);
    }
    [Fact]
    /// <summary>
    /// Verifies that GetAsyncList returns a correctly deserialized list of items when the response is valid.
    /// </summary>
    public async Task GetAsyncList_ReturnsDeserializedList()
    {
        try
        {
            var list = new List<string> { "Item1", "Item2" };
            var json = JsonConvert.SerializeObject(list);
            var client = CreateMockHttpClient(HttpStatusCode.OK, json);
            var httpCore = new HttpCoreTestable(client);
            var result = await httpCore.GetAsyncList<string>("http://test");

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("Item1", result[0]);
            Assert.Equal("Item2", result[1]);
        }
        catch (HttpCoreException ex)
        {
            Assert.Fail($"HttpCoreException thrown: {ex.Message} | {ex.ErrorMessage} | {ex.Reason}");
        }
    }
    [Fact]
    /// <summary>
    /// Ensures that PutAsync returns a deserialized result from a mocked HTTP response.
    /// </summary>
    public async Task PutAsync_ReturnsDeserializedResponse()
    {
        var requestData = new ProblemDetailsDto { Title = "Request", Status = 201 };
        var expectedResponse = new ProblemDetailsDto { Title = "Response", Status = 200 };
        var json = JsonConvert.SerializeObject(expectedResponse);
        var client = CreateMockHttpClient(HttpStatusCode.OK, json);
        var httpCore = new HttpCoreTestable(client);
        var result = await httpCore.PutAsync<ProblemDetailsDto, ProblemDetailsDto>("http://put-endpoint", requestData);

        Assert.NotNull(result);
        Assert.Equal("Response", result.Title);
        Assert.Equal(200, result.Status);
    }
    [Fact]
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

        Assert.NotNull(result);
        Assert.Equal("Deleted", result.Title);
        Assert.Equal(204, result.Status);
    }
    [Fact]
    /// <summary>
    /// Validates that an HttpCoreException is thrown for a 400 BadRequest with detailed content.
    /// </summary>
    public async Task ValidateStatusResponse_WithBadRequest_ShouldThrowWithContent()
    {
        var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
        {
            Content = new StringContent("Invalid input")
        };

        var ex = await Assert.ThrowsAsync<HttpCoreException>(() => HttpCoreForTest.CallValidateStatusResponse(response));

        Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
        Assert.Equal("Invalid input", ex.ErrorMessage);
        Assert.Equal("BadRequest", ex.ErrorType.ToString());
    }
    [Fact]
    /// <summary>
    /// Validates that an HttpCoreException is thrown for a 418 status code not in the enum.
    /// </summary>
    public async Task ValidateStatusResponse_WithUnknownStatus_ShouldThrowWithFallback()
    {
        var response = new HttpResponseMessage((HttpStatusCode)418)
        {
            ReasonPhrase = "I'm a teapot",
            Content = new StringContent("")
        };

        var ex = await Assert.ThrowsAsync<HttpCoreException>(() => HttpCoreForTest.CallValidateStatusResponse(response));

        Assert.Equal((HttpStatusCode)418, ex.HttpStatusCode);
        Assert.Equal("I'm a teapot", ex.ErrorMessage);
        Assert.Null(ex.ErrorType);
    }
    [Fact]
    /// <summary>
    /// Validates that ValidateStatusResponse does not throw when status is OK.
    /// </summary>
    public async Task ValidateStatusResponse_WithSuccessStatus_ShouldNotThrow()
    {
        var response = new HttpResponseMessage(HttpStatusCode.OK);
        await HttpCoreForTest.CallValidateStatusResponse(response);
    }
    [Fact]
    /// <summary>
    /// Ensures that PutAsync returns a deserialized result from a mocked HTTP response.
    /// </summary>
    public async Task PutAsync_ShouldReturnTypedResponse()
    {
        var requestData = new ProblemDetailsDto { Title = "Request", Status = 100 };
        var expected = new ProblemDetailsDto { Title = "Updated", Status = 200 };
        var json = JsonConvert.SerializeObject(expected);
        var client = CreateMockHttpClient(HttpStatusCode.OK, json);
        var httpCore = new HttpCoreTestable(client);
        var result = await httpCore.PutAsync<ProblemDetailsDto, ProblemDetailsDto>("http://api/update", requestData);

        Assert.NotNull(result);
        Assert.Equal("Updated", result.Title);
        Assert.Equal(200, result.Status);
    }
    [Fact]
    /// <summary>
    /// Tests that DeleteAsync returns the deserialized object from a successful response.
    /// </summary>
    public async Task DeleteAsync_ShouldReturnDeserializedResult()
    {
        var expected = new ProblemDetailsDto { Title = "Removed", Status = 204 };
        var json = JsonConvert.SerializeObject(expected);
        var client = CreateMockHttpClient(HttpStatusCode.OK, json);
        var httpCore = new HttpCoreTestable(client);
        var result = await httpCore.DeleteAsync<ProblemDetailsDto>("http://api/remove");

        Assert.NotNull(result);
        Assert.Equal("Removed", result.Title);
        Assert.Equal(204, result.Status);
    }
}