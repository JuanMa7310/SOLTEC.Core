using Newtonsoft.Json;

namespace SOLTEC.Core.Adapters.CSV;

public class CSVAdapter : Adapter 
{
    private ConvertCSVToJson _convertToJson;

    public CSVAdapter() 
    {
        _convertToJson = new ConvertCSVToJson();
    }

    public virtual IList<T> Execute<T>(string pathFile, string pathTemplate) 
    {
        var adapterDto = CreateAdapterDto(pathTemplate);
        var adaptedJson = _convertToJson.Execute(pathFile, adapterDto);
        return JsonConvert.DeserializeObject<IList<T>>(adaptedJson)!;
    }

    public virtual IList<T> Execute<T>(StreamReader reader, string pathTemplate) 
    {
        var adapterDto = CreateAdapterDto(pathTemplate);
        var adaptedJson = _convertToJson.Execute(reader, adapterDto);
        return JsonConvert.DeserializeObject<IList<T>>(adaptedJson)!;
    }

    private AdapterDto CreateAdapterDto(string pathTemplate) 
    {
        var streamReader = new StreamReader(pathTemplate);
        var json = streamReader.ReadToEnd();
        var adapterDto = JsonConvert.DeserializeObject<AdapterDto>(json);
        return adapterDto!;
    }
}

