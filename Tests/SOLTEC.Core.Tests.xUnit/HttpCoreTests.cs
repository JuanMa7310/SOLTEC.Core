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
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            });

        return new HttpClient(handlerMock.Object);
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

        try
        {
            var _ = await httpCore.GetAsync<ProblemDetailsDto>("http://mock");

            Assert.Fail("Expected HttpCoreException was not thrown."); 
        }
        catch (HttpCoreException ex)
        {
            Assert.Equal(HttpStatusCode.BadRequest, ex.HttpStatusCode);
            Assert.Equal("Custom Title", ex.Key);
            Assert.Equal("Custom Type", ex.Reason);
            Assert.Equal("Detailed message", ex.ErrorMessage);
            Assert.Equal(HttpCoreErrorEnum.BadRequest, ex.ErrorType);
        }
    }

    [Fact]
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
            Assert.Equal(HttpStatusCode.Forbidden, ex.HttpStatusCode);
            Assert.Equal(HttpCoreErrorEnum.Forbidden, ex.ErrorType);
        }
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

        try
        {
            var _ = await httpCore.GetAsync<ProblemDetailsDto>("http://mock");

            Assert.Fail("Expected HttpCoreException was not thrown.");
        }
        catch (HttpCoreException ex)
        {
            Assert.Equal(HttpStatusCode.InternalServerError, ex.HttpStatusCode);
            Assert.Equal(HttpCoreErrorEnum.InternalServerError, ex.ErrorType);
            Assert.Equal("JsonDeserializationError", ex.Key);
        }
    }

    private class HttpCoreTestable(HttpClient client) : HttpCore
    {
        protected override HttpClient CreateConfiguredHttpClient(Dictionary<string, string>? headers)
            => client;
    }
}
