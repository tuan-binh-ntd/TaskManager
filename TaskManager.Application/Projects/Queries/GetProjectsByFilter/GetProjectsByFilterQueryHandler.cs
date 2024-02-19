namespace TaskManager.Application.Projects.Queries.GetProjectsByFilter;

internal class GetProjectsByFilterQueryHandler(
    IProjectRepository projectRepository
    )
    : IQueryHandler<GetProjectsByFilterQuery, Maybe<object>>
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<Maybe<object>> Handle(GetProjectsByFilterQuery request, CancellationToken cancellationToken)
    {
        if (request.PaginationInput.IsPaging())
        {
            var pagedProjects = await _projectRepository.GetProjectsByUserIdPagingAsync(request.UserId, request.GetProjectByFilterDto, request.PaginationInput);
            var res = new PaginationResult<ProjectViewModel>()
            {
                TotalCount = pagedProjects.TotalCount,
                TotalPage = pagedProjects.TotalPage,
                Content = pagedProjects.Content.Adapt<IReadOnlyCollection<ProjectViewModel>>()
            };
            return Maybe<PaginationResult<ProjectViewModel>>.From(res);
        }
        else
        {
            var res = await _projectRepository.GetProjectsByUserIdAsync(request.UserId, request.GetProjectByFilterDto);
            return Maybe<IReadOnlyCollection<ProjectViewModel>>.From(res.Adapt<IReadOnlyCollection<ProjectViewModel>>());
        }
    }
}
