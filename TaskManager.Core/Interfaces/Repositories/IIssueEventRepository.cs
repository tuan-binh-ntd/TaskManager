namespace TaskManager.Core.Interfaces.Repositories;

public interface IIssueEventRepository
{
    Task<IReadOnlyCollection<IssueEvent>> GetIssueEventsAsync();
    Task<IReadOnlyCollection<IssueEventViewModel>> GetIssueEventViewModelsAsync();
}
