using System.Threading.Tasks;

namespace Business.Services.Interfaces {
    public interface IEmailService {
        Task SendEmailAsync (string email, string subject, string message);
    }
}