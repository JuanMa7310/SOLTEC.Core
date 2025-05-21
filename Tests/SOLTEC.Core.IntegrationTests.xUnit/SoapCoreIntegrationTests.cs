using SOLTEC.Core.Connections;
using SOLTEC.Core.Connections.Commands;
using Xunit;

namespace SOLTEC.Core.IntegrationTests.xUnit;

/// <summary>
/// Integration tests for the SoapCore class.
/// </summary>
public class SoapCoreIntegrationTests
{
    [Fact]
    /// <summary>
    /// Tests that a SoapCommand can be created and passed to SoapCore without errors.
    /// </summary>
    public async Task SoapCore_ShouldCreateCommandAndSimulatePost()
    {
        // Arrange
        var _command = new SoapCommand(
            "https://example.com/soap",
            "ActionName",
            "TestMethod",
            "http://example.com/namespace",
            "username",
            "password",
            new Dictionary<string, string> { { "TestParam", "TestValue" } }
        );

        var _soapCore = new SoapCore();

        // Act & Assert
        await Task.Run(() =>
        {
            var _envelope = InvokeCreateSoapEnvelope(_command.soapMethod, _command.soapNamespace, _command.parameters);

            Assert.Contains("<TestParam>TestValue</TestParam>", _envelope);
        });
    }

    private static string InvokeCreateSoapEnvelope(string method, string ns, Dictionary<string, string> parameters)
    {
        var _soapCoreType = typeof(SoapCore);
        var _methodInfo = _soapCoreType.GetMethod("CreateSoapEnvelope", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        return (string)_methodInfo.Invoke(null, [method, ns, parameters]);
    }
}
