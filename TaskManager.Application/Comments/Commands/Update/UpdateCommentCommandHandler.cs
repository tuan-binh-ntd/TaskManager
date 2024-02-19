namespace TaskManager.Application.Comments.Commands.Update;

internal sealed class UpdateCommentCommandHandler(
    ICommentRepository commentRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<UpdateCommentCommand, Result>
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(UpdateCommentCommand updateCommentCommand, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(updateCommentCommand.CommentId);
        if (comment is null) return Result.Failure(Error.NotFound);

        comment.CommentUpdated(updateCommentCommand.UpdateCommentDto.Content);
        _commentRepository.Update(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success(comment.Adapt<CommentViewModel>());
    }
}
