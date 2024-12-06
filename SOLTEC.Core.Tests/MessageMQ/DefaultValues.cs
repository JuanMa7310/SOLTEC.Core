using System.Net;

namespace SOLTEC.Core.Tests.MessageMQ;

public static class DefaultValues 
{
    public static Guid EventId = Guid.NewGuid();
    public static Guid ExchangeId = Guid.NewGuid();
    public static Guid SubscriberGuid = Guid.NewGuid();
    public static Guid MessageGuid = Guid.NewGuid();
    public static Guid EventMessageGuid = Guid.NewGuid();
    public static Guid EventSubscriptorId = Guid.NewGuid();
    public static string Message = "associated message";
    public static string CreationDate = "20220613";
    public static string CreationWithTime = "20220613235641";
    public static string ProcessedDate = "20220620234512";
    public static string EventName = "EventName";
    public static string OtherEventName = "OtherEventName";
    public static string SubscriberName = "name subscriptor";
    public static string SmtpHost = "SmtpHost";
    public static string SmtpPort = "SmtpPort";
    public static string SmtpAdress = "SmtpAdress";
    public static string SmtpPassword = "SmtpPassword";
    public static string SenderSMPT = "SenderSMPT";
    public static string SenderSMPTDisplayName = "SenderSMPTDisplayName";
    public static string EWSUri = "EWSUri";
    public static string AppName = "AppName";
    public static string Subject = "Subject";
    public static string AdhesionId = "2439731";
    public static string FileName = "FileName.html";
    public static string pdfPath = "pdfPath";
    public static string ErrorMessage = "ErrorMessage";
    public static string Publisher = "Publisher";
    public static HttpStatusCode HttpStatusCode = HttpStatusCode.OK;
}
