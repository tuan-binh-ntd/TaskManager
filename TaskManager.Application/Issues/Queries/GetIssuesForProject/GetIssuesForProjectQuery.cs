namespace TaskManager.Application.Issues.Queries.GetIssuesForProject;

public sealed class GetIssuesForProjectQuery(
    Guid projectId
    )
    : IQuery<Maybe<IReadOnlyCollection<IssueForProjectViewModel>>>
{
    public Guid ProjectId { get; private set; } = projectId;
}
