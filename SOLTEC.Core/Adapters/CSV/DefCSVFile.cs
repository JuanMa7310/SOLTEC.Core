using System.Globalization;
using System.Reflection;
using SOLTEC.Core.Adapters.CSV;

namespace SOLTEC.Core.Adapters.CSV;

public class DefCSVFile
{
    public string Separador { get; set; }
    public List<DefCSVFileField>? Campos;
    public DefCSVFileDateTimeConverter? ConversorDeFechas;

    public DefCSVFile() : this(";") { }

    public DefCSVFile(string separador)
    {
        Separador = separador; // Punto y coma por defecto
        Campos = new List<DefCSVFileField>();
        ConversorDeFechas = null;
    }

    public void AddFileField(string cabecera, string campo, int posicion)
    {
        AddFileField(cabecera, campo, posicion, null);
    }

    public void AddFileField(string cabecera, string campo, int posicion, object? defaultValue)
    {
        AddFileField(cabecera, campo, posicion, defaultValue, null);
    }

    public void AddFileField(string cabecera, string campo, int posicion, object? defaultValue, int? longitud)
    {
        if (Campos == null)
            Campos = new List<DefCSVFileField>();

        Campos.Add(new DefCSVFileField()
        {
            CabeceraFila = cabecera,
            Campo = campo,
            Posicion = posicion,
            DefaultValue = defaultValue,
            Longitud = longitud
        });
    }

    public int GetFileFieldPosition(string cabecera, string campo)
    {
        if (Campos != null)
            if (Campos.Exists(c => c.CabeceraFila == cabecera && c.Campo == campo))
                return Campos.First(c => c.CabeceraFila == cabecera && c.Campo == campo).Posicion;
            else
                return -1;
        else
            return -1;
    }

    /// <summary>
    /// Parsea la fila colocando cada valor en la propiedad equivalente de la clase pasada por parámetro en "obj"
    /// El array de "Fila" tiene que tener la CabeceraFila en la primera posición para que el proceso sepa identificar
    /// que campos tiene que procesar. Si alguno de los campos no existe en la clase dará error.
    /// Si la fila no contiene cabecera hay que usar la otra definición de este método
    /// </summary>
    /// <param name="fila"></param>
    /// <param name="obj"></param>
    /// <param name="posicionInicial"></param>
    /// <returns></returns>
    public ServiceResponse ParseLineaToClass(string[] fila, object obj, int posicionInicial = 1)
    {
        return ParseLineaToClass(fila[0].ToString(), posicionInicial, fila, obj);
    }

    /// <summary>
    /// Parsea la fila colocando cada valor en la propiedad equivalente de la clase pasada por parámetro en "obj"
    /// El array de "Fila" NO contiene campo cabecera por lo que la primera posición de valor importable es la 0
    /// Si alguno de los campos no existe en la clase dará error.
    /// </summary>
    /// <param name="cabecera">El valor con el que se filtrará la colección de campos que definen el CSV</param>
    /// <param name="fila"></param>
    /// <param name="obj"></param>
    /// <param name="posicionInicial">La posición donde se encuentra el primer valor de la fila (Si contiene cabecera 1, si no 0)</param>
    /// /// <returns></returns>
    public ServiceResponse ParseLineaToClass(string cabecera, string[] fila, object obj, int posicionInicial = 1)
    {
        return ParseLineaToClass(cabecera, posicionInicial, fila, obj);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cabecera">El valor con el que se filtrará la colección de campos que definen el CSV</param>
    /// <param name="posicionInicial">La posición donde se encuentra el primer valor de la fila (Si contiene cabecera 1, si no 0)</param>
    /// <param name="fila">Array con la fila del archivo</param>
    /// <param name="obj">Objeto que recibirá los datos. Los campos definidos tienen que ser propiedades de este objeto </param>
    /// <returns></returns>
    public ServiceResponse ParseLineaToClass(string cabecera, int posicionInicial, string[] fila, object obj)
    {
        return ParseLineaToClass(cabecera, posicionInicial, fila, obj, true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="cabecera">El valor con el que se filtrará la colección de campos que definen el CSV</param>
    /// <param name="posicionInicial">La posición donde se encuentra el primer valor de la fila (Si contiene cabecera 1, si no 0)</param>
    /// <param name="fila">Array con la fila del archivo</param>
    /// <param name="obj">Objeto que recibirá los datos. Los campos definidos tienen que ser propiedades de este objeto </param>
    /// <param name="ficheroConSeparador">Indica si el fichero tiene un caracter separador o los campos se separan por posiciones y longitud</param>
    /// <returns></returns>
    public ServiceResponse ParseLineaToClass(string cabecera, int posicionInicial, string[] fila, object obj, 
        bool ficheroConSeparador)
    {
        var res = new ServiceResponse();
        PropertyInfo propertyInfo;
        string value;
        var ultimoCampoTratado = "";

        try
        {
            var camposFila = Campos!.Where(c => c.CabeceraFila == cabecera).ToList();

            foreach (var c in camposFila)
            {
                ultimoCampoTratado = c.CabeceraFila + " - " + c.Campo + " - " + c.Posicion;

                try
                {
                    propertyInfo = obj.GetType().GetProperty(c.Campo)!;

                    if (ficheroConSeparador)
                        value = fila[c.Posicion - posicionInicial];
                    else
                        value = fila[0].Substring(c.Posicion, c.Longitud.Value).Trim();                        

                    if (value == "NeuN")
                        value = "";

                    if (value == "" && c.DefaultValue != null)
                        value = c.DefaultValue.ToString()!;

                    if ((propertyInfo.PropertyType == typeof(double)
                        || propertyInfo.PropertyType == typeof(short)
                        || propertyInfo.PropertyType == typeof(long)
                        || propertyInfo.PropertyType == typeof(decimal)
                        || propertyInfo.PropertyType == typeof(int)
                        || propertyInfo.PropertyType == typeof(float)))
                    {
                        if (value == "")
                            value = "0";
                        if (value.Substring(0, 1) == ".")
                            value = "0" + value;
                        if (value.Substring(0, 1) == ",")
                            value = "0" + value;

                        //  if (Convert.ToDecimal("1,123") < Convert.ToDecimal("1.123")) //Solo si el digito decimal es una coma ","
                        value = value.Replace(".", ",");
                    }

                    if (propertyInfo.PropertyType == typeof(bool))
                    {
                        if (value == "0")
                        { 
                            value = false.ToString();
                        }
                        else
                        {
                            if (value == "1")
                                value = true.ToString();
                            else 
                                if (value == "")
                                    value = false.ToString();
                        }
                    }

                    if (propertyInfo.PropertyType == typeof(DateTime)
                        && !value.Contains("/") && ConversorDeFechas != null)
                        value = ConversorDeFechas.ConvertToDatetime(value).ToString(CultureInfo.InvariantCulture);

                    propertyInfo.SetValue(obj, Convert.ChangeType(value, propertyInfo.PropertyType), null);
                }
                catch (Exception)
                {
                    res = new ServiceResponse();
                    res.ErrorMessage = ultimoCampoTratado + res.ErrorMessage;
                }
            }
        }
        catch (Exception)
        {
            res = new ServiceResponse();
            res.ErrorMessage = ultimoCampoTratado + res.ErrorMessage;
        }

        return res;
    }
}