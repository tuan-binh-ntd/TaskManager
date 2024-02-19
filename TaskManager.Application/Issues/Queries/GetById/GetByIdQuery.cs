namespace TaskManager.Application.Issues.Queries.GetById;

public sealed class GetByIdQuery(
    Guid issueId
    )
    : IQuery<Maybe<IssueViewModel>>
{
    public Guid IssueId { get; private set; } = issueId;
}
