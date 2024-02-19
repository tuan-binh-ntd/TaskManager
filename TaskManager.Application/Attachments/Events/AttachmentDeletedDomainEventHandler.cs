namespace TaskManager.Application.Attachments.Events;

public sealed class AttachmentDeletedDomainEventHandler(
    IIssueRepository issueRepository,
    INotificationRepository notificationRepository,
    IIssueHistoryRepository issueHistoryRepository,
    UserManager<AppUser> userManager,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<AttachmentDeletedDomainEvent>
{
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly INotificationRepository _notificationRepository = notificationRepository;
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(AttachmentDeletedDomainEvent notification, CancellationToken cancellationToken)
    {
        var issue = await _issueRepository.GetByIdAsync(notification.IssueId) ?? throw new IssueNullException();
        var projectId = await _issueRepository.GetProjectIdOfIssueAsync(issue.Id);
        var attachmentDeletedEvent = await _notificationRepository.GetNotificationConfigurationByProjectIdAndEventNameAsync(projectId, IssueEventConstants.AttachmentDeletedName);

        var issueHistory = IssueHistory.Create(IssueConstants.Deleted_Attachment_IssueHistoryName,
            notification.UserId,
            $"{notification.FileName} to {IssueConstants.None_IssueHistoryContent}",
            notification.IssueId
            );

        _issueHistoryRepository.Insert(issueHistory);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var senderName = await _userManager.Users.Where(u => u.Id == notification.UserId).Select(u => u.Name).FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? IssueConstants.None_IssueHistoryContent;
        var projectName = await _issueRepository.GetProjectNameOfIssueAsync(notification.IssueId);
        var projectCode = await _issueRepository.GetProjectCodeOfIssueAsync(notification.IssueId);
        var avatarUrl = await _userManager.Users.Where(u => u.Id == notification.UserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? TextToImageConstants.AnonymousAvatar;

        var deleteNewAttachmentIssueEmailContentDto = new DeleteNewAttachmentIssueEmailContentDto(senderName, DateTime.Now, avatarUrl)
        {
            AttachmentName = notification.FileName,
        };

        string emailContent = EmailContentConstants.DeleteNewAttachmentIssueContent(deleteNewAttachmentIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.AddOneNewAttachment, projectName, issue.Code, issue.Name, emailContent, projectCode, notification.IssueId);

        if (attachmentDeletedEvent is not null)
        {
            var sendEmailModel = SendEmailModel.Create(notification.IssueId, notification.UserId, buidEmailTemplateBaseDto, attachmentDeletedEvent);
            await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
        }
    }
}
