using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories;

public interface IIssueDetailRepository : IRepository<IssueDetail>
{
    Task<IReadOnlyCollection<IssueDetailViewModel>> Gets();
    IssueDetailViewModel Add(IssueDetail issueDetail);
    void Update(IssueDetail issueDetail);
    void Delete(Guid id);
    Task<IssueDetail> GetById(Guid id);
    Task<CurrentAssigneeAndReporterViewModel?> GetCurrentAssigneeAndReporter(Guid issueId);
    Task<Guid?> GetCurrentAssignee(Guid issueId);
    Task<Guid> GetReporter(Guid issueId);
}
