namespace TaskManager.Infrastructure.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;
    private readonly IEmailSender _emailSender;
    private readonly UserManager<AppUser> _userManager;
    private readonly IIssueRepository _issueRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IMapper _mapper;

    public CommentService(
        ICommentRepository commentRepository,
        IEmailSender emailSender,
        UserManager<AppUser> userManager,
        IIssueRepository issueRepository,
        INotificationRepository notificationRepository,
        IMapper mapper
        )
    {
        _commentRepository = commentRepository;
        _emailSender = emailSender;
        _userManager = userManager;
        _issueRepository = issueRepository;
        _notificationRepository = notificationRepository;
        _mapper = mapper;
    }

    public async Task<CommentViewModel> CreateComment(Guid issueId, CreateCommentDto createCommentDto)
    {
        var comment = _mapper.Map<Comment>(createCommentDto);
        comment.IssueId = issueId;
        _commentRepository.Add(comment);
        await _commentRepository.UnitOfWork.SaveChangesAsync();

        var issue = await _issueRepository.Get(issueId);
        var projectId = await _issueRepository.GetProjectIdOfIssue(issue.Id);
        var notificationConfig = await _notificationRepository.GetNotificationIssueEventByProjectId(projectId);
        var someoneMadeCommentEvent = notificationConfig.Where(n => n.EventName == CoreConstants.SomeoneMadeACommentName).FirstOrDefault();

        var senderName = await _userManager.Users.Where(u => u.Id == createCommentDto.CreatorUserId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;
        var projectName = await _issueRepository.GetProjectNameOfIssue(issueId);
        var projectCode = await _issueRepository.GetProjectCodeOfIssue(issueId);
        var avatarUrl = await _userManager.Users.Where(u => u.Id == createCommentDto.CreatorUserId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

        var addNewCommentIssueEmailContentDto = new AddNewCommentIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue, createCommentDto.Content, avatarUrl);

        string emailContent = EmailContentConstants.AddNewCommentIssueContent(addNewCommentIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.MadeOneUpdate, projectName, issue.Code, issue.Name, emailContent, projectCode, issueId);

        if (someoneMadeCommentEvent is not null)
        {
            await _emailSender.SendEmailWhenUpdateIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: createCommentDto.CreatorUserId, buidEmailTemplateBaseDto, someoneMadeCommentEvent);
        }
        return comment.Adapt<CommentViewModel>();
    }

    public async Task<Guid> DeleteComment(Guid issueId, Guid id, Guid userId)
    {
        var comment = await _commentRepository.GetById(id) ?? throw new CommentNullException();
        _commentRepository.Delete(comment);
        await _commentRepository.UnitOfWork.SaveChangesAsync();

        var issue = await _issueRepository.Get(issueId);
        var projectId = await _issueRepository.GetProjectIdOfIssue(issue.Id);
        var notificationConfig = await _notificationRepository.GetNotificationIssueEventByProjectId(projectId);
        var commentDeletedEvent = notificationConfig.Where(n => n.EventName == CoreConstants.CommentDeletedName).FirstOrDefault();

        var senderName = await _userManager.Users.Where(u => u.Id == userId).Select(u => u.Name).FirstOrDefaultAsync() ?? IssueConstants.None_IssueHistoryContent;
        var projectCode = await _issueRepository.GetProjectCodeOfIssue(issueId);

        var projectName = await _issueRepository.GetProjectNameOfIssue(issueId);
        var avatarUrl = await _userManager.Users.Where(u => u.Id == userId).Select(u => u.AvatarUrl).FirstOrDefaultAsync() ?? CoreConstants.AnonymousAvatar;

        var deleteCommentIssueEmailContentDto = new DeleteCommentIssueEmailContentDto(senderName, IssueConstants.UpdateTime_Issue, comment.Content, avatarUrl);

        string emailContent = EmailContentConstants.DeleteCommentIssueContent(deleteCommentIssueEmailContentDto);

        var buidEmailTemplateBaseDto = new BuidEmailTemplateBaseDto(senderName, EmailConstants.DeleteOneComment, projectName, issue.Code, issue.Name, emailContent, projectCode, issueId);

        if (commentDeletedEvent is not null)
        {
            await _emailSender.SendEmailWhenUpdateIssue(issue.Id, subjectOfEmail: $"({issue.Code}) {issue.Name}", from: userId, buidEmailTemplateBaseDto, commentDeletedEvent);
        }

        return id;
    }

    public async Task<IReadOnlyCollection<CommentViewModel>> GetCommentsByIssueId(Guid issueId)
    {
        var comments = await _commentRepository.GetByIssueId(issueId);
        return comments.Adapt<IReadOnlyCollection<CommentViewModel>>();
    }

    public async Task<CommentViewModel> UpdateComment(Guid id, UpdateCommentDto updateCommentDto, Guid issueId)
    {
        var projectId = await _issueRepository.GetProjectIdOfIssue(issueId);
        var notificationConfig = await _notificationRepository.GetNotificationIssueEventByProjectId(projectId);
        var commentEditedEvent = notificationConfig.Where(n => n.EventName == CoreConstants.CommentEditedName).FirstOrDefault();

        var comment = await _commentRepository.GetById(id) ?? throw new CommentNullException();
        comment.Content = updateCommentDto.Content;
        comment.IsEdited = true;
        _commentRepository.Update(comment);
        await _commentRepository.UnitOfWork.SaveChangesAsync();
        return comment.Adapt<CommentViewModel>();
    }
}
