using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IIssueRepository : IRepository<Issue>
    {
        /// <summary>
        /// Get all issue
        /// </summary>
        /// <returns>List of issue</returns>
        Task<IReadOnlyCollection<IssueViewModel>> Gets();
        /// <summary>
        /// Add issue
        /// </summary>
        /// <param name="issue">Issue entity</param>
        /// <returns>Issue entity</returns>
        Issue Add(Issue issue);
        /// <summary>
        /// Update issue
        /// </summary>
        /// <param name="issue">Issue entity</param>
        void Update(Issue issue);
        /// <summary>
        /// Delete issue by id
        /// </summary>
        /// <param name="id">Id of issue entity</param>
        void Delete(Guid id);
        /// <summary>
        /// Get issue by id
        /// </summary>
        /// <param name="id">Id of issue entity</param>
        /// <returns>Issue entity</returns>
        Task<Issue> Get(Guid id);
        /// <summary>
        /// Get all issue for sprint
        /// </summary>
        /// <param name="sprintId">Id of sprint entity</param>
        /// <returns>List of issue</returns>
        Task<IReadOnlyCollection<Issue>> GetIssueBySprintId(Guid sprintId);
        /// <summary>
        /// Get all issue for backlog
        /// </summary>
        /// <param name="backlogId">Id of backlog entity</param>
        /// <returns>List of issue</returns>
        Task<IReadOnlyCollection<Issue>> GetIssueByBacklogId(Guid backlogId);
        /// <summary>
        /// Update range issue
        /// </summary>
        /// <param name="issues">List of issue</param>
        void UpdateRange(IReadOnlyCollection<Issue> issues);
        /// <summary>
        /// Get all child of issue
        /// </summary>
        /// <param name="parentId">Id of parent issue entity</param>
        /// <returns>List of child issue</returns>
        Task<IReadOnlyCollection<Issue>> GetChildIssues(Guid parentId);
        /// <summary>
        /// Remove range issue
        /// </summary>
        /// <param name="issues">List of issue</param>
        void DeleteRange(IReadOnlyCollection<Issue> issues);
        /// <summary>
        /// Load full related entities to issue
        /// Use Explicit Loading
        /// </summary>
        /// <param name="issue">Issue entity</param>
        void LoadEntitiesRelationship(Issue issue);

        Task<IReadOnlyCollection<Issue>> GetByIds(IReadOnlyCollection<Guid> ids);
        Task<IReadOnlyCollection<Issue>> GetEpicByProjectId(Guid projectId);
    }
}
