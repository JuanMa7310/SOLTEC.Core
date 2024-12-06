using SOLTEC.Core.Validators;
using FluentAssertions;

namespace SOLTEC.Core.Tests.Validators.Actions;

public class EmailValidatorShould 
{
    [TestCase("juan@email.com", true)]
    [TestCase("juan1231@312email.es", true)]
    [TestCase("juan12.31@3.12email.1231", false)]
    public void validate_email(string email, bool expectedValue) 
    {
        var result = EmailValidator.IsValidEmail(email);

        result.Should().Be(expectedValue);
    }
}
