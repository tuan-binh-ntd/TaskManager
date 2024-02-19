namespace TaskManager.Application.Issues.Events;

internal sealed class IssueUpdatedAssigneeDomainEventHandler(
    IIssueRepository issueRepository,
    UserManager<AppUser> userManager,
    IIssueHistoryRepository issueHistoryRepository,
    INotificationRepository notificationRepository,
    IEmailSender emailSender
    )
    : IDomainEventHandler<IssueUpdatedAssigneeDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task Handle(IssueUpdatedAssigneeDomainEvent notification, CancellationToken cancellationToken)
    {
        var changedTheAssigneeHis = IssueHistory.Create(IssueConstants.Assignee_IssueHistoryName,
            notification.UpdateIssueDto.ModificationUserId,
            string.Empty,
            notification.Issue.Id);

        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Issue.Id) ?? throw new ProjectNullException();
        var someoneAssignedEvent = await _notificationRepository
            .GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.SomeoneAssignedToAIssueName);

        var sender = await _userManager.Users
            .Where(u => u.Id == notification.UpdateIssueDto.ModificationUserId)
            .FirstOrDefaultAsync(cancellationToken) ?? throw new UserNullException();

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name, EmailConstants.MadeOneUpdate, project.Name, notification.Issue.Code, notification.Issue.Name, string.Empty, project.Code, notification.Issue.Id);
        var changeAssigneeIssueEmailContentDto = new ChangeAssigneeIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl);

        if (notification.UpdateIssueDto.AssigneeId is Guid newAssigneeId
            && notification.Issue.IssueDetail!.AssigneeId is Guid oldAssigneeId)
        {
            changedTheAssigneeHis.Content = new AssigneeFromTo
            {
                From = oldAssigneeId,
                To = newAssigneeId,
            }.ToJson();
            _issueHistoryRepository.Insert(changedTheAssigneeHis);

            var fromAssigneeName = await _userManager.Users
                .Where(u => u.Id == oldAssigneeId)
                .Select(u => u.Name)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            var toAssigneeName = await _userManager.Users
                .Where(u => u.Id == newAssigneeId)
                .Select(u => u.Name)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            changeAssigneeIssueEmailContentDto.FromAssigneeName = fromAssigneeName ?? string.Empty;
            changeAssigneeIssueEmailContentDto.ToAssigneeName = toAssigneeName ?? string.Empty;

            string emailContent = EmailContentConstants.ChangeAssigneeIssueContent(changeAssigneeIssueEmailContentDto);
            buidEmailTemplateBaseDto.EmailContent = emailContent;
            if (someoneAssignedEvent is not null)
            {
                var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                     notification.UpdateIssueDto.ModificationUserId,
                     buidEmailTemplateBaseDto,
                someoneAssignedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }
            notification.Issue.IssueDetail.AssigneeId = newAssigneeId;
        }
        else if (notification.UpdateIssueDto.AssigneeId is Guid newAssigneeId1
            && notification.Issue.IssueDetail!.AssigneeId is null)
        {
            changedTheAssigneeHis.Content = new AssigneeFromTo
            {
                From = null,
                To = newAssigneeId1,
            }.ToJson();
            _issueHistoryRepository.Insert(changedTheAssigneeHis);
            var toAssigneeName = await _userManager.Users.Where(u => u.Id == newAssigneeId1).Select(u => u.Name).FirstOrDefaultAsync(cancellationToken: cancellationToken);
            changeAssigneeIssueEmailContentDto.FromAssigneeName = IssueConstants.Unassigned_IssueHistoryContent;
            changeAssigneeIssueEmailContentDto.ToAssigneeName = toAssigneeName ?? string.Empty;

            var emailContent = EmailContentConstants.ChangeAssigneeIssueContent(changeAssigneeIssueEmailContentDto);
            buidEmailTemplateBaseDto.EmailContent = emailContent;
            if (someoneAssignedEvent is not null)
            {
                var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                     notification.UpdateIssueDto.ModificationUserId,
                     buidEmailTemplateBaseDto,
                someoneAssignedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }
            notification.Issue.IssueDetail!.AssigneeId = newAssigneeId1;
        }
        else if (notification.UpdateIssueDto.AssigneeId is null
            && notification.Issue.IssueDetail!.AssigneeId is Guid oldAssigneeId1)
        {
            changedTheAssigneeHis.Content = new AssigneeFromTo
            {
                From = oldAssigneeId1,
                To = null,
            }.ToJson();
            _issueHistoryRepository.Insert(changedTheAssigneeHis);
            var fromAssigneeName = await _userManager.Users.Where(u => u.Id == oldAssigneeId1).Select(u => u.Name).FirstOrDefaultAsync();
            changeAssigneeIssueEmailContentDto.FromAssigneeName = fromAssigneeName ?? string.Empty;
            changeAssigneeIssueEmailContentDto.ToAssigneeName = IssueConstants.Unassigned_IssueHistoryContent;

            string emailContent = EmailContentConstants.ChangeAssigneeIssueContent(changeAssigneeIssueEmailContentDto);
            buidEmailTemplateBaseDto.EmailContent = emailContent;
            if (someoneAssignedEvent is not null)
            {
                var sendEmailModel = SendEmailModel.Create(notification.Issue.Id,
                     notification.UpdateIssueDto.ModificationUserId,
                     buidEmailTemplateBaseDto,
                someoneAssignedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }

            notification.Issue.IssueDetail!.AssigneeId = null;
        }
    }
}
