namespace TaskManager.Core.Interfaces.Repositories;

public interface IIssueDetailRepository
{
    Task<CurrentAssigneeAndReporterViewModel?> GetCurrentAssigneeAndReporterAsync(Guid issueId);
    Task<Guid?> GetCurrentAssigneeIdAsync(Guid issueId);
    Task<Guid> GetReporterIdAsync(Guid issueId);
    Task UpdateOneColumnForIssueDetailAsync(Guid oldValue, Guid? newValue, NameColumn nameColumn);
    Task<IssueDetail?> GetByIdAsync(Guid id);
    void Insert(IssueDetail issueDetail);
    void Update(IssueDetail issueDetail);
    void Remove(IssueDetail issueDetail);
    Task<IssueDetail?> GetIssueDetailByIssueIdAsync(Guid issueId);
}
