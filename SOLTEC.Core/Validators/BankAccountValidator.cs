using System.Text;
using System.Text.RegularExpressions;

namespace SOLTEC.Core.Validators;

public class BankAccountValidator : IDisposable 
{
    public void Dispose() 
    {
        GC.SuppressFinalize(this);
    }

    public virtual bool IsValidBankAccount(string ibanCode, string bankCode, string branchCode, 
        string Cta1, string Cta2, string Cta3) 
    {
        try 
        {
            if (!ValidateValidBankAccount(ibanCode + bankCode + branchCode + Cta1 + Cta2 + Cta3)) return false;
            return ValidateDigitControl(bankCode, branchCode, Cta1, Cta2, Cta3);
        }
        catch 
        {
            return false;
        }
    }

    private bool ValidateValidBankAccount(string bankAccount) 
    {
        bankAccount = bankAccount.ToUpper().Replace(" ", string.Empty);
        if (!IsValidBankAccountNumber(bankAccount)) 
            return false;
        var bankCode = bankAccount.Substring(4, bankAccount.Length - 4) + 
                       bankAccount.Substring(0, 4);
        var bankCodeString = GetBankCodeString(bankCode);
        return CheckBankCodeString(bankCodeString);
    }

    private bool ValidateDigitControl(string bankCode, string branchCode, string Cta1, string Cta2, string Cta3) 
    {            
        var controlDigit = "  ";
        var accountNumber = "  " + Cta2 + Cta3;
        if (Cta1.Length >= 2)
            controlDigit = Cta1.Substring(0, 2);
        if (Cta1.Length >= 4)
            accountNumber = Cta1.Substring(2, 2) + Cta2 + Cta3;
        return controlDigit == GetDCCuentaBancaria(bankCode,branchCode, accountNumber);
    }

    private bool CheckBankCodeString(string bankCodeString) 
    {
        var checksum = int.Parse(bankCodeString.Substring(0, 1));
        for (var i = 1; i < bankCodeString.Length; i++) 
        {
            var nextNumber = int.Parse(bankCodeString.Substring(i, 1));
            checksum *= 10;
            checksum += nextNumber;
            checksum %= 97;
        }
        return checksum == 1;
    }

    private string GetBankCodeString(string bankCode) 
    {
        var asciiShift = 55;
        var stringBuilder = new StringBuilder();
        foreach (var character in bankCode) 
        {
            int characterNumber;
            if (char.IsLetter(character)) 
                characterNumber = character - asciiShift;
            else 
                characterNumber = int.Parse(character.ToString());
            stringBuilder.Append(characterNumber);
        }
        return stringBuilder.ToString();
    }

    private bool IsValidBankAccountNumber(string bankAccount) 
    {
        if (string.IsNullOrWhiteSpace(bankAccount)) 
            return false;
        if (!Regex.IsMatch(bankAccount, "^[A-Z]{2}[A-Z0-9]{18,30}")) 
            return false;
        return true;
    }

    private string GetDCCuentaBancaria(string bankCode, string branchCode, string accountNumber) 
    {
        // Primero compruebo que la longitud del parámetro
        // sea de 18 caracteres, y que éstos sean números.
        string numeroCuenta;
        numeroCuenta = bankCode + branchCode + accountNumber;
        if (numeroCuenta.Length != 18) 
            return null;
        foreach (var ch in numeroCuenta) 
        {
            if (!char.IsNumber(ch))
                return null;
        }
        int cociente1, cociente2, resto1, resto2;
        string sucursal, cuenta, dc1, dc2;
        int suma1 = 0, suma2 = 0;
        // Matriz que contiene los pesos utilizados en el
        // algoritmo de cálculo de los dos dígitos de control.
        var pesos = new[] { 6, 3, 7, 9, 10, 5, 8, 4, 2, 1 };
        sucursal = numeroCuenta.Substring(0, 8);
        cuenta = numeroCuenta.Substring(8, 10);
        // Obtengo el primer dígito de control que verificará
        // los códigos de Entidad y Oficina.
        for (var n = 7; n >= 0; n += -1)
            suma1 += Convert.ToInt32(sucursal.Substring(n, 1)) * pesos[7 - n]; 
        // Calculamos el cociente de dividir el resultado
        // de la suma entre 11.
        cociente1 = suma1 / 11; // Nos da un resultado entero.
        // Calculo el resto de la división. Para ello,
        // en lugar de utilizar el operador Mod, utilizo
        // la fórmula para obtener el resto de la división.
        resto1 = suma1 - (11 * cociente1);
        dc1 = (11 - resto1).ToString();
        switch (dc1) 
        {
            case "11": 
            {
                    dc1 = "0";
                    break;
            }
            case "10": 
            {
                    dc1 = "1";
                    break;
            }
        }
       // Ahora obtengo el segundo dígíto, que verificará
        // el número de cuenta de cliente. 
        for (var n = 9; n >= 0; n += -1)
            suma2 += Convert.ToInt32(cuenta.Substring(n, 1)) * pesos[9 - n];
        // Calculamos el cociente de dividir el resultado
        // de la suma entre 11.
        cociente2 = suma2 / 11; // Nos da un resultado entero
        // Calculo el resto de la división. Para ello,
        // en lugar de utilizar el operador Mod, utilizo
        // la fórmula para obtener el resto de la división.
        resto2 = suma2 - (11 * cociente2);
        dc2 = (11 - resto2).ToString();
       switch (dc2) 
        {
            case "11": 
            {
                    dc2 = "0";
                    break;
            }
            case "10": 
            {
                    dc2 = "1";
                    break;
            }
        }
        // Devuelvo el dígito de control.
        return dc1 + dc2;
    }
}
    