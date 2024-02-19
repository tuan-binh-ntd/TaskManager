namespace TaskManager.Application.Issues.Events;

internal sealed class IssueUpdatedSprintDomainEventHandler(
    IIssueRepository issueRepository,
    INotificationRepository notificationRepository,
    ISprintRepository sprintRepository,
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
)
    : IDomainEventHandler<IssueUpdatedSprintDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly ISprintRepository _sprintRepository = sprintRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueUpdatedSprintDomainEvent notification, CancellationToken cancellationToken)
    {
        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Issue.Id) ?? throw new ProjectNullException();
        var issueMovedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueMovedName);
        var sender = await _userManager.Users
                .Where(u => u.Id == notification.UpdateIssueDto.ModificationUserId)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new UserNullException();

        if (notification.UpdateIssueDto.SprintId is Guid newSprintId)
        {
            if (notification.Issue.SprintId is Guid oldSprintId)
            {
                string? oldSprintName = await _sprintRepository.GetNameOfSprintAsync(oldSprintId);
                string? newSprintName = await _sprintRepository.GetNameOfSprintAsync(newSprintId);
                var changedTheParentHis = IssueHistory.Create(IssueConstants.Parent_IssueHistoryName,
                    notification.UpdateIssueDto.ModificationUserId,
                    $"{oldSprintName} to {newSprintName}",
                    notification.Issue.Id);

                _issueHistoryRepository.Insert(changedTheParentHis);

                var changeSprintIssueEmailContentDto = new ChangeSprintIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
                {
                    FromSprintName = oldSprintName ?? string.Empty,
                    ToSprintName = newSprintName ?? string.Empty,
                };

                string emailContent = EmailContentConstants.ChangeSprintIssueContent(changeSprintIssueEmailContentDto);
                var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name,
                    EmailConstants.MadeOneUpdate,
                    project.Name,
                    notification.Issue.Code,
                    notification.Issue.Name,
                    emailContent,
                    project.Code,
                    notification.Issue.Id);

                if (issueMovedEvent is not null)
                {
                    var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                        notification.UpdateIssueDto.ModificationUserId,
                        buidEmailTemplateBaseDto,
                    issueMovedEvent);

                    await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
                }
            }
            else if (notification.Issue.SprintId is null)
            {
                string? newSprintName = await _sprintRepository.GetNameOfSprintAsync(newSprintId);
                var changedTheParentHis = IssueHistory.Create(IssueConstants.Parent_IssueHistoryName,
                    notification.UpdateIssueDto.ModificationUserId,
                    $"{IssueConstants.None_IssueHistoryContent} to {newSprintName}",
                    notification.Issue.Id);

                _issueHistoryRepository.Insert(changedTheParentHis);

                var changeSprintIssueEmailContentDto = new ChangeSprintIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
                {
                    FromSprintName = string.Empty,
                    ToSprintName = newSprintName ?? string.Empty,
                };
                string emailContent = EmailContentConstants.ChangeSprintIssueContent(changeSprintIssueEmailContentDto);
                var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name,
                    EmailConstants.MadeOneUpdate,
                    project.Name,
                    notification.Issue.Code,
                    notification.Issue.Name,
                    emailContent,
                    project.Code,
                    notification.Issue.Id);
                if (issueMovedEvent is not null)
                {
                    var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                        notification.UpdateIssueDto.ModificationUserId,
                        buidEmailTemplateBaseDto,
                        issueMovedEvent);

                    await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
                }
            }

            notification.Issue.SprintId = newSprintId;
        }
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
