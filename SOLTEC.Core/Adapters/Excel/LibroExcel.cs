using System.Data;
using System.Text;
using ExcelDataReader;

namespace SOLTEC.Core.Adapters.Excel;

public class LibroExcel
{
    public string Fichero { get; set; }

    public DataSet Datos { get; set; }

    public ServiceResponse Abrir(string _Fichero)
    {
        Fichero = _Fichero;

        ServiceResponse Res = new ServiceResponse();

        bool isXLSX = Fichero.ToUpper().IndexOf(".XLSX") == -1 ? false : true;
        using (FileStream stream = File.Open(Fichero, FileMode.Open, FileAccess.Read))
        {
            Res = Abrir(stream, isXLSX);
        }

        return Res;
    }
    public ServiceResponse Abrir(Stream stream, bool isXLSX)
    {
        IExcelDataReader excelReader;
        ServiceResponse Res = new ServiceResponse();
        try
        {
            if (!isXLSX)
            {
                excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }

            //DataSet - The result of each spreadsheet will be created in the result.Tables
            Datos = excelReader.AsDataSet();
            excelReader.Close();
            if (Datos == null)
            {
                Res = ServiceResponse.CreateError(5, "Tipo de Fichero Incorrecto");
            }

        }
        catch (Exception ex)
        {
            Res = ServiceResponse.CreateError(-1, ex.Message); ;
        }
        return Res;
    }


    public int Hojas()
    {
        int NumHojas = 0;
        if (Datos != null)
            NumHojas = Datos.Tables.Count;
        return NumHojas;
    }
    public int Filas(int hoja)
    {
        return Datos.Tables[hoja].Rows.Count;
    }
    public int Columnas(int hoja)
    {
        return Datos.Tables[hoja].Columns.Count;
    }

    public string NombreHoja(int hoja)
    {
        return Datos.Tables[hoja].TableName;
    }
    public decimal? LeerCeldaDecimal(int Hoja, string columna, int fila)
    {
        int nCol = Columna(columna);
        fila--;
        if (Datos.Tables[Hoja].Rows[fila][nCol].ToString().Length > 0)
        {
            return Convert.ToDecimal(Datos.Tables[Hoja].Rows[fila][nCol]);
        }
        else
        {
            return null;
        }
    }
    public float? LeerCeldaFloat(int Hoja, string columna, int fila)
    {
        int nCol = Columna(columna);
        fila--;
        if (Datos.Tables[Hoja].Rows[fila][nCol].ToString().Length > 0)
        {
            return Convert.ToSingle(Datos.Tables[Hoja].Rows[fila][nCol]);
        }
        else
        {
            return null;
        }
    }
    public int? LeerCeldaInt32(int Hoja, string columna, int fila)
    {
        int nCol = Columna(columna);
        fila--;
        if (Datos.Tables[Hoja].Rows[fila][nCol].ToString().Length > 0)
        {
            return Convert.ToInt32(Datos.Tables[Hoja].Rows[fila][nCol]);
        }
        else
        {
            return null;
        }
    }
    public long? LeerCeldaInt64(int Hoja, string columna, int fila)
    {
        int nCol = Columna(columna);
        fila--;
        if (Datos.Tables[Hoja].Rows[fila][nCol].ToString().Length > 0)
        {
            return Convert.ToInt64(Datos.Tables[Hoja].Rows[fila][nCol]);
        }
        else
        {
            return null;
        }
    }
    public DateTime? LeerCeldaFecha(int Hoja, string columna, int fila)
    {
        int nCol = Columna(columna);
        fila--;

        try
        {
            if (Datos.Tables[Hoja].Rows[fila][nCol].ToString().Length > 0)
            {
                string fecha = Datos.Tables[Hoja].Rows[fila][nCol].ToString();
                DateTime datFecha = new DateTime();
                if (DateTime.TryParse(fecha, out datFecha))
                {
                    return datFecha;
                }
                else
                {
                    double date = double.Parse(fecha);
                    return DateTime.FromOADate(date);

                }
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }



    }
    public string LeerCelda(int Hoja, string columna, int fila)
    {
        int nCol = Columna(columna);
        fila--;
        if (Datos.Tables[Hoja].Rows[fila][nCol].ToString().Length > 0)
        {
            return Datos.Tables[Hoja].Rows[fila][nCol].ToString();
        }
        else
        {
            return "";
        }
    }
    public string LeerCelda(int Hoja, int columna, int fila)
    {
        columna--;
        fila--;
        if (Datos.Tables[Hoja].Rows[fila][columna].ToString().Length > 0)
        {
            return Datos.Tables[Hoja].Rows[fila][columna].ToString();
        }
        else
        {
            return "";
        }
    }
    private int Columna(string columna)
    {
        //  C	3	676	2028
        //  A	1	26	26
        //  C	3	1	3
        //  2056 

        byte[] columnaAscii;
        columnaAscii = Encoding.ASCII.GetBytes(columna.ToUpper());
        int numColumnas = Encoding.ASCII.GetBytes("Z")[0] - Encoding.ASCII.GetBytes("A")[0] + 1;
        int resultado = 0;
        int p = columnaAscii.Length - 1;

        foreach (byte e in columnaAscii)
        {
            int prev = e - Encoding.ASCII.GetBytes("A")[0] + 1;
            int c = Convert.ToInt32(Math.Pow(numColumnas, p));
            resultado += (c * prev);
            p--;
        }
        resultado--;
        return resultado;
    }
}
