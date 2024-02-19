namespace TaskManager.Application.Comments.Commands.Update;

public sealed class UpdateCommentCommand(
    Guid commentId,
    UpdateCommentDto updateCommentDto
    )
    : ICommand<Result>
{
    public Guid CommentId { get; private set; } = commentId;
    public UpdateCommentDto UpdateCommentDto { get; private set; } = updateCommentDto;
}
