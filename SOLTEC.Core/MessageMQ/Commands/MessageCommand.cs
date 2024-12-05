namespace SOLTEC.Core.MessageMQ.Commands;

public class MessageCommand 
{
    public string Publisher { get; set; }
    public string EventName { get; set; }
    public string Message { get; set; }
}
