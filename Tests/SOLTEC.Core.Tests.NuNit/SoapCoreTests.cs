using SOLTEC.Core.Connections;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the SoapCore class.
/// </summary>
public class SoapCoreTests
{
    [Test]
    /// <summary>
    /// Tests that the CreateSoapEnvelope method generates a correct SOAP envelope string.
    /// </summary>
    public void CreateSoapEnvelope_ShouldGenerateValidSoapEnvelope()
    {
        // Arrange
        var _parameters = new Dictionary<string, string>
            {
                { "Param1", "Value1" },
                { "Param2", "Value2" }
            };
        var _methodName = "TestMethod";
        var _namespace = "http://example.com/namespace";

        // Act
        var _soapEnvelope = InvokeCreateSoapEnvelope(_methodName, _namespace, _parameters);

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(_soapEnvelope, Does.Contain("<TestMethod xmlns=\"http://example.com/namespace\">"));
            Assert.That(_soapEnvelope, Does.Contain("<Param1>Value1</Param1>"));
            Assert.That(_soapEnvelope, Does.Contain("<Param2>Value2</Param2>"));
        });
    }

    private static string InvokeCreateSoapEnvelope(string method, string ns, Dictionary<string, string> parameters)
    {
        var _soapCoreType = typeof(SoapCore);
        var _methodInfo = _soapCoreType.GetMethod("CreateSoapEnvelope", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static) ?? throw new InvalidOperationException("The method 'CreateSoapEnvelope' was not found in the SoapCore class.");
        var _result = _methodInfo.Invoke(null, [method, ns, parameters]);

        return _result is null
            ? throw new InvalidOperationException("The invoked method 'CreateSoapEnvelope' returned null, which is not expected.")
            : (string)_result;
    }
}
