namespace TaskManager.Application.Filters.Queries.GetIssuesByConfiguration;

public sealed class GetIssuesByConfigurationQuery(
    GetIssueByConfigurationDto getIssueByConfigurationDto
    )
    : IQuery<Maybe<IReadOnlyCollection<IssueViewModel>>>
{
    public GetIssueByConfigurationDto GetIssueByConfigurationDto { get; private set; } = getIssueByConfigurationDto;
}
