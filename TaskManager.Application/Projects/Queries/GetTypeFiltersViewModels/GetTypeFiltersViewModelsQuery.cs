namespace TaskManager.Application.Projects.Queries.GetTypeFiltersViewModels;

public sealed class GetTypeFiltersViewModelsQuery(
    Guid projectId
    )
    : IQuery<Maybe<IReadOnlyCollection<TypeFilterViewModel>>>
{
    public Guid ProjectId { get; private set; } = projectId;
}
