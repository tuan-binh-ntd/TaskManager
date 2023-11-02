using MimeKit.Text;
using TaskManager.Core.DTOs;

namespace TaskManager.Core.Interfaces.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessageDto message, TextFormat textFormat = TextFormat.Text);
    }
}
