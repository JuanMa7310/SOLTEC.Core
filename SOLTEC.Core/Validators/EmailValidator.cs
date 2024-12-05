using System.Text.RegularExpressions;

namespace SOLTEC.Core.Validators;

public static class EmailValidator 
{
    static Regex ValidEmailRegex = CreateValidEmailRegex();

    public static bool IsValidEmail(string emailAddress) 
    {
        var isValid = ValidEmailRegex.IsMatch(emailAddress);
        return isValid;
    }

    private static Regex CreateValidEmailRegex() 
    {
        var validEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
                                + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
                                + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
        return new Regex(validEmailPattern, RegexOptions.IgnoreCase);
    }
}
