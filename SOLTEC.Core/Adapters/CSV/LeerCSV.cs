using System.Text;

namespace SOLTEC.Core.Adapters.CSV;

public static class LeerCSV
{
    public static ServiceResponse Leer<T>(string filePath, DefCSVFile defCSV, out List<T> list, out List<ErrorLineaFichero> listError)
    {
        return Leer(filePath, defCSV, 1, "", out list, out listError);
    }

    public static ServiceResponse Leer<T>(string filePath, DefCSVFile defCSV, int filaInicial, out List<T> list, out List<ErrorLineaFichero> listError)
    {
        return Leer(filePath, defCSV, filaInicial, "", out list, out listError);
    }

    public static ServiceResponse Leer<T>(string filePath, DefCSVFile defCSV, int filaInicial, string filtroColumna0, out List<T> list, out List<ErrorLineaFichero> listError)
    {
        return Leer(filePath, defCSV, filaInicial, filtroColumna0, true, out list, out listError);
    }
    public static ServiceResponse Leer<T>(string filePath, DefCSVFile defCSV, int filaInicial, string filtroColumna0, bool ficheroConSeparador, out List<T> list, out List<ErrorLineaFichero> listError)
    {
        FileStream file = File.Open(filePath, FileMode.Open, FileAccess.Read);
        return Leer(file, defCSV, filaInicial, filtroColumna0, ficheroConSeparador, out list, out listError);
    }

    public static ServiceResponse Leer<T>(Stream stream, DefCSVFile defCSV, int filaInicial, string filtroColumna0, bool ficheroConSeparador, out List<T> list, out List<ErrorLineaFichero> listError)
    {
        return Leer(stream, defCSV, filaInicial, filtroColumna0, ficheroConSeparador, Encoding.UTF8, out list, out listError);
    }

    public static ServiceResponse Leer<T>(Stream stream, DefCSVFile defCSV, int filaInicial, string filtroColumna0, bool ficheroConSeparador, Encoding encoding, out List<T> list, out List<ErrorLineaFichero> listError)
    {
        var counter = 1;
        ServiceResponse res;
        list = new List<T>();
        listError = new List<ErrorLineaFichero>();
        StreamReader? file = null;

        try
        {
            string line;
            file = new StreamReader(stream, encoding); // Abro el fichero

            while ((line = file.ReadLine()) != null) // Leo la siguiente línea
            {
                if (counter >= filaInicial)
                {
                    if (ficheroConSeparador)
                    {
                        string[] values = line.Split(defCSV.Separador.ToCharArray());
                        if (values.Count() > 0)
                        {
                            if (filtroColumna0 == "" || values[0] == filtroColumna0)
                            {
                                var elemento = (T)Activator.CreateInstance(typeof(T));

                                res = defCSV.ParseLineaToClass(typeof(T).Name, values, elemento, 1);
                                if (res.Success)
                                {
                                    list.Add(elemento);
                                }
                                else
                                {
                                    listError.Add(new ErrorLineaFichero()
                                    {
                                        NumLineaFichero = counter,
                                        LineaFichero = line,
                                        Error = res.ErrorMessage
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        // Código nuevo incluido para realizar el caso de un fichero que no tiene caracter separador, y que funciona por las posiciones y longitud de sus campos
                        var elemento = (T)Activator.CreateInstance(typeof(T));

                        var values = new[] { line };

                        res = defCSV.ParseLineaToClass(typeof(T).Name, 0, values, elemento, ficheroConSeparador);
                        if (res.Success)
                        {
                            list.Add(elemento);
                        }
                        else
                        {
                            listError.Add(new ErrorLineaFichero()
                            {
                                NumLineaFichero = counter,
                                LineaFichero = line,
                                Error = res.ErrorMessage
                            });
                        }
                    }
                }

                counter++; // Mantengo el contador de líneas
            }

            // Hacemos esto para que aunque haya ocurrido un error en una línea se devuelva IsOk=True y el proceso siga ejecutándose.
            // En la lista listError vamos guardando cada línea que falla y el error.
            res = new ServiceResponse();
        }
        catch (Exception)
        {
            res = new ServiceResponse();
        }

        file!.Close();
        return res;
    }
}
