using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IReadOnlyCollection<Project>> GetAllAsync();
        Task<Project?> GetAsync(Guid id);
        Project Add(Project project);
        void Update(Project project);
        void DeleteAsync(Guid id);
    }
}
