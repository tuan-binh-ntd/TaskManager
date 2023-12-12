using MimeKit.Text;
using TaskManager.Core.DTOs;

namespace TaskManager.Core.Interfaces.Services;

public interface IEmailSender
{
    Task SendEmailAsync(EmailMessageDto message, TextFormat textFormat = TextFormat.Text);
    Task SendEmailWhenUpdateIssue(Guid issueId, string subjectOfEmail, Guid from, BuidEmailTemplateBaseDto buidEmailTemplateBase);
    Task SendEmailWhenCreatedIssue(Guid issueId, string subjectOfEmail, Guid from, BuidEmailTemplateBaseDto buidEmailTemplateBase);
}
