namespace TaskManager.Application.Core.Emails;

public class SendEmailModel
{
    private SendEmailModel(Guid issueId,
        Guid fromUserId,
        BuidEmailTemplateBaseDto buidEmailTemplate,
        NotificationEventViewModel notificationEventViewModel)
    {
        IssueId = issueId;
        FromUserId = fromUserId;
        BuidEmailTemplate = buidEmailTemplate;
        NotificationEventViewModel = notificationEventViewModel;
    }

    public Guid IssueId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public Guid FromUserId { get; set; }
    public BuidEmailTemplateBaseDto BuidEmailTemplate { get; set; }
    public NotificationEventViewModel NotificationEventViewModel { get; set; }

    public static SendEmailModel Create(Guid issueId,
        Guid fromUserId,
        BuidEmailTemplateBaseDto buidEmailTemplate,
        NotificationEventViewModel notificationEventViewModel)
    {
        return new SendEmailModel(issueId, fromUserId, buidEmailTemplate, notificationEventViewModel);
    }
}
