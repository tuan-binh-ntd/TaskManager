namespace TaskManager.Application.Epics.Events;

internal sealed class EpicUpdatedPriorityDomainEventHandler(
    IPriorityRepository priorityRepository,
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    IIssueRepository issueRepository,
    INotificationRepository notificationRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<EpicUpdatedPriorityDomainEvent>
{
    private readonly IPriorityRepository _priorityRepository = priorityRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpicUpdatedPriorityDomainEvent notification, CancellationToken cancellationToken)
    {
        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Epic.Id) ?? throw new ProjectNullException();
        var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueEditedName);
        var sender = await _userManager.Users
                .Where(u => u.Id == notification.UpdateEpicDto.ModificationUserId)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new UserNullException();
        if (notification.UpdateEpicDto.PriorityId is Guid newPriorityId)
        {

            if (notification.Epic.PriorityId is Guid oldPriorityId)
            {
                string? newPriorityName = await _priorityRepository.GetNameOfPriorityByIdAsync(newPriorityId);
                string? oldPriorityName = await _priorityRepository.GetNameOfPriorityByIdAsync(oldPriorityId);
                var changedThePriorityHis = IssueHistory.Create(IssueConstants.Priority_IssueHistoryName,
                    notification.UpdateEpicDto.ModificationUserId,
                    $"{oldPriorityName} to {newPriorityName}",
                    notification.Epic.Id);

                _issueHistoryRepository.Insert(changedThePriorityHis);

                var changePriorityIssueEmailContentDto = new ChangePriorityIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
                {
                    FromPriorityName = oldPriorityName ?? string.Empty,
                    ToPriorityName = newPriorityName ?? string.Empty,
                };
                string emailContent = EmailContentConstants.ChangePriorityIssueContent(changePriorityIssueEmailContentDto);
                var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name, EmailConstants.MadeOneUpdate, project.Name, notification.Epic.Code, notification.Epic.Name, emailContent, project.Code, notification.Epic.Id);

                if (issueEditedEvent is not null)
                {
                    var sendEmailModel = SendEmailModel.Create(notification.Epic.Id,
                        notification.UpdateEpicDto.ModificationUserId,
                        buidEmailTemplateBaseDto,
                    issueEditedEvent);

                    await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
                }
            }
            else
            {
                string? newPriorityName = await _priorityRepository.GetNameOfPriorityByIdAsync((Guid)notification.UpdateEpicDto.PriorityId!);
                var changedThePriorityHis = IssueHistory.Create(IssueConstants.Priority_IssueHistoryName,
                    notification.UpdateEpicDto.ModificationUserId,
                    $"{IssueConstants.None_IssueHistoryContent} to {newPriorityName}",
                    notification.Epic.Id);

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
            }

            notification.Epic.PriorityId = newPriorityId;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
