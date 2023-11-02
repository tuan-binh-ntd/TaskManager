using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IIssueEventRepository : IRepository<IssueEvent>
    {
        Task<IReadOnlyCollection<IssueEvent>> Gets();
    }
}
