using Contract.Services.EmailSenderProvider;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Application.Services.EmailSenderProider;

public class EmailSender : IEmailSender
{
    public Task Execute(string userEmail, string body, string subject)
    {
        SmtpClient client = new()
        {
            Port = 587, //gmail server
            Host = "smtp.gmail.com",
            EnableSsl = true,
            Timeout = 1000000,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential("meysam.ghiasvand@gmail.com", "Meysam2048Gmail")
        };
        MailMessage message = new("meysam.ghiasvand@gmail.com", userEmail, subject, body)
        {
            IsBodyHtml = true,
            BodyEncoding = Encoding.UTF8,
            DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess
        };
        client.Send(message);
        return Task.CompletedTask;
    }
}