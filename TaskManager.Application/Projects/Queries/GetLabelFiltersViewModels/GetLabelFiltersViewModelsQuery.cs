namespace TaskManager.Application.Projects.Queries.GetLabelFiltersViewModels;

public sealed class GetLabelFiltersViewModelsQuery(
    Guid projectId
    )
    : IQuery<Maybe<IReadOnlyCollection<LabelFilterViewModel>>>
{
    public Guid ProjectId { get; private set; } = projectId;
}
