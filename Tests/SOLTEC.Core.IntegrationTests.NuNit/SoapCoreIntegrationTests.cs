using NUnit.Framework;
using SOLTEC.Core.Connections.Commands;
using SOLTEC.Core.Connections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SOLTEC.Core.IntegrationTests.NuNit;

[TestFixture]
/// <summary>
/// Integration tests for the SoapCore class.
/// </summary>
public class SoapCoreIntegrationTests
{
    [Test]
    /// <summary>
    /// Tests that a SoapCommand can be created and passed to SoapCore without errors.
    /// </summary>
    public void SoapCore_ShouldCreateCommandAndSimulatePost()
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
        Assert.DoesNotThrow(() =>
        {
            var _envelope = InvokeCreateSoapEnvelope(_command.soapMethod, _command.soapNamespace, _command.parameters);

            StringAssert.Contains("<TestParam>TestValue</TestParam>", _envelope);
        });
    }

    private static string InvokeCreateSoapEnvelope(string method, string ns, Dictionary<string, string> parameters)
    {
        var _soapCoreType = typeof(SoapCore);
        var _methodInfo = _soapCoreType.GetMethod("CreateSoapEnvelope", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);

        return (string)_methodInfo.Invoke(null, [method, ns, parameters]);
    }
}
