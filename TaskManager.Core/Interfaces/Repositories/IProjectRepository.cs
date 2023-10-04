using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IReadOnlyCollection<Project>> GetAll();
        Task<Project?> GetById(Guid id);
        Task<Project?> GetByCode(string code);
        Project Add(Project project);
        void Update(Project project);
        void Delete(Guid id);
        Task<IReadOnlyCollection<Project>> GetByLeaderId(Guid leaderId, GetProjectByFilterDto input = null!);
    }
}
