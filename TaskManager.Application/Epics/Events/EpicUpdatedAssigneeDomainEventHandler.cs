namespace TaskManager.Application.Epics.Events;

internal sealed class EpicUpdatedAssigneeDomainEventHandler(
    INotificationRepository notificationRepository,
    IIssueRepository issueRepository,
    UserManager<AppUser> userManager,
    IIssueHistoryRepository issueHistoryRepository,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<EpicUpdatedAssgineeDomainEvent>
{
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(EpicUpdatedAssgineeDomainEvent notification, CancellationToken cancellationToken)
    {
        await _issueRepository.LoadIssueDetailAsync(notification.Epic);

        var changedTheAssigneeHis = IssueHistory.Create(IssueConstants.Assignee_IssueHistoryName,
            notification.UpdateEpicDto.ModificationUserId,
            notification.Epic.Id);

        var project = await _issueRepository.GetProjectByIssueIdAsync(notification.Epic.Id) ?? throw new ProjectNullException();
        var someoneAssignedEvent = await _notificationRepository
            .GetNotificationConfigurationByProjectIdAndEventNameAsync(project.Id, IssueEventConstants.SomeoneAssignedToAIssueName);

        var sender = await _userManager.Users
                .Where(u => u.Id == notification.UpdateEpicDto.ModificationUserId)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? throw new UserNullException();

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(sender.Name, EmailConstants.MadeOneUpdate, project.Name, notification.Epic.Code, notification.Epic.Name, string.Empty, project.Code, notification.Epic.Id);
        var changeAssigneeIssueEmailContentDto = new ChangeAssigneeIssueEmailContentDto(sender.Name, DateTime.Now, sender.AvatarUrl);

        if (notification.UpdateEpicDto.AssigneeId is Guid newAssigneeId && notification.Epic.IssueDetail!.AssigneeId is Guid oldAssigneeId)
        {
            changedTheAssigneeHis.Content = new AssigneeFromTo
            {
                From = oldAssigneeId,
                To = newAssigneeId,
            }.ToJson();
            _issueHistoryRepository.Insert(changedTheAssigneeHis);

            var fromAssigneeName = await _userManager.Users.Where(u => u.Id == oldAssigneeId).Select(u => u.Name).FirstOrDefaultAsync();

            var toAssigneeName = await _userManager.Users.Where(u => u.Id == newAssigneeId).Select(u => u.Name).FirstOrDefaultAsync();

            changeAssigneeIssueEmailContentDto.FromAssigneeName = fromAssigneeName ?? string.Empty;
            changeAssigneeIssueEmailContentDto.ToAssigneeName = toAssigneeName ?? string.Empty;

            string emailContent = EmailContentConstants.ChangeAssigneeIssueContent(changeAssigneeIssueEmailContentDto);
            buidEmailTemplateBaseDto.EmailContent = emailContent;
            if (someoneAssignedEvent is not null)
            {
                var sendEmailModel = SendEmailModel.Create(notification.Epic.Id,
                      notification.UpdateEpicDto.ModificationUserId,
                      buidEmailTemplateBaseDto,
                      someoneAssignedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }
            notification.Epic.IssueDetail.AssigneeId = newAssigneeId;
        }
        else if (notification.UpdateEpicDto.AssigneeId is Guid newAssigneeId1 && notification.Epic.IssueDetail!.AssigneeId is null)
        {
            changedTheAssigneeHis.Content = new AssigneeFromTo
            {
                From = null,
                To = newAssigneeId1,
            }.ToJson();
            _issueHistoryRepository.Insert(changedTheAssigneeHis);
            var toAssigneeName = await _userManager.Users.Where(u => u.Id == newAssigneeId1).Select(u => u.Name).FirstOrDefaultAsync();
            changeAssigneeIssueEmailContentDto.FromAssigneeName = IssueConstants.Unassigned_IssueHistoryContent;
            changeAssigneeIssueEmailContentDto.ToAssigneeName = toAssigneeName ?? string.Empty;

            var emailContent = EmailContentConstants.ChangeAssigneeIssueContent(changeAssigneeIssueEmailContentDto);
            buidEmailTemplateBaseDto.EmailContent = emailContent;
            if (someoneAssignedEvent is not null)
            {
                var sendEmailModel = SendEmailModel.Create(notification.Epic.Id,
                     notification.UpdateEpicDto.ModificationUserId,
                     buidEmailTemplateBaseDto,
                     someoneAssignedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }
            notification.Epic.IssueDetail.AssigneeId = newAssigneeId1;
        }
        else if (notification.UpdateEpicDto.AssigneeId is null && notification.Epic.IssueDetail!.AssigneeId is Guid oldAssigneeId1)
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
                var sendEmailModel = SendEmailModel.Create(notification.Epic.Id,
                     notification.UpdateEpicDto.ModificationUserId,
                     buidEmailTemplateBaseDto,
                     someoneAssignedEvent);

                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }

            notification.Epic.IssueDetail.AssigneeId = null;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
