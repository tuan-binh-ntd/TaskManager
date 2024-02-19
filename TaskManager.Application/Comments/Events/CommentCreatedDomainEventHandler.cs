namespace TaskManager.Application.Comments.Events;

internal sealed class CommentCreatedDomainEventHandler(
    IIssueRepository issueRepository,
    INotificationRepository notificationRepository,
    UserManager<AppUser> userManager,
    IEmailSender emailSender
    )
    : IDomainEventHandler<CommentCreatedDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task Handle(CommentCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetByIdAsync(notification.IssueId) ?? throw new IssueNullException();
        var projectId = await _issueRepository.GetProjectIdOfIssueAsync(issue.Id);
        var someoneMadeCommentEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(projectId, IssueEventConstants.SomeoneMadeACommentName);

        var senderName = await _userManager.Users
            .Where(u => u.Id == notification.CreatorUserId)
            .Select(u => u.Name).FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? IssueConstants.None_IssueHistoryContent;
        var projectName = await _issueRepository.GetProjectNameOfIssueAsync(notification.IssueId);
        var projectCode = await _issueRepository.GetProjectCodeOfIssueAsync(notification.IssueId);
        var avatarUrl = await _userManager.Users
            .Where(u => u.Id == notification.CreatorUserId)
            .Select(u => u.AvatarUrl).FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? TextToImageConstants.AnonymousAvatar;

        var addNewCommentIssueEmailContentDto = new AddNewCommentIssueEmailContentDto(senderName, DateTime.Now, notification.Content, avatarUrl);

        string emailContent = EmailContentConstants.AddNewCommentIssueContent(addNewCommentIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, emailContent, projectCode, notification.IssueId);

        if (someoneMadeCommentEvent is not null)
        {
            var sendEmailModel = SendEmailModel.Create(notification.IssueId, notification.CreatorUserId, buidEmailTemplateBaseDto, someoneMadeCommentEvent);
            await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
        }
    }
}
