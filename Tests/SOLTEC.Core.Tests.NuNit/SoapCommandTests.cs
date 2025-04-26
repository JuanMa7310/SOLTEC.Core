using SOLTEC.Core.Connections.Commands;

namespace SOLTEC.Core.Tests.NuNit;

[TestFixture]
/// <summary>
/// Unit tests for the SoapCommand record.
/// </summary>
public class SoapCommandTests
{
    [Test]
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

        Assert.Multiple(() =>
        {
            // Assert
            Assert.That(_soapCommand.soapUrl, Is.EqualTo("https://example.com/soap"));
            Assert.That(_soapCommand.soapAction, Is.EqualTo("ActionName"));
            Assert.That(_soapCommand.soapMethod, Is.EqualTo("MethodName"));
            Assert.That(_soapCommand.soapNamespace, Is.EqualTo("http://example.com/namespace"));
            Assert.That(_soapCommand.username, Is.EqualTo("username"));
            Assert.That(_soapCommand.password, Is.EqualTo("password"));
            Assert.That(_soapCommand.parameters, Is.EqualTo(_parameters));
        });
    }
}
