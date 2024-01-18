namespace TaskManager.Core.Interfaces.Repositories;

public interface IBacklogRepository : IRepository<Backlog>
{
    Task<Backlog?> GetAsync(Guid id);
    Backlog Add(Backlog backlog);
    Task<IReadOnlyCollection<Issue>> GetIssues(Guid backlogId);
    Task<BacklogViewModel> GetBacklog(Guid projectId);
    Task<Backlog?> GetByProjectId(Guid projectId);
    Task<IReadOnlyCollection<Guid>> GetBacklogIdsByProjectIds(IReadOnlyCollection<Guid> projectIds);
}
