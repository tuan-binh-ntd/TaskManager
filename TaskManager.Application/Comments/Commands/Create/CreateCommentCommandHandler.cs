namespace TaskManager.Application.Comments.Commands.Create;

public sealed class CreateCommentCommandHandler(
    ICommentRepository commentRepository,
    IUnitOfWork unitOfWork
    )
    : ICommandHandler<CreateCommentCommand, Result>
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result> Handle(CreateCommentCommand createCommentCommand, CancellationToken cancellationToken)
    {
        var comment = Comment.Create(createCommentCommand.CreatorUserId, createCommentCommand.Content, createCommentCommand.IssueId);
        comment.CommentCreated();
        _commentRepository.Insert(comment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);


        return Result.Success(comment.Adapt<CommentViewModel>());
    }
}
