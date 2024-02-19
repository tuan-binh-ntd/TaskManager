namespace TaskManager.Application.Issues.Events;

internal sealed class IssueUpdatedNameDomainEventHandler(
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    INotificationRepository notificationRepository,
    IIssueRepository issueRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueUpdatedNameDomainEvent>
{
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueUpdatedNameDomainEvent notification, CancellationToken cancellationToken)
    {
        var updatedTheSumaryHis = IssueHistory.Create(IssueConstants.Summary_IssueHistoryName,
            notification.UpdateIssueDto.ModificationUserId,
            $"{notification.Issue.Name} to {notification.UpdateIssueDto.Name}",
            notification.Issue.Id);

        _issueHistoryRepository.Insert(updatedTheSumaryHis);
        var sender = await _userManager.Users
            .Where(u => u.Id == notification.UpdateIssueDto.ModificationUserId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new UserNullException();

        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Issue.Id) ?? throw new ProjectNullException();
        var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueEditedName);

        var changeNameIssueEmailContentDto = new ChangeNameIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
        {
            FromName = notification.Issue.Name,
            ToName = notification.UpdateIssueDto.Name ?? string.Empty,
        };
        string emailContent = EmailContentConstants.ChangeNameIssueContent(changeNameIssueEmailContentDto);
        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name, EmailConstants.MadeOneUpdate, project.Name, notification.Issue.Code, notification.Issue.Name, emailContent, project.Code, notification.Issue.Id);
        if (issueEditedEvent is not null)
        {
            var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                notification.UpdateIssueDto.ModificationUserId,
                buidEmailTemplateBaseDto,
                issueEditedEvent);

            await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
        }

        notification.Issue.Name = notification.UpdateIssueDto.Name ?? string.Empty;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
