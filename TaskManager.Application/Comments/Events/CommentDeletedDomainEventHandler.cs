namespace TaskManager.Application.Comments.Events;

internal sealed class CommentDeletedDomainEventHandler(
    IIssueRepository issueRepository,
    INotificationRepository notificationRepository,
    UserManager<AppUser> userManager,
    IEmailSender emailSender
    )
    : IDomainEventHandler<CommentDeletedDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;

    public async Task Handle(CommentDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetByIdAsync(notification.IssueId) ?? throw new IssueNullException();
        var projectId = await _issueRepository.GetProjectIdOfIssueAsync(notification.IssueId);
        var commentDeletedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(projectId, IssueEventConstants.CommentDeletedName);

        var senderName = await _userManager.Users.Where(u => u.Id == notification.UserId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;
        var projectCode = await _issueRepository.GetProjectCodeOfIssueAsync(notification.IssueId);

        var projectName = await _issueRepository.GetProjectNameOfIssueAsync(notification.IssueId);
        var avatarUrl = await _userManager.Users.Where(u => u.Id == notification.IssueId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? TextToImageConstants.AnonymousAvatar;

        var deleteCommentIssueEmailContentDto = new DeleteCommentIssueEmailContentDto(senderName, DateTime.Now, notification.Content, avatarUrl);
        string emailContent = EmailContentConstants.DeleteCommentIssueContent(deleteCommentIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.DeleteOneComment, projectName, issue.Code, issue.Name, emailContent, projectCode, notification.IssueId);
        if (commentDeletedEvent is not null)
        {
            var sendEmailModel = SendEmailModel.Create(notification.IssueId, notification.UserId, buidEmailTemplateBaseDto, commentDeletedEvent);
            await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
        }
    }
}
