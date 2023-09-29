using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IReadOnlyCollection<Project>> GetAll();
        Task<Project?> GetById(Guid id);
        Project Add(Project project);
        void Update(Project project);
        void Delete(Guid id);
    }
}
