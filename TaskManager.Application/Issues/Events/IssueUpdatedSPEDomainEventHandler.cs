namespace TaskManager.Application.Issues.Events;

internal sealed class IssueUpdatedSPEDomainEventHandler(
    IIssueRepository issueRepository,
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    INotificationRepository notificationRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueUpdatedSPEDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueUpdatedSPEDomainEvent notification, CancellationToken cancellationToken)
    {
        var updatedTheSPEHis = IssueHistory.Create(IssueConstants.StoryPointEstimate_IssueHistoryName,
            notification.UpdateIssueDto.ModificationUserId,
            $"{notification.Issue.IssueDetail!.StoryPointEstimate} to {notification.UpdateIssueDto.StoryPointEstimate}",
            notification.Issue.Id);

        _issueHistoryRepository.Insert(updatedTheSPEHis);
        var sender = await _userManager.Users
        .Where(u => u.Id == notification.UpdateIssueDto.ModificationUserId)
            .FirstOrDefaultAsync() ?? throw new UserNullException();

        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Issue.Id) ?? throw new ProjectNullException();
        var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueEditedName);

        var changeSPEIssueEmailContentDto = new ChangeSPEIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
        {
            FromSPEName = notification.Issue.IssueDetail!.StoryPointEstimate.ToString(),
            ToSPEName = notification.UpdateIssueDto.StoryPointEstimate?.ToString() ?? "0",
        };
        string emailContent = EmailContentConstants.ChangeSPEIssueContent(changeSPEIssueEmailContentDto);
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

        notification.Issue.IssueDetail!.StoryPointEstimate = notification.UpdateIssueDto.StoryPointEstimate ?? 0;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
