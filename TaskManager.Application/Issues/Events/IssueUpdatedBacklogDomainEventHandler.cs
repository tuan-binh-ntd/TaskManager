namespace TaskManager.Application.Issues.Events;

internal class IssueUpdatedBacklogDomainEventHandler(
    IIssueRepository issueRepository,
    INotificationRepository notificationRepository,
    ISprintRepository sprintRepository,
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueUpdatedBacklogDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueUpdatedBacklogDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Issue.SprintId is Guid oldSprintId)
        {
            string? oldSprintName = await _sprintRepository.GetNameOfSprintAsync(oldSprintId);
            var changedTheBacklogHis = IssueHistory.Create(IssueConstants.Sprint_IssueHistoryName,
                notification.UpdateIssueDto.ModificationUserId,
                 $"{oldSprintName} to {IssueConstants.None_IssueHistoryContent}",
                 notification.Issue.Id);

            _issueHistoryRepository.Insert(changedTheBacklogHis);
            var sender = await _userManager.Users
                .Where(u => u.Id == notification.UpdateIssueDto.ModificationUserId)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new UserNullException();

            var changeSprintIssueEmailContentDto = new ChangeSprintIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
            {
                FromSprintName = oldSprintName ?? string.Empty,
                ToSprintName = string.Empty,
            };
            var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Issue.Id) ?? throw new ProjectNullException();
            var issueMovedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueMovedName);
            string emailContent = EmailContentConstants.ChangeSprintIssueContent(changeSprintIssueEmailContentDto);
            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name, EmailConstants.MadeOneUpdate, project.Name, notification.Issue.Code, notification.Issue.Name, emailContent, project.Code, notification.Issue.Id);

            if (issueMovedEvent is not null)
            {
                var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                    notification.UpdateIssueDto.ModificationUserId,
                    buidEmailTemplateBaseDto,
                issueMovedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }
        }
        notification.Issue.BacklogId = notification.UpdateIssueDto.BacklogId;
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
