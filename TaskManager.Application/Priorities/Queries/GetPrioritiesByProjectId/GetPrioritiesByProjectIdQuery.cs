namespace TaskManager.Application.Priorities.Queries.GetPrioritiesByProjectId;

public sealed class GetPrioritiesByProjectIdQuery(
    Guid projectId,
    PaginationInput paginationInput
    )
    : IQuery<Maybe<object>>
{
    public Guid ProjectId { get; private set; } = projectId;
    public PaginationInput PaginationInput { get; private set; } = paginationInput;
}
