namespace TaskManager.Application.Issues.Events;

internal sealed class IssueDeletedDomainEventHandler(
    IIssueRepository issueRepository,
    IEmailSender emailSender,
    UserManager<AppUser> userManager,
    INotificationRepository notificationRepository
    )
    : IDomainEventHandler<IssueDeletedDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly INotificationRepository _notificationRepository = notificationRepository;

    public async Task Handle(IssueDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Issue.Id);
        var issueDeletedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueDeletedName);

        if (issueDeletedEvent is not null)
        {
            var sender = await _userManager.Users
                .Where(u => u.Id == notification.UserId)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new UserNullException();

            var deletedIssueEmailContentDto = new DeletedIssueEmailContentDto(sender.Name, notification.Issue.CreationTime, sender.AvatarUrl)
            {
                IssueName = notification.Issue.Name,
            };

            string emailContent = EmailContentConstants.DeleteIssueContent(deletedIssueEmailContentDto);
            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name, EmailConstants.DeletedIssue, project.Name, notification.Issue.Code, notification.Issue.Name, emailContent, project.Code, notification.Issue.Id);

            if (issueDeletedEvent is not null)
            {
                var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                        sender.Id,
                        buidEmailTemplateBaseDto,
                issueDeletedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }
        }
    }
}
