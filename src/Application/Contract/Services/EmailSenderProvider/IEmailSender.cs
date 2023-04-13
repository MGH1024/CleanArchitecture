using System.Threading.Tasks;

namespace Contract.Services.EmailSenderProvider;

public interface IEmailSender
{
    Task Execute(string userEmail, string body, string subject);
}