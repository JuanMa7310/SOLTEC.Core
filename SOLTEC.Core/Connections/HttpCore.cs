using Newtonsoft.Json;
using SOLTEC.Core.Connections.Exceptions;
using SOLTEC.Core.DTOS;
using SOLTEC.Core.Enums;
using System.Net;
using System.Security.Authentication;
using System.Text;

namespace SOLTEC.Core.Connections;

/// <summary>
/// Provides utility methods for performing HTTP operations (GET, POST, PUT, DELETE) with optional headers and validation.
/// </summary>
/// <example>
/// Example usage:
/// <![CDATA[
/// var client = new HttpCore();
/// var result = await client.GetAsync&lt;MyDto&gt;("https://api.example.com/data");
/// ]]>
/// </example>
public class HttpCore
{
    /// <summary>
    /// Sends an HTTP GET request and returns the deserialized response.
    /// </summary>
    /// <typeparam name="T">The type of the response object.</typeparam>
    /// <param name="uri">The target URI.</param>
    /// <param name="headerParameters">Optional headers to include in the request.</param>
    /// <returns>Deserialized object of type <typeparamref name="T"/>.</returns>
    /// <example>
    /// <![CDATA[
    /// var response = await new HttpCore().GetAsync&lt;User&gt;("https://api.example.com/users/1");
    /// ]]>
    /// </example>
    public virtual async Task<T?> GetAsync<T>(string uri, Dictionary<string, string>? headerParameters = null)
    {
        using var _client = CreateConfiguredHttpClient(headerParameters);
        var _response = await _client.GetAsync(uri);

        await ValidateStatusResponse(_response);

        var _result = await _response.Content.ReadAsStringAsync();

        ValidateResult(_result);

        return JsonConvert.DeserializeObject<T>(_result);
    }
    /// <summary>
    /// Sends an HTTP GET request and returns a deserialized list of objects.
    /// </summary>
    /// <typeparam name="T">The type of each item in the list.</typeparam>
    /// <param name="uri">The target URI.</param>
    /// <param name="headerParameters">Optional headers to include in the request.</param>
    /// <returns>A list of <typeparamref name="T"/> objects.</returns>
    public virtual async Task<IList<T>?> GetAsyncList<T>(string uri, Dictionary<string, string>? headerParameters = null)
    {
        using var _client = CreateConfiguredHttpClient(headerParameters);
        var _response = await _client.GetAsync(uri);

        await ValidateStatusResponse(_response);

        var _result = await _response.Content.ReadAsStringAsync();

        ValidateResult(_result);

        return JsonConvert.DeserializeObject<IList<T>>(_result);
    }
    /// <summary>
    /// Sends an HTTP POST request with no content.
    /// </summary>
    /// <param name="uri">The target URI.</param>
    /// <param name="headerParameters">Optional headers to include in the request.</param>
    /// <example>
    /// <![CDATA[
    /// var response = await new HttpCore().PostAsync&lt;User&gt;("https://api.example.com/users", newUser);
    /// ]]>
    /// </example>
    public virtual async Task PostAsync(string uri, Dictionary<string, string>? headerParameters = null)
    {
        using var _client = CreateConfiguredHttpClient(headerParameters);
        var _response = await _client.PostAsync(uri, null);

        await ValidateStatusResponse(_response);
    }
    /// <summary>
    /// Sends an HTTP POST request with a serialized request body.
    /// </summary>
    /// <typeparam name="T">The type of the request body.</typeparam>
    /// <param name="uri">The target URI.</param>
    /// <param name="request">The request object to be serialized.</param>
    /// <param name="headerParameters">Optional headers to include in the request.</param>
    public virtual async Task PostAsync<T>(string uri, T request, Dictionary<string, string>? headerParameters = null)
    {
        using var _client = CreateConfiguredHttpClient(headerParameters);
        using var _content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var _response = await _client.PostAsync(uri, _content);

        await ValidateStatusResponse(_response);
    }
    /// <summary>
    /// Sends an HTTP POST request and returns a deserialized response.
    /// </summary>
    /// <typeparam name="T">The type of the request body.</typeparam>
    /// <typeparam name="TResult">The type of the expected response.</typeparam>
    /// <param name="uri">The target URI.</param>
    /// <param name="request">The request object to be serialized.</param>
    /// <param name="headerParameters">Optional headers to include in the request.</param>
    /// <returns>The deserialized response object.</returns>
    public virtual async Task<TResult?> PostAsync<T, TResult>(string uri, T request, Dictionary<string, string>? headerParameters = null)
    {
        using var _client = CreateConfiguredHttpClient(headerParameters);
        using var _content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var _response = await _client.PostAsync(uri, _content);

        await ValidateStatusResponse(_response);

        var _result = await _response.Content.ReadAsStringAsync();

        ValidateResult(_result);

        return JsonConvert.DeserializeObject<TResult>(_result);
    }
    /// <summary>
    /// Sends an HTTP POST request with no content and returns a deserialized response.
    /// </summary>
    /// <typeparam name="TResult">The type of the expected response.</typeparam>
    /// <param name="uri">The target URI.</param>
    /// <param name="headerParameters">Optional headers to include in the request.</param>
    /// <returns>The deserialized response object.</returns>
    public virtual async Task<TResult?> PostAsync<TResult>(string uri, Dictionary<string, string>? headerParameters = null)
    {
        using var _client = CreateConfiguredHttpClient(headerParameters);
        var _response = await _client.PostAsync(uri, null);

        await ValidateStatusResponse(_response);

        var _result = await _response.Content.ReadAsStringAsync();

        ValidateResult(_result);

        return JsonConvert.DeserializeObject<TResult>(_result);
    }
    /// <summary>
    /// Sends an HTTP PUT request with no content.
    /// </summary>
    /// <param name="uri">The target URI.</param>
    /// <param name="headerParameters">Optional headers to include in the request.</param>
    /// <example>
    /// <![CDATA[
    /// var response = await new HttpCore().PutAsync&lt;User&gt;("https://api.example.com/users/1", updatedUser);
    /// ]]>
    /// </example>
    public virtual async Task PutAsync(string uri, Dictionary<string, string>? headerParameters = null)
    {
        using var _client = CreateConfiguredHttpClient(headerParameters);
        var _response = await _client.PutAsync(uri, null);

        await ValidateStatusResponse(_response);
    }
    /// <summary>
    /// Sends an HTTP PUT request with a serialized request body.
    /// </summary>
    /// <typeparam name="T">The type of the request body.</typeparam>
    /// <param name="uri">The target URI.</param>
    /// <param name="request">The object to send.</param>
    /// <param name="headerParameters">Optional headers to include in the request.</param>
    public virtual async Task PutAsync<T>(string uri, T request, Dictionary<string, string>? headerParameters = null)
    {
        using var _client = CreateConfiguredHttpClient(headerParameters);
        using var _content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var _response = await _client.PutAsync(uri, _content);

        await ValidateStatusResponse(_response);
    }
    /// <summary>
    /// Sends an HTTP PUT request and returns a deserialized response.
    /// </summary>
    /// <typeparam name="T">The type of the request body.</typeparam>
    /// <typeparam name="TResult">The type of the expected response.</typeparam>
    /// <param name="uri">The target URI.</param>
    /// <param name="request">The object to send.</param>
    /// <param name="headerParameters">Optional headers to include in the request.</param>
    /// <returns>The deserialized response object.</returns>
    public virtual async Task<TResult?> PutAsync<T, TResult>(string uri, T request, Dictionary<string, string>? headerParameters = null)
    {
        using var _client = CreateConfiguredHttpClient(headerParameters);
        using var _content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
        var _response = await _client.PutAsync(uri, _content);

        await ValidateStatusResponse(_response);

        var _result = await _response.Content.ReadAsStringAsync();

        ValidateResult(_result);

        return JsonConvert.DeserializeObject<TResult>(_result);
    }
    /// <summary>
    /// Sends an HTTP PUT request with no content and returns a deserialized response.
    /// </summary>
    /// <typeparam name="TResult">The type of the expected response.</typeparam>
    /// <param name="uri">The target URI.</param>
    /// <param name="headerParameters">Optional headers to include in the request.</param>
    /// <returns>The deserialized response object.</returns>
    public virtual async Task<TResult?> PutAsync<TResult>(string uri, Dictionary<string, string>? headerParameters = null)
    {
        using var _client = CreateConfiguredHttpClient(headerParameters);
        var _response = await _client.PutAsync(uri, null);

        await ValidateStatusResponse(_response);

        var _result = await _response.Content.ReadAsStringAsync();

        ValidateResult(_result);

        return JsonConvert.DeserializeObject<TResult>(_result);
    }
    /// <summary>
    /// Sends an HTTP DELETE request and returns a deserialized response.
    /// </summary>
    /// <typeparam name="TResult">The type of the expected response.</typeparam>
    /// <param name="uri">The target URI.</param>
    /// <param name="headerParameters">Optional headers to include in the request.</param>
    /// <returns>The deserialized response object.</returns>
    /// <example>
    /// <![CDATA[
    /// var result = await new HttpCore().DeleteAsync<User>("https://api.example.com/users/1");
    /// ]]>
    /// </example>
    public virtual async Task<TResult?> DeleteAsync<TResult>(string uri, Dictionary<string, string>? headerParameters = null)
    {
        using var _client = CreateConfiguredHttpClient(headerParameters);
        var _response = await _client.DeleteAsync(uri);

        await ValidateStatusResponse(_response);

        var _result = await _response.Content.ReadAsStringAsync();

        ValidateResult(_result);

        return JsonConvert.DeserializeObject<TResult>(_result);
    }

