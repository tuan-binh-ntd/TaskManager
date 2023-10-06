using TaskManager.Core.DTOs;
using TaskManager.Core.Helper;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    /// <summary>
    /// Implementation business logic of project 
    /// </summary>
    public interface IProjectService
    {
        Task<IReadOnlyCollection<ProjectViewModel>> GetProjects();
        Task<ProjectViewModel> Create(Guid userId, CreateProjectDto projectDto);
        Task<ProjectViewModel> Update(Guid id, UpdateProjectDto updateProjectDto);
        Task<bool> Delete(Guid id);
        Task<object> GetProjectsByFilter(Guid userId, GetProjectByFilterDto filter, PaginationInput paginationInput);
        Task<ProjectViewModel> Get(Guid projectId);
        Task<ProjectViewModel?> Get(string code);
        Task<ProjectViewModel> AddMember(AddMemberToProjectDto addMemberToProjectDto);
        Task<ProjectViewModel> ChangeLeader(Guid userId, Guid projectId, UpdateProjectDto updateProjectDto);
    }
}
