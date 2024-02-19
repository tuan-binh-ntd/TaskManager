namespace TaskManager.Application.Epics.Events;

internal sealed class EpicUpdatedStartDateDomainEventHandler(
        IIssueRepository issueRepository,
        IIssueHistoryRepository issueHistoryRepository,
        UserManager<AppUser> userManager,
        INotificationRepository notificationRepository,
        IEmailSender emailSender,
        IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<EpicUpdatedStartDateDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpicUpdatedStartDateDomainEvent notification, CancellationToken cancellationToken)
    {
        var updatedTheStartDateHis = IssueHistory.Create(IssueConstants.StartDate_EpicHistoryName,
            notification.UpdateEpicDto.ModificationUserId,
            notification.Epic.Id);

        var sender = await _userManager.Users
            .Where(u => u.Id == notification.UpdateEpicDto.ModificationUserId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new UserNullException();

        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Epic.Id) ?? throw new ProjectNullException();
        var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueEditedName);

        var changeStartDateIssueEmailContentDto = new ChangeStartDateIssueEmailContentDto(sender.Name, notification.Epic.CreationTime, sender.AvatarUrl);

        if (notification.UpdateEpicDto.StartDate is DateTime newStartDate && notification.Epic.StartDate is null)
        {
            updatedTheStartDateHis.Content = $"{IssueConstants.None_IssueHistoryContent} to {newStartDate:MMM dd, yyyy}";

            changeStartDateIssueEmailContentDto.FromStartDate = IssueConstants.None_IssueHistoryContent;
            changeStartDateIssueEmailContentDto.ToStartDate = newStartDate.ToString("dd/MMM/yy");

            notification.Epic.StartDate = newStartDate;
        }
        else if (notification.UpdateEpicDto.StartDate is DateTime newStartDate1 && notification.Epic.StartDate is DateTime oldStartDate1)
        {
            updatedTheStartDateHis.Content = $"{oldStartDate1:MMM dd, yyyy} to {newStartDate1:MMM dd, yyyy}";

            changeStartDateIssueEmailContentDto.FromStartDate = oldStartDate1.ToString("dd/MMM/yy");
            changeStartDateIssueEmailContentDto.ToStartDate = newStartDate1.ToString("dd/MMM/yy");

            notification.Epic.StartDate = newStartDate1;
        }
        else if (notification.UpdateEpicDto.StartDate is null && notification.Epic.StartDate is DateTime oldStartDate2)
        {
            updatedTheStartDateHis.Content = $"{oldStartDate2:MMM dd, yyyy} to {IssueConstants.None_IssueHistoryContent}";

            changeStartDateIssueEmailContentDto.FromStartDate = oldStartDate2.ToString("dd/MMM/yy");
            changeStartDateIssueEmailContentDto.ToStartDate = IssueConstants.None_IssueHistoryContent;

            notification.Epic.StartDate = null;
            notification.UpdateEpicDto.StartDate = null;
        }
        string emailContent = EmailContentConstants.ChangeStartDateIssueContent(changeStartDateIssueEmailContentDto);
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

        _issueHistoryRepository.Insert(updatedTheStartDateHis);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
