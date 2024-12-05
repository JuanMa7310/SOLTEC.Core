using System.Globalization;
using System.Text.RegularExpressions;

namespace SOLTEC.Core.Validators;

public class IdentificationValidator
{
    public bool ValidateCURP(string identification)
    {
        var re = @"^([A-Z][AEIOUX][A-Z]{2}\d{2}(?:0[1-9]|1[0-2])(?:0[1-9]|[12]\d|3[01])[HM](?:AS|B[CS]|C[CLMSH]|D[FG]|G[TR]|HG|JC|M[CNS]|N[ETL]|OC|PL|Q[TR]|S[PLR]|T[CSL]|VZ|YN|ZS)[B-DF-HJ-NP-TV-Z]{3}[A-Z\d])(\d)$";
        var rx = new Regex(re, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        var validado = rx.IsMatch(identification);

        if (!validado)
            return false; 
        if (!identification.EndsWith(DigitoVerificador(identification.ToUpper())))
            return false;
        return true; //Validado
    }
    private string DigitoVerificador(string curp17)
    {
        //Fuente https://consultas.curp.gob.mx/CurpSP/
        var diccionario = "0123456789ABCDEFGHIJKLMNÑOPQRSTUVWXYZ";
        var suma = 0.0d;
        double digito;
        for (var i = 0; i < 17; i++)
            suma += diccionario.IndexOf(curp17[i]) * (18 - i);
        digito = 10d - suma % 10;
        if (Math.Abs(digito - 10.0d) < double.Epsilon) 
            return "0";
        return digito.ToString(CultureInfo.CurrentCulture);
    }
}
