namespace SOLTEC.Core;

using System.Net;

/// <summary>
/// Represents a generic version of <see cref="ServiceResponse"/>, allowing the response to include data of any type.
/// </summary>
/// <typeparam name="T">The type of data to be included in the response.</typeparam>
/// <example>
/// Example 1: Creating a successful response with data
/// <![CDATA[
/// var data = new User { Id = 1, Name = "John" };
/// var response = ServiceResponse&lt;User&gt;.CreateSuccess(data, 200);
/// ]]>
/// </example>
/// <example>
/// Example 2: Creating an error response.
/// <![CDATA[
/// var response = ServiceResponse&lt;string&gt;.CreateError(400, "Invalid request");
/// ]]>
/// </example>
public class ServiceResponse<T> : ServiceResponse
{
    /// <summary>
    /// The data returned in the response.
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class.
    /// </summary>
    public ServiceResponse() : base()
    {
        Data = default;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResponse{T}"/> class with specified data.
    /// </summary>
    /// <param name="data">The data to include in the response.</param>
    public ServiceResponse(T data) : base()
    {
        Data = data;
    }

    /// <summary>
    /// Creates a successful response with data and response code.
    /// </summary>
    /// <param name="data">The data to include.</param>
    /// <param name="responseCode">The response code.</param>
    /// <returns>A new successful response.</returns>
    /// <example>
    /// <![CDATA[
    /// var result = ServiceResponse<string>.CreateSuccess("Done", 200);
    /// ]]>
    /// </example>
    public static ServiceResponse<T> CreateSuccess(T data, int responseCode) => CreateSuccess(data, responseCode, null);
    /// <summary>
    /// Creates a successful response with data and HTTP status code.
    /// </summary>
    /// <param name="data">The data to include.</param>
    /// <param name="responseCode">The HTTP status code.</param>
    /// <returns>A new successful response.</returns>
    /// <example>
    /// <![CDATA[
    /// var response = ServiceResponse.CreateSuccess("OK", HttpStatusCode.OK);
    /// ]]>
    /// </example>
    public static ServiceResponse<T> CreateSuccess(T data, HttpStatusCode responseCode) => CreateSuccess(data, responseCode, null);
    /// <summary>
    /// Creates a successful response with data, code and warnings.
    /// </summary>
    /// <param name="data">The data to include.</param>
    /// <param name="responseCode">The response code.</param>
    /// <param name="warningMessages">Optional warning messages.</param>
    /// <returns>A new successful response.</returns>
    /// <example>
    /// <![CDATA[
    /// var response = ServiceResponse.CreateSuccess("OK", 200, null);
    /// ]]>
    /// </example>
    public static ServiceResponse<T> CreateSuccess(T data, int responseCode, string[]? warningMessages)
    {
        return new ServiceResponse<T>(data)
        {
            Success = true,
            ResponseCode = responseCode,
            WarningMessages = warningMessages,
        };
    }
    /// <summary>
    /// Creates a successful response with data, HTTP code and warnings.
    /// </summary>
    /// <param name="data">The data to include.</param>
    /// <param name="responseCode">The HTTP status code.</param>
    /// <param name="warningMessages">Optional warning messages.</param>
    /// <returns>A new successful response.</returns>
    /// <example>
    /// <![CDATA[
    /// var response = ServiceResponse.CreateSuccess("OK", HttpStatusCode.OK, null);
    /// ]]>
    /// </example>
    public static ServiceResponse<T> CreateSuccess(T data, HttpStatusCode responseCode, string[]? warningMessages)
    {
        return new ServiceResponse<T>(data)
        {
            Success = true,
            ResponseCode = (int)responseCode,
            WarningMessages = warningMessages,
        };
    }

    /// <summary>
    /// Creates an error response with code and error message.
    /// </summary>
    /// <param name="responseCode">The response code.</param>
    /// <param name="errorMessage">The error message to include.</param>
    /// <returns>A new error response.</returns>
    /// <example>
    /// <![CDATA[
    /// var error = ServiceResponse<string>.CreateError(404, "Not found");
    /// ]]>
    /// </example>
    public new static ServiceResponse<T> CreateError(int responseCode, string errorMessage) => CreateError(responseCode, errorMessage, null);
    /// <summary>
    /// Creates an error response with HTTP code and error message.
    /// </summary>
    /// <param name="responseCode">The HTTP status code.</param>
    /// <param name="errorMessage">The error message to include.</param>
    /// <returns>A new error response.</returns>
    /// <example>
    /// <![CDATA[
    /// var error = ServiceResponse<string>.CreateError(HttpStatusCode.NotFound, "Not found");
    /// ]]>
    /// </example>
    public new static ServiceResponse<T> CreateError(HttpStatusCode responseCode, string errorMessage) => CreateError((int)responseCode, errorMessage, null);
    /// <summary>
    /// Creates an error response with code, message and warnings.
    /// </summary>
    /// <param name="responseCode">The response code.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="warningMessages">Optional warning messages.</param>
    /// <returns>A new error response.</returns>
    /// <example>
    /// <![CDATA[
    /// var error = ServiceResponse<string>.CreateError(404, "Not found", null);
    /// ]]>
    /// </example>
    public new static ServiceResponse<T> CreateError(int responseCode, string errorMessage, string[]? warningMessages)
    {
        return new ServiceResponse<T>()
        {
            Success = false,
            ResponseCode = responseCode,
            ErrorMessage = errorMessage,
            WarningMessages = warningMessages,
            Data = default,
        };
    }
    /// <summary>
    /// Creates an error response with HTTP code, message and warnings.
    /// </summary>
    /// <param name="responseCode">The HTTP status code.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="warningMessages">Optional warning messages.</param>
    /// <returns>A new error response.</returns>
    /// <example>
    /// <![CDATA[
    /// var error = ServiceResponse<string>.CreateError(HttpStatusCode.NotFound, "Not found", null);
    /// ]]>
    /// </example>
    public new static ServiceResponse<T> CreateError(HttpStatusCode responseCode, string errorMessage, string[]? warningMessages)
    {
        return new ServiceResponse<T>()
        {
            Success = false,
            ResponseCode = (int)responseCode,
            ErrorMessage = errorMessage,
            WarningMessages = warningMessages,
            Data = default,
        };
    }

    /// <summary>
    /// Creates an warning response with code and error message.
    /// </summary>
    /// <param name="responseCode">The response code.</param>
    /// <param name="errorMessage">The warning message to include.</param>
    /// <returns>A new warning response.</returns>
    /// <example>
    /// <![CDATA[
    /// var response = ServiceResponse.CreateWarning(206, "Partial result");
    /// ]]>
    /// </example>
    public new static ServiceResponse<T> CreateWarning(int responseCode, string errorMessage) => CreateWarning(responseCode, errorMessage, null);
    /// <summary>
    /// Creates an warning response with HTTP code and error message.
    /// </summary>
    /// <param name="responseCode">The HTTP status code.</param>
    /// <param name="errorMessage">The warning message to include.</param>
    /// <returns>A new erwarningror response.</returns>
    /// <example>
    /// <![CDATA[
    /// var response = ServiceResponse.CreateWarning(HttpStatusCode.PartialContent, "Partial result");
    /// ]]>
    /// </example>
    public new static ServiceResponse<T> CreateWarning(HttpStatusCode responseCode, string errorMessage) => CreateWarning((int)responseCode, errorMessage, null);
    /// <summary>
    /// Creates an warning response with code, message and warnings.
    /// </summary>
    /// <param name="responseCode">The response code.</param>
    /// <param name="errorMessage">The warning message.</param>
    /// <param name="warningMessages">Optional warning messages.</param>
    /// <returns>A new error response.</returns>
    /// <example>
    /// <![CDATA[
    /// var response = ServiceResponse.CreateWarning(206, "Partial result", new[] { "Partial data", "Rate limit applied" });
    /// ]]>
    /// </example>
    public new static ServiceResponse<T> CreateWarning(int responseCode, string errorMessage, string[]? warningMessages)
    {
        return new ServiceResponse<T>()
        {
            Success = true,
            ResponseCode = responseCode,
            Message = errorMessage,
            WarningMessages = warningMessages,
            Data = default,
        };
    }
    /// <summary>
    /// Creates an warning response with HTTP code, message and warnings.
    /// </summary>
    /// <param name="responseCode">The HTTP status code></param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="warningMessgaes">Optional warning messages.</param>
    /// <returns>A new warning response.</returns>
    /// <example>
    /// <![CDATA[
    /// var response = ServiceResponse.CreateWarning(HttpStatusCode.PartialContent, "Partial result", new[] { "Partial data", "Rate limit applied" });
    /// ]]>
    /// </example>
    public new static ServiceResponse<T> CreateWarning(HttpStatusCode responseCode, string errorMessage, string[]? warningMessgaes) 
    {
        return new ServiceResponse<T>()
        {
            Success = true,
            Message = errorMessage,
            ResponseCode = (int)responseCode,
            WarningMessages = warningMessgaes,
            Data = default
        };
    }
}