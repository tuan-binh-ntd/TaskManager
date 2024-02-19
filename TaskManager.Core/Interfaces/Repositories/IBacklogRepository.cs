namespace TaskManager.Core.Interfaces.Repositories;

public interface IBacklogRepository
{
    Task<IReadOnlyCollection<Issue>> GetIssuesByBacklogIdAsync(Guid backlogId);
    Task<BacklogViewModel?> GetBacklogByProjectIdAsync(Guid projectId);
    Task<IReadOnlyCollection<Guid>> GetBacklogIdsByProjectIdsAsync(IReadOnlyCollection<Guid> projectIds);
    Task<Backlog?> GetByIdAsync(Guid id);
    void Insert(Backlog backlog);
    void Update(Backlog backlog);
    void Remove(Backlog backlog);
}
