namespace TaskManager.Application.Comments.Queries.GetCommentsByIssueId;

internal class GetCommentsByIssueIdQueryHandler(
    ICommentRepository commentRepository
    )
    : IQueryHandler<GetCommentsByIssueIdQuery, Maybe<IReadOnlyCollection<CommentViewModel>>>
{
    private readonly ICommentRepository _commentRepository = commentRepository;

    public async Task<Maybe<IReadOnlyCollection<CommentViewModel>>> Handle(GetCommentsByIssueIdQuery request, CancellationToken cancellationToken)
    {
        if (request.IssueId == Guid.Empty) return Maybe<IReadOnlyCollection<CommentViewModel>>.None;
        var comments = await _commentRepository.GetCommentsByIssueIdAsync(request.IssueId);
        return Maybe<IReadOnlyCollection<CommentViewModel>>.From(comments);
    }
}
