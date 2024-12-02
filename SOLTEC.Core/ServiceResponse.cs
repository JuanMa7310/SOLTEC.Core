using System.Net;

namespace SOLTEC.Core;

public class ServiceResponse
{
    public bool Success { get; set; }

    public string Message { get; set; }

    public string ErrorMessage { get; set; }

    public string[]? WarningMessages { get; set; }

    public int ResponseCode { get; set; }

    public ServiceResponse()
    {
        Success = true;
        Message = null;
        ErrorMessage = null;
    }

    public static ServiceResponse CreateSuccess(int responseCode) => CreateSuccess(responseCode, null);
    public static ServiceResponse CreateSuccess(HttpStatusCode responseCode) => CreateSuccess(responseCode, null);

    public static ServiceResponse CreateSuccess(int responseCode, string[]? warningMessages)
    {
        return new ServiceResponse()
        {
            Success = true,
            ResponseCode = responseCode,
            WarningMessages = warningMessages,
        };
    }

    public static ServiceResponse CreateSuccess(HttpStatusCode responseCode, string[] warningMessages)
    {
        return new ServiceResponse()
        {
            Success = true,
            ResponseCode = (int)responseCode,
            WarningMessages = warningMessages,
        };
    }

    public static ServiceResponse CreateError(int responseCode, string errorMessage) => CreateError(responseCode, errorMessage, null);

    public static ServiceResponse CreateError(HttpStatusCode responseCode, string errorMessage) => CreateError(responseCode, errorMessage, null);

    public static ServiceResponse CreateError(int responseCode, string errorMessage, string[] warningMessages)
    {
        return new ServiceResponse()
        {
            Success = false,
            ResponseCode = responseCode,
            ErrorMessage = errorMessage,
            WarningMessages = warningMessages,
        };
    }

    public static ServiceResponse CreateError(HttpStatusCode responseCode, string errorMessage, string[] warningMessages)
    {
        return new ServiceResponse()
        {
            Success = false,
            ResponseCode = (int)responseCode,
            ErrorMessage = errorMessage,
            WarningMessages = warningMessages,
        };
    }
}

public class ServiceResponse<T> : ServiceResponse
{
    public T Data { get; set; }

    public ServiceResponse() : base()
    {
        Data = default;
    }

    public ServiceResponse(T data) : base()
    {
        Data = data;
    }

    public static ServiceResponse<T> CreateSuccess(T data, int responseCode) => ServiceResponse<T>.CreateSuccess(data, responseCode, null);

    public static ServiceResponse<T> CreateSuccess(T data, HttpStatusCode responseCode) => ServiceResponse<T>.CreateSuccess(data, responseCode, null);

    public static ServiceResponse<T> CreateSuccess(T data, int responseCode, string[] warningMessages)
    {
        return new ServiceResponse<T>(data)
        {
            Success = true,
            ResponseCode = responseCode,
            WarningMessages = warningMessages,
        };
    }

    public static ServiceResponse<T> CreateSuccess(T data, HttpStatusCode responseCode, string[] warningMessages)
    {
        return new ServiceResponse<T>(data)
        {
            Success = true,
            ResponseCode = (int)responseCode,
            WarningMessages = warningMessages,
        };
    }

    public static ServiceResponse<T> CreateError(int responseCode, string errorMessage) => ServiceResponse<T>.CreateError(responseCode, errorMessage, null);

    public static ServiceResponse<T> CreateError(HttpStatusCode responseCode, string errorMessage) => ServiceResponse<T>.CreateError(responseCode, errorMessage, null);

    public static ServiceResponse<T> CreateError(int responseCode, string errorMessage, string[] warningMessages)
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

    public static ServiceResponse<T> CreateError(HttpStatusCode responseCode, string errorMessage, string[] warningMessages)
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