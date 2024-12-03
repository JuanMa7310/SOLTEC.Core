namespace SOLTEC.Core.Adapters.CSV;

public class Config 
{
    public bool ReadHeader { get; set; }
    public int StartLine { get; set; }
    public char Separator { get; set; }
    public int ItemsCount { get; set; }
}
