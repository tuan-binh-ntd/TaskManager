namespace TaskManager.Application.Attachments.Events;

public sealed class AttachmentCreatedDomainEventHandler(
    IIssueHistoryRepository issueHistoryRepository,
    INotificationIssueEventRepository notificationIssueEventRepository,
    IIssueRepository issueRepository,
    UserManager<AppUser> userManager,
    IEmailSender emailSender,
    IUnitOfWork unitOfWork
    )
    : IDomainEventHandler<AttachmentCreatedDomainEvent>
{
    private readonly IIssueHistoryRepository _issueHistoryRepository = issueHistoryRepository;
    private readonly INotificationIssueEventRepository _notificationIssueEventRepository = notificationIssueEventRepository;
    private readonly IIssueRepository _issueRepository = issueRepository;
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(AttachmentCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var projectId = await _issueRepository.GetProjectIdOfIssueAsync(notification.IssueId);
        var someoneAddedAttachmentEvent = await _notificationIssueEventRepository.GetSomeoneAddedAnAttachmentEventByProjectIdAsync(projectId);

        var issueHistory = IssueHistory.Create(
            IssueConstants.Added_Attachment_IssueHistoryName,
            notification.UserId,
            $"{IssueConstants.None_IssueHistoryContent} to {notification.FileName}",
            notification.IssueId);

        _issueHistoryRepository.Insert(issueHistory);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var senderName = await _userManager.Users
            .Where(u => u.Id == notification.UserId)
            .Select(u => u.Name)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ?? IssueConstants.None_IssueHistoryContent;

        var avatarUrl = await _userManager.Users
            .Where(u => u.Id == notification.UserId)
            .Select(u => u.AvatarUrl).FirstOrDefaultAsync(cancellationToken) ?? TextToImageConstants.AnonymousAvatar;

        var addNewAttachmentIssueEmailContentDto = new AddNewAttachmentIssueEmailContentDto(senderName, DateTime.Now, avatarUrl)
        {
            AttachmentName = notification.FileName,
        };

        string emailContent = EmailContentConstants.AddNewAttachmentIssueContent(addNewAttachmentIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(EmailConstants.AddOneNewAttachment, emailContent, notification.IssueId);

        if (someoneAddedAttachmentEvent is not null)
        {
            var sendEmailModel = SendEmailModel.Create(notification.IssueId, notification.UserId, buidEmailTemplateBaseDto, someoneAddedAttachmentEvent);
            await _emailSender.SendEmailWhenCreatedIssue(sendEmailModel);
        }
    }
}
