namespace TaskManager.Application.Epics.Events;

internal sealed class EpicUpdatedNameDomainEventHandler(
    UserManager<AppUser> userManager,
    IIssueHistoryRepository issueHistoryRepository,
    INotificationRepository notificationRepository,
    IEmailSender emailSender,
    IIssueRepository issueRepository,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<EpicUpdatedNameDomainEvent>
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpicUpdatedNameDomainEvent notification, CancellationToken cancellationToken)
    {
        var updatedTheSumaryHis = IssueHistory.Create(IssueConstants.Summary_IssueHistoryName,
            notification.UpdateEpicDto.ModificationUserId,
            $"{notification.Epic.Name} to {notification.UpdateEpicDto.Name}",
            notification.Epic.Id);
        _issueHistoryRepository.Insert(updatedTheSumaryHis);

        if (notification.Epic.ProjectId is Guid projectId)
        {
            var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(projectId, IssueEventConstants.IssueEditedName);
            var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Epic.Id) ?? throw new ProjectNullException();
            var avatarUrl = await _userManager.Users
                .Where(u => u.Id == notification.UpdateEpicDto.ModificationUserId)
                .Select(u => u.AvatarUrl).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                ?? TextToImageConstants.AnonymousAvatar;

            var senderName = await _userManager.Users
                .Where(u => u.Id == notification.UpdateEpicDto.ModificationUserId)
                .Select(u => u.Name).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                ?? TextToImageConstants.AnonymousAvatar;

            var changeNameIssueEmailContentDto = new ChangeNameIssueEmailContentDto(senderName, DateTime.Now, avatarUrl)
            {
                FromName = notification.Epic.Name,
                ToName = notification.UpdateEpicDto.Name ?? string.Empty,
            };
            string emailContent = EmailContentConstants.ChangeNameIssueContent(changeNameIssueEmailContentDto);

            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName,
                EmailConstants.MadeOneUpdate,
                project.Name,
                notification.Epic.Code,
                notification.Epic.Name,
                emailContent,
                project.Code,
                notification.Epic.Id);

            if (issueEditedEvent is not null)
            {
                var sendEmailModel = SendEmailModel.Create(notification.Epic.Id,
                    notification.UpdateEpicDto.ModificationUserId,
                    buidEmailTemplateBaseDto,
                    issueEditedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }

            notification.Epic.Name = notification.UpdateEpicDto.Name ?? string.Empty;

            _issueRepository.Update(notification.Epic);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
