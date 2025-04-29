using SOLTEC.Core.Connections.Commands;

namespace SOLTEC.Core.Tests.xUnit;

/// <summary>
/// Unit tests for the SoapCommand record.
/// </summary>
public class SoapCommandTests
{
    [Fact]
    /// <summary>
    /// Tests that a SoapCommand is correctly created with the provided values.
    /// </summary>
    public void SoapCommand_ShouldCreateInstance_WithCorrectValues()
    {
        // Arrange
        var _parameters = new Dictionary<string, string>
            {
                { "Key1", "Value1" },
                { "Key2", "Value2" }
            };

        // Act
        var _soapCommand = new SoapCommand(
            "https://example.com/soap",
            "ActionName",
            "MethodName",
            "http://example.com/namespace",
            "username",
            "password",
            _parameters
        );

        // Assert
        Assert.Equal("https://example.com/soap", _soapCommand.soapUrl);
        Assert.Equal("ActionName", _soapCommand.soapAction);
        Assert.Equal("MethodName", _soapCommand.soapMethod);
        Assert.Equal("http://example.com/namespace", _soapCommand.soapNamespace);
        Assert.Equal("username", _soapCommand.username);
        Assert.Equal("password", _soapCommand.password);
        Assert.Equal(_parameters, _soapCommand.parameters);
    }
}
