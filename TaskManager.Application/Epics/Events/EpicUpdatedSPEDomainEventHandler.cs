namespace TaskManager.Application.Epics.Events;

internal sealed class EpicUpdatedSPEDomainEventHandler(
    IIssueRepository issueRepository,
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    INotificationRepository notificationRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<EpicUpdatedSPEDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpicUpdatedSPEDomainEvent notification, CancellationToken cancellationToken)
    {
        await _issueRepository.LoadIssueDetailAsync(notification.Epic);
        var updatedTheSPEHis = IssueHistory.Create(IssueConstants.StoryPointEstimate_IssueHistoryName,
            notification.UpdateEpicDto.ModificationUserId,
            $"{notification.Epic.IssueDetail!.StoryPointEstimate} to {notification.UpdateEpicDto.StoryPointEstimate}",
            notification.Epic.Id);

        _issueHistoryRepository.Insert(updatedTheSPEHis);
        var sender = await _userManager.Users
            .Where(u => u.Id == notification.UpdateEpicDto.ModificationUserId)
            .FirstOrDefaultAsync() ?? throw new UserNullException();

        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Epic.Id) ?? throw new ProjectNullException();
        var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueEditedName);

        var changeSPEIssueEmailContentDto = new ChangeSPEIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
        {
            FromSPEName = notification.Epic.IssueDetail!.StoryPointEstimate.ToString(),
            ToSPEName = notification.UpdateEpicDto.StoryPointEstimate?.ToString() ?? "0",
        };
        string emailContent = EmailContentConstants.ChangeSPEIssueContent(changeSPEIssueEmailContentDto);
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

        notification.Epic.IssueDetail!.StoryPointEstimate = notification.UpdateEpicDto.StoryPointEstimate ?? 0;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
