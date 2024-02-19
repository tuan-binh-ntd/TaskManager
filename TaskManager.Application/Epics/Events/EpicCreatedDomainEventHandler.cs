namespace TaskManager.Application.Epics.Events;

internal sealed class EpicCreatedDomainEventHandler(
    IIssueHistoryRepository issueHistoryRepository,
    IUnitOfWork unitOfWork,
    INotificationRepository notificationRepository,
    UserManager<AppUser> userManager,
    IProjectRepository projectRepository,
    IIssueRepository issueRepository,
    IIssueTypeRepository issueTypeRepository,
    IEmailSender emailSender
    )
    : IDomainEventHandler<EpicCreatedDomainEvent>
{
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly IIssueTypeRepository _issueTypeRepository = issueTypeRepository;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task Handle(EpicCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var issueHistory = IssueHistory.Create(IssueConstants.Created_IssueHistoryName,
            notification.CreateEpicDto.CreatorUserId,
            string.Empty,
            notification.Epic.Id);

        _issueHistoryRepository.Insert(issueHistory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        if (notification.Epic.ProjectId is Guid projectId)
        {
            var issueCreatedEvent = await _notificationRepository
                .GetNotificationConfigurationByProjectIdAndEventNameAsync(projectId, IssueEventConstants.IssueCreatedName);
            if (issueCreatedEvent is not null)
            {
                var reporterName = await _userManager.Users.Where(u => u.Id == notification.CreateEpicDto.CreatorUserId).Select(u => u.Name).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? IssueConstants.None_IssueHistoryContent;
                var senderName = await _userManager.Users.Where(u => u.Id == notification.CreateEpicDto.CreatorUserId).Select(u => u.Name).FirstOrDefaultAsync(cancellationToken: cancellationToken)
                    ?? IssueConstants.None_IssueHistoryContent;
                var projectName = await _projectRepository.GetProjectNameAsync(notification.CreateEpicDto.ProjectId);
                var projectCode = await _issueRepository.GetProjectCodeOfIssueAsync(notification.Epic.Id);
                var avatarUrl = await _userManager.Users.Where(u => u.Id == notification.CreateEpicDto.CreatorUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? TextToImageConstants.AnonymousAvatar;

                var createdIssueEmailContentDto = new CreatedIssueEmailContentDto(reporterName, notification.Epic.CreationTime, avatarUrl)
                {
                    IssueTypeName = await _issueTypeRepository.GetNameOfIssueTypeAsync(notification.Epic.IssueTypeId) ?? IssueConstants.None_IssueHistoryContent,
                    AssigneeName = await _userManager.Users.Where(u => u.Id == notification.IssueDetail.AssigneeId).Select(u => u.Name).FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? IssueConstants.Unassigned_IssueHistoryContent,
                    PriorityName = IssueConstants.None_IssueHistoryContent,
                    AssigneeAvatarUrl = await _userManager.Users.Where(u => u.Id == notification.IssueDetail.AssigneeId).Select(u => u.AvatarUrl).FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? TextToImageConstants.AnonymousAvatar + TextToImageConstants.AccessTokenAvatar,
                };
                string emailContent = EmailContentConstants.CreatedIssueContent(createdIssueEmailContentDto);
                var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, notification.Epic.Code, notification.Epic.Name, emailContent, projectCode, notification.Epic.Id);

                var sendEmailModel = SendEmailModel.Create(notification.Epic.Id, notification.CreateEpicDto.CreatorUserId, buidEmailTemplateBaseDto, issueCreatedEvent);
                await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
            }
        }
    }
}
