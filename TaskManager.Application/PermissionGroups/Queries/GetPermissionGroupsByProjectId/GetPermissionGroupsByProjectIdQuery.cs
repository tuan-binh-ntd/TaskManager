namespace TaskManager.Application.PermissionGroups.Queries.GetPermissionGroupsByProjectId;

public class GetPermissionGroupsByProjectIdQuery(
    Guid projectId,
    PaginationInput paginationInput
    )
    : IQuery<Maybe<object>>
{
    public Guid ProjectId { get; private set; } = projectId;
    public PaginationInput PaginationInput { get; private set; } = paginationInput;
}
