namespace TaskManager.Application.Projects.Queries.GetLabelFiltersViewModels;

internal sealed class GetLabelFiltersViewModelsQueryHandler(
    IProjectRepository projectRepository
    )
    : IQueryHandler<GetLabelFiltersViewModelsQuery, Maybe<IReadOnlyCollection<LabelFilterViewModel>>>
{
    private readonly IProjectRepository _projectRepository = projectRepository;

    public async Task<Maybe<IReadOnlyCollection<LabelFilterViewModel>>> Handle(GetLabelFiltersViewModelsQuery request, CancellationToken cancellationToken)
    {
        var labelFilterViewModels = await _projectRepository.GetLabelFiltersByProjectIdAsync(request.ProjectId);
        return Maybe<IReadOnlyCollection<LabelFilterViewModel>>.From(labelFilterViewModels);
    }
}
