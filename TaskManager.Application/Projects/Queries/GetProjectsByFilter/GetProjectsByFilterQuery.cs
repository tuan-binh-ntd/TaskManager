namespace TaskManager.Application.Projects.Queries.GetProjectsByFilter;

public sealed class GetProjectsByFilterQuery(
    Guid userId,
    GetProjectByFilterDto getProjectByFilterDto,
    PaginationInput paginationInput
    )
    : IQuery<Maybe<object>>
{
    public Guid UserId { get; private set; } = userId;
    public GetProjectByFilterDto GetProjectByFilterDto { get; private set; } = getProjectByFilterDto;
    public PaginationInput PaginationInput { get; private set; } = paginationInput;
}
