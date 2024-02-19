namespace TaskManager.Core.Interfaces.Repositories;

public interface ISprintRepository
{
    Task<IReadOnlyCollection<Issue>> GetIssuesBySprintIdAsync(Guid sprintId, Guid projectId);
    Task<IReadOnlyCollection<SprintViewModel>> GetSprintsByProjectIdAsync(Guid projectId);
    Task<IReadOnlyCollection<Guid>> GetSprintIdsByProjectIdAsync(Guid projectId);
    Task<IReadOnlyCollection<Issue>> GetIssuesBySprintIdAsync(Guid sprintId);
    Task<string?> GetNameOfSprintAsync(Guid sprintId);
    Task<IReadOnlyCollection<Guid>> GetSprintIdsByProjectIdsAsync(IReadOnlyCollection<Guid> projectIds);
    Task<IReadOnlyCollection<SprintViewModel>> GetSprintViewModelsByProjectIdAsync(Guid projectId);
    Task<Sprint?> GetByIdAsync(Guid id);
    void Insert(Sprint sprint);
    void Update(Sprint sprint);
    void Remove(Sprint sprint);
}
