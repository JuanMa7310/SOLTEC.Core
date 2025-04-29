using SOLTEC.Core.Enums;

namespace SOLTEC.Core.Tests.NuNit;

/// <summary>
/// Unit tests for the HttpCoreErrorEnum enum using xUnit and NUnit.
/// </summary>
public class HttpCoreErrorEnumTests
{
    [Theory]
    [InlineData(400, HttpCoreErrorEnum.BadRequest)]
    [InlineData(401, HttpCoreErrorEnum.Unauthorized)]
    [InlineData(403, HttpCoreErrorEnum.Forbidden)]
    [InlineData(404, HttpCoreErrorEnum.NotFound)]
    [InlineData(405, HttpCoreErrorEnum.MethodNotAllowed)]
    [InlineData(409, HttpCoreErrorEnum.Conflict)]
    [InlineData(500, HttpCoreErrorEnum.InternalServerError)]
    [InlineData(501, HttpCoreErrorEnum.NotImplemented)]
    [InlineData(502, HttpCoreErrorEnum.BadGateway)]
    [InlineData(503, HttpCoreErrorEnum.ServiceUnavailable)]
    [InlineData(504, HttpCoreErrorEnum.GatewayTimeout)]
    /// <summary>
    /// Verifies that each HttpCoreErrorEnum value matches its expected HTTP status code.
    /// </summary>
    /// <param name="expectedValue">The expected HTTP status code (int).</param>
    /// <param name="enumValue">The enum value to validate.</param>
    /// <remarks>It tests the mapping between enum and numeric HTTP codes.</remarks>
    public void Enum_HasCorrectIntegerValue(int expectedValue, HttpCoreErrorEnum enumValue)
    {
        Assert.Equal(expectedValue, (int)enumValue);
    }
}