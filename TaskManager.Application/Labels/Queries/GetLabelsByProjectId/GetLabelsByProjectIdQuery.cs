namespace TaskManager.Application.Labels.Queries.GetLabelsByProjectId;

public class GetLabelsByProjectIdQuery(
    Guid projectId,
    PaginationInput paginationInput
    )
    : IQuery<Maybe<object>>
{
    public Guid ProjectId { get; private set; } = projectId;
    public PaginationInput PaginationInput { get; private set; } = paginationInput;
}
