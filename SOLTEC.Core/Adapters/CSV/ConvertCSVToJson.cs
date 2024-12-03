using System.Text;

namespace SOLTEC.Core.Adapters.CSV;

public class ConvertCSVToJson 
{
    public string Execute(string path, AdapterDto adapterDto) 
    {
        string content;            
        using (var reader = new StreamReader(path)) 
        {
            content = reader.ReadToEnd();
        }
        return CanvertToJson(content, adapterDto);
    }

    public string Execute(StreamReader streamReader, AdapterDto adapterDto) 
    {
        var content = streamReader.ReadToEnd();
        return CanvertToJson(content, adapterDto);
    }

    private string CanvertToJson(string content, AdapterDto adapterDto) 
    {
        var json = string.Empty;
        var lines = content.Split(new[] { "\n" }, StringSplitOptions.None);
        return ProcessLines(adapterDto, ref json, lines);
    }

    private string ProcessLines(AdapterDto adapterDto, ref string json, string[] lines) 
    {
        if (lines.Length > 1) 
        {
            var headers = CreateHeader(adapterDto, adapterDto.Config.Separator, lines);
            var stringBulder = InitializeJson();
            for (var i = 1; i < lines.Length; i++) 
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                if (string.IsNullOrEmpty(lines[i])) continue;
                AppendItem(lines, headers, stringBulder, i, adapterDto.Items, adapterDto.Config.Separator);
            }
            json = EndJson(stringBulder);
        }
        return json;
    }

    private string[] CreateHeader(AdapterDto adapterDto, char separator, string[] lines) 
    {
        var headersAux = lines[0].Split(separator);
        var headers = new string[headersAux.Length + 1];
        for (var i = 0; i < headersAux.Length; i++) 
        {
            headers[i] = adapterDto.Items.First(x => x.ColumName.Equals(headersAux[i].Replace("\r", ""))).Name;
        }
        headers[headers.Length - 1] = "Error";
        return headers;
    }

    private StringBuilder InitializeJson() 
    {
        StringBuilder sbjson = new StringBuilder();
        sbjson.Clear();
        sbjson.Append("[");
        return sbjson;
    }

    private void AppendItem(string[] lines, string[] headers, StringBuilder stringBulder, int index, List<Item> items, char separator) 
    {
        stringBulder.Append("{");
        string[] data = lines[index].Split(separator);
        var error = "";
        for (int h = 0; h < headers.Length - 1; h++) {

            if (items[h].MaxLength != 0 && data[h].Replace("\r", "").Length > items[h].MaxLength) {
                error = "LenghtFieldError";
            }

            stringBulder.Append($"\"{headers[h]}\": \"{data[h].Replace("\r", "")}\"" + (h < headers.Length - 1 ? "," : null));
        }

        stringBulder.Append($"\"{headers[headers.Length - 1]}\": \"{error}\"");

        stringBulder.Append("}" + (index < lines.Length - 1 ? "," : null));
    }

    private string EndJson(StringBuilder sbjson) 
    {
        string json;
        sbjson.Append("]");
        json = sbjson.ToString();
        return json;
    }
}