    /// <summary>
    /// Validates the status of an HTTP response and throws a <see cref="HttpCoreException"/>
    /// if the response indicates an error (i.e., status code is not successful).
    /// </summary>
    /// <param name="response">The <see cref="HttpResponseMessage"/> returned by an HTTP operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous validation operation.</returns>
    /// <exception cref="HttpCoreException">
    /// Thrown when the response status code is not successful. Includes mapped <see cref="HttpCoreErrorEnum"/>
    /// if the status code is recognized.
    /// </exception>
    /// <example>
    /// <![CDATA[
    /// var response = await httpClient.GetAsync("https://api.example.com/resource"); 
    /// await ValidateStatusResponse(response);
    /// ]]>
    /// </example>
    protected static async Task ValidateStatusResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var _content = await response.Content.ReadAsStringAsync();

            HttpCoreErrorEnum? _errorType = Enum.IsDefined(typeof(HttpCoreErrorEnum), (int)response.StatusCode)
                ? (HttpCoreErrorEnum)(int)response.StatusCode
                : null;

            throw new HttpCoreException(nameof(HttpCoreException),
                                        _errorType?.ToString() ?? "Unhandled",
                                        response.StatusCode,
                                        response.StatusCode == HttpStatusCode.BadRequest
                                            ? _content
                                            : response.ReasonPhrase ?? _content,
                                        _errorType);
        }
    }
    /// <summary>
    /// Validates the response content by attempting to deserialize it as a ProblemDetailsDto.
    /// If deserialization succeeds and contains an error status, throws a HttpCoreException.
    /// </summary>
    /// <param name="result">The HTTP response content as a string.</param>
    /// <exception cref="HttpCoreException">Thrown if the response contains a known error structure.</exception>
    /// <example>
    /// <![CDATA[
    /// var response = await client.GetStringAsync("http://api/test");
    /// ValidateResult(response);
    /// ]]>
    /// </example>
    protected static void ValidateResult(string result)
    {
        if (string.IsNullOrWhiteSpace(result))
        {
            return;
        }
        try
        {
            var _problem = JsonConvert.DeserializeObject<ProblemDetailsDto>(result);

            if (_problem?.Status != null && Enum.IsDefined(typeof(HttpCoreErrorEnum), _problem.Status))
            {
                throw new HttpCoreException(
                    _problem.Title ?? "Unknown Title",
                    _problem.Type ?? "Unknown Type",
                    (HttpStatusCode)_problem.Status,
                    _problem.Detail ?? "No additional details provided.",
                    (HttpCoreErrorEnum)_problem.Status
                );
            }
        }
        catch (JsonException _exception)
        {
            throw new HttpCoreException(
                key: "JsonDeserializationError",
                reason: _exception.GetType().Name,
                httpStatusCode: HttpStatusCode.InternalServerError,
                errorMessage: _exception.Message,
                errorType: HttpCoreErrorEnum.InternalServerError
            );
        }
    }
    /// <summary>
    /// Creates a configured HttpClient with optional headers.
    /// </summary>
    /// <param name="headers">Headers to add to the client.</param>
    /// <returns>Configured HttpClient instance.</returns>
    protected virtual HttpClient CreateConfiguredHttpClient(Dictionary<string, string>? headers)
    {
        var _handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
            SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13
        };
        var _client = new HttpClient(_handler);

        if (headers is not null)
        {
            foreach (var _item in headers)
            {
                _client.DefaultRequestHeaders.Add(_item.Key, _item.Value);
            }
        }

        return _client;
    }
}