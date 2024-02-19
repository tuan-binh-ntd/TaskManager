namespace TaskManager.Application.Issues.Events;

internal sealed class IssueUpdatedPriorityDomainEventHandler(
    IPriorityRepository priorityRepository,
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    IIssueRepository issueRepository,
    INotificationRepository notificationRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueUpdatedPriorityDomainEvent>
{
    private readonly IPriorityRepository _priorityRepository = priorityRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueUpdatedPriorityDomainEvent notification, CancellationToken cancellationToken)
    {
        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Issue.Id) ?? throw new ProjectNullException();
        var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueEditedName);
        var sender = await _userManager.Users
                .Where(u => u.Id == notification.UpdateIssueDto.ModificationUserId)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new UserNullException();
        if (notification.UpdateIssueDto.PriorityId is Guid newPriorityId)
        {

            if (notification.Issue.PriorityId is Guid oldPriorityId)
            {
                string? newPriorityName = await _priorityRepository.GetNameOfPriorityByIdAsync(newPriorityId);
                string? oldPriorityName = await _priorityRepository.GetNameOfPriorityByIdAsync(oldPriorityId);
                var changedThePriorityHis = IssueHistory.Create(IssueConstants.Priority_IssueHistoryName,
                notification.UpdateIssueDto.ModificationUserId,
                    $"{oldPriorityName} to {newPriorityName}",
                    notification.Issue.Id);

                _issueHistoryRepository.Insert(changedThePriorityHis);

                var changePriorityIssueEmailContentDto = new ChangePriorityIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
                {
                    FromPriorityName = oldPriorityName ?? string.Empty,
                    ToPriorityName = newPriorityName ?? string.Empty,
                };
                string emailContent = EmailContentConstants.ChangePriorityIssueContent(changePriorityIssueEmailContentDto);
                var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name, EmailConstants.MadeOneUpdate, project.Name, notification.Issue.Code, notification.Issue.Name, emailContent, project.Code, notification.Issue.Id);

                if (issueEditedEvent is not null)
                {
                    var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                        notification.UpdateIssueDto.ModificationUserId,
                        buidEmailTemplateBaseDto,
                    issueEditedEvent);

                    await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
                }
            }
            else
            {
                string? newPriorityName = await _priorityRepository.GetNameOfPriorityByIdAsync((Guid)notification.UpdateIssueDto.PriorityId!);
                var changedThePriorityHis = IssueHistory.Create(IssueConstants.Priority_IssueHistoryName,
                    notification.UpdateIssueDto.ModificationUserId,
                    $"{IssueConstants.None_IssueHistoryContent} to {newPriorityName}",
                    notification.Issue.Id);

                _issueHistoryRepository.Insert(changedThePriorityHis);

                var changePriorityIssueEmailContentDto = new ChangePriorityIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
                {
                    FromPriorityName = IssueConstants.None_IssueHistoryContent,
                    ToPriorityName = newPriorityName ?? string.Empty,
                };
                string emailContent = EmailContentConstants.ChangePriorityIssueContent(changePriorityIssueEmailContentDto);
                var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name,
                    EmailConstants.MadeOneUpdate,
                    project.Name,
                    notification.Issue.Code,
                    notification.Issue.Name,
                    emailContent,
                    project.Code,
                    notification.Issue.Id);

                if (issueEditedEvent is not null)
                {
                    var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                    notification.UpdateIssueDto.ModificationUserId,
                        buidEmailTemplateBaseDto,
                        issueEditedEvent);

                    await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
                }
            }

            notification.Issue.PriorityId = newPriorityId;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
