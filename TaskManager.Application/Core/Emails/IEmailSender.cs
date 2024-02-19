namespace TaskManager.Application.Core.Emails;

public interface IEmailSender
{
    Task SendEmailWhenUpdateIssue(Guid issueId, string subjectOfEmail, Guid from, BuidEmailTemplateBaseDto buidEmailTemplateBase, NotificationEventViewModel notificationEventViewModel);
    Task SendEmailWhenCreatedIssue(SendEmailModel sendEmailModel);
}
