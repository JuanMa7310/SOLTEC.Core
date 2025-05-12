using SOLTEC.Core.Connections;
using SOLTEC.Core.Connections.Commands;
using SOLTEC.Core.IntegrationTests.xUnit.Helpers;
using System.Net;
using System.Text;
using Xunit;

namespace SOLTEC.Core.IntegrationTests.xUnit;

/// <summary>
/// Integration tests for the SoapCore class using mocked HTTP client.
/// </summary>
public class SoapCoreIntegrationAdvancedTests
{
    [Fact]
    /// <summary>
    /// Tests that a valid SOAP response is correctly deserialized using a mocked HTTP client.
    /// </summary>
    public async Task Post_ShouldDeserializeValidSoapResponse_WhenMockedHttpClient()
    {
        // Arrange
        var _soapResponse = @"<?xml version=""1.0"" encoding=""utf-8""?>
<soap:Envelope xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
    <soap:Body>
        <TestResponse><Result>Success</Result></TestResponse>
    </soap:Body>
</soap:Envelope>";

        var _mockResponse = new HttpResponseMessage(HttpStatusCode.OK)
        {
            Content = new StringContent(_soapResponse, Encoding.UTF8, "text/xml")
        };

        var _mockHandler = new MockHttpMessageHandler(_mockResponse);
        var _httpClient = new HttpClient(_mockHandler);

        var _soapCore = new SoapCoreForTest(_httpClient);

        var _command = new SoapCommand(
            "https://example.com/soap",
            "ActionName",
            "TestResponse",
            "http://example.com/namespace",
            "username",
            "password",
            []
        );

        // Act
        var _result = await _soapCore.Post<TestResponse>(_command);

        // Assert
        Assert.Equal("Success", _result.Result);
    }

    /// <summary>
    /// Temporary subclass to inject mocked HttpClient.
    /// </summary>
    private class SoapCoreForTest(HttpClient httpClient) : SoapCore(httpClient)
    {
    }

    /// <summary>
    /// Test model for response.
    /// </summary>
    public class TestResponse
    {
        public string Result { get; set; }
    }
}
