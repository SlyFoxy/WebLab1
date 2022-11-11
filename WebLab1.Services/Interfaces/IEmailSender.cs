using System.Threading.Tasks;

namespace WebLab1.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string To, string ToName, string Subject, string Body);
    }
}