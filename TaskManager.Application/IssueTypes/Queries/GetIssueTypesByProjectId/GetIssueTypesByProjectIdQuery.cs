namespace TaskManager.Application.IssueTypes.Queries.GetIssueTypesByProjectId;

public sealed class GetIssueTypesByProjectIdQuery(
    Guid projectId,
    PaginationInput paginationInput
    )
    : IQuery<Maybe<object>>
{
    public Guid ProjectId { get; private set; } = projectId;
    public PaginationInput PaginationInput { get; private set; } = paginationInput;
}
