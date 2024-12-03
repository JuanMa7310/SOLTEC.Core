namespace SOLTEC.Core.Adapters.CSV;

public class ErrorLineaFichero
{
    public int NumLineaFichero { get; set; }
    public string LineaFichero { get; set; }
    public string Error { get; set; }

    public ErrorLineaFichero()
    {
        NumLineaFichero = 0;
        LineaFichero = "";
        Error = "";
    }

    public ErrorLineaFichero(string lineaFichero, int numLineaFichero, string error)
    {
        NumLineaFichero = numLineaFichero;
        LineaFichero = lineaFichero;
        Error = error;
    }
}