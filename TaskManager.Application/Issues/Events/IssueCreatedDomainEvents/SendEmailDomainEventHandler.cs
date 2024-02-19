namespace TaskManager.Application.Issues.Events.IssueCreatedDomainEvents;

internal sealed class SendEmailDomainEventHandler(
    UserManager<AppUser> userManager,
    INotificationRepository notificationRepository,
    IIssueTypeRepository issueTypeRepository,
    IPriorityRepository priorityRepository,
    IProjectRepository projectRepository,
    IEmailSender emailSender
    )
    : IDomainEventHandler<IssueCreatedDomainEvent>
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IIssueTypeRepository _issueTypeRepository = issueTypeRepository;
    private readonly IPriorityRepository _priorityRepository = priorityRepository;
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task Handle(IssueCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var creatorUser = await _userManager.FindByIdAsync(notification.CreatorUserId.ToString()) ?? throw new UserNullException();

        var issueCreatedEvent = await _notificationRepository
            .GetNotificationConfigurationByProjectIdAndEventNameAsync(notification.ProjectId, IssueEventConstants.IssueCreatedName);

        if (issueCreatedEvent is not null)
        {
            var project = await _projectRepository.GetByIdAsync(notification.ProjectId) ?? throw new ProjectNullException();

            var createdIssueEmailContentDto = new CreatedIssueEmailContentDto(creatorUser.Name, notification.Issue.CreationTime, creatorUser.AvatarUrl)
            {
                IssueTypeName = await _issueTypeRepository.GetNameOfIssueTypeAsync(notification.Issue.IssueTypeId) ?? IssueConstants.None_IssueHistoryContent,
                AssigneeName = await _userManager.Users.Where(u => u.Id == notification.DefaultAssigneeId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.Unassigned_IssueHistoryContent,
                PriorityName = notification.Issue.PriorityId is Guid priorityId ? await _priorityRepository.GetNameOfPriorityByIdAsync(priorityId) ?? IssueConstants.None_IssueHistoryContent : IssueConstants.None_IssueHistoryContent,
                AssigneeAvatarUrl = await _userManager.Users.Where(u => u.Id == notification.DefaultAssigneeId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? TextToImageConstants.AnonymousAvatar + TextToImageConstants.AccessTokenAvatar,
            };

            string emailContent = EmailContentConstants.CreatedIssueContent(createdIssueEmailContentDto);
            var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(creatorUser.Name, EmailConstants.CreatedAnIssue, project.Name, notification.Issue.Code, notification.Issue.Name, emailContent, project.Code, notification.Issue.Id);

            var sendEmailModel = SendEmailModel.Create(notification.Issue.Id, notification.CreatorUserId, buidEmailTemplateBaseDto, issueCreatedEvent);
            await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
        }
    }
}
