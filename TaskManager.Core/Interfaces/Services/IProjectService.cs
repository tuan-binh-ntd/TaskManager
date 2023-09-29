using TaskManager.Core.DTOs;
using TaskManager.Core.ViewModel;

namespace TaskManager.Core.Interfaces.Services
{
    /// <summary>
    /// Implementation business logic of project 
    /// </summary>
    public interface IProjectService
    {
        Task<IReadOnlyCollection<ProjectViewModel>> GetProjects();
        Task<ProjectViewModel> Create(Guid userId, ProjectDto projectDto);
        Task<ProjectViewModel> Update(Guid id, ProjectDto projectDto);
        Task<bool> Delete(Guid id);
    }
}
