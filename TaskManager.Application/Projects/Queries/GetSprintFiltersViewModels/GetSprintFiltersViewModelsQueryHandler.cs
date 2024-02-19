namespace TaskManager.Application.Projects.Queries.GetSprintFiltersViewModels;

internal sealed class GetSprintFiltersViewModelsQueryHandler(
    IProjectRepository projectRepository
    )
    : IQueryHandler<GetSprintFiltersViewModelsQuery, Maybe<IReadOnlyCollection<SprintFilterViewModel>>>
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<Maybe<IReadOnlyCollection<SprintFilterViewModel>>> Handle(GetSprintFiltersViewModelsQuery request, CancellationToken cancellationToken)
    {
        var sprintFilterViewModels = await _projectRepository.GetSprintFiltersByProjectIdAsync(request.ProjectId);
        return Maybe<IReadOnlyCollection<SprintFilterViewModel>>.From(sprintFilterViewModels);
    }
}
