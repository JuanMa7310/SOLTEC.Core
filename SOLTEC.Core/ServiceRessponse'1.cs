using System.Net;

namespace SOLTEC.Core;

/// <summary>
/// Represents a generic version of <see cref="ServiceResponse"/>, allowing the response to include data of any type.
/// </summary>
/// <typeparam name="T">The type of data to be included in the response.</typeparam>
/// <example>
/// Example of creating a successful response with data:
/// <code>
/// var data = new User { Id = 1, Name = "John" };
/// var response = ServiceResponse&lt;User&gt;.CreateSuccess(data, 200);
/// </code>
/// </example>
/// <example>
/// Example of creating an error response:
/// <code>
/// var response = ServiceResponse&lt;string&gt;.CreateError(400, "Invalid request");
/// </code>
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
    public static ServiceResponse<T> CreateSuccess(T data, int responseCode) => CreateSuccess(data, responseCode, null);

    /// <summary>
    /// Creates a successful response with data and HTTP status code.
    /// </summary>
    /// <param name="data">The data to include.</param>
    /// <param name="responseCode">The HTTP status code.</param>
    /// <returns>A new successful response.</returns>
    public static ServiceResponse<T> CreateSuccess(T data, HttpStatusCode responseCode) => CreateSuccess(data, responseCode, null);

    /// <summary>
    /// Creates a successful response with data, code and warnings.
    /// </summary>
    /// <param name="data">The data to include.</param>
    /// <param name="responseCode">The response code.</param>
    /// <param name="warningMessages">Optional warning messages.</param>
    /// <returns>A new successful response.</returns>
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
    public static ServiceResponse<T> CreateError(int responseCode, string errorMessage) => CreateError(responseCode, errorMessage, null);

    /// <summary>
    /// Creates an error response with HTTP code and error message.
    /// </summary>
    /// <param name="responseCode">The HTTP status code.</param>
    /// <param name="errorMessage">The error message to include.</param>
    /// <returns>A new error response.</returns>
    public static ServiceResponse<T> CreateError(HttpStatusCode responseCode, string errorMessage) => CreateError((int)responseCode, errorMessage, null);

    /// <summary>
    /// Creates an error response with code, message and warnings.
    /// </summary>
    /// <param name="responseCode">The response code.</param>
    /// <param name="errorMessage">The error message.</param>
    /// <param name="warningMessages">Optional warning messages.</param>
    /// <returns>A new error response.</returns>
    public static ServiceResponse<T> CreateError(int responseCode, string errorMessage, string[]? warningMessages)
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
    public static ServiceResponse<T> CreateError(HttpStatusCode responseCode, string errorMessage, string[]? warningMessages)
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
}