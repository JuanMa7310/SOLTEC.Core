using SOLTEC.Core.Validators;
using FluentAssertions;

namespace SOLTEC.Core.Tests.Validators.Actions;

public class BankAccountValidatorShould 
{
    [TestCase("ES67", "0081", "3977", "6741", "7952", "7577", true)]
    [TestCase("ES18", "1552", "0232", "3484", "5319", "4843", false)]
    [TestCase("ES00", "0000", "0000", "0000", "0000", "0000", false)]
    [TestCase("ES15", "0049", "5450", "0526", "9503", "7748", true)]
    [TestCase("ES15", "00A9", "5450", "0526", "9503", "7748", false)]
    [TestCase("ES15", "0049", "54B0", "0526", "9503", "7748", false)]
    [TestCase("ES15", "0049", "5450", "05E6", "9503", "7748", false)]
    [TestCase("ES15", "0049", "5450", "0526", "95C3", "7748", false)]
    [TestCase("ES15", "0049", "5450", "0526", "9503", "7R48", false)]
    public void iban_validator(string ibanCode, string bankCode, string branchCode, string cta1, string cta2, 
        string cta3, bool expectedValue) 
    {
        using (var bankAccountValidator = new BankAccountValidator()) 
        {
            var result = bankAccountValidator.IsValidBankAccount(ibanCode, bankCode, branchCode, cta1, cta2, cta3);
            
            result.Should().Be(expectedValue);
        }
    }
}
