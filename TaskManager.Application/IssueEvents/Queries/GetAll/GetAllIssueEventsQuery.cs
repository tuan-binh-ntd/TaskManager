namespace TaskManager.Application.IssueEvents.Queries.GetAll;

public sealed class GetAllIssueEventsQuery
    : IQuery<Maybe<IReadOnlyCollection<IssueEventViewModel>>>
{
}
