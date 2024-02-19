namespace TaskManager.Application.Issues.Events;

internal sealed class IssueUpdatedStatusDomainEventHandler(
    IStatusRepository statusRepository,
    IIssueRepository issueRepository,
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    INotificationRepository notificationRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueUpdatedStatusDomainEvent>
{
    private readonly IStatusRepository _statusRepository = statusRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueUpdatedStatusDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.Issue.StatusId is Guid oldStatusId && notification.UpdateIssueDto.StatusId is Guid newStatusId)
        {
            var isComplete = await _statusRepository.CheckStatusBelongDoneAsync(newStatusId);
            if (isComplete)
            {
                notification.Issue.CompleteDate = DateTime.Now;
            }

            string? newStatusName = await _statusRepository.GetNameOfStatusAsync(newStatusId);
            string? oldStatusName = await _statusRepository.GetNameOfStatusAsync(oldStatusId);

            var changedTheStatusHis = IssueHistory.Create(IssueConstants.Status_IssueHistoryName,
                notification.UpdateIssueDto.ModificationUserId,
                $"{oldStatusName} to {newStatusName}",
                notification.Issue.Id);

            _issueHistoryRepository.Insert(changedTheStatusHis);

            var sender = await _userManager.Users
                .Where(u => u.Id == notification.UpdateIssueDto.ModificationUserId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new UserNullException();

            var changeStatusIssueEmailContentDto = new ChangeStatusIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
            {
                FromStatusName = oldStatusName ?? string.Empty,
                ToStatusName = newStatusName ?? string.Empty,
            };
            var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Issue.Id) ?? throw new ProjectNullException();
            var issueMovedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueMovedName);
            string emailContent = EmailContentConstants.ChangeStatusIssueContent(changeStatusIssueEmailContentDto);
            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name, EmailConstants.MadeOneUpdate, project.Name, notification.Issue.Code, notification.Issue.Name, emailContent, project.Code, notification.Issue.Id);
            if (issueMovedEvent is not null)
            {
                var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                    notification.UpdateIssueDto.ModificationUserId,
                    buidEmailTemplateBaseDto,
                issueMovedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }

            notification.Issue.StatusId = newStatusId;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
