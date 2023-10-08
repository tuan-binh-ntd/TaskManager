using TaskManager.Core.DTOs;
using TaskManager.Core.Entities;
using TaskManager.Core.Helper;
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
        Task<PaginationResult<Project>> GetByUserId(Guid userId, GetProjectByFilterDto filter, PaginationInput paginationInput);
        Task<IReadOnlyCollection<Project>> GetByUserId(Guid userId, GetProjectByFilterDto input);
        Task<IReadOnlyCollection<UserViewModel>> GetMembers(Guid projectId);
        Task<Project?> GetByCode(string code);
        Task<BacklogViewModel> GetBacklog(Guid projectId);
    }
}
