using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IIssueRepository : IRepository<Issue>
    {
        Task<IReadOnlyCollection<IssueViewModel>> Gets();
        Issue Add(Issue issue);
        void Update(Issue issue);
        void Delete(Guid id);
        Issue? Get(Guid id);
        Task<IReadOnlyCollection<Issue>> GetIssueBySprintId(Guid sprintId);
        Task<IReadOnlyCollection<Issue>> GetIssueByBacklogId(Guid backlogId);
        void UpdateRange(IReadOnlyCollection<Issue> issues);
        Task<IReadOnlyCollection<Issue>> GetChildIssues(Guid parentId);
        void DeleteRange(IReadOnlyCollection<Issue> issues);
        void LoadEntitiesRelationship(Issue issue);
    }
}
