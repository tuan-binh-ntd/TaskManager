namespace TaskManager.Application.Epics.Events;

internal sealed class EpicUpdatedStatusDomainEventHandler(
    IStatusRepository statusRepository,
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    IIssueRepository issueRepository,
    INotificationRepository notificationRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<EpicUpdatedStatusDomainEvent>
{
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpicUpdatedStatusDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Epic.StatusId is Guid oldStatusId && notification.UpdateEpicDto.StatusId is Guid newStatusId)
        {
            var isComplete = await _statusRepository.CheckStatusBelongDoneAsync(newStatusId);
            if (isComplete)
            {
                notification.Epic.CompleteDate = DateTime.Now;
            }

            string? newStatusName = await _statusRepository.GetNameOfStatusAsync(newStatusId);
            string? oldStatusName = await _statusRepository.GetNameOfStatusAsync(oldStatusId);

            var changedTheStatusHis = IssueHistory.Create(IssueConstants.Status_IssueHistoryName,
                notification.UpdateEpicDto.ModificationUserId,
                $"{oldStatusName} to {newStatusName}",
                notification.Epic.Id);

            _issueHistoryRepository.Insert(changedTheStatusHis);

            var sender = await _userManager.Users
                .Where(u => u.Id == notification.UpdateEpicDto.ModificationUserId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new UserNullException();

            var changeStatusIssueEmailContentDto = new ChangeStatusIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
            {
                FromStatusName = oldStatusName ?? string.Empty,
                ToStatusName = newStatusName ?? string.Empty,
            };
            var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Epic.Id) ?? throw new ProjectNullException();
            var issueMovedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueMovedName);
            string emailContent = EmailContentConstants.ChangeStatusIssueContent(changeStatusIssueEmailContentDto);
            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name, EmailConstants.MadeOneUpdate, project.Name, notification.Epic.Code, notification.Epic.Name, emailContent, project.Code, notification.Epic.Id);
            if (issueMovedEvent is not null)
            {
                var sendEmailModel = SendEmailModel.Create(notification.Epic.Id,
                    notification.UpdateEpicDto.ModificationUserId,
                    buidEmailTemplateBaseDto,
                issueMovedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }

            notification.Epic.StatusId = newStatusId;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
