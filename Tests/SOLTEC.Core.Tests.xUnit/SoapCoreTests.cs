using SOLTEC.Core.Connections;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the SoapCore class.
/// </summary>
public class SoapCoreTests
{
    [Fact]
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

        // Assert
        Assert.Contains("<TestMethod xmlns=\"http://example.com/namespace\">", _soapEnvelope);
        Assert.Contains("<Param1>Value1</Param1>", _soapEnvelope);
        Assert.Contains("<Param2>Value2</Param2>", _soapEnvelope);
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
