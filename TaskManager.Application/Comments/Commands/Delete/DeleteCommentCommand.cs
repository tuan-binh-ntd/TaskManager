namespace TaskManager.Application.Comments.Commands.Delete;

public sealed class DeleteCommentCommand(
    Guid commentId,
    Guid userId
    )
    : ICommand<Result>
{
    public Guid CommentId { get; private set; } = commentId;
    public Guid UserId { get; private set; } = userId;
}
