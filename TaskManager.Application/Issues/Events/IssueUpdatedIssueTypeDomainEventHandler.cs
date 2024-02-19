namespace TaskManager.Application.Issues.Events;

internal sealed class IssueUpdatedIssueTypeDomainEventHandler(
    IIssueTypeRepository issueTypeRepository,
    IIssueHistoryRepository issueHistoryRepository,
    IIssueRepository issueRepository,
    INotificationRepository notificationRepository,
    UserManager<AppUser> userManager,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueUpdatedIssueTypeDomainEvent>
{
    private readonly IIssueTypeRepository _issueTypeRepository = issueTypeRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueUpdatedIssueTypeDomainEvent notification, CancellationToken cancellationToken)
    {
        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Issue.Id) ?? throw new ProjectNullException();
        var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueEditedName);
        var sender = await _userManager.Users
            .Where(u => u.Id == notification.UpdateIssueDto.ModificationUserId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new UserNullException();

        if (notification.UpdateIssueDto.IssueTypeId is Guid newIssueTypeId)
        {
            string? newIssueTypeName = await _issueTypeRepository.GetNameOfIssueTypeAsync(newIssueTypeId);
            string? oldIssueTypeName = await _issueTypeRepository.GetNameOfIssueTypeAsync(notification.Issue.IssueTypeId);
            var updatedTheIssueTypeHis = IssueHistory.Create(IssueConstants.IssueType_IssueHistoryName,
                notification.UpdateIssueDto.ModificationUserId,
                $"{oldIssueTypeName} to {newIssueTypeName}",
                notification.Issue.Id);

            _issueHistoryRepository.Insert(updatedTheIssueTypeHis);

            var changeIssueTypeIssueEmailContentDto = new ChangeIssueTypeIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
            {
                FromIssueTypeName = oldIssueTypeName ?? string.Empty,
                ToIssueTypeName = newIssueTypeName ?? string.Empty,
            };
            string emailContent = EmailContentConstants.ChangeIssueTypeIssueContent(changeIssueTypeIssueEmailContentDto);
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

            notification.Issue.IssueTypeId = newIssueTypeId;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
