﻿using System.Net;

namespace SOLTEC.Core;

/// <summary>
/// Represents a standardized structure for service responses, including success or error information.
/// </summary>
/// <example>
/// Example of creating a successful response:
/// <code>
/// var response = ServiceResponse.CreateSuccess(200);
/// </code>
/// 
/// Example of creating an error response:
/// <code>
/// var response = ServiceResponse.CreateError(500, "Internal server error");
/// </code>
/// </example>
public class ServiceResponse
{
    /// <summary>
    /// Indicates whether the operation was successful.
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Optional success message.
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// Optional error message in case of failure.
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Optional warning messages.
    /// </summary>
    public string[]? WarningMessages { get; set; }

    /// <summary>
    /// HTTP response code or custom response code.
    /// </summary>
    public int ResponseCode { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ServiceResponse"/> class with default values.
    /// </summary>
    public ServiceResponse()
    {
        Success = true;
        Message = null;
        ErrorMessage = null;
    }

    /// <summary>
    /// Creates a successful response.
    /// </summary>
    /// <param name="responseCode">The HTTP status or custom code.</param>
    /// <returns>A new successful <see cref="ServiceResponse"/>.</returns>
    public static ServiceResponse CreateSuccess(int responseCode) => CreateSuccess(responseCode, null);

    /// <summary>
    /// Creates a successful response from an HTTP status code.
    /// </summary>
    /// <param name="responseCode">The HTTP status code.</param>
    /// <returns>A new successful <see cref="ServiceResponse"/>.</returns>
    public static ServiceResponse CreateSuccess(HttpStatusCode responseCode) => CreateSuccess((int)responseCode, null);

    /// <summary>
    /// Creates a successful response with optional warning messages.
    /// </summary>
    /// <param name="responseCode">The HTTP status or custom code.</param>
    /// <param name="warningMessages">An array of warning messages.</param>
    /// <returns>A new successful <see cref="ServiceResponse"/>.</returns>
    public static ServiceResponse CreateSuccess(int responseCode, string[]? warningMessages)
    {
        return new ServiceResponse
        {
            Success = true,
            ResponseCode = responseCode,
            WarningMessages = warningMessages,
        };
    }

    /// <summary>
    /// Creates a successful response from an HTTP status code with warning messages.
    /// </summary>
    /// <param name="responseCode">The HTTP status code.</param>
    /// <param name="warningMessages">An array of warning messages.</param>
    /// <returns>A new successful <see cref="ServiceResponse"/>.</returns>
    public static ServiceResponse CreateSuccess(HttpStatusCode responseCode, string[] warningMessages)
    {
        return new ServiceResponse
        {
            Success = true,
            ResponseCode = (int)responseCode,
            WarningMessages = warningMessages,
        };
    }

    /// <summary>
    /// Creates an error response.
    /// </summary>
    /// <param name="responseCode">The HTTP status or custom code.</param>
    /// <param name="errorMessage">The error message to include.</param>
    /// <returns>A new error <see cref="ServiceResponse"/>.</returns>
    public static ServiceResponse CreateError(int responseCode, string errorMessage) => CreateError(responseCode, errorMessage, null);

    /// <summary>
    /// Creates an error response from an HTTP status code.
    /// </summary>
    /// <param name="responseCode">The HTTP status code.</param>
    /// <param name="errorMessage">The error message to include.</param>
    /// <returns>A new error <see cref="ServiceResponse"/>.</returns>
    public static ServiceResponse CreateError(HttpStatusCode responseCode, string errorMessage) => CreateError((int)responseCode, errorMessage, null);

    /// <summary>
    /// Creates an error response with optional warning messages.
    /// </summary>
    /// <param name="responseCode">The HTTP status or custom code.</param>
    /// <param name="errorMessage">The error message to include.</param>
    /// <param name="warningMessages">An array of warning messages.</param>
    /// <returns>A new error <see cref="ServiceResponse"/>.</returns>
    public static ServiceResponse CreateError(int responseCode, string errorMessage, string[]? warningMessages)
    {
        return new ServiceResponse
        {
            Success = false,
            ResponseCode = responseCode,
            ErrorMessage = errorMessage,
            WarningMessages = warningMessages,
        };
    }

    /// <summary>
    /// Creates an error response from an HTTP status code with optional warning messages.
    /// </summary>
    /// <param name="responseCode">The HTTP status code.</param>
    /// <param name="errorMessage">The error message to include.</param>
    /// <param name="warningMessages">An array of warning messages.</param>
    /// <returns>A new error <see cref="ServiceResponse"/>.</returns>
    public static ServiceResponse CreateError(HttpStatusCode responseCode, string errorMessage, string[] warningMessages)
    {
        return new ServiceResponse
        {
            Success = false,
            ResponseCode = (int)responseCode,
            ErrorMessage = errorMessage,
            WarningMessages = warningMessages,
        };
    }
}