using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories;

public interface ISprintRepository : IRepository<Sprint>
{
    Task<IReadOnlyCollection<SprintViewModel>> Gets();
    SprintViewModel Add(Sprint sprint);
    void Update(Sprint sprint);
    void Delete(Guid id);
    Sprint? Get(Guid id);
    Task<IReadOnlyCollection<Issue>> GetIssues(Guid sprintId, Guid projectId);
    Task<IReadOnlyCollection<SprintViewModel>> GetSprintByProjectId(Guid projectId);
    Task<IReadOnlyCollection<Guid>> GetSprintIdsByProjectId(Guid projectId, GetSprintByFilterDto getSprintByFilterDto);
    Task<IReadOnlyCollection<Issue>> GetIssues(Guid sprintId);
    Task<string?> GetNameOfSprint(Guid sprintId);
    Task<IReadOnlyCollection<Guid>> GetSprintIdsByProjectIds(IReadOnlyCollection<Guid> projectIds);
}
