using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Repositories
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<IReadOnlyCollection<Project>> GetAll();
        Task<Project> GetById(Guid id);
        Project Add(Project project);
        void Update(Project project);
        void Delete(Guid id);
        Task<object> GetByUserId(Guid userId, GetProjectByFilterDto input = null!);
        Task<IReadOnlyCollection<UserViewModel>> GetMembers(Guid projectId);
        Task<Project?> GetByCode(string code);
    }
}
