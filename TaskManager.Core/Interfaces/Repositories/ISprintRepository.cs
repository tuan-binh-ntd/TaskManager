using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface ISprintRepository : IRepository<Sprint>
    {
        Task<IReadOnlyCollection<SprintViewModel>> Gets();
        SprintViewModel Add(Sprint sprint);
        void Update(Sprint sprint);
        void Delete(Guid id);
        Sprint? Get(Guid id);
        Task<IReadOnlyCollection<IssueViewModel>> GetIssues(Guid sprintId);
        Task<IReadOnlyCollection<SprintViewModel>> GetSprintByProjectId(Guid projectId);
    }
}
