
namespace SOLTEC.Core.Adapters.CSV;

public class DefCSVFileField
{
    public string CabeceraFila { get; set; } // Puede estar vacío
    public string Campo { get; set; } // Nombre del campo que será igual a la propiedad de la clase destino
    public int Posicion { get; set; } // Posición del campo dentro de la línea 
    public object? DefaultValue { get; set; } // Valor Por defecto si null
    public int? Longitud { get; set; } // Longitud del campo a partir de la Posicion. Puede estar vacío
}
