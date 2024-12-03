namespace SOLTEC.Core.Extensions;

public static class IntegerExtensions
{
    public static int? ParseOrDefault(string s, int? defaultValue)
    {
        try
        {
            return int.Parse(s);
        }
        catch
        {
            return defaultValue ?? default(int?);
        }
    }
}
