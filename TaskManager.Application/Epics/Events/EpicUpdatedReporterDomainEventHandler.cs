namespace TaskManager.Application.Epics.Events;

internal sealed class EpicUpdatedReporterDomainEventHandler(
    IIssueRepository issueRepository,
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    INotificationRepository notificationRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<EpicUpdatedReporterDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpicUpdatedReporterDomainEvent notification, CancellationToken cancellationToken)
    {
        await _issueRepository.LoadIssueDetailAsync(notification.Epic);
        if (notification.UpdateEpicDto.ReporterId is Guid reporterId)
        {

            var updatedTheReporterHis = IssueHistory.Create(IssueConstants.Reporter_IssueHistoryName,
                notification.UpdateEpicDto.ModificationUserId,
                new ReporterFromTo
                {
                    From = notification.Epic.IssueDetail!.ReporterId,
                    To = reporterId
                }.ToJson(),
                notification.Epic.Id);

            _issueHistoryRepository.Insert(updatedTheReporterHis);

            var sender = await _userManager.Users
                .Where(u => u.Id == notification.UpdateEpicDto.ModificationUserId)
                .FirstOrDefaultAsync() ?? throw new UserNullException();

            var fromReporterName = await _userManager.Users
                .Where(u => u.Id == notification.Epic.IssueDetail!.ReporterId)
                .Select(u => u.Name)
                .FirstOrDefaultAsync();

            var toReporterName = await _userManager.Users
                .Where(u => u.Id == reporterId)
                .Select(u => u.Name)
                .FirstOrDefaultAsync();

            var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Epic.Id) ?? throw new ProjectNullException();
            var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueEditedName);

            var changeReporterIssueEmailContentDto = new ChangeReporterIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
            {
                FromReporterName = fromReporterName ?? string.Empty,
                ToReporterName = toReporterName ?? string.Empty,
            };

            string emailContent = EmailContentConstants.ChangeReporterIssueContent(changeReporterIssueEmailContentDto);
            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name, EmailConstants.MadeOneUpdate, project.Name, notification.Epic.Code, notification.Epic.Name, emailContent, project.Code, notification.Epic.Id);
            if (issueEditedEvent is not null)
            {
                var sendEmailModel = SendEmailModel.Create(notification.Epic.Id,
                    notification.UpdateEpicDto.ModificationUserId,
                    buidEmailTemplateBaseDto,
            issueEditedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }

            notification.Epic.IssueDetail.ReporterId = reporterId;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
