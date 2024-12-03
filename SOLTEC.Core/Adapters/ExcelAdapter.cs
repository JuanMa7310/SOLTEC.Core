using Ganss.Excel;

namespace SOLTEC.Core.Adapters;

public class ExcelAdapter : Adapter 
{
    public virtual IEnumerable<T> Execute<T>(string pathFile, bool headerRow = false, string? sheetName = null) {
        if (string.IsNullOrWhiteSpace(sheetName))
            return new ExcelMapper(pathFile) { HeaderRow = headerRow }.Fetch<T>();
        return new ExcelMapper() { HeaderRow = headerRow }.Fetch<T>(pathFile, sheetName);
    }

    public virtual IEnumerable<T> Execute<T>(Stream stream, bool headerRow = false, string? sheetName = null) {
        if (string.IsNullOrWhiteSpace(sheetName))
            return new ExcelMapper(stream) { HeaderRow = headerRow }.Fetch<T>();
        return new ExcelMapper() { HeaderRow = headerRow }.Fetch<T>(stream, sheetName);
    }
}