namespace TaskManager.Application.Issues.Events;

internal sealed class IssueUpdatedParentDomainEventHandler(
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    INotificationRepository notificationRepository,
    IIssueRepository issueRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueUpdatedParentDomainEvent>
{
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueUpdatedParentDomainEvent notification, CancellationToken cancellationToken)
    {
        string? oldParentName = notification.Issue.ParentId is Guid parentId
            ? await _issueRepository.GetNameOfIssueAsync(parentId)
            : IssueConstants.None_IssueHistoryContent;

        string? newParentName = IssueConstants.None_IssueHistoryContent;

        if (notification.UpdateIssueDto.ParentId is Guid newParentId)
        {
            newParentName = await _issueRepository.GetNameOfIssueAsync(newParentId);
            notification.Issue.ParentId = newParentId;
        }

        var changedTheParentHis = IssueHistory.Create(IssueConstants.Parent_IssueHistoryName,
            notification.UpdateIssueDto.ModificationUserId,
            $"{oldParentName} to {newParentName}",
            notification.Issue.Id);
        _issueHistoryRepository.Insert(changedTheParentHis);

        var sender = await _userManager.Users
            .Where(u => u.Id == notification.UpdateIssueDto.ModificationUserId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new UserNullException();

        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Issue.Id) ?? throw new ProjectNullException();
        var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueEditedName);

        var changeParentIssueEmailContentDto = new ChangeParentIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
        {
            FromParentName = oldParentName ?? string.Empty,
            ToParentName = newParentName ?? string.Empty,
        };
        string emailContent = EmailContentConstants.ChangeParentIssueContent(changeParentIssueEmailContentDto);
        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name, EmailConstants.MadeOneUpdate, project.Name, notification.Issue.Code, notification.Issue.Name, emailContent, project.Code, notification.Issue.Id);
        if (issueEditedEvent is not null)
        {
            var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                    notification.UpdateIssueDto.ModificationUserId,
                    buidEmailTemplateBaseDto,
                    issueEditedEvent);

            await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
