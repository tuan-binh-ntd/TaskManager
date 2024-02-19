namespace TaskManager.Application.Comments.Commands.Create;

public sealed class CreateCommentCommand(
    Guid creatorUserId,
    string content,
    Guid issueId
    )
    : ICommand<Result>
{
    public Guid CreatorUserId { get; private set; } = creatorUserId;
    public string Content { get; private set; } = content;
    public Guid IssueId { get; private set; } = issueId;
}
