namespace TaskManager.Application.Projects.Queries.GetTypeFiltersViewModels;

internal sealed class GetTypeFiltersViewModelsQueryHandler(
    IProjectRepository projectRepository
    )
    : IQueryHandler<GetTypeFiltersViewModelsQuery, Maybe<IReadOnlyCollection<TypeFilterViewModel>>>
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<Maybe<IReadOnlyCollection<TypeFilterViewModel>>> Handle(GetTypeFiltersViewModelsQuery request, CancellationToken cancellationToken)
    {
        var typeFilterViewModels = await _projectRepository.GetIssueTypeFiltersByProjectIdAsync(request.ProjectId);
        return Maybe<IReadOnlyCollection<TypeFilterViewModel>>.From(typeFilterViewModels);
    }
}
