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
    private HttpClient CreateMockHttpClient(HttpStatusCode statusCode, string content)
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

    private class HttpCoreTestable(HttpClient client) : HttpCore
    {
        protected override HttpClient CreateConfiguredHttpClient(Dictionary<string, string>? headers)
            => client;
    }
}
