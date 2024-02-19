namespace TaskManager.Application.Issues.Events;

internal sealed class IssueUpdatedReporterDomainEventHandler(
    IIssueRepository issueRepository,
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    INotificationRepository notificationRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<IssueUpdatedReporterDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(IssueUpdatedReporterDomainEvent notification, CancellationToken cancellationToken)
    {
        if (notification.UpdateIssueDto.ReporterId is Guid reporterId)
        {

            var updatedTheReporterHis = IssueHistory.Create(IssueConstants.Reporter_IssueHistoryName,
                notification.UpdateIssueDto.ModificationUserId,
                new ReporterFromTo
                {
                    From = notification.Issue.IssueDetail!.ReporterId,
                    To = reporterId
                }.ToJson(),
                notification.Issue.Id);

            _issueHistoryRepository.Insert(updatedTheReporterHis);

            var sender = await _userManager.Users
                .Where(u => u.Id == notification.UpdateIssueDto.ModificationUserId)
                .FirstOrDefaultAsync() ?? throw new UserNullException();

            var fromReporterName = await _userManager.Users
                .Where(u => u.Id == notification.Issue.IssueDetail!.ReporterId)
                .Select(u => u.Name)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            var toReporterName = await _userManager.Users
            .Where(u => u.Id == reporterId)
                .Select(u => u.Name)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Issue.Id) ?? throw new ProjectNullException();
            var issueEditedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.IssueEditedName);

            var changeReporterIssueEmailContentDto = new ChangeReporterIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl)
            {
                FromReporterName = fromReporterName ?? string.Empty,
                ToReporterName = toReporterName ?? string.Empty,
            };

            string emailContent = EmailContentConstants.ChangeReporterIssueContent(changeReporterIssueEmailContentDto);
            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name, EmailConstants.MadeOneUpdate, project.Name, notification.Issue.Code, notification.Issue.Name, emailContent, project.Code, notification.Issue.Id);
            if (issueEditedEvent is not null)
            {
                var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                    notification.UpdateIssueDto.ModificationUserId,
                    buidEmailTemplateBaseDto,
            issueEditedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }

            notification.Issue.IssueDetail.ReporterId = reporterId;
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
