namespace TaskManager.Application.Epics.Events;

internal sealed class EpicUpdatedDueDateDomainEventHandler(
        IIssueRepository issueRepository,
        IIssueHistoryRepository issueHistoryRepository,
        UserManager<AppUser> userManager,
        INotificationRepository notificationRepository,
        IEmailSender emailSender,
        IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<EpicUpdatedDueDateDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpicUpdatedDueDateDomainEvent notification, CancellationToken cancellationToken)
    {
        var updatedTheDueDateHis = IssueHistory.Create(IssueConstants.DueDate_EpicHistoryName,
            notification.UpdateEpicDto.ModificationUserId,
            notification.Epic.Id);

        var sender = await _userManager.Users
            .Where(u => u.Id == notification.UpdateEpicDto.ModificationUserId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new UserNullException();

        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Epic.Id) ?? throw new ProjectNullException();
        var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueEditedName);

        var changeDueDateIssueEmailContentDto = new ChangeDueDateIssueEmailContentDto(sender.Name, notification.Epic.CreationTime, sender.AvatarUrl);

        if (notification.UpdateEpicDto.DueDate is DateTime newDueDate && notification.Epic.DueDate is null)
        {
            updatedTheDueDateHis.Content = $"{IssueConstants.None_IssueHistoryContent} to {newDueDate:MMM dd, yyyy}";

            changeDueDateIssueEmailContentDto.FromDueDate = IssueConstants.None_IssueHistoryContent;
            changeDueDateIssueEmailContentDto.ToDueDate = newDueDate.ToString("dd/MMM/yy");

            notification.Epic.DueDate = newDueDate;
        }
        else if (notification.UpdateEpicDto.DueDate is DateTime newDueDate1 && notification.Epic.DueDate is DateTime oldDueDate1)
        {
            updatedTheDueDateHis.Content = $"{oldDueDate1:MMM dd, yyyy} to {newDueDate1:MMM dd, yyyy}";

            changeDueDateIssueEmailContentDto.FromDueDate = oldDueDate1.ToString("dd/MMM/yy");
            changeDueDateIssueEmailContentDto.ToDueDate = newDueDate1.ToString("dd/MMM/yy");
            notification.Epic.DueDate = newDueDate1;

        }
        else if (notification.UpdateEpicDto.DueDate is null && notification.Epic.DueDate is DateTime oldDueDate2)
        {
            updatedTheDueDateHis.Content = $"{oldDueDate2:MMM dd, yyyy} to {IssueConstants.None_IssueHistoryContent}";

            changeDueDateIssueEmailContentDto.FromDueDate = oldDueDate2.ToString("dd/MMM/yy");
            changeDueDateIssueEmailContentDto.ToDueDate = IssueConstants.None_IssueHistoryContent;

            notification.Epic.DueDate = null;
            notification.UpdateEpicDto.DueDate = null;
        }
        string emailContent = EmailContentConstants.ChangeDueDateIssueContent(changeDueDateIssueEmailContentDto);
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

        _issueHistoryRepository.Insert(updatedTheDueDateHis);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
