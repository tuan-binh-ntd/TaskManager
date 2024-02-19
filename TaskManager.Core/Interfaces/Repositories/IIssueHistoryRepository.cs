namespace TaskManager.Core.Interfaces.Repositories;

public interface IIssueHistoryRepository
{
    void Insert(IssueHistory issueHistory);
    Task<IReadOnlyCollection<IssueHistory>> GetIssueHistoriesByIssueIdAsync(Guid issueId);
    void InsertRange(IReadOnlyCollection<IssueHistory> issueHistories);
}
