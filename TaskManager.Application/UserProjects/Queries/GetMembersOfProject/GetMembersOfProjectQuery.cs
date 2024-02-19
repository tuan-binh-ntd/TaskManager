namespace TaskManager.Application.UserProjects.Queries.GetMembersOfProject;

public sealed class GetMembersOfProjectQuery(
    Guid projectId,
    PaginationInput paginationInput
    )
    : IQuery<Maybe<object>>
{
    public Guid ProjectId { get; private set; } = projectId;
    public PaginationInput PaginationInput { get; private set; } = paginationInput;
}
