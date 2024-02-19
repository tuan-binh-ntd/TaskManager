namespace TaskManager.Application.Comments.Commands.Delete;

internal class DeleteCommentCommandHandler(
    ICommentRepository commentRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<DeleteCommentCommand, Result>
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(DeleteCommentCommand deleteCommentCommand, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(deleteCommentCommand.CommentId);
        if (comment is null) return Result.Failure(Error.NotFound);

        comment.CommentDeleted();
        _commentRepository.Remove(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(deleteCommentCommand.CommentId);
    }
}
