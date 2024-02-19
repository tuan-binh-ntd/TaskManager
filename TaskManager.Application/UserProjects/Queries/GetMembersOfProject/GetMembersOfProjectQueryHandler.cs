namespace TaskManager.Application.UserProjects.Queries.GetMembersOfProject;

internal sealed class GetMembersOfProjectQueryHandler(
    IUserProjectRepository userProjectRepository
    )
    : IQueryHandler<GetMembersOfProjectQuery, Maybe<object>>
{
    private readonly IUserProjectRepository _userProjectRepository = userProjectRepository;

    public async Task<Maybe<object>> Handle(GetMembersOfProjectQuery request, CancellationToken cancellationToken)
    {
        var members = await _userProjectRepository.GetMembersByProjectIdAsync(request.ProjectId, request.PaginationInput);
        return Maybe<object>.From(members);
    }
}
