using SOLTEC.Core.Enums;

namespace SOLTEC.Core.Validators;

public class IdentificationValidator 
{
    public bool ValidateCIF(string identification) 
    {
        var lastLetter = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "0" };
        var pairNumbers = 0;
        var unpairNumbers = 0;
        string tmpPos;

        if (identification.Length < 9) 
            return false;

        identification = identification.ToUpper();
        var lastCharacter = identification.Substring(8, 1);

        var count = 1;
        for (var cont = 1; cont < 7; cont++) 
        {
            tmpPos = (2 * int.Parse(identification.Substring(cont++, 1))) + "0";
            unpairNumbers += int.Parse(tmpPos.ToString().Substring(0, 1)) + 
                             int.Parse(tmpPos.ToString().Substring(1, 1));
            pairNumbers += int.Parse(identification.Substring(cont, 1));
        }

        tmpPos = (2 * int.Parse(identification.Substring(count, 1))) + "0";
        unpairNumbers += int.Parse(tmpPos.Substring(0, 1)) + 
                         int.Parse(tmpPos.Substring(1, 1));

        var totalSum = pairNumbers + unpairNumbers;
        var number = int.Parse(totalSum.ToString().Substring(totalSum.ToString().Length - 1, 1));
        number = 10 - number;
        if (number == 10) 
            number = 0;
        return (lastCharacter == number.ToString()) || (lastCharacter == lastLetter[number - 1]);
    }

    public bool ValidateNIE(string identification) 
    {
        if (string.IsNullOrWhiteSpace(identification) || (identification.Length != 9 && identification.Length != 11)) 
            return false;
        var identificacionNumbers = identification.Substring(1, identification.Length - 2);
        var firstLetter = identification.Substring(0, 1);
        var lastLetter = identification.Substring(identification.Length - 1, 1);
        var numbersValid = int.TryParse(identificacionNumbers, out int identificacionNumberInteger);
        if (!numbersValid) 
            return false;
        if (firstLetter == "Y")
            identificacionNumberInteger += 10000000;
        else if (firstLetter == "Z") 
            identificacionNumberInteger += 20000000;
        return CalculateDNILeter(identificacionNumberInteger) == lastLetter;
    }

    public bool ValidateDNI(string identification) 
    {
        if (string.IsNullOrWhiteSpace(identification) || identification.Length != 9) 
            return false;
        var identificacionNumbers = identification.Substring(0, identification.Length - 1);
        var identificacionLetter = identification.Substring(identification.Length - 1, 1);
        var numbersValid = int.TryParse(identificacionNumbers, out int identificacionNumberInteger);
        if (!numbersValid) 
            return false;
        if (CalculateDNILeter(identificacionNumberInteger) != identificacionLetter) 
            return false;
        return true;
    }

    private string CalculateDNILeter(int dniNumbers) 
    {
        string[] controlLetters = { "T", "R", "W", "A", "G", "M", "Y", "F", "P", "D", "X", "B", "N", "J", "Z", "S", "Q", "V", "H", "L", "C", "K", "E" };
        var mod = dniNumbers % 23;
        
        return controlLetters[mod];
    }
}
