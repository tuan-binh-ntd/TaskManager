namespace TaskManager.Application.Comments.Queries.GetCommentsByIssueId;

public sealed class GetCommentsByIssueIdQuery(
    Guid issueId
    )
    : IQuery<Maybe<IReadOnlyCollection<CommentViewModel>>>
{
    public Guid IssueId { get; private set; } = issueId;
}
