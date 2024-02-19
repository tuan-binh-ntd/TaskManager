namespace TaskManager.Application.Epics.Events;

internal sealed class EpicDeletedDomainEventHandler(
    INotificationRepository notificationRepository,
    UserManager<AppUser> userManager,
    IIssueRepository issueRepository,
    IEmailSender emailSender
    )
    : IDomainEventHandler<EpicDeletedDomainEvent>
{
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task Handle(EpicDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Epic.ProjectId is Guid projectId)
        {
            var issueDeletedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(projectId, IssueEventConstants.IssueDeletedName);

            if (issueDeletedEvent is not null)
            {
                var reporterName = await _userManager.Users
                    .Where(u => u.Id == notification.UserId)
                    .Select(u => u.Name)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? IssueConstants.None_IssueHistoryContent;
                var avatarUrl = await _userManager.Users
                    .Where(u => u.Id == notification.UserId)
                    .Select(u => u.AvatarUrl)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? TextToImageConstants.AnonymousAvatar;

                var deletedIssueEmailContentDto = new DeletedIssueEmailContentDto(reporterName, notification.Epic.CreationTime, avatarUrl)
                {
                    IssueName = notification.Epic.Name,
                };

                string emailContent = EmailContentConstants.DeleteIssueContent(deletedIssueEmailContentDto);
                var senderName = await _userManager.Users
                    .Where(u => u.Id == notification.UserId)
                    .Select(u => u.Name)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? IssueConstants.None_IssueHistoryContent;

                var projectName = await _issueRepository.GetProjectNameOfIssueAsync(notification.Epic.Id);
                var projectCode = await _issueRepository.GetProjectCodeOfIssueAsync(notification.Epic.Id);

                var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName,
                    EmailConstants.DeletedIssue,
                    projectName,
                    notification.Epic.Code,
                    notification.Epic.Name,
                    emailContent,
                    projectCode,
                    notification.Epic.Id);

                var sendEmailModel = SendEmailModel.Create(notification.Epic.Id, notification.UserId, buidEmailTemplateBaseDto, issueDeletedEvent);
                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }
        }
    }
}
