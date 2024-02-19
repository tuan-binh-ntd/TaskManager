namespace TaskManager.Application.Epics.Events;

internal sealed class EpicUpdatedParentDomainEventHandler(
    IIssueRepository issueRepository,
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    INotificationRepository notificationRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<EpicUpdatedParentDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpicUpdatedParentDomainEvent notification, CancellationToken cancellationToken)
    {
        string oldParentName = IssueConstants.None_IssueHistoryContent;
        if (notification.Epic.ParentId is Guid oldParentId) oldParentName = await _issueRepository.GetNameOfIssueAsync(oldParentId);

        string newParentName = IssueConstants.None_IssueHistoryContent;
        if (notification.UpdateEpicDto.ParentId is Guid newParentId)
        {
            newParentName = await _issueRepository.GetNameOfIssueAsync(newParentId);
            notification.Epic.ParentId = newParentId;
        }

        var changedTheParentHis = IssueHistory.Create(IssueConstants.Parent_IssueHistoryName,
        notification.UpdateEpicDto.ModificationUserId,
        $"{oldParentName} to {newParentName}",
        notification.Epic.Id);

        _issueHistoryRepository.Insert(changedTheParentHis);

        var avatarUrl = await _userManager.Users
            .Where(u => u.Id == notification.UpdateEpicDto.ModificationUserId)
            .Select(u => u.AvatarUrl)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? TextToImageConstants.AnonymousAvatar;
        var senderName = await _userManager.Users

            .Where(u => u.Id == notification.UpdateEpicDto.ModificationUserId)
            .Select(u => u.Name)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
        ?? IssueConstants.None_IssueHistoryContent;

        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Epic.Id) ?? throw new ProjectNullException();
        var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueEditedName);

        var changeParentIssueEmailContentDto = new ChangeParentIssueEmailContentDto(senderName, DateTime.Now, avatarUrl)
        {
            FromParentName = oldParentName,
            ToParentName = newParentName,
        };
        string emailContent = EmailContentConstants.ChangeParentIssueContent(changeParentIssueEmailContentDto);
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
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
