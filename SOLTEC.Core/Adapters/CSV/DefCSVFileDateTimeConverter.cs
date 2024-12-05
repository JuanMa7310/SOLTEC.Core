namespace SOLTEC.Core.Adapters.CSV;

public struct Point
{
    public int X, Y;

    public Point(int px, int py)
    {
        X = px;
        Y = py;
    }
}
public class DefCSVFileDateTimeConverter
{
    public Point DayPosition { get; set; }
    public Point MonthPosition { get; set; }
    public Point YearPosition { get; set; }
    public Point HourPosition { get; set; }
    public Point MinutePosition { get; set; }
    public Point SecondPosition { get; set; }
    public Point MilisecondPosition { get; set; }

    public DefCSVFileDateTimeConverter()
    {
        DayPosition = new Point(0, 0);
        MonthPosition = new Point(0, 0);
        YearPosition = new Point(0, 0);
        HourPosition = new Point(0, 0);
        MinutePosition = new Point(0, 0);
        SecondPosition = new Point(0, 0);
        MilisecondPosition = new Point(0, 0);
    }

    public DateTime ConvertToDatetime(string strValue)
    {
        DateTime fecha;

        try
        {
            fecha = DateTime.MinValue;
            if (YearPosition.Y - YearPosition.X > 0 && strValue.Length > YearPosition.Y)
                fecha = fecha.AddYears(Convert.ToInt32(strValue.Substring(YearPosition.X,
                                           YearPosition.Y - YearPosition.X + 1)) -
                                       1);
            if (MonthPosition.Y - MonthPosition.X > 0 && strValue.Length > MonthPosition.Y)
                fecha = fecha.AddMonths(Convert.ToInt32(strValue.Substring(MonthPosition.X,
                                            MonthPosition.Y - MonthPosition.X + 1)) -
                                        1);
            if (DayPosition.Y - DayPosition.X > 0 && strValue.Length > DayPosition.Y)
                fecha = fecha.AddDays(Convert.ToInt32(strValue.Substring(DayPosition.X,
                                          DayPosition.Y - DayPosition.X + 1)) -
                                      1);
            if (HourPosition.Y - HourPosition.X > 0 && strValue.Length > HourPosition.Y)
                fecha = fecha.AddHours(Convert.ToInt32(strValue.Substring(HourPosition.X,
                    HourPosition.Y - HourPosition.X + 1)));
            if (MinutePosition.Y - MinutePosition.X > 0 && strValue.Length > MinutePosition.Y)
                fecha = fecha.AddMinutes(Convert.ToInt32(strValue.Substring(MinutePosition.X,
                    MinutePosition.Y - MinutePosition.X + 1)));
            if (SecondPosition.Y - SecondPosition.X > 0 && strValue.Length > SecondPosition.Y)
                fecha = fecha.AddSeconds(Convert.ToInt32(strValue.Substring(SecondPosition.X,
                    SecondPosition.Y - SecondPosition.X + 1)));
            if (MilisecondPosition.Y - MilisecondPosition.X > 0 && strValue.Length > MilisecondPosition.Y)
                fecha = fecha.AddMilliseconds(Convert.ToInt32(strValue.Substring(MilisecondPosition.X,
                    MilisecondPosition.Y - MilisecondPosition.X + 1)));
        }
        catch (Exception)
        {
            throw;
        }

        return fecha;
    }
}
