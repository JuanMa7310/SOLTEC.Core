using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SOLTEC.Core.IntegrationTests.xUnit.Helpers;

/// <summary>
/// Mock handler for simulating HTTP responses.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MockHttpMessageHandler"/> class.
/// </remarks>
/// <param name="response">The response to return when a request is sent.</param>
public class MockHttpMessageHandler(HttpResponseMessage response) : HttpMessageHandler
{

    /// <summary>
    /// Sends an HTTP request and returns a preconfigured response.
    /// </summary>
    /// <param name="request">The HTTP request message.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The predefined HTTP response message.</returns>
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return Task.FromResult(response);
    }
}
