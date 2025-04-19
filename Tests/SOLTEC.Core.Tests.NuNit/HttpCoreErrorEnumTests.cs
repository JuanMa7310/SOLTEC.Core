using SOLTEC.Core.Enums;

namespace SOLTEC.Core.Tests.NuNit;

/// <summary>
/// Unit tests for the HttpCoreErrorEnum enum using xUnit and NUnit.
/// </summary>
public class HttpCoreErrorEnumTests
{
    [TestCase(400, HttpCoreErrorEnum.BadRequest)]
    [TestCase(401, HttpCoreErrorEnum.Unauthorized)]
    [TestCase(403, HttpCoreErrorEnum.Forbidden)]
    [TestCase(404, HttpCoreErrorEnum.NotFound)]
    [TestCase(405, HttpCoreErrorEnum.MethodNotAllowed)]
    [TestCase(409, HttpCoreErrorEnum.Conflict)]
    [TestCase(500, HttpCoreErrorEnum.InternalServerError)]
    [TestCase(501, HttpCoreErrorEnum.NotImplemented)]
    [TestCase(502, HttpCoreErrorEnum.BadGateway)]
    [TestCase(503, HttpCoreErrorEnum.ServiceUnavailable)]
    [TestCase(504, HttpCoreErrorEnum.GatewayTimeout)]
    /// <summary>
    /// Verifies that each HttpCoreErrorEnum value matches its expected HTTP status code.
    /// </summary>
    /// <param name="expectedValue">The expected HTTP status code (int).</param>
    /// <param name="enumValue">The enum value to validate.</param>
    /// <remarks>It tests the mapping between enum and numeric HTTP codes.</remarks>
    public void Enum_HasCorrectIntegerValue(int expectedValue, HttpCoreErrorEnum enumValue)
    {
        Assert.That((int)enumValue, Is.EqualTo(expectedValue));
    }
}